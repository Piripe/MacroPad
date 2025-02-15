using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin.Protocol
{
    public class DeviceDetectedEventArgs
    {
        public IProtocolDevice Device { get; set; }
        public DeviceDetectedEventArgs(IProtocolDevice device)
        {
            Device = device;
        }
    }
}
