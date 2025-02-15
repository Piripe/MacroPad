using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Device
{
    public interface IPaletteValue
    {
        public int Value { get; }
        public string Name { get; }
        public uint Color { get; }
        public string? Image { get; }
    }
}
