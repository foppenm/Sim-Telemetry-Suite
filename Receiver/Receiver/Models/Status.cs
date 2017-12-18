using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver.Models
{
    public enum Status
    {
        Unknown = 0,
        Pit = 1,
        InLap = 2,
        OutLap = 3,
        Driving = 4,
        Finished = 5
    }
}
