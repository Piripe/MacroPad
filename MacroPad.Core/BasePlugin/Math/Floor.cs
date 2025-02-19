using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Math
{
    public class Floor : INodeGetter
    {
        public string Name => "Floor";

        public string Description => "Returns the floor of the input number.";

        public string Id => "Floor";

        public TypeNamePair[] Inputs => [new(typeof(decimal), "")];

        public TypeNamePair[] Outputs => [new(typeof(decimal), "")];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [System.Math.Floor((decimal)resource.GetValue(0))];
        }
    }
}
