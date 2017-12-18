using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Receiver.Models
{
    public class Vehicle
    {
        public bool NewLap { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string DriverName { get; set; }

        public float[] Position { get; set; }

        public float[] PreviousPosition { get; set; }

        public float Velocity { get; set; }

        public float PreviousVelocity { get; set; }

        public float TopVelocity { get; set; }

        public int Place { get; set; }

        public Sector Sector { get; set; }

        public Sector PreviousSector { get; set; }

        public Status Status { get; set; }

        public Status PreviousStatus { get; set; }

        public List<Lap> Laps { get; } = new List<Lap>();

        public string CurrentSpeed
        {
            get
            {
                // * 3.6 for the conversion from m/s to kph
                var rawSpeed = Velocity * 3.6f;
                return rawSpeed.ToString("n1", CultureInfo.InvariantCulture);
            }
        }

        public string TopSpeed
        {
            get
            {
                var rawSpeed = TopVelocity * 3.6f;
                return rawSpeed.ToString("n1", CultureInfo.InvariantCulture);
            }
        }

        public Lap BestLap
        {
            get
            {
                if (Laps.Count == 0) { return null; }
                return Laps.Aggregate((left, right) => (left.Sector3 < right.Sector3 ? left : right));
            }
        }

        public float BestLapTime
        {
            get
            {
                if (Laps.Count == 0) { return 0f; }
                return Laps.Min(l => l.Sector3);
            }
        }

        public string BestLapTimeString
        {
            get
            {
                if (Laps.Count == 0) { return string.Empty; }
                TimeSpan time = TimeSpan.FromSeconds(BestLapTime);

                // Here backslash is must to tell that colon is not the part of format, it just a character that we want in output
                return time.ToString(@"mm\:ss\.fff", CultureInfo.InvariantCulture);
            }
        }
    }
}
