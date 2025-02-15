using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HarfBuzzSharp;
using MacroPad.Core;
using MacroPad.Core.BasePlugin.Device;
using MacroPad.Core.Device;
using MacroPad.Pages.Settings;
using MacroPad.Shared.Plugin;

namespace MacroPad.Controls.Settings;

public partial class DeviceCard : UserControl
{
    public DeviceCore Device { get; set; } = new DeviceCore(new BaseDevice());
    public DeviceCard()
    {
        InitializeComponent();

        EnableDevice.IsCheckedChanged += EnableDevice_IsCheckedChanged;
        DeviceIcon.ZoomAndPan.CornerRadius = new CornerRadius(4);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        DeviceName.Text = Device.Name;
        EnableDevice.IsChecked = Device.ProtocolDevice.IsConnected;
        DeviceIcon.Device = Device;
    }

    private void EnableDevice_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Device != null) {
            if (EnableDevice.IsChecked ?? false) Device.Connect();
            else Device.Disconnect();
            DeviceManager.Config.EnabledDevices[Device.ProtocolDevice.Id] = EnableDevice.IsChecked ?? false;
        }
    }
}