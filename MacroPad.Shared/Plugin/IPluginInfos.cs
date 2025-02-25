using MacroPad.Shared.Plugin.Settings;

namespace MacroPad.Shared.Plugin
{
    public interface IPluginInfos
    {
        public IProtocol[] Protocols { get; }
        public INodeCategory[] NodeCategories { get; }
        public NodeType[] NodeTypes { get; }
        public ISettingsComponent[] Settings { get; }
    }
}
