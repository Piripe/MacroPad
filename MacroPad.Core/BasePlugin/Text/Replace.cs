using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.BasePlugin.Text
{
    public class Replace : INodeGetter
    {
        public string Name => "Replace Text";

        public string Description => "Replace the \"from\" input by the \"to\" input in the input text";

        public string Id => "Replace";

        public TypeNamePair[] Inputs => new TypeNamePair[] { new TypeNamePair(typeof(string),""), new TypeNamePair(typeof(string), "From"), new TypeNamePair(typeof(string), "To") };

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(string), "") };

        public INodeComponent[] Components => new INodeComponent[0];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { ((string)resource.GetValue(0)).Replace((string)resource.GetValue(1), (string)resource.GetValue(2)) };
        }
    }
}
