using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.Core;
using MacroPad.Controls;
using MacroPad.Controls.Home;
using MacroPad.Core;
using MacroPad.Core.Config;
using MacroPad.Core.Device;
using System.Collections.Generic;
using System.Linq;

namespace MacroPad.Pages;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();

        DeviceManager.DeviceDetected += DeviceManager_DeviceDetected;
        DeviceManager.DeviceDisconnected += DeviceManager_DeviceDetected;
        DeviceSelector.SelectionChanged += DeviceSelector_SelectionChanged;
        RefreshDeviceSelector();
    }

    private void DeviceManager_DeviceDetected(object? sender, Shared.Plugin.Protocol.DeviceDetectedEventArgs e)
    {
        RefreshDeviceSelector();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        RefreshDeviceSelector();
    }

    private void RefreshDeviceSelector()
    {
        object? selectedDevice = DeviceSelector.SelectedItem;
        DeviceSelector.ItemsSource = DeviceManager.ConnectedDevices.Where(x => x.ProtocolDevice.IsConnected);
        if (DeviceSelector.ItemsSource.Count() > 0)
        {
            if (DeviceSelector.Items.Contains(selectedDevice)) DeviceSelector.SelectedItem = selectedDevice;
            else DeviceSelector.SelectedIndex = 0;
        }
    }

    private void DeviceSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        DeviceViewerContainer.Children.Clear();
        DeviceProfileSelectorContainer.Children.Clear();
        NodesEditorContainer.Children.Clear();
        ButtonStatusEditorContainer.Children.Clear();
        if (DeviceSelector.SelectedItem is DeviceCore selectedDevice)
        {
            DeviceManager.SelectedDevice = selectedDevice;
            var deviceViewer = new DeviceViewer() { Device = selectedDevice };
            deviceViewer.ButtonPressed += DeviceViewer_ButtonPressed;
            DeviceViewerContainer.Children.Add(deviceViewer);
            DeviceProfileSelectorContainer.Children.Add(new DeviceProfileSelector() { Device = selectedDevice, DeviceViewer=deviceViewer, MainPage=this });
        }
    }

    private void DeviceViewer_ButtonPressed(object? sender, DeviceViewerButtonPressedEventArgs e)
    {
        NodesEditorContainer.Children.Clear();
        ButtonStatusEditorContainer.Children.Clear();
        if (DeviceSelector.SelectedItem is DeviceCore selectedDevice && selectedDevice.CurrentProfile != null && e.Button != null)
        {
            if (!selectedDevice.CurrentProfile.ButtonsConfig.ContainsKey(e.Button.X)) selectedDevice.CurrentProfile.ButtonsConfig.Add(e.Button.X, []);
            Dictionary<int, ButtonConfig> buttonColumn = selectedDevice.CurrentProfile.ButtonsConfig[e.Button.X];
            if (!buttonColumn.ContainsKey(e.Button.Y)) buttonColumn.Add(e.Button.Y, new ButtonConfig());
            ButtonConfig buttonConfig = buttonColumn[e.Button.Y];
            NodesEditorContainer.Children.Add(new NodesEditor() { ButtonConfig = buttonConfig, Button=e.Button, Device = selectedDevice });
            ButtonStatusEditorContainer.Children.Add(new StatusEditor() { ButtonConfig = buttonConfig, Button = e.Button, Device = selectedDevice});
        }
    }
}