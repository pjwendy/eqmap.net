using System;
using System.Collections.Generic;

namespace OpenEQ.Netcode.GameClient.Events
{
    public enum PacketDirection
    {
        Inbound,
        Outbound
    }

    public enum RecordingMode
    {
        Off,              // No recording
        EventsOnly,       // Semantic events for narration
        PacketsOnly,      // Raw packets for debugging
        Full,             // Both events and packets
        Replay           // Record for replay capability
    }

    public interface IPacketEventEmitter : IDisposable
    {
        RecordingMode RecordingMode { get; set; }
        
        // Raw packet for debugging/analysis
        void EmitRawPacket(PacketDirection direction, byte[] data, string packetType, string streamType);
        
        // Semantic event for narration engine
        void EmitGameEvent(GameEvent gameEvent);
        
        // Flush any buffered events
        void Flush();
    }

    public class GameEvent
    {
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string EventType { get; set; }  // "combat.damage", "npc.spawn", "player.death"
        public string ActorId { get; set; }
        public string ActorName { get; set; }
        public string TargetId { get; set; }
        public string TargetName { get; set; }
        public string Zone { get; set; }
        public float? X { get; set; }
        public float? Y { get; set; }
        public float? Z { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public string Severity { get; set; } = "info"; // info, minor, major, critical
        public string NarrativeContext { get; set; } // Additional context for narration
    }

    public class RawPacket
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public PacketDirection Direction { get; set; }
        public string StreamType { get; set; }  // Login, World, Zone
        public string PacketType { get; set; }
        public byte[] Data { get; set; }
        public int Size { get; set; }
        public uint? Sequence { get; set; }
        
        // Hex representation for readable serialization
        public string DataHex => Data != null ? BitConverter.ToString(Data).Replace("-", " ") : null;
    }
}