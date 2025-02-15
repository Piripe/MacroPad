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
    public class Min : INodeGetter
    {
        public string Name => "Min";

        public string Description => "Returns the smallest number of the two inputs.";

        public string Id => "Min";

        public TypeNamePair[] Inputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), ""), new TypeNamePair(typeof(decimal), "") };

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), "") };

        public INodeComponent[] Components => new INodeComponent[0];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { System.Math.Min((decimal)resource.GetValue(0), (decimal)resource.GetValue(1)) };
        }
    }
}
