using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver.Json
{
    public class Vehicle
    {
        public int id { get; set; }
        public string driverName { get; set; }
        public string vehicleName { get; set; }
        public int totalLaps { get; set; }
        public int sector { get; set; }
        public int finishStatus { get; set; }
        public float lapDist { get; set; }
        public float pathLateral { get; set; }
        public float relevantTrackEdge { get; set; }
        public float[] best { get; set; }
        public float[] last { get; set; }
        public float currentSector1 { get; set; }
        public float currentSector2 { get; set; }
        public int numPitstops { get; set; }
        public int numPenalties { get; set; }
        public int isPlayer { get; set; }
        public int control { get; set; }
        public int inPits { get; set; }
        public float lapStartET { get; set; }
        public int place { get; set; }
        public string vehicleClass { get; set; }
        public float timeBehindNext { get; set; }
        public int lapsBehindNext { get; set; }
        public float timeBehindLeader { get; set; }
        public int lapsBehindLeader { get; set; }
        public float[] Pos { get; set; }
        public float[] localVel { get; set; }
        public float[] localAccel { get; set; }
        public float[][] orientationMatrix { get; set; }
        public float[] localRot { get; set; }
        public float[] localRotAccel { get; set; }
    }
}
