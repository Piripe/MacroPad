using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin.Nodes.Components
{
    public class NumericUpDown : INodeComponent
    {
        public decimal Min { get; set; } = 0;
        public decimal Max { get; set; } = 100;
        public Action<IResourceManager, decimal>? ValueChanged { get; set; }
        public Func<IResourceManager, decimal>? GetValue { get; set; }
    }
}
