using MacroPad.Shared.Device;

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
