using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQLogs.Models;
using EQLogs.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EQLogs.ViewModels
{
    /// <summary>
    /// ViewModel for virtualized log display - handles large log files efficiently
    /// </summary>
    public partial class VirtualizedLogViewModel : ObservableObject, IDisposable
    {
        private readonly ChunkedLogReaderService _chunkedReader;
        private readonly IDockerLogService _dockerService;

        // Virtual scrolling parameters
        private const int PageSize = 100; // Number of packets to load per page
        private const int ViewportBuffer = 50; // Extra packets to load around viewport

        // Current state
        private LogFileInfo? _currentFile;
        private int _currentScrollIndex = 0;
        private int _totalPacketCount = 0;
        private CancellationTokenSource? _loadCancellation;

        [ObservableProperty]
        private ObservableCollection<PacketData> _displayedPackets = new();

        [ObservableProperty]
        private ObservableCollection<LogFileInfo> _availableLogFiles = new();

        [ObservableProperty]
        private LogFileInfo? _selectedLogFile;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _loadingStatus = "";

        [ObservableProperty]
        private int _scrollPosition = 0;

        [ObservableProperty]
        private int _scrollMaximum = 0;

        // Search functionality
        [ObservableProperty]
        private string _searchText = "";

        [ObservableProperty]
        private ObservableCollection<PacketSearchResult> _searchResults = new();

        [ObservableProperty]
        private bool _isSearching;

        [ObservableProperty]
        private PacketSearchResult? _selectedSearchResult;

        [ObservableProperty]
        private PacketData? _selectedPacket;

        // Filter options
        [ObservableProperty]
        private string _opcodeFilter = "";

        [ObservableProperty]
        private PacketDirection? _directionFilter;

        public VirtualizedLogViewModel(ChunkedLogReaderService chunkedReader, IDockerLogService dockerService)
        {
            _chunkedReader = chunkedReader;
            _dockerService = dockerService;
        }

        [RelayCommand]
        private async Task LoadLogFiles()
        {
            try
            {
                IsLoading = true;
                LoadingStatus = "Loading available log files...";

                System.Diagnostics.Debug.WriteLine("VirtualizedLogViewModel: Starting LoadLogFiles");
                var logFiles = await _dockerService.GetLogFilesAsync();
                System.Diagnostics.Debug.WriteLine($"VirtualizedLogViewModel: Got {logFiles.Count} log files from service");

                AvailableLogFiles.Clear();

                foreach (var file in logFiles.OrderByDescending(f => f.TotalSize))
                {
                    System.Diagnostics.Debug.WriteLine($"VirtualizedLogViewModel: Adding file {file.FilePath} ({file.TotalSize} bytes)");
                    AvailableLogFiles.Add(file);
                }

                LoadingStatus = $"Found {logFiles.Count} log files";
                System.Diagnostics.Debug.WriteLine($"VirtualizedLogViewModel: LoadLogFiles completed with {AvailableLogFiles.Count} files in collection");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"VirtualizedLogViewModel: LoadLogFiles error: {ex}");
                MessageBox.Show($"Error loading log files: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SelectLogFile()
        {
            System.Diagnostics.Debug.WriteLine($"VirtualizedLogViewModel: SelectLogFile called, SelectedLogFile = {SelectedLogFile?.FilePath}");

            if (SelectedLogFile == null)
            {
                System.Diagnostics.Debug.WriteLine("VirtualizedLogViewModel: SelectLogFile - SelectedLogFile is null, returning");
                return;
            }

            try
            {
                IsLoading = true;
                LoadingStatus = "Initializing log file...";
                System.Diagnostics.Debug.WriteLine($"VirtualizedLogViewModel: Starting to load file: {SelectedLogFile.FilePath}");

                _loadCancellation?.Cancel();
                _loadCancellation = new CancellationTokenSource();

                _currentFile = await _chunkedReader.InitializeAsync(
                    SelectedLogFile.FilePath,
                    _loadCancellation.Token);

                System.Diagnostics.Debug.WriteLine($"VirtualizedLogViewModel: ChunkedReader initialized, estimated packet count: {_currentFile.EstimatedPacketCount}");

                _totalPacketCount = _currentFile.EstimatedPacketCount;
                ScrollMaximum = Math.Max(0, _totalPacketCount - PageSize);
                ScrollPosition = 0;
                _currentScrollIndex = 0;

                LoadingStatus = $"Loading packets from {_currentFile.FilePath}...";

                // Load initial page
                System.Diagnostics.Debug.WriteLine("VirtualizedLogViewModel: Loading initial packet page");
                await LoadPacketsAtIndex(0);

                System.Diagnostics.Debug.WriteLine($"VirtualizedLogViewModel: Loaded {DisplayedPackets.Count} packets");
                LoadingStatus = $"Loaded log file with ~{_totalPacketCount:N0} packets ({_currentFile.TotalSize:N0} bytes)";
            }
            catch (OperationCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("VirtualizedLogViewModel: SelectLogFile cancelled");
                LoadingStatus = "Load cancelled";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"VirtualizedLogViewModel: SelectLogFile error: {ex}");
                MessageBox.Show($"Error loading log file: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                LoadingStatus = "Error loading file";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ScrollToPosition(int position)
        {
            if (_currentFile == null || IsLoading) return;

            try
            {
                var targetIndex = Math.Max(0, Math.Min(position, _totalPacketCount - PageSize));

                // Only reload if we've moved significantly
                if (Math.Abs(targetIndex - _currentScrollIndex) > ViewportBuffer)
                {
                    await LoadPacketsAtIndex(targetIndex);
                }

                ScrollPosition = position;
            }
            catch (Exception ex)
            {
                LoadingStatus = $"Error scrolling: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task Search()
        {
            if (_currentFile == null || string.IsNullOrWhiteSpace(SearchText)) return;

            try
            {
                IsSearching = true;
                LoadingStatus = $"Searching for '{SearchText}'...";

                _loadCancellation?.Cancel();
                _loadCancellation = new CancellationTokenSource();

                var criteria = new PacketSearchCriteria
                {
                    OpcodeName = SearchText.Trim()
                };

                var results = await _chunkedReader.SearchAsync(criteria, _loadCancellation.Token);

                SearchResults.Clear();
                foreach (var result in results.Take(1000)) // Limit results for performance
                {
                    SearchResults.Add(result);
                }

                LoadingStatus = $"Found {results.Count} matches for '{SearchText}'";
            }
            catch (OperationCanceledException)
            {
                LoadingStatus = "Search cancelled";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                LoadingStatus = "Search error";
            }
            finally
            {
                IsSearching = false;
            }
        }

        [RelayCommand]
        private async Task JumpToSearchResult(PacketSearchResult searchResult)
        {
            if (_currentFile == null || searchResult == null) return;

            try
            {
                // Jump to the packet index
                await LoadPacketsAtIndex(searchResult.PacketIndex);
                ScrollPosition = searchResult.PacketIndex;

                // Highlight the found packet (you could extend PacketData with IsHighlighted property)
                LoadingStatus = $"Jumped to packet at index {searchResult.PacketIndex}";
            }
            catch (Exception ex)
            {
                LoadingStatus = $"Error jumping to result: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task ApplyFilters()
        {
            if (_currentFile == null) return;

            try
            {
                IsLoading = true;
                LoadingStatus = "Applying filters...";

                var criteria = new PacketSearchCriteria
                {
                    OpcodeName = string.IsNullOrWhiteSpace(OpcodeFilter) ? null : OpcodeFilter.Trim(),
                    Direction = DirectionFilter
                };

                if (criteria.OpcodeName != null || criteria.Direction.HasValue)
                {
                    // Apply search with filters
                    var results = await _chunkedReader.SearchAsync(criteria, CancellationToken.None);

                    DisplayedPackets.Clear();
                    foreach (var result in results.Take(PageSize))
                    {
                        DisplayedPackets.Add(result.Packet);
                    }

                    LoadingStatus = $"Filtered to {results.Count} packets";
                }
                else
                {
                    // No filters, reload current view
                    await LoadPacketsAtIndex(_currentScrollIndex);
                    LoadingStatus = "Filters cleared";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filters: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                LoadingStatus = "Filter error";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void ClearFilters()
        {
            OpcodeFilter = "";
            DirectionFilter = null;
            SearchText = "";
            SearchResults.Clear();
        }

        private async Task LoadPacketsAtIndex(int startIndex)
        {
            if (_currentFile == null) return;

            try
            {
                _loadCancellation?.Cancel();
                _loadCancellation = new CancellationTokenSource();

                var result = await _chunkedReader.GetPacketRangeAsync(
                    startIndex,
                    PageSize + ViewportBuffer,
                    _loadCancellation.Token);

                // Update total count if we got more accurate information
                if (result.IsComplete && result.TotalCount != _totalPacketCount)
                {
                    _totalPacketCount = result.TotalCount;
                    ScrollMaximum = Math.Max(0, _totalPacketCount - PageSize);
                }

                DisplayedPackets.Clear();
                foreach (var packet in result.Packets)
                {
                    DisplayedPackets.Add(packet);
                }

                _currentScrollIndex = startIndex;
            }
            catch (OperationCanceledException)
            {
                // Expected when switching views
            }
        }

        /// <summary>
        /// Get packet count for UI display
        /// </summary>
        public string PacketCountDisplay
        {
            get
            {
                if (_currentFile == null)
                    return "No file loaded";

                var displayed = DisplayedPackets.Count;
                var total = _totalPacketCount;

                return $"Showing {displayed:N0} of ~{total:N0} packets";
            }
        }

        /// <summary>
        /// Get file size for UI display
        /// </summary>
        public string FileSizeDisplay
        {
            get
            {
                if (_currentFile == null)
                    return "";

                var bytes = _currentFile.TotalSize;
                string[] sizes = { "B", "KB", "MB", "GB" };
                double len = bytes;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }

                return $"{len:0.##} {sizes[order]}";
            }
        }

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            // Update computed properties when relevant properties change
            if (e.PropertyName == nameof(CurrentFile) ||
                e.PropertyName == nameof(DisplayedPackets) ||
                e.PropertyName == nameof(TotalPacketCount))
            {
                OnPropertyChanged(nameof(PacketCountDisplay));
                OnPropertyChanged(nameof(FileSizeDisplay));
            }
        }

        public void Dispose()
        {
            _loadCancellation?.Cancel();
            _loadCancellation?.Dispose();
            _chunkedReader?.Dispose();
        }

        // Properties for computed values
        public LogFileInfo? CurrentFile
        {
            get => _currentFile;
            private set
            {
                _currentFile = value;
                OnPropertyChanged();
            }
        }

        public int TotalPacketCount
        {
            get => _totalPacketCount;
            private set
            {
                _totalPacketCount = value;
                OnPropertyChanged();
            }
        }
    }
}