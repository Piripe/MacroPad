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
    public class Map : INodeGetter
    {
        public string Name => "Map";

        public string Description => "Returns the cross product of the input value with the min/max input and min/max output.";

        public string Id => "Map";

        public TypeNamePair[] Inputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), "Value"), new TypeNamePair(typeof(decimal), "Min Input"), new TypeNamePair(typeof(decimal), "Max Input"), new TypeNamePair(typeof(decimal), "Min Output"), new TypeNamePair(typeof(decimal), "Max Output") };

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), "") };

        public INodeComponent[] Components => new INodeComponent[0];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            decimal value = (decimal)resource.GetValue(0);
            decimal minIn = (decimal)resource.GetValue(1);
            decimal maxIn = (decimal)resource.GetValue(2);
            decimal minOut = (decimal)resource.GetValue(3);
            decimal maxOut = (decimal)resource.GetValue(4);
            return new object[] { (value-minIn)/System.Math.Max(maxIn-minIn,0.0001m)*(maxOut-minOut)+minOut };
        }
    }
}
