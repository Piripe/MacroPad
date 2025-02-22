using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Text
{
    public class Join : INodeGetter
    {
        public string Name => "Join Text";

        public string Description => "Concatenate two texts together.";

        public string Id => "Join";

        public TypeNamePair[] Inputs => [new(typeof(string), "Text 1"), new(typeof(string), "Text 2")];

        public TypeNamePair[] Outputs => [new(typeof(string), "")];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(INodeResourceManager resource)
        {
            return [string.Concat(resource.GetValue(0), resource.GetValue(1))];
        }
    }
}
