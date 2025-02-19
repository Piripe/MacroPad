using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Components;
using Newtonsoft.Json.Linq;

namespace MacroPad.Core.BasePlugin.Variables
{
    public class Set : INodeRunner
    {
        public string Name => "Set Variable";

        public string Description => "Set the value of a variable";

        public string Id => "Set";

        public TypeNamePair[] Inputs => [new(typeof(object), "")];

        public TypeNamePair[] Outputs => [];

        public INodeComponent[] Components => [ new TextBox() {
            GetText = (IResourceManager resource) => resource.GetData<string>("v") ?? "",
            TextChanged = (IResourceManager resource, string text) => resource.SetData("v", text),
        } ];

        public int RunnerOutputCount => 1;

        public string[] RunnerOutputsName => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;

        public NodeRunnerResult Run(IResourceManager resource)
        {
            string var = resource.GetData<string>("v") ?? "";
            JToken value = JToken.FromObject(resource.GetValue(0));
            if (!DeviceManager.Config.Variables.TryAdd(var, value)) DeviceManager.Config.Variables[var] = value;
            return new NodeRunnerResult() { Results = [], RunnerOutputIndex = 0 };
        }
    }
}
