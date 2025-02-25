using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Debug
{
    public class WriteLine : INodeRunner
    {
        public string Name => "Write Line";

        public string Description => "Write a line in the console.";

        public string Id => "WriteLine";

        public TypeNamePair[] Inputs => [new(typeof(string), "Text")];

        public TypeNamePair[] Outputs => [];

        public int RunnerOutputCount => 1;
        public string[] RunnerOutputsName => [];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public NodeRunnerResult Run(INodeResourceManager resource)
        {
            System.Diagnostics.Debug.WriteLine(resource.GetValue(0));
            return new NodeRunnerResult() { Results = [], RunnerOutputIndex = 0 };
        }
    }
}
