using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Path dictionary, containing the lap distance as key and Position as value
        /// </summary>
        public Dictionary<int, float[]> Path { get; set; } = new Dictionary<int, float[]>();

        public float PathTime { get; set; }
    }
}
