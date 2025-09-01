using System;

namespace EQBot.GameClient.Models
{
    public class Player : Entity
    {
        public uint Level { get; set; }
        public uint HP { get; set; }
        public uint MaxHP { get; set; }
        public float HPPercentage => MaxHP > 0 ? (float)HP / MaxHP * 100 : 0;
        
        // Guild information
        public uint GuildID { get; set; }
        public string GuildName { get; set; } = "";
        public uint GuildRank { get; set; }
        
        // Player flags
        public bool IsGM { get; set; }
        public bool IsTrader { get; set; }
        public bool IsLFG { get; set; }
        public bool IsAnonymous { get; set; }
        
        // PvP
        public bool IsPvP { get; set; }
        public uint PvPPoints { get; set; }
        
        // Group/Raid
        public bool IsInGroup { get; set; }
        public bool IsInRaid { get; set; }
        public bool IsGroupLeader { get; set; }
        
        // Equipment (visible items)
        public uint PrimaryWeapon { get; set; }
        public uint SecondaryWeapon { get; set; }
        public uint Helmet { get; set; }
        public uint Chest { get; set; }
        public uint Arms { get; set; }
        public uint Legs { get; set; }
        public uint Feet { get; set; }
        
        public bool IsAlive => HP > 0;
        public bool IsLowHealth => HPPercentage < 25;
        public bool HasGuild => !string.IsNullOrEmpty(GuildName);
        
        public override string ToString()
        {
            var guildText = HasGuild ? $"<{GuildName}>" : "";
            var status = IsAlive ? $"Level {Level}" : "DEAD";
            return $"{Name} {guildText} ({status}) [{X:F1}, {Y:F1}]";
        }
    }
}