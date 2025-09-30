using System;

namespace OpenEQ.Netcode.GameClient.Models
{
    /// <summary>
    /// Represents a player character in EverQuest with extended properties and state information.
    /// Inherits from Entity and adds player-specific attributes like level, guild, PvP status, etc.
    /// </summary>
    public class Player : Entity
    {
        /// <summary>
        /// Gets or sets the player's current level
        /// </summary>
        public uint Level { get; set; }

        /// <summary>
        /// Gets or sets the player's current hit points
        /// </summary>
        public uint HP { get; set; }

        /// <summary>
        /// Gets or sets the player's maximum hit points
        /// </summary>
        public uint MaxHP { get; set; }

        /// <summary>
        /// Gets the player's hit point percentage (0-100)
        /// </summary>
        public float HPPercentage => MaxHP > 0 ? (float)HP / MaxHP * 100 : 0;

        /// <summary>
        /// Gets or sets the player's guild ID (0 if not in a guild)
        /// </summary>
        public uint GuildID { get; set; }

        /// <summary>
        /// Gets or sets the player's guild name
        /// </summary>
        public string GuildName { get; set; } = "";

        /// <summary>
        /// Gets or sets the player's rank within their guild
        /// </summary>
        public uint GuildRank { get; set; }

        /// <summary>
        /// Gets or sets whether the player is a Game Master
        /// </summary>
        public bool IsGM { get; set; }

        /// <summary>
        /// Gets or sets whether the player is currently in trader mode
        /// </summary>
        public bool IsTrader { get; set; }

        /// <summary>
        /// Gets or sets whether the player is looking for group
        /// </summary>
        public bool IsLFG { get; set; }

        /// <summary>
        /// Gets or sets whether the player has anonymous flag enabled
        /// </summary>
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// Gets or sets whether the player is flagged for PvP
        /// </summary>
        public bool IsPvP { get; set; }

        /// <summary>
        /// Gets or sets the player's PvP points
        /// </summary>
        public uint PvPPoints { get; set; }

        /// <summary>
        /// Gets or sets whether the player is currently in a group
        /// </summary>
        public bool IsInGroup { get; set; }

        /// <summary>
        /// Gets or sets whether the player is currently in a raid
        /// </summary>
        public bool IsInRaid { get; set; }

        /// <summary>
        /// Gets or sets whether the player is the leader of their group
        /// </summary>
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
