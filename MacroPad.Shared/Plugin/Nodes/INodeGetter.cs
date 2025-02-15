using MacroPad.Shared.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin.Nodes
{
    public interface INodeGetter
    {
        public string Name { get; }
        public string Description { get; }
        public string Id { get; }

        public TypeNamePair[] Inputs { get; }
        public TypeNamePair[] Outputs { get; }
        public INodeComponent[] Components { get; }
        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output);
        public object[] GetOutputs(IResourceManager resource);

    }
}
