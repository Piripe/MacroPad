using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Math
{
    public class Round : INodeGetter
    {
        public string Name => "Round";

        public string Description => "Returns the rounded value of the input number.";

        public string Id => "Round";

        public TypeNamePair[] Inputs => [new(typeof(decimal), "")];

        public TypeNamePair[] Outputs => [new(typeof(decimal), "")];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [System.Math.Round((decimal)resource.GetValue(0))];
        }
    }
}
