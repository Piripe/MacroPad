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
    internal class Boolean : INodeGetter
    {
        public string Name => "Boolean";

        public string Description => "A boolean constant.";

        public string Id => "Boolean";
        
        public TypeNamePair[] Inputs => new TypeNamePair[0];

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(bool), "") };

        public INodeComponent[] Components => new INodeComponent[] { new ComboBox() {
            Items = new string[] {"False","True" },
            GetSelection = (IResourceManager resource) => resource.GetData<int>("v"),
            SelectionChanged = (IResourceManager resource, int value) => resource.SetData("v", value),
        } };

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { resource.GetData<bool?>("v") ?? false };
        }
    }
}
