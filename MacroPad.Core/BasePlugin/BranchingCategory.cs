using MacroPad.Core.BasePlugin.Branching;
using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.BasePlugin
{
    public class BranchingCategory : INodeCategory
    {
        public string Name => "Branching";

        public string Id => "Branching";
        public Color Color => new Color(40, 40, 40);

        public INodeGetter[] Getters => new INodeGetter[] { new GetBranch() };

        public INodeRunner[] Runners => new INodeRunner[] { new SetPaletteColor() };
    }
}
