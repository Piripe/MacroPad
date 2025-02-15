using MacroPad.Core.BasePlugin.Text;
using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Nodes.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.BasePlugin.Constants
{
    internal class Number : INodeGetter
    {
        public string Name => "Number";

        public string Description => "A number constant.";

        public string Id => "Number";
        
        public TypeNamePair[] Inputs => new TypeNamePair[0];

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), "") };

        public INodeComponent[] Components => new INodeComponent[] { new NumericUpDown() {
            Min = decimal.MinValue,
            Max = decimal.MaxValue,
            GetValue = (IResourceManager resource) => resource.GetData<decimal>("v"),
            ValueChanged = (IResourceManager resource, decimal value) => resource.SetData("v", value),
        } };

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { resource.GetData<decimal?>("v") ?? 0m };
        }
    }
}
