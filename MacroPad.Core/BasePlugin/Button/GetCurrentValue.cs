using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin.Button
{
    public class GetCurrentValue : INodeGetter
    {
        public string Name => "Get Current Slider Value";

        public string Description => "Returns the value of the slider that raises the event.";

        public string Id => "GetCurrentValue";

        public TypeNamePair[] Inputs => [];

        public TypeNamePair[] Outputs => [new(typeof(decimal),"")];

        public INodeComponent[] Components => [];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => button.Type == ButtonType.Slider;
        public object[] GetOutputs(IResourceManager resource)
        {
            return [(decimal)(NodeManager.CurrentDevice != null && NodeManager.CurrentButton != null ? NodeManager.CurrentDevice.ButtonsCurrentValue[NodeManager.CurrentButton.Id].Value : 0)];
        }
    }
}
