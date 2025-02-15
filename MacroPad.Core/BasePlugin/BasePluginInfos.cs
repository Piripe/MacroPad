using MacroPad.Shared.Plugin;

namespace MacroPad.Core.BasePlugin
{
    public class BasePluginInfos : IPluginInfos
    {
        public string Name => "Base Plugin";

        public string Description => "The usefulest plugin of MacroPad.";

        public string Version => "1.0.0";

        public string Author => "Piripe";

        public string? AuthorUrl => null;

        public string? SourceUrl => null;

        public IProtocol[] Protocols => new IProtocol[0];

        public INodeCategory[] NodeCategories => new INodeCategory[] { new BranchingCategory(), new ConditionsCategory() ,new ConstantsCategory(), new DebugCategory(), new TextCategory()};

        public NodeType[] NodeTypes => DefaultTypes.types.ToArray();
    }
}