using Avalonia;
using Avalonia.Controls;
using MacroPad.Controls.Settings;
using MacroPad.Core;
using MacroPad.Shared.Plugin;
using MacroPad.ViewModels;

namespace MacroPad.Pages.Settings;

public partial class Plugins : UserControl
{
    public Plugins()
    {
        DataContext = new GeneralViewModel2();
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        PluginsList.Children.Clear();
        foreach (IPluginInfos plugin in PluginLoader.plugins)
        {
            PluginsList.Children.Add(new PluginCard() { Plugin=plugin, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch});
        }
    }
}