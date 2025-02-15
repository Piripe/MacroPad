using MacroPad.Shared.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin.Nodes
{
    public interface INodeRunner
    {
        public string Name { get; }
        public string Description { get; }
        public string Id { get; }

        public TypeNamePair[] Inputs { get; }
        public TypeNamePair[] Outputs { get; }
        public int RunnerOutputCount { get; }
        public string[] RunnerOutputsName { get; }
        public INodeComponent[] Components { get; }


        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output);
        public NodeRunnerResult Run(IResourceManager resource);

    }
}
