using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Math
{
    public class MinMax : INodeGetter
    {
        public string Name => "Min Max";

        public string Description => "Returns the input value between the min and max.";

        public string Id => "MinMax";

        public TypeNamePair[] Inputs => [new(typeof(decimal), "Value"), new(typeof(decimal), "Min"), new(typeof(decimal), "Max")];

        public TypeNamePair[] Outputs => [new(typeof(decimal), "")];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [System.Math.Min(System.Math.Max((decimal)resource.GetValue(0), (decimal)resource.GetValue(1)), (decimal)resource.GetValue(2))];
        }
    }
}
