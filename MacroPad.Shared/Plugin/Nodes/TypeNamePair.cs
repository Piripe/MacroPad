using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin.Nodes
{
    public struct TypeNamePair
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public TypeNamePair(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
