using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin.Protocol
{
    public class DeviceInputEventArgs
    {
        public int ButtonID { get; }
        public float Value { get; }

        public bool IsPressed => Value == 1;

        public DeviceInputEventArgs(int buttonID, float value)
        {
            ButtonID = buttonID;
            Value = value;
        }
    }
}
