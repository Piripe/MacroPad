using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin.Nodes.Components
{
    public class TextBox : INodeComponent
    {
        public Action<IResourceManager, string>? TextChanged { get; set; }
        public Func<IResourceManager, string>? GetText { get; set; }
    }
}
