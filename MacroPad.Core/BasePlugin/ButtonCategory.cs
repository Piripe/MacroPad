using MacroPad.Core.BasePlugin.Button;
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
    public class ButtonCategory : INodeCategory
    {
        public string Name => "Button";

        public string Id => "Button";
        public Color Color => new Color(40, 40, 40);

        public INodeGetter[] Getters => new INodeGetter[] { new GetCurrentValue() };

        public INodeRunner[] Runners => new INodeRunner[] { new SetCurrentPaletteColor() };
    }
}
