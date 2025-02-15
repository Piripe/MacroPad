using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin
{
    public interface IPluginInfos
    {
        public string Name { get; }
        public string Description { get; }
        public string Version { get; }
        public string Author { get; }
        public string? AuthorUrl { get; }
        public string? SourceUrl { get; }

        public IProtocol[] Protocols { get; }
        public INodeCategory[] NodeCategories { get; }
        public NodeType[] NodeTypes { get; }
    }
}
