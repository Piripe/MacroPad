using Avalonia;
using Avalonia.Controls;
using MacroPad.Core;

namespace MacroPad.Controls.Settings;

public partial class PluginFeature : UserControl
{
    public string FeatureName { get; set; } = "";
    public string FeatureId { get; set; } = "";
    public bool Disabled { get; set; } = false;
    public PluginFeature()
    {
        InitializeComponent();
    }
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        FeatureNameDisplay.Text = FeatureName;
        EnableFeature.IsEnabled = !Disabled;
        EnableFeature.IsChecked = Disabled || (DeviceManager.Config.PluginsConfig.TryGetValue(FeatureId, out bool value) && value);

        EnableFeature.IsCheckedChanged += EnableFeature_IsCheckedChanged;
    }

    private void EnableFeature_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (EnableFeature.IsChecked??false) DeviceManager.EnablePluginFeature(FeatureId);
        else DeviceManager.DisablePluginFeature(FeatureId);
    }
}