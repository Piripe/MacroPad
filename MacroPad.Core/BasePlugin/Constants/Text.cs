using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Components;

namespace MacroPad.Core.BasePlugin.Constants
{
    internal class Text : INodeGetter
    {
        public string Name => "Text";

        public string Description => "A text constant.";

        public string Id => "Text";

        public TypeNamePair[] Inputs => [];

        public TypeNamePair[] Outputs => [new(typeof(string), "")];

        public INodeComponent[] Components => [ new TextBox() {
            GetText = (IResourceManager resource) => resource.GetData<string>("v") ?? "",
            TextChanged = (IResourceManager resource, string text) => resource.SetData("v", text),
        } ];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [resource.GetData<string>("v") ?? ""];
        }
    }
}
