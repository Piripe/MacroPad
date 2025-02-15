using MacroPad.Core.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Controls.Home
{
    public class DeviceViewerButtonPressedEventArgs
    {
        public DeviceLayoutButton? Button { get; set; }

        public DeviceViewerButtonPressedEventArgs(DeviceLayoutButton? button)
        {
            Button = button;
        }
    }
}
