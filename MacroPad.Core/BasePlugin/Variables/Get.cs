using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Nodes.Components;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.BasePlugin.Variables
{
    public class Get : INodeGetter
    {
        public string Name => "Get Variable";

        public string Description => "Get the value of a variable";

        public string Id => "Get";

        public TypeNamePair[] Inputs => new TypeNamePair[] { };

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(object), "") };

        public INodeComponent[] Components => new INodeComponent[] { new TextBox() {
            GetText = (IResourceManager resource) => resource.GetData<string>("v") ?? "",
            TextChanged = (IResourceManager resource, string text) => resource.SetData("v", text),
        } };

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public object[] GetOutputs(IResourceManager resource)
        {
            string var = resource.GetData<string>("v") ?? "";
            return new object[] { (DeviceManager.Config.Variables.ContainsKey(var) ? DeviceManager.Config.Variables[var].Value<object>() : null) ?? "" };
        }
    }
}
