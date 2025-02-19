using MacroPad.Core.Device;
using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Components;

namespace MacroPad.Core.BasePlugin.Profile
{
    public class SetProfile : INodeRunner
    {
        public string Name => "Select Profile";

        public string Description => "Select the current profile.";

        public string Id => "SetProfile";

        public TypeNamePair[] Inputs => [];

        public TypeNamePair[] Outputs => [];

        public int RunnerOutputCount => 1;
        public string[] RunnerOutputsName => [];

        private int GetProfileIndex(DeviceCore? device, IResourceManager resource)
        {
            int index = resource.GetData<int>("profileIndex");
            if (device == null) return index;
            string profile = resource.GetData<string>("profile") ?? "";
            if (device.DeviceProfiles[index].Name == profile) return index;
            int deviceProfile = device.DeviceProfiles.FindIndex(x => x.Name == profile);
            if (deviceProfile != -1) return deviceProfile;
            return index;
        }

        public INodeComponent[] Components => [
            new ComboBox() {
                GetItems = (IResourceManager resource, IDeviceLayoutButton button, IDeviceOutput output) =>
                {
                    return DeviceManager.SelectedDevice?.DeviceProfiles.Select(x=>x.Name).ToArray() ?? [];
                },
                GetSelection = (IResourceManager resource) => GetProfileIndex(DeviceManager.SelectedDevice,resource),
                SelectionChanged = (IResourceManager resource, int selection) =>
                {
                    if (DeviceManager.SelectedDevice != null) resource.SetData("profile", DeviceManager.SelectedDevice.DeviceProfiles[selection].Name);
                    resource.SetData("profileIndex", selection);
                }
            }
        ];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public NodeRunnerResult Run(IResourceManager resource)
        {
            if (NodeManager.CurrentDevice != null) NodeManager.CurrentDevice.SelectProfile(GetProfileIndex(NodeManager.CurrentDevice, resource));
            return new NodeRunnerResult() { Results = [], RunnerOutputIndex = 0 };
        }
    }
}
