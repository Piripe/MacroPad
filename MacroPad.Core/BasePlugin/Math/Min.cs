using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Math
{
    public class Min : INodeGetter
    {
        public string Name => "Min";

        public string Description => "Returns the smallest number of the two inputs.";

        public string Id => "Min";

        public TypeNamePair[] Inputs => [new(typeof(decimal), ""), new(typeof(decimal), "")];

        public TypeNamePair[] Outputs => [new(typeof(decimal), "")];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [System.Math.Min((decimal)resource.GetValue(0), (decimal)resource.GetValue(1))];
        }
    }
}
