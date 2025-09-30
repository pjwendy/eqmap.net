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
        private readonly ChunkedLogReaderService _chunkedReader;

        // New virtualized log viewer
        public VirtualizedLogViewModel VirtualizedLogViewer { get; }
        
        [ObservableProperty]
        private ObservableCollection<LogFile> availableLogFiles = new();
        
        [ObservableProperty]
        private ObservableCollection<PacketData> packets = new();

        [ObservableProperty]
        private ObservableCollection<PacketData> botPackets = new();

        // Session filtering
        [ObservableProperty]
        private ObservableCollection<string> availableSessions = new();

        [ObservableProperty]
        private string? selectedSession;

        [ObservableProperty]
        private bool isSessionFilterEnabled;

        // Store original unfiltered data for session filtering
        private List<PacketData> _allPackets = new();

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
            _chunkedReader = new ChunkedLogReaderService(_dockerService, _packetParser);

            // Initialize virtualized log viewer
            VirtualizedLogViewer = new VirtualizedLogViewModel(_chunkedReader, _dockerService);

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

                // Store all packets for session filtering
                _allPackets.Clear();
                _allPackets.AddRange(parsedPackets);

                // Update available sessions
                UpdateAvailableSessions();

                // Apply current session filter
                Packets.Clear();
                ApplySessionFilter();

                // Count OP_ClientUpdate packets for debugging
                var clientUpdateCount = parsedPackets.Count(p => p.OpcodeName == "OP_ClientUpdate");
                var totalLinesWithClientUpdate = logContent.Split('\n').Count(line => line.Contains("OP_ClientUpdate"));

                StatusMessage = $"Loaded {parsedPackets.Count} packets from {SelectedLogFile.Name} (OP_ClientUpdate: {clientUpdateCount} parsed, {totalLinesWithClientUpdate} total in log)";

                // If we have sessions, show session info
                if (AvailableSessions.Any())
                {
                    StatusMessage += $" - {AvailableSessions.Count} unique sessions found";
                }
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

        [RelayCommand]
        private void ShowGenericParserDiagnostics()
        {
            try
            {
                var genericParser = new GenericPacketParserService();
                var diagnosticInfo = genericParser.GetDiagnosticInfo();

                MessageBox.Show(diagnosticInfo, "Generic Parser Diagnostics", MessageBoxButton.OK, MessageBoxImage.Information);
                StatusMessage = "Generic parser diagnostics shown";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error showing diagnostics: {ex.Message}";
                MessageBox.Show($"Failed to show diagnostics:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void TestSessionParsing()
        {
            try
            {
                PacketParserTest.RunBasicTest();

                // Also show current session filtering status
                var sessionDebugInfo = $"Current Session Filter Status:\n" +
                                     $"  Total packets loaded: {_allPackets.Count}\n" +
                                     $"  Available sessions: {AvailableSessions.Count}\n" +
                                     $"  Sessions: {string.Join(", ", AvailableSessions.Take(3))}\n" +
                                     $"  Filter enabled: {IsSessionFilterEnabled}\n" +
                                     $"  Selected session: '{SelectedSession}'\n" +
                                     $"  Displayed packets: {Packets.Count}";

                MessageBox.Show(sessionDebugInfo, "Session Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                StatusMessage = "Session parsing test and debug info shown";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error running session test: {ex.Message}";
                MessageBox.Show($"Failed to run session test:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void ToggleSessionFilter()
        {
            IsSessionFilterEnabled = !IsSessionFilterEnabled;

            if (!IsSessionFilterEnabled)
            {
                // When disabling filter, clear the selected session
                SelectedSession = null;
            }

            ApplySessionFilter();
            StatusMessage = IsSessionFilterEnabled
                ? $"Session filter enabled: {SelectedSession ?? "None selected"}"
                : "Session filter disabled";
        }

        [RelayCommand]
        private void ClearSessionFilter()
        {
            SelectedSession = null;
            IsSessionFilterEnabled = false;
            ApplySessionFilter();
            StatusMessage = "Session filter cleared";
        }

        private void ApplySessionFilter()
        {
            Packets.Clear();

            if (string.IsNullOrEmpty(SelectedSession))
            {
                // Show all packets when no session is selected
                foreach (var packet in _allPackets)
                {
                    Packets.Add(packet);
                }
            }
            else
            {
                // Extract session number from display format "Session 2 (bifar:Bifar) - 150 packets" -> 2
                var sessionNumber = ExtractSessionNumberFromDisplayName(SelectedSession);

                // Filter by selected session number only
                var filteredPackets = _allPackets.Where(p => p.SessionNumber == sessionNumber).ToList();
                foreach (var packet in filteredPackets)
                {
                    Packets.Add(packet);
                }

                System.Diagnostics.Debug.WriteLine($"Applied session filter: Session {sessionNumber}, showing {filteredPackets.Count} of {_allPackets.Count} packets");
            }
        }

        private int ExtractSessionNumberFromDisplayName(string displayName)
        {
            // Format is "Session 2 (bifar:Bifar) - 150 packets" -> extract 2
            // or "Session 2 - 150 packets" -> extract 2
            var match = System.Text.RegularExpressions.Regex.Match(displayName, @"Session (\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int sessionNumber))
            {
                return sessionNumber;
            }
            return 0; // Fallback if parsing fails
        }

        private void UpdateAvailableSessions()
        {
            AvailableSessions.Clear();

            // Get unique session numbers from packets (only those with session numbers > 0)
            var sessionNumbers = _allPackets
                .Where(p => p.SessionNumber > 0)
                .Select(p => p.SessionNumber)
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            // Add to available sessions list - simple format since we only filter by session number
            foreach (var sessionNumber in sessionNumbers)
            {
                // Count packets for this session
                var packetCount = _allPackets.Count(p => p.SessionNumber == sessionNumber);

                // Get sample account/player info for display (just first occurrence)
                var samplePacket = _allPackets.FirstOrDefault(p => p.SessionNumber == sessionNumber && !string.IsNullOrEmpty(p.AccountName));

                var displayName = $"Session {sessionNumber}";
                if (samplePacket != null)
                {
                    displayName += $" ({samplePacket.AccountName}";
                    if (!string.IsNullOrEmpty(samplePacket.PlayerName))
                    {
                        displayName += $":{samplePacket.PlayerName}";
                    }
                    displayName += $") - {packetCount} packets";
                }
                else
                {
                    displayName += $" - {packetCount} packets";
                }

                AvailableSessions.Add(displayName);
            }
        }

        partial void OnSelectedSessionChanged(string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                // Automatically enable filtering when a session is selected
                IsSessionFilterEnabled = true;
            }
            else if (string.IsNullOrEmpty(value) && IsSessionFilterEnabled)
            {
                // If session is cleared and filter was enabled, disable it
                IsSessionFilterEnabled = false;
            }

            // Apply the filter regardless of checkbox state when session changes
            ApplySessionFilter();

            // Update status message
            if (string.IsNullOrEmpty(value))
            {
                StatusMessage = "Showing all sessions";
            }
            else
            {
                var sessionNumber = ExtractSessionNumberFromDisplayName(value);
                var packetCount = _allPackets.Count(p => p.SessionNumber == sessionNumber);
                StatusMessage = $"Filtering by Session {sessionNumber} - {packetCount} packets";
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
            if (value != null)
            {
                // When a packet is selected, auto-refresh the structure data using the new generic parser
                // This ensures the Structure tab always shows the latest parsed data
                try
                {
                    if (string.IsNullOrEmpty(value.StructureData) || value.StructureData.Contains("Unknown packet structure"))
                    {
                        // Re-parse using the generic parser if structure data is missing or generic
                        var genericParser = new GenericPacketParserService();
                        value.StructureData = genericParser.ParsePacketStructure(value);

                        // Auto-switch to the structure tab to show the parsed result
                        SelectedTabIndex = 1;
                    }
                }
                catch (Exception ex)
                {
                    // Log error but don't crash the UI
                    System.Diagnostics.Debug.WriteLine($"Error re-parsing packet structure: {ex.Message}");
                }
            }
        }
        
        public void Cleanup()
        {
            _dockerService?.Dispose();
        }
    }
}