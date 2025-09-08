using System;

namespace OpenEQ.Netcode.GameClient.Models
{
    public class Character : Entity
    {
        public uint Level { get; set; }
        public uint Zone { get; set; }
        public string ZoneName { get; set; } = "";
        
        // Stats
        public uint HP { get; set; }
        public uint MaxHP { get; set; }
        public uint Mana { get; set; }
        public uint MaxMana { get; set; }
        public uint Endurance { get; set; }
        public uint MaxEndurance { get; set; }
        
        // Attributes
        public uint Strength { get; set; }
        public uint Stamina { get; set; }
        public uint Agility { get; set; }
        public uint Dexterity { get; set; }
        public uint Wisdom { get; set; }
        public uint Intelligence { get; set; }
        public uint Charisma { get; set; }
        
        // Guild
        public uint GuildID { get; set; }
        public string GuildName { get; set; } = "";
        
        // Experience
        public uint Experience { get; set; }
        public uint AAExperience { get; set; }
        
        public override string ToString()
        {
            return $"{Name} (Level {Level}) - {ZoneName} [{X:F1}, {Y:F1}, {Z:F1}]";
        }
    }
}