using System;

namespace OpenEQ.Netcode.GameClient.Models
{
    public class UpdatePositionPacket
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }
        public float DeltaX { get; set; }
        public float DeltaY { get; set; }
        public float DeltaZ { get; set; }
        public float DeltaHeading { get; set; }
        public uint Animation { get; set; } = 0; // 0 = standing, 1 = walking, 2 = running
        public uint Sequence { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public byte[] ToBytes()
        {
            // This is a simplified packet structure
            // You'll need to adjust this based on your specific EQ server's packet format
            var packet = new byte[64];
            
            using (var ms = new System.IO.MemoryStream(packet))
            using (var writer = new System.IO.BinaryWriter(ms))
            {
                // Position
                writer.Write(X);
                writer.Write(Y);
                writer.Write(Z);
                writer.Write(Heading);
                
                // Delta values (for smooth movement prediction)
                writer.Write(DeltaX);
                writer.Write(DeltaY);
                writer.Write(DeltaZ);
                writer.Write(DeltaHeading);
                
                // Animation state
                writer.Write(Animation);
                writer.Write(Sequence);
                
                // Padding
                while (ms.Position < packet.Length)
                {
                    writer.Write((byte)0);
                }
            }
            
            return packet;
        }
    }

    public class ClientPositionUpdate
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }
        public uint Sequence { get; set; }
        public bool IsRunning { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        public UpdatePositionPacket ToPacket()
        {
            return new UpdatePositionPacket
            {
                X = X,
                Y = Y,
                Z = Z,
                Heading = Heading,
                Animation = IsRunning ? 2u : 1u, // 2 = running, 1 = walking
                Sequence = Sequence,
                Timestamp = Timestamp
            };
        }
    }
}