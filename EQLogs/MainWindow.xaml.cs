using EQLogs.Models;
using EQLogs.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EQLogs;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
        DataContext = _viewModel;
    }

    protected override void OnClosed(EventArgs e)
    {
        _viewModel?.Cleanup();
        base.OnClosed(e);
    }

    private void PacketList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBox listBox && listBox.SelectedItem is PacketData selectedPacket)
        {
            FindPacketInServerLog(selectedPacket);
        }
    }

    private void FindPacketInServerLog(PacketData packet)
    {
        try
        {
            // Switch to Server Log tab (index 3)
            _viewModel.PacketListTabIndex = 3;

            // Get the server log content
            var serverLogContent = _viewModel.ServerLogContent;
            if (string.IsNullOrEmpty(serverLogContent))
            {
                _viewModel.StatusMessage = "No server log content available";
                return;
            }

            // Create search patterns for the packet based on timestamp and opcode
            var timestampFormats = new[]
            {
                packet.Timestamp.ToString("MM-dd-yyyy HH:mm:ss"),
                packet.Timestamp.ToString("dd-MM-yyyy HH:mm:ss"),
                packet.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                packet.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")
            };

            int foundPosition = -1;
            string foundLine = null;

            // Try to find the packet using different timestamp formats
            foreach (var timestampFormat in timestampFormats)
            {
                // Look for lines containing both timestamp and opcode
                var searchPattern = $@".*{Regex.Escape(timestampFormat)}.*{Regex.Escape(packet.OpcodeName)}.*";
                var matches = Regex.Matches(serverLogContent, searchPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                if (matches.Count > 0)
                {
                    // If multiple matches, try to find the one with the exact direction
                    Match bestMatch = null;
                    foreach (Match match in matches)
                    {
                        var line = match.Value;
                        // Check if the direction matches
                        if (packet.Direction == PacketDirection.ClientToServer && line.Contains("C->S"))
                        {
                            bestMatch = match;
                            break;
                        }
                        else if (packet.Direction == PacketDirection.ServerToClient && line.Contains("S->C"))
                        {
                            bestMatch = match;
                            break;
                        }

                        // If no direction-specific match found, use the first one
                        if (bestMatch == null)
                            bestMatch = match;
                    }

                    if (bestMatch != null)
                    {
                        foundPosition = bestMatch.Index;
                        foundLine = bestMatch.Value;
                        break;
                    }
                }
            }

            if (foundPosition >= 0)
            {
                // Position found, now set the selection in the TextBox
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        ServerLogTextBox.Focus();
                        ServerLogTextBox.CaretIndex = foundPosition;

                        // Select the entire line for better visibility
                        var lineStart = foundPosition;
                        var lineEnd = serverLogContent.IndexOf('\n', foundPosition);
                        if (lineEnd == -1) lineEnd = serverLogContent.Length;

                        ServerLogTextBox.SelectionStart = lineStart;
                        ServerLogTextBox.SelectionLength = lineEnd - lineStart;

                        // Scroll to make the selection visible
                        ServerLogTextBox.ScrollToLine(GetLineNumber(serverLogContent, foundPosition));

                        _viewModel.StatusMessage = $"Found packet at position {foundPosition} in server log";
                    }
                    catch (Exception ex)
                    {
                        _viewModel.StatusMessage = $"Error positioning in server log: {ex.Message}";
                    }
                }));
            }
            else
            {
                _viewModel.StatusMessage = $"Could not find packet {packet.OpcodeName} at {packet.Timestamp:HH:mm:ss.fff} in server log";
            }
        }
        catch (Exception ex)
        {
            _viewModel.StatusMessage = $"Error searching server log: {ex.Message}";
        }
    }

    private int GetLineNumber(string text, int position)
    {
        if (position >= text.Length) return 0;

        int lineNumber = 1;
        for (int i = 0; i < position && i < text.Length; i++)
        {
            if (text[i] == '\n')
                lineNumber++;
        }
        return lineNumber - 1; // TextBox uses 0-based line numbering
    }

    private void EQMapEventList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBox listBox && listBox.SelectedItem is EQMapLogEntry selectedEntry)
        {
            ShowEQMapEntryDetails(selectedEntry);
        }
    }

    private void ShowEQMapEntryDetails(EQMapLogEntry entry)
    {
        try
        {
            // Create a detailed view window for the EQMap log entry
            var detailWindow = new Window
            {
                Title = $"EQMap Log Entry Details - {entry.PacketName ?? "Log Entry"}",
                Width = 800,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            var tabControl = new TabControl();

            // Raw Log Tab
            var rawLogTab = new TabItem { Header = "Raw Log" };
            var rawLogTextBox = new TextBox
            {
                Text = entry.RawLogText,
                IsReadOnly = true,
                FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                FontSize = 10,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            rawLogTab.Content = new ScrollViewer { Content = rawLogTextBox };
            tabControl.Items.Add(rawLogTab);

            // Hex Dump Tab (if available)
            if (entry.HasHexDump)
            {
                var hexTab = new TabItem { Header = "Hex Dump" };
                var hexTextBox = new TextBox
                {
                    Text = entry.HexDump,
                    IsReadOnly = true,
                    FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                    FontSize = 10,
                    TextWrapping = TextWrapping.NoWrap,
                    AcceptsReturn = true,
                    Background = System.Windows.Media.Brushes.Black,
                    Foreground = System.Windows.Media.Brushes.Lime,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
                };
                hexTab.Content = new ScrollViewer { Content = hexTextBox };
                tabControl.Items.Add(hexTab);
            }

            // Structure Tab (if available)
            if (entry.HasStructure)
            {
                var structTab = new TabItem { Header = "Structure" };
                var structTextBox = new TextBox
                {
                    Text = entry.StructureData,
                    IsReadOnly = true,
                    FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                    FontSize = 11,
                    TextWrapping = TextWrapping.Wrap,
                    AcceptsReturn = true,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
                };
                structTab.Content = new ScrollViewer { Content = structTextBox };
                tabControl.Items.Add(structTab);
            }

            // Info Tab
            var infoTab = new TabItem { Header = "Info" };
            var infoContent = $"Timestamp: {entry.Timestamp:yyyy-MM-dd HH:mm:ss.ffff}\n" +
                             $"Log Level: {entry.LogLevel}\n" +
                             $"Source: {entry.Source}\n" +
                             $"Message: {entry.Message}\n";

            if (entry.IsPacketEntry)
            {
                infoContent += $"\nPacket Information:\n" +
                              $"  Name: {entry.PacketName}\n" +
                              $"  OpCode: {entry.OpCode}\n" +
                              $"  Size: {entry.PacketSize} bytes\n" +
                              $"  Direction: {entry.Direction}\n" +
                              $"  Stream: {entry.StreamType}\n";
            }

            var infoTextBox = new TextBox
            {
                Text = infoContent,
                IsReadOnly = true,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            infoTab.Content = new ScrollViewer { Content = infoTextBox };
            tabControl.Items.Add(infoTab);

            detailWindow.Content = tabControl;
            detailWindow.Show();

            _viewModel.StatusMessage = $"Opened details for {entry.PacketName ?? "log entry"} at {entry.Timestamp:HH:mm:ss.fff}";
        }
        catch (Exception ex)
        {
            _viewModel.StatusMessage = $"Error showing entry details: {ex.Message}";
            MessageBox.Show($"Failed to show entry details:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}