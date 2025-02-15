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
    public class Max : INodeGetter
    {
        public string Name => "Max";

        public string Description => "Returns the greatest number of the two inputs.";

        public string Id => "Max";

        public TypeNamePair[] Inputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), ""), new TypeNamePair(typeof(decimal), "") };

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal), "") };

        public INodeComponent[] Components => new INodeComponent[0];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { System.Math.Max((decimal)resource.GetValue(0), (decimal)resource.GetValue(1)) };
        }
    }
}
