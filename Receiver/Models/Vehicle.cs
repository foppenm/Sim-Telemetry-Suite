using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DriverName { get; set; }

        public float[] Position { get; set; }

        public float CurrentSector1 { get; set; }

        public float CurrentSector2 { get; set; }

        public float[] Best { get; set; }

        public int Place { get; set; }

        public int Pit { get; set; }

        public string BestLapTime
        {
            get
            {
                var rawTime = Best[2];
                TimeSpan time = TimeSpan.FromSeconds(rawTime);

                // Here backslash is must to tell that colon is not the part of format, it just a character that we want in output
                return time.ToString(@"hh\:mm\:ss\.fff");
            }
        }
    }
}
