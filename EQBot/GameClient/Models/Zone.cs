using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace EQBot.GameClient.Models
{
    public class Zone
    {
        public uint ZoneID { get; set; }
        public string Name { get; set; } = "";
        public string LongName { get; set; } = "";
        
        // Collections are thread-safe for concurrent access
        public ConcurrentDictionary<uint, NPC> NPCs { get; } = new ConcurrentDictionary<uint, NPC>();
        public ConcurrentDictionary<uint, Player> Players { get; } = new ConcurrentDictionary<uint, Player>();
        public ConcurrentDictionary<uint, Door> Doors { get; } = new ConcurrentDictionary<uint, Door>();
        
        // Zone properties
        public bool IsOutdoor { get; set; }
        public bool IsPvPZone { get; set; }
        public uint MinLevel { get; set; }
        public uint MaxLevel { get; set; }
        
        public void AddNPC(NPC npc)
        {
            NPCs.AddOrUpdate(npc.SpawnID, npc, (key, oldValue) => npc);
        }
        
        public void RemoveNPC(uint spawnId)
        {
            NPCs.TryRemove(spawnId, out _);
        }
        
        public void AddPlayer(Player player)
        {
            Players.AddOrUpdate(player.SpawnID, player, (key, oldValue) => player);
        }
        
        public void RemovePlayer(uint spawnId)
        {
            Players.TryRemove(spawnId, out _);
        }
        
        public IEnumerable<NPC> GetNearbyNPCs(float x, float y, float radius)
        {
            foreach (var npc in NPCs.Values)
            {
                var distance = Math.Sqrt(Math.Pow(npc.X - x, 2) + Math.Pow(npc.Y - y, 2));
                if (distance <= radius)
                {
                    yield return npc;
                }
            }
        }
        
        public override string ToString()
        {
            return $"{LongName} (ID: {ZoneID}) - NPCs: {NPCs.Count}, Players: {Players.Count}";
        }
    }
}