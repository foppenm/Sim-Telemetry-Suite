using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver.Models
{
    public class Track
    {
        public string Name { get; set; }

        public int Distance { get; set; }

        public int Phase { get; set; }

        public string SectorFlags { get; set; }

        public Session Session { get; set; }

        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
