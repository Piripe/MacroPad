using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Math
{
    public class Map : INodeGetter
    {
        public string Name => "Map";

        public string Description => "Returns the cross product of the input value with the min/max input and min/max output.";

        public string Id => "Map";

        public TypeNamePair[] Inputs => [new(typeof(decimal), "Value"), new(typeof(decimal), "Min Input"), new(typeof(decimal), "Max Input"), new(typeof(decimal), "Min Output"), new(typeof(decimal), "Max Output")];

        public TypeNamePair[] Outputs => [new(typeof(decimal), "")];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            decimal value = (decimal)resource.GetValue(0);
            decimal minIn = (decimal)resource.GetValue(1);
            decimal maxIn = (decimal)resource.GetValue(2);
            decimal minOut = (decimal)resource.GetValue(3);
            decimal maxOut = (decimal)resource.GetValue(4);
            return [(value - minIn) / System.Math.Max(maxIn - minIn, 0.0001m) * (maxOut - minOut) + minOut];
        }
    }
}
