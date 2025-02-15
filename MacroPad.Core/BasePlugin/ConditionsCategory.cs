using MacroPad.Core.BasePlugin.Button;
using MacroPad.Core.BasePlugin.Conditions;
using MacroPad.Core.BasePlugin.Math;
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
    public class ConditionsCategory : INodeCategory
    {
        public string Name => "Conditions";

        public string Id => "Conditions";
        public Color Color => new Color(40, 40, 40);

        public INodeGetter[] Getters => new INodeGetter[] { new Condition() };

        public INodeRunner[] Runners => new INodeRunner[] { };
    }
}
