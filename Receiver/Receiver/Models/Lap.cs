using System;
using System.Collections.Generic;
using System.Globalization;

namespace Receiver.Models
{
    public class Lap
    {
        public int Number { get; set; }

        public float[] Time { get; set; } = new float[3];

        /// <summary>
        /// Path dictionary, containing the lap distance as key and Position as value
        /// </summary>
        public Dictionary<int, float[]> Path { get; } = new Dictionary<int, float[]>();

        public float Sector1 { get { return Time[0]; } }

        public float Sector2 { get { return Time[1]; } }

        public float Sector3 { get { return Time[2]; } }

        public string TimeString
        {
            get
            {
                var rawTime = Time[2];
                TimeSpan time = TimeSpan.FromSeconds(rawTime);

                // Here backslash is must to tell that colon is not the part of format, it just a character that we want in output
                return time.ToString(@"mm\:ss\.fff", CultureInfo.InvariantCulture);
            }
        }
    }
}
