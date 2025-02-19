using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Components;

namespace MacroPad.Core.BasePlugin.Constants
{
    internal class Boolean : INodeGetter
    {
        public string Name => "Boolean";

        public string Description => "A boolean constant.";

        public string Id => "Boolean";
        
        public TypeNamePair[] Inputs => [];

        public TypeNamePair[] Outputs => [new(typeof(bool), "")];

        public INodeComponent[] Components => [ new ComboBox() {
            Items = ["False","True"],
            GetSelection = (IResourceManager resource) => resource.GetData<int>("v"),
            SelectionChanged = (IResourceManager resource, int value) => resource.SetData("v", value),
        } ];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [resource.GetData<bool?>("v") ?? false];
        }
    }
}
