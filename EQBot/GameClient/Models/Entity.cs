using System;

namespace EQBot.GameClient.Models
{
    public abstract class Entity
    {
        public uint SpawnID { get; set; }
        public string Name { get; set; } = "";
        public uint Race { get; set; }
        public uint Gender { get; set; }
        public uint Class { get; set; }
        
        // Position and movement
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }
        
        // Animation and state
        public uint Animation { get; set; }
        public bool IsInvisible { get; set; }
        public bool IsLinkDead { get; set; }
        public bool IsAFK { get; set; }
        
        // Last update tracking
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        
        public float DistanceTo(Entity other)
        {
            return DistanceTo(other.X, other.Y, other.Z);
        }
        
        public float DistanceTo(float x, float y, float z)
        {
            return (float)Math.Sqrt(
                Math.Pow(X - x, 2) + 
                Math.Pow(Y - y, 2) + 
                Math.Pow(Z - z, 2)
            );
        }
        
        public float Distance2D(Entity other)
        {
            return Distance2D(other.X, other.Y);
        }
        
        public float Distance2D(float x, float y)
        {
            return (float)Math.Sqrt(
                Math.Pow(X - x, 2) + 
                Math.Pow(Y - y, 2)
            );
        }
    }
}