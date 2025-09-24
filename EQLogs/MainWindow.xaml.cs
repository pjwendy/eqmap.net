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
}