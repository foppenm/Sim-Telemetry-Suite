using System;
using System.Collections.Generic;
using System.Text;

namespace TelemetryReceiver.Models
{
    public class Scoring
    {
        public string Application { get; set; }
        public string Type { get; set; }
        public string TrackName { get; set; }
        public int Session { get; set; }
        public int NumVehicles { get; set; }
        public float CurrentET { get; set; }
        public int EndET { get; set; }
        public int MaxLaps { get; set; }
        public float LapDist { get; set; }
        public int GamePhase { get; set; }
        public int YellowFlagState { get; set; }
        public int[] SectorFlags { get; set; }
        public int InRealTime { get; set; }
        public int StartLight { get; set; }
        public int NumRedLights { get; set; }
        public string PlayerName { get; set; }
        public string PlrFileName { get; set; }
        public int DarkCloud { get; set; }
        public int Raining { get; set; }
        public int AmbientTemp { get; set; }
        public int TrackTemp { get; set; }
        public int[] Wind { get; set; }
        public int MinPathWetness { get; set; }
        public int MaxPathWetness { get; set; }
        public Vehicle[] Vehicles { get; set; }
    }
}
