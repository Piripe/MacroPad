using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Branching
{
    public class GetBranch : INodeGetter
    {
        public string Name => "Get Branch";

        public string Description => "Get one of two inputs depending on the boolean input.";

        public string Id => "GetBranch";

        public TypeNamePair[] Inputs => [new(typeof(bool), ""), new(typeof(object), "False"), new(typeof(object), "True")];

        public TypeNamePair[] Outputs => [new(typeof(object), "")];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { resource.GetValue((bool)resource.GetValue(0) ? 2 : 1) };
        }
    }
}
