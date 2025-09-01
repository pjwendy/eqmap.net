using System;

namespace EQBot.GameClient.Models
{
    public class Door
    {
        public uint DoorID { get; set; }
        public string Name { get; set; } = "";
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }
        
        // Door properties
        public uint DoorType { get; set; }
        public uint Size { get; set; }
        public bool IsOpen { get; set; }
        public bool IsLocked { get; set; }
        public uint KeyRequired { get; set; }
        
        // Zone connection (for zone doors)
        public uint DestinationZone { get; set; }
        public float DestinationX { get; set; }
        public float DestinationY { get; set; }
        public float DestinationZ { get; set; }
        public float DestinationHeading { get; set; }
        
        public bool IsZoneDoor => DestinationZone > 0;
        public bool CanOpen => !IsLocked || KeyRequired == 0;
        
        public float DistanceTo(float x, float y, float z)
        {
            return (float)Math.Sqrt(
                Math.Pow(X - x, 2) + 
                Math.Pow(Y - y, 2) + 
                Math.Pow(Z - z, 2)
            );
        }
        
        public override string ToString()
        {
            var status = IsOpen ? "OPEN" : IsLocked ? "LOCKED" : "CLOSED";
            var type = IsZoneDoor ? $"-> Zone {DestinationZone}" : "Regular";
            return $"{Name} ({status}, {type}) [{X:F1}, {Y:F1}, {Z:F1}]";
        }
    }
}