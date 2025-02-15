using MacroPad.Core.BasePlugin.Debug;
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
    public class DebugCategory : INodeCategory
    {
        public string Name => "Debug";

        public string Id => "Debug";
        public Color Color => new Color(40, 40, 40);

        public INodeGetter[] Getters => new INodeGetter[] { };

        public INodeRunner[] Runners => new INodeRunner[] { new WriteLine() };
    }
}
