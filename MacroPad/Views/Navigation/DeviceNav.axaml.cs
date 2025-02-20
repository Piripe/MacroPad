using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Markup.Xaml;

namespace MacroPad.Views.Navigation;

public partial class DeviceNav : UserControl
{
    public DeviceNav()
    {
        InitializeComponent();
        enableTextBlock.TextTrimming = new TextTrailingTrimming(" ", true);
        deviceViewer.ZoomAndPan.Background = Brushes.Transparent;
    }
}