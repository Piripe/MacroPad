using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Nodes.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.BasePlugin.Math
{
    public class MinMax : INodeGetter
    {
        public string Name => "Min Max";

        public string Description => "Returns the input value between the min and max.";

        public string Id => "MinMax";

        public TypeNamePair[] Inputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), "Value"), new TypeNamePair(typeof(decimal), "Min"), new TypeNamePair(typeof(decimal), "Max") };

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), "") };

        public INodeComponent[] Components => new INodeComponent[0];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { System.Math.Min(System.Math.Max((decimal)resource.GetValue(0), (decimal)resource.GetValue(1)), (decimal)resource.GetValue(2)) };
        }
    }
}
