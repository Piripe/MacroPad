using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Device
{
    public class DeviceCoreInputEventArgs
    {
        public DeviceLayoutButton Button { get; set; }
        public float Value { get; set; }
        public bool IsPressed { get; set; }

        public DeviceCoreInputEventArgs(DeviceLayoutButton button, float value, bool isPressed)
        {
            Button = button;
            Value = value;
            IsPressed = isPressed;
        }
    }
}
