using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Components;

namespace MacroPad.Core.BasePlugin.Constants
{
    internal class Number : INodeGetter
    {
        public string Name => "Number";

        public string Description => "A number constant.";

        public string Id => "Number";
        
        public TypeNamePair[] Inputs => [];

        public TypeNamePair[] Outputs => [new(typeof(decimal), "")];

        public INodeComponent[] Components => [ new NumericUpDown() {
            Min = decimal.MinValue,
            Max = decimal.MaxValue,
            GetValue = (IResourceManager resource) => resource.GetData<decimal>("v"),
            ValueChanged = (IResourceManager resource, decimal value) => resource.SetData("v", value),
        } ];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { resource.GetData<decimal?>("v") ?? 0m };
        }
    }
}
