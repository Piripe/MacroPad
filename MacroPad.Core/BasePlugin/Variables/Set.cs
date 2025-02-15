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
    public class Set : INodeRunner
    {
        public string Name => "Set Variable";

        public string Description => "Set the value of a variable";

        public string Id => "Set";

        public TypeNamePair[] Inputs => new TypeNamePair[] { new TypeNamePair(typeof(object), "") };

        public TypeNamePair[] Outputs => new TypeNamePair[] { };

        public INodeComponent[] Components => new INodeComponent[] { new TextBox() {
            GetText = (IResourceManager resource) => resource.GetData<string>("v") ?? "",
            TextChanged = (IResourceManager resource, string text) => resource.SetData("v", text),
        } };

        public int RunnerOutputCount => 1;

        public string[] RunnerOutputsName => new string[0];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;

        public NodeRunnerResult Run(IResourceManager resource)
        {
            string var = resource.GetData<string>("v") ?? "";
            JToken value = JToken.FromObject(resource.GetValue(0));
            if (DeviceManager.Config.Variables.ContainsKey(var)) DeviceManager.Config.Variables[var] = value;
            else DeviceManager.Config.Variables.Add(var, value);
            return new NodeRunnerResult() { Results = new string[0], RunnerOutputIndex = 0 };
        }
    }
}
