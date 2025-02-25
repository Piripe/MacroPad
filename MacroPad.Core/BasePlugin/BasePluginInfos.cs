using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Settings;

namespace MacroPad.Core.BasePlugin
{
    public class BasePluginInfos : IPluginInfos
    {
        public IProtocol[] Protocols => [];

        public INodeCategory[] NodeCategories => [new BranchingCategory(), new ButtonCategory(), new ConditionsCategory(), new ConstantsCategory(), new DebugCategory(), new MathCategory(), new ProfileCategory(), new TextCategory(), new VariableCategory()];

        public NodeType[] NodeTypes => [.. DefaultTypes.types];
        public ISettingsComponent[] Settings => [];
    }
}