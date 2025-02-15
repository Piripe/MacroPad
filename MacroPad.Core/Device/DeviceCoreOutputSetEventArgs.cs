using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Device
{
    public class DeviceCoreOutputSetEventArgs
    {
        public DeviceLayoutButton Button { get; set; }
        public object Value { get; set; }

        public DeviceCoreOutputSetEventArgs(DeviceLayoutButton button, object value)
        {
            Button = button;
            Value = value;
        }
    }
}
