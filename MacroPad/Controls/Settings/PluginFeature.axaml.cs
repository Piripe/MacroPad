using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MacroPad.Core;
using MacroPad.Pages.Settings;
using MacroPad.Shared.Plugin;
using System.Diagnostics;
using System.Linq;

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
        EnableFeature.IsChecked = Disabled ? true : DeviceManager.Config.PluginsConfig.ContainsKey(FeatureId) ? DeviceManager.Config.PluginsConfig[FeatureId] : false;

        EnableFeature.IsCheckedChanged += EnableFeature_IsCheckedChanged;
    }

    private void EnableFeature_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (EnableFeature.IsChecked??false) DeviceManager.EnablePluginFeature(FeatureId);
        else DeviceManager.DisablePluginFeature(FeatureId);
    }
}