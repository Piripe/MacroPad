using MacroPad.Core.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Device
{
    public interface IDeviceOutput
    {
        public OutputType OutputType { get; }
        public IPaletteValue[] Palette { get; }
        public string? CornerRadius { get; }
        public string? Image { get; }
        public uint Color { get; }
    }
}
