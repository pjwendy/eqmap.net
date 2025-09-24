using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQLogs.Models;
using EQLogs.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EQLogs.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DockerLogService _dockerService;
        private readonly PacketParserService _packetParser;
        
        [ObservableProperty]
        private ObservableCollection<LogFile> availableLogFiles = new();
        
        [ObservableProperty]
        private ObservableCollection<PacketData> packets = new();
        
        [ObservableProperty]
        private ObservableCollection<PacketData> botPackets = new();
        
        [ObservableProperty]
        private ObservableCollection<PacketData> unmatchedPackets = new();
        
        [ObservableProperty]
        private PacketData? selectedPacket;
        
        [ObservableProperty]
        private LogFile? selectedLogFile;
        
        [ObservableProperty]
        private string statusMessage = "Ready";
        
        [ObservableProperty]
        private bool isLoading = false;
        
        [ObservableProperty]
        private int selectedTabIndex = 0; // 0=Hex, 1=Structure
        
        [ObservableProperty]
        private int packetListTabIndex = 0; // 0=Server, 1=Bot, 2=Unmatched, 3=ServerLog

        [ObservableProperty]
        private string serverLogContent = "";

        private string _originalServerLogContent = "";

        [ObservableProperty]
        private string filterText = "";

        public MainViewModel()
        {
            _dockerService = new DockerLogService();
            _packetParser = new PacketParserService();
            
            // Auto-load available log files on startup
            _ = LoadAvailableLogFiles();
        }
        
        [RelayCommand]
        private async Task LoadAvailableLogFiles()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading available log files...";
                
                var logFiles = await _dockerService.GetAvailableLogFilesAsync();
                AvailableLogFiles.Clear();
                
                foreach (var logFile in logFiles)
                {
                    AvailableLogFiles.Add(logFile);
                }
                
                StatusMessage = $"Found {logFiles.Count} log files";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading log files: {ex.Message}";
                MessageBox.Show($"Failed to load log files:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        
        [RelayCommand]
        private async Task LoadSelectedLogFile()
        {
            if (SelectedLogFile == null) return;

            try
            {
                IsLoading = true;
                StatusMessage = $"Loading {SelectedLogFile.Name}...";

                var logContent = await _dockerService.ReadLogFileAsync(SelectedLogFile.Path);

                // Store original content and load raw server log content for the Server Log tab
                _originalServerLogContent = logContent;
                ServerLogContent = logContent;

                // Parse packets for the packet tabs
                var parsedPackets = _packetParser.ParseLogContent(logContent);

                Packets.Clear();
                foreach (var packet in parsedPackets)
                {
                    Packets.Add(packet);
                }

                // Count OP_ClientUpdate packets for debugging
                var clientUpdateCount = parsedPackets.Count(p => p.OpcodeName == "OP_ClientUpdate");
                var totalLinesWithClientUpdate = logContent.Split('\n').Count(line => line.Contains("OP_ClientUpdate"));

                StatusMessage = $"Loaded {parsedPackets.Count} packets from {SelectedLogFile.Name} (OP_ClientUpdate: {clientUpdateCount} parsed, {totalLinesWithClientUpdate} total in log)";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading log file: {ex.Message}";
                MessageBox.Show($"Failed to load log file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        
        [RelayCommand]
        private async Task LoadBotLogFile()
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Log files (*.txt;*.log)|*.txt;*.log|All files (*.*)|*.*",
                    Title = "Select Bot Log File"
                };
                
                if (dialog.ShowDialog() == true)
                {
                    IsLoading = true;
                    StatusMessage = "Loading bot log file...";
                    
                    var logContent = await File.ReadAllTextAsync(dialog.FileName);
                    var parsedPackets = _packetParser.ParseLogContent(logContent);
                    
                    BotPackets.Clear();
                    foreach (var packet in parsedPackets)
                    {
                        packet.Source = "Bot"; // Mark as bot packets
                        BotPackets.Add(packet);
                    }
                    
                    StatusMessage = $"Loaded {parsedPackets.Count} bot packets";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading bot log file: {ex.Message}";
                MessageBox.Show($"Failed to load bot log file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        
        [RelayCommand]
        private void ComparePackets()
        {
            try
            {
                if (!Packets.Any() || !BotPackets.Any())
                {
                    MessageBox.Show("Please load both server and bot log files first.", "Compare Packets", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                StatusMessage = "Comparing packets...";
                
                var unmatched = _packetParser.FindMatchingPackets(Packets.ToList(), BotPackets.ToList());
                
                UnmatchedPackets.Clear();
                foreach (var packet in unmatched)
                {
                    UnmatchedPackets.Add(packet);
                }
                
                var matchedCount = Packets.Count(p => p.IsMatched) + BotPackets.Count(p => p.IsMatched);
                StatusMessage = $"Comparison complete: {matchedCount} matched, {unmatched.Count} unmatched";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error comparing packets: {ex.Message}";
                MessageBox.Show($"Failed to compare packets:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        [RelayCommand]
        private void FilterPackets(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                StatusMessage = "Filter cleared";
                return;
            }

            // Apply filter based on current tab
            if (PacketListTabIndex == 3) // Server Log tab
            {
                FilterServerLog(filter);
            }
            else
            {
                // Filter packets in the current packet list
                FilterPacketList(filter);
            }
        }

        private void FilterServerLog(string filter)
        {
            if (SelectedLogFile == null)
            {
                StatusMessage = "No log file loaded";
                return;
            }

            try
            {
                // Get the full log content and filter lines containing the search term
                var allLines = ServerLogContent.Split('\n');
                var filteredLines = allLines.Where(line =>
                    line.Contains(filter, StringComparison.OrdinalIgnoreCase));

                ServerLogContent = string.Join('\n', filteredLines);

                StatusMessage = $"Server log filtered by: {filter} - {filteredLines.Count()} matching lines";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error filtering server log: {ex.Message}";
            }
        }

        private void FilterPacketList(string filter)
        {
            // Basic packet filtering - you can enhance this later
            StatusMessage = $"Packet filtering by: {filter} (not yet implemented)";
        }

        [RelayCommand]
        private void ClearServerLogFilter()
        {
            if (!string.IsNullOrEmpty(_originalServerLogContent))
            {
                // Restore the original log content
                ServerLogContent = _originalServerLogContent;
                FilterText = "";
                StatusMessage = "Server log filter cleared";
            }
            else
            {
                StatusMessage = "No log file loaded to clear filter";
            }
        }
        
        partial void OnSelectedLogFileChanged(LogFile? value)
        {
            // Auto-load the selected log file
            if (value != null)
            {
                _ = LoadSelectedLogFile();
            }
        }
        
        partial void OnSelectedPacketChanged(PacketData? value)
        {
            // When a packet is selected, we might want to auto-switch to the structure tab
            // or perform other actions
        }
        
        public void Cleanup()
        {
            _dockerService?.Dispose();
        }
    }
}