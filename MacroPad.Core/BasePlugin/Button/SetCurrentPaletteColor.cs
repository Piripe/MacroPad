using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Components;

namespace MacroPad.Core.BasePlugin.Button
{
    public class SetCurrentPaletteColor : INodeRunner
    {
        public string Name => "Set Current Button Color";

        public string Description => "Set the palette color of the button that raise the event.";

        public string Id => "SetCurrentPaletteColor";

        public TypeNamePair[] Inputs => [];

        public TypeNamePair[] Outputs => [];

        public int RunnerOutputCount => 1;
        public string[] RunnerOutputsName => [];

        public INodeComponent[] Components => [
            new ComboBox()
            {
                GetItems = (IResourceManager resource, IDeviceLayoutButton button, IDeviceOutput output) =>
                {
                    return output.Palette.Select(x=>x.Name).ToArray();
                },
                GetSelection = (IResourceManager resource) =>
                {
                    return resource.GetData<int>("color");
                },
                SelectionChanged = (IResourceManager resource, int selection) =>
                {
                    resource.SetData("color", selection);
                }
            }    
        ];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => output.OutputType == OutputType.Palette;
        public NodeRunnerResult Run(IResourceManager resource)
        {
            if (NodeManager.CurrentButton != null) NodeManager.CurrentDevice?.SetButtonContent(NodeManager.CurrentButton, resource.GetData<int>("color"));
            return new NodeRunnerResult() { Results = [], RunnerOutputIndex = 0 };
        }
    }
}
