﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Device
{
    public enum DeviceDetectionMode
    {
        Name = 0,
        ID = 1,
        Contains = 2,
        Equal = 4,
        Regex = 8
    }
}
