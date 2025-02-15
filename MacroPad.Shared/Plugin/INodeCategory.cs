using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin
{
    public interface INodeCategory
    {
        public string Name { get; }
        public string Id { get; }
        public Color Color { get; }

        public INodeGetter[] Getters { get; }
        public INodeRunner[] Runners { get; }
    }
}
