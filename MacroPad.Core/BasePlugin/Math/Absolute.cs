using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Math
{
    public class Absolute : INodeGetter
    {
        public string Name => "Absolute";

        public string Description => "Returns the absolute of the input number.";

        public string Id => "Absolute";

        public TypeNamePair[] Inputs => [new(typeof(decimal), "")];

        public TypeNamePair[] Outputs => [new(typeof(decimal), "")];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [System.Math.Abs((decimal)resource.GetValue(0))];
        }
    }
}
