using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.BasePlugin.Branching
{
    public class GetBranch : INodeGetter
    {
        public string Name => "Get Branch";

        public string Description => "Get one of two inputs depending on the boolean input.";

        public string Id => "GetBranch";

        public TypeNamePair[] Inputs => new TypeNamePair[] { new TypeNamePair(typeof(bool), ""), new TypeNamePair(typeof(object), "False"), new TypeNamePair(typeof(object), "True") };

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(object), "") };

        public INodeComponent[] Components => new INodeComponent[0];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { resource.GetValue((bool)resource.GetValue(0) ? 2 : 1) };
        }
    }
}
