using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver.Json
{
    public class Track
    {
        public string application { get; set; }
        public string type { get; set; }
        public string trackName { get; set; }
        public int session { get; set; }
        public int numVehicles { get; set; }
        public float currentET { get; set; }
        public float endET { get; set; }
        public int maxLaps { get; set; }
        public float lapDist { get; set; }
        public int gamePhase { get; set; }
        public int yellowFlagState { get; set; }
        public int[] sectorFlags { get; set; }
        public int inRealTime { get; set; }
        public int startLight { get; set; }
        public int numRedLights { get; set; }
        public string playerName { get; set; }
        public string plrFileName { get; set; }
        public float darkCloud { get; set; }
        public float raining { get; set; }
        public float ambientTemp { get; set; }
        public float trackTemp { get; set; }
        public float[] wind { get; set; }
        public float minPathWetness { get; set; }
        public float maxPathWetness { get; set; }
        public Vehicle[] vehicles { get; set; }
    }
}
