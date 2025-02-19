using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Components;
using Newtonsoft.Json.Linq;

namespace MacroPad.Core.BasePlugin.Variables
{
    public class Get : INodeGetter
    {
        public string Name => "Get Variable";

        public string Description => "Get the value of a variable";

        public string Id => "Get";

        public TypeNamePair[] Inputs => [];

        public TypeNamePair[] Outputs => [new(typeof(object), "")];

        public INodeComponent[] Components => [ new TextBox() {
            GetText = (IResourceManager resource) => resource.GetData<string>("v") ?? "",
            TextChanged = (IResourceManager resource, string text) => resource.SetData("v", text),
        } ];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            string var = resource.GetData<string>("v") ?? "";
            return [(DeviceManager.Config.Variables.TryGetValue(var, out JToken? value) ? value.Value<object>() : null) ?? ""];
        }
    }
}
