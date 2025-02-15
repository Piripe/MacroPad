using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MacroPad.Controls.Settings;
using MacroPad.Core;
using MacroPad.Core.Device;
using MacroPad.Shared.Plugin;

namespace MacroPad.Pages.Settings;

public partial class Devices : UserControl
{
    public Devices()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        DevicesList.Children.Clear();
        foreach (DeviceCore device in DeviceManager.ConnectedDevices)
        {
            DevicesList.Children.Add(new DeviceCard() { Device = device, Margin = new Thickness(4), Width=400, Height=136});
        }
    }
}