using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Device
{
    public enum ButtonEvent
    {
        Init = 0,
        Pressed = 1,
        Released = 2,
        ValueChanged = 3,
        LongPress = 4, //TODO: Implement LongPress
    }
}
