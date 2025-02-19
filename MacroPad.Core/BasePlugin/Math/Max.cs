using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Math
{
    public class Max : INodeGetter
    {
        public string Name => "Max";

        public string Description => "Returns the greatest number of the two inputs.";

        public string Id => "Max";

        public TypeNamePair[] Inputs => [new(typeof(decimal), ""), new(typeof(decimal), "")];

        public TypeNamePair[] Outputs => [new(typeof(decimal), "")];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [System.Math.Max((decimal)resource.GetValue(0), (decimal)resource.GetValue(1))];
        }
    }
}
