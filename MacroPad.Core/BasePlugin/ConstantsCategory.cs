using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MacroPad.Shared.Media;
using MacroPad.Core.BasePlugin.Constants;

namespace MacroPad.Core.BasePlugin
{
    internal class ConstantsCategory : INodeCategory
    {
        public string Name => "Constants";

        public string Id => "Constants";
        public Color Color => new Color(40, 40, 40);

        public INodeGetter[] Getters => new INodeGetter[] { new Constants.Boolean(), new Constants.Number(), new Constants.Text() };

        public INodeRunner[] Runners => new INodeRunner[] { };
    }
}
