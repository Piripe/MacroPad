using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MacroPad.Shared.Plugin;
using MacroPad.Core.BasePlugin;

namespace MacroPad.Controls.Settings;

public partial class PluginCard : UserControl
{
    public IPluginInfos Plugin { get; set; } = new BasePluginInfos();
    public PluginCard()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        PluginName.Text = Plugin.Name;
        PluginDescription.Text = Plugin.Description;

        PluginFeatures.Children.Clear();
        foreach (IProtocol protocol in Plugin.Protocols)
        {
            PluginFeatures.Children.Add(new PluginFeature() { FeatureName = protocol.Name, FeatureId = protocol.Id });
        }
        foreach (INodeCategory nodeCategory in Plugin.NodeCategories)
        {
            PluginFeatures.Children.Add(new PluginFeature() { FeatureName = nodeCategory.Name, FeatureId = nodeCategory.Id, Disabled = true });
        }
        foreach (NodeType nodeType in Plugin.NodeTypes)
        {
            PluginFeatures.Children.Add(new PluginFeature() { FeatureName = nodeType.Name, Disabled = true });
        }
    }
}