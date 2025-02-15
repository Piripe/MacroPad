using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Nodes.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.BasePlugin.Button
{
    public class GetCurrentValue : INodeGetter
    {
        public string Name => "Get Current Slider Value";

        public string Description => "Returns the value of the slider that raises the event.";

        public string Id => "GetCurrentValue";

        public TypeNamePair[] Inputs => new TypeNamePair[0];

        public TypeNamePair[] Outputs => new TypeNamePair[] { new TypeNamePair(typeof(decimal),"")};

        public INodeComponent[] Components => new INodeComponent[0];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => button.Type == ButtonType.Slider;
        public object[] GetOutputs(IResourceManager resource)
        {
            return new object[] { (decimal)(NodeManager.CurrentDevice != null && NodeManager.CurrentButton != null ? NodeManager.CurrentDevice.ButtonsCurrentValue[NodeManager.CurrentButton.Id].Value : 0)};
        }
    }
}
