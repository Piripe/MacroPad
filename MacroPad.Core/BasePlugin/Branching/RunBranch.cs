using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.BasePlugin.Branching
{
    public class SetPaletteColor : INodeRunner
    {
        public string Name => "Run Branch";

        public string Description => "Run one of the two outputs depending on the boolean input.";

        public string Id => "RunBranch";

        public TypeNamePair[] Inputs => new TypeNamePair[] { new TypeNamePair(typeof(bool), "") };

        public TypeNamePair[] Outputs => new TypeNamePair[0];

        public int RunnerOutputCount => 2;
        public string[] RunnerOutputsName => new string[] { "False","True"};

        public INodeComponent[] Components => new INodeComponent[0];

        public bool IsVisible(IDeviceLayoutButton button, IDeviceOutput output) => true;
        public NodeRunnerResult Run(IResourceManager resource)
        {
            return new NodeRunnerResult() { Results = new object[0], RunnerOutputIndex = (bool)resource.GetValue(0) ? 1 : 0 };
        }
    }
}
