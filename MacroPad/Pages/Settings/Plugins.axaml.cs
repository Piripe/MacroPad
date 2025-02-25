using Avalonia;
using Avalonia.Controls;
using MacroPad.Controls.Settings;
using MacroPad.Core;
using MacroPad.Core.Models.Plugin;
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
        foreach (PluginInfos plugin in PluginManager.Plugins)
        {
            PluginsList.Children.Add(new PluginCard() { Plugin=plugin, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch});
        }
    }
}