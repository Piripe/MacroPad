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
    internal class Text : INodeGetter
    {
        public string Name => "Text";

        public string Description => "A text constant.";

        public string Id => "Text";

        public TypeNamePair[] Inputs => new TypeNamePair[0];

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(string), "") };

        public INodeComponent[] Components => new INodeComponent[] { new TextBox() {
            GetText = (IResourceManager resource) => resource.GetData<string>("v") ?? "",
            TextChanged = (IResourceManager resource, string text) => resource.SetData("v", text),
        } };

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { resource.GetData<string>("v") ?? "" };
        }
    }
}
