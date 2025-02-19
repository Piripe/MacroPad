using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Branching
{
    public class SetPaletteColor : INodeRunner
    {
        public string Name => "Run Branch";

        public string Description => "Run one of the two outputs depending on the boolean input.";

        public string Id => "RunBranch";

        public TypeNamePair[] Inputs => [new(typeof(bool), "")];

        public TypeNamePair[] Outputs => [];

        public int RunnerOutputCount => 2;
        public string[] RunnerOutputsName => ["False", "True"];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public NodeRunnerResult Run(IResourceManager resource)
        {
            return new NodeRunnerResult() { Results = [], RunnerOutputIndex = (bool)resource.GetValue(0) ? 1 : 0 };
        }
    }
}
