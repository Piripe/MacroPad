using MacroPad.Core.BasePlugin.Debug;
using MacroPad.Core.BasePlugin.Text;
using MacroPad.Core.BasePlugin.Variables;
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
    public class VariableCategory : INodeCategory
    {
        public string Name => "Variables";

        public string Id => "Variable";
        public Color Color => new Color(20, 250, 20);

        public INodeGetter[] Getters => new INodeGetter[] { new Get() };

        public INodeRunner[] Runners => new INodeRunner[] { new Set() };
    }
}
