using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Device
{
    public interface IDeviceLayoutButton
    {
        public int Id { get; }
        public ButtonType Type { get; }
        public int X { get; }
        public int Y { get; }
        public int DX { get; }
        public int DY { get; }
        public int DWidth { get; }
        public int DHeight { get; }
        public int Rotation { get; }
    }
}
