using MacroPad.Core.BasePlugin.Debug;
using MacroPad.Core.BasePlugin.Text;
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
    public class TextCategory : INodeCategory
    {
        public string Name => "Text Management";

        public string Id => "Text";
        public Color Color => new Color(20, 250, 20);

        public INodeGetter[] Getters => new INodeGetter[] { new Join(), new Replace() };

        public INodeRunner[] Runners => new INodeRunner[] { };
    }
}
