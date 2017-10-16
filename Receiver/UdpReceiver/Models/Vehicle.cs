using System;
using System.Collections.Generic;
using System.Text;

namespace UdpReceiver.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string DriverName { get; set; }
        public string VehicleName { get; set; }
        public int TotalLaps { get; set; }
        public int Sector { get; set; }
        public int FinishStatus { get; set; }
        public float LapDist { get; set; }
        public float PathLateral { get; set; }
        public float RelevantTrackEdge { get; set; }
        public int[] Best { get; set; }
        public int[] Last { get; set; }
        public int CurrentSector1 { get; set; }
        public int CurrentSector2 { get; set; }
        public int NumPitstops { get; set; }
        public int NumPenalties { get; set; }
        public int IsPlayer { get; set; }
        public int Control { get; set; }
        public int InPits { get; set; }
        public int LapStartET { get; set; }
        public int Place { get; set; }
        public string VehicleClass { get; set; }
        public float TimeBehindNext { get; set; }
        public int LapsBehindNext { get; set; }
        public float TimeBehindLeader { get; set; }
        public int LapsBehindLeader { get; set; }
        public float[] Pos { get; set; }
        public int[] LocalVel { get; set; }
        public int[] LocalAccel { get; set; }
        public float[][] OrientationMatrix { get; set; }
        public int[] LocalRot { get; set; }
        public int[] LocalRotAccel { get; set; }
    }
}
