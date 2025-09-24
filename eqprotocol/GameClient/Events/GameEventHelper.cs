using System;
using System.Collections.Generic;

namespace OpenEQ.Netcode.GameClient.Events
{
    public static class GameEventHelper
    {
        public static GameEvent CreateSpawnEvent(string actorId, string actorName, string eventType, string zone, float x, float y, float z, Dictionary<string, object> additionalData = null)
        {
            var gameEvent = new GameEvent
            {
                EventType = eventType,
                ActorId = actorId,
                ActorName = actorName,
                Zone = zone,
                X = x,
                Y = y,
                Z = z,
                Severity = "minor",
                NarrativeContext = $"{actorName} {(eventType.Contains("spawn") ? "appeared" : "vanished")} in {zone}"
            };

            if (additionalData != null)
            {
                foreach (var kvp in additionalData)
                {
                    gameEvent.Data[kvp.Key] = kvp.Value;
                }
            }

            return gameEvent;
        }

        public static GameEvent CreateCombatEvent(string eventType, string actorId, string actorName, string targetId, string targetName, 
            string zone, Dictionary<string, object> combatData)
        {
            var severity = "info";
            if (combatData.ContainsKey("damage"))
            {
                var damage = Convert.ToInt32(combatData["damage"]);
                severity = damage > 100 ? "major" : damage > 50 ? "minor" : "info";
            }

            return new GameEvent
            {
                EventType = eventType,
                ActorId = actorId,
                ActorName = actorName,
                TargetId = targetId,
                TargetName = targetName,
                Zone = zone,
                Data = combatData,
                Severity = severity,
                NarrativeContext = GenerateCombatNarrative(eventType, actorName, targetName, combatData)
            };
        }

        public static GameEvent CreateChatEvent(string actorId, string actorName, string channel, string message, string zone)
        {
            return new GameEvent
            {
                EventType = "communication.chat",
                ActorId = actorId,
                ActorName = actorName,
                Zone = zone,
                Data = new Dictionary<string, object>
                {
                    {"channel", channel},
                    {"message", message}
                },
                Severity = "info",
                NarrativeContext = $"{actorName} says in {channel}: \"{message}\""
            };
        }

        public static GameEvent CreateZoneEvent(string actorId, string actorName, string fromZone, string toZone)
        {
            return new GameEvent
            {
                EventType = "world.zone_change",
                ActorId = actorId,
                ActorName = actorName,
                Zone = toZone,
                Data = new Dictionary<string, object>
                {
                    {"from_zone", fromZone},
                    {"to_zone", toZone}
                },
                Severity = "major",
                NarrativeContext = $"{actorName} traveled from {fromZone} to {toZone}"
            };
        }

        private static string GenerateCombatNarrative(string eventType, string actorName, string targetName, Dictionary<string, object> data)
        {
            if (eventType.Contains("damage"))
            {
                var damage = data.ContainsKey("damage") ? data["damage"].ToString() : "some";
                var damageType = data.ContainsKey("damage_type") ? data["damage_type"].ToString() : "";
                return $"{actorName} dealt {damage} {damageType} damage to {targetName}";
            }
            else if (eventType.Contains("death"))
            {
                return $"{targetName} was slain by {actorName}";
            }
            else if (eventType.Contains("spell"))
            {
                var spellName = data.ContainsKey("spell_name") ? data["spell_name"].ToString() : "a spell";
                return $"{actorName} cast {spellName} on {targetName}";
            }
            
            return $"{actorName} performed {eventType} on {targetName}";
        }
    }
}