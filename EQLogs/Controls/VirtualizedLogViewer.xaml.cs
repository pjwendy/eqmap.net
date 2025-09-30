using EQLogs.ViewModels;
using EQLogs.Models;
using System.Windows.Controls;

namespace EQLogs.Controls
{
    /// <summary>
    /// Interaction logic for VirtualizedLogViewer.xaml
    /// </summary>
    public partial class VirtualizedLogViewer : UserControl
    {
        public VirtualizedLogViewer()
        {
            InitializeComponent();
        }

        private void PacketScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (DataContext is VirtualizedLogViewModel viewModel && e.VerticalChange != 0)
            {
                // Calculate approximate packet index based on scroll position
                var scrollViewer = (ScrollViewer)sender;
                var scrollRatio = scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight;
                var estimatedIndex = (int)(scrollRatio * viewModel.TotalPacketCount);

                // Trigger loading of packets at this position
                _ = viewModel.ScrollToPositionCommand.ExecuteAsync(estimatedIndex);
            }
        }

        private void DirectionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is VirtualizedLogViewModel viewModel && sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var tag = selectedItem.Tag?.ToString();
                viewModel.DirectionFilter = tag switch
                {
                    "ClientToServer" => PacketDirection.ClientToServer,
                    "ServerToClient" => PacketDirection.ServerToClient,
                    _ => null
                };
            }
        }
    }
}