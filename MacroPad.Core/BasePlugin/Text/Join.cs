using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.BasePlugin.Text
{
    public class Join : INodeGetter
    {
        public string Name => "Join Text";

        public string Description => "Concatenate two texts together.";

        public string Id => "Join";

        public TypeNamePair[] Inputs => new TypeNamePair[] { new TypeNamePair(typeof(string),"Text 1"), new TypeNamePair(typeof(string), "Text 2") };

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(string), "") };

        public INodeComponent[] Components => new INodeComponent[0];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { string.Concat(resource.GetValue(0), resource.GetValue(1)) };
        }
    }
}
