using System;

namespace OpenEQ.Netcode.GameClient.Models
{
    public class NPC : Entity
    {
        public uint NPCTypeID { get; set; }
        public bool IsNamedMob { get; set; }
        public uint BodyType { get; set; }
        public uint Size { get; set; }
        
        // Combat stats
        public uint Level { get; set; }
        public uint HP { get; set; }
        public uint MaxHP { get; set; }
        public float HPPercentage => MaxHP > 0 ? (float)HP / MaxHP * 100 : 0;
        
        // Behavior flags
        public bool IsAggressive { get; set; }
        public bool IsAssistable { get; set; }
        public bool IsSummoned { get; set; }
        public bool IsPet { get; set; }
        
        // Targeting
        public uint TargetID { get; set; }
        public bool IsTargetingPlayer => TargetID > 0;
        
        // Loot
        public bool HasLoot { get; set; }
        
        // Special flags
        public bool IsTrader { get; set; }
        public bool IsBanker { get; set; }
        public bool IsGuildMaster { get; set; }
        
        public bool IsAlive => HP > 0;
        public bool IsLowHealth => HPPercentage < 25;
        
        public override string ToString()
        {
            var status = IsAlive ? $"HP: {HPPercentage:F0}%" : "DEAD";
            return $"{Name} (Level {Level}) - {status} [{X:F1}, {Y:F1}]";
        }
    }
}
