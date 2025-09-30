using EQProtocol.Streams.Common;
using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

#region Comments about Structures and ENCODE/DECODE
// C++ Structure Definition:

// struct PlayerPositionUpdateClient_Struct
// {
//    /*0000*/
//    uint16 spawn_id;
//    /*0022*/
//    uint16 sequence;    //increments one each packet
//    /*0004*/
//    float y_pos;                 // y coord
//    /*0008*/
//    float delta_z;            // Change in z
//    /*0016*/
//    float delta_x;            // Change in x
//    /*0012*/
//    float delta_y;            // Change in y
//    /*0020*/
//    sint32 animation:10,     // animation
//    delta_heading:10,  // change in heading
//    padding0020:12;   // ***Placeholder (mostly 1)
//    /*0024*/
//    float x_pos;                 // x coord
//    /*0028*/
//    float z_pos;                 // z coord
//    /*0034*/
//    uint16 heading:12,     // Directional heading
//    padding0004:4;  // ***Placeholder
//    /*0032*/
//    uint8 unknown0006[2];  // ***Placeholder
//    /*0036*/
// };

// struct PlayerPositionUpdateServer_Struct
// {
// /*0000*/	uint16		spawn_id;
// /*0002*/	signed		padding0000 : 12;	// ***Placeholder
// signed		delta_x : 13;		// change in x
// signed		padding0005 : 7;	// ***Placeholder
// /*0006*/	signed		delta_heading : 10;	// change in heading
// signed		delta_y : 13;		// change in y
// signed		padding0006 : 9;	// ***Placeholder
// /*0010*/	signed		y_pos : 19;			// y coord
// signed		animation : 10;		// animation
// signed		padding0010 : 3;	// ***Placeholder
// /*0014*/	unsigned	heading : 12;		// heading
// signed		x_pos : 19;			// x coord
// signed		padding0014 : 1;	// ***Placeholder
// /*0018*/	signed		z_pos : 19;			// z coord
// signed		delta_z : 13;		// change in z
// /*0022*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_ClientUpdate)
// {
// // for some odd reason, there is an extra byte on the end of this on occasion..
// DECODE_LENGTH_ATLEAST(structs::PlayerPositionUpdateClient_Struct);
// SETUP_DIRECT_DECODE(PlayerPositionUpdateClient_Struct, structs::PlayerPositionUpdateClient_Struct);
// 
// IN(spawn_id);
// IN(sequence);
// IN(x_pos);
// IN(y_pos);
// IN(z_pos);
// IN(heading);
// IN(delta_x);
// IN(delta_y);
// IN(delta_z);
// IN(delta_heading);
// IN(animation);
// emu->vehicle_id = 0;
// 
// FINISH_DIRECT_DECODE();
// }
// ENCODE(OP_ClientUpdate)
// {
// ENCODE_LENGTH_EXACT(PlayerPositionUpdateServer_Struct);
// SETUP_DIRECT_ENCODE(PlayerPositionUpdateServer_Struct, structs::PlayerPositionUpdateServer_Struct);

// OUT(spawn_id);
// OUT(x_pos);
// OUT(delta_x);
// OUT(delta_y);
// OUT(z_pos);
// OUT(delta_heading);
// OUT(y_pos);
// OUT(delta_z);
// OUT(animation);
// OUT(heading);
// FINISH_ENCODE();
// }

// public struct PlayerPositionUpdate : IEQStruct
// {
//    public ushort ID;
//    public UpdatePosition Position;

//    public PlayerPositionUpdate(ushort ID, UpdatePosition Position) : this()
//    {
//        this.ID = ID;
//        this.Position = Position;
//    }

//    public PlayerPositionUpdate(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public PlayerPositionUpdate(BinaryReader br) : this()
//    {
//        Unpack(br);
//    }
//    public void Unpack(byte[] data, int offset = 0)
//    {
//        using (var ms = new MemoryStream(data, offset, data.Length - offset))
//        {
//            using (var br = new BinaryReader(ms))
//            {
//                Unpack(br);
//            }
//        }
//    }
//    public void Unpack(BinaryReader br)
//    {
//        ID = br.ReadUInt16();
//        Position = new UpdatePosition(br);
//    }

//    public byte[] Pack()
//    {
//        using (var ms = new MemoryStream())
//        {
//            using (var bw = new BinaryWriter(ms))
//            {
//                Pack(bw);
//                return ms.ToArray();
//            }
//        }
//    }
//    public void Pack(BinaryWriter bw)
//    {
//        bw.Write(ID);
//        Position.Pack(bw);
//    }

//    public override string ToString()
//    {
//        var ret = "struct PlayerPositionUpdate {\n";
//        ret += "\tID = ";
//        try
//        {
//            ret += $"{Indentify(ID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPosition = ";
//        try
//        {
//            ret += $"{Indentify(Position)}\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        return ret + "}";
//    }
// }
// void Mob::MakeSpawnUpdate(PlayerPositionUpdateServer_Struct * spu) {
//    spu->spawn_id = GetID();
//    spu->x_pos = FloatToEQ19(m_Position.x);
//    spu->y_pos = FloatToEQ19(m_Position.y);
//    spu->z_pos = FloatToEQ19(m_Position.z);
//    spu->delta_x = FloatToEQ13(m_Delta.x);
//    spu->delta_y = FloatToEQ13(m_Delta.y);
//    spu->delta_z = FloatToEQ13(m_Delta.z);
//    spu->heading = FloatToEQ12(m_Position.w);
//    if (IsClient() || IsBot())
//        spu->animation = animation;
//    else
//        spu->animation = pRunAnimSpeed;//animation;

//    spu->delta_heading = FloatToEQ10(m_Delta.w);
// }
//float EQ13toFloat(int d)
//{
//    return static_cast<float>(d) / 64.0f;
//}

//int FloatToEQ13(float d)
//{
//    return static_cast<int>(d * 64.0f);
//}

//float EQ19toFloat(int d)
//{
//    return static_cast<float>(d) / 8.0f;
//}

//int FloatToEQ19(float d)
//{
//    return static_cast<int>(d * 8.0f);
//}

//float EQ12toFloat(int d)
//{
//    return static_cast<float>(d) / 4.0f;
//}

//int FloatToEQ12(float d)
//{
//    return static_cast<int>((d + 2048.0f) * 4.0f) % 2048;
//}

//float EQ10toFloat(int d)
//{
//    return static_cast<float>(d) / 20.0f;
//}

//int FloatToEQ10(float d)
//{
//    return static_cast<int>(d * 20.0f);
//}
#endregion

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the ClientUpdate packet structure for EverQuest network communication.
    /// </summary>
    public struct ClientUpdateToServer : IEQStruct
    {
        public uint Sequence;
        public ushort ID;
        public ushort Vehicle;        
        public UpdatePositionToServer Position;

        public ClientUpdateToServer(uint Sequence, ushort ID, ushort Vehicle, UpdatePositionToServer Position) : this()
        {
            this.Sequence = Sequence;
            this.ID = ID;
            this.Vehicle = Vehicle;
            this.Position = Position;
        }

        public ClientUpdateToServer(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public ClientUpdateToServer(BinaryReader br) : this()
        {
            Unpack(br);
        }
        public void Unpack(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, data.Length - offset))
            {
                using (var br = new BinaryReader(ms))
                {
                    Unpack(br);
                }
            }
        }
        public void Unpack(BinaryReader br)
        {
            Sequence = br.ReadUInt16();
            ID = br.ReadUInt16();       
            Vehicle = br.ReadUInt16();
            br.ReadUInt32(); // unknown0004[4]
            Position = new UpdatePositionToServer(br);
        }

        public byte[] Pack()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    Pack(bw);
                    return ms.ToArray();
                }
            }
        }
        public void Pack(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Sequence);
            bw.Write(Vehicle);
            bw.Write((uint)0); // unknown0004[4]
            Position.Pack(bw);
        }

        public override string ToString()
        {
            var ret = "struct ClientUpdateToServer {\n";
            ret += "\tSequnce = ";
            try
            {
                ret += $"{Indentify(Sequence)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tID = ";
            try
            {
                ret += $"{Indentify(ID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tVehicle = ";
            try
            {
                ret += $"{Indentify(Vehicle)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPosition = ";
            try
            {
                ret += $"{Indentify(Position)}\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }

    public struct UpdatePositionToServer : IEQStruct
    {
        public float DeltaX;
        public short DeltaHeading;
        public float DeltaY;
        public float Y;
        public short Animation;
        public ushort Heading;
        public float X;
        public float Z;
        public float DeltaZ;

        //struct PlayerPositionUpdateClient_Struct
        //{
        //    /*0000*/
        //    uint16 sequence;            // increments one each packet - Verified
        //    /*0002*/
        //    uint16 spawn_id;            // Player's spawn id
        //    /*0004*/
        //    uint16 vehicle_id;          // Player's vehicle spawn id
        //    /*0006*/
        //    uint8 unknown0004[4];       // ***Placeholder
        //    /*0010*/
        //    float delta_x;          // Change in x
        //    /*0014*/
        //    unsigned heading : 12;      // Directional heading
        //    unsigned padding0040 : 20;  // ***Placeholder
        //    /*0018*/
        //    float x_pos;                // x coord (2nd loc value)
        //    /*0022*/
        //    float delta_z;          // Change in z
        //    /*0026*/
        //    float z_pos;                // z coord (3rd loc value)
        //    /*0030*/
        //    float y_pos;                // y coord (1st loc value)
        //    /*0034*/
        //    unsigned animation : 10;        // ***Placeholder
        //    unsigned padding0024 : 22;  // animation
        //    /*0038*/
        //    float delta_y;          // Change in y
        //    /*0042*/
        //    signed delta_heading : 10;  // change in heading
        //    unsigned padding0041 : 22;  // ***Placeholder
        //    /*0046*/
        //};

        public UpdatePositionToServer(float DeltaX, short DeltaHeading, float DeltaY, float Y, short Animation, ushort Heading, float X, float Z, float DeltaZ) : this()
        {
            this.DeltaX = DeltaX;
            this.DeltaHeading = DeltaHeading;
            this.DeltaY = DeltaY;
            this.Y = Y;
            this.Animation = Animation;
            this.Heading = Heading;
            this.X = X;
            this.Z = Z;
            this.DeltaZ = DeltaZ;
        }

        public UpdatePositionToServer(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public UpdatePositionToServer(BinaryReader br) : this()
        {
            Unpack(br);
        }
        public void Unpack(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, data.Length - offset))
            {
                using (var br = new BinaryReader(ms))
                {
                    Unpack(br);
                }
            }
        }
        public void Unpack(BinaryReader br)
        {
            // Server-to-Client format: PlayerPositionUpdateServer_Struct
            // This is a heavily bit-packed structure, NOT individual float values

            // Structure layout:
            // uint16 spawn_id;                    // 2 bytes (handled by parent)
            // Followed by 20 bytes of bit-packed data:
            // signed padding0000 : 12;            // bits 0-11
            // signed delta_x : 13;                // bits 12-24
            // signed padding0005 : 7;             // bits 25-31
            // signed delta_heading : 10;          // bits 32-41
            // signed delta_y : 13;                // bits 42-54
            // signed padding0006 : 9;             // bits 55-63
            // signed y_pos : 19;                  // bits 64-82
            // signed animation : 10;              // bits 83-92
            // signed padding0010 : 3;             // bits 93-95
            // unsigned heading : 12;              // bits 96-107
            // signed x_pos : 19;                  // bits 108-126
            // signed padding0014 : 1;             // bit 127
            // signed z_pos : 19;                  // bits 128-146
            // signed delta_z : 13;                // bits 147-159

            // Read the bit-packed data in 64-bit chunks for easier bit extraction
            ulong firstChunk = br.ReadUInt64();   // Bits 0-63
            ulong secondChunk = br.ReadUInt64();  // Bits 64-127
            uint thirdChunk = br.ReadUInt32();    // Bits 128-159

            // Extract delta_x (bits 12-24, 13-bit signed) and convert from EQ13 format
            int deltaXRaw = (int)((firstChunk >> 12) & 0x1FFF);
            int deltaXSigned = (deltaXRaw >= 0x1000) ? deltaXRaw - 0x2000 : deltaXRaw;
            DeltaX = deltaXSigned / 64.0f; // EQ13 to float conversion

            // Extract delta_heading (bits 32-41, 10-bit signed) and convert from EQ10 format
            int deltaHeadingRaw = (int)((firstChunk >> 32) & 0x3FF);
            int deltaHeadingSigned = (deltaHeadingRaw >= 0x200) ? deltaHeadingRaw - 0x400 : deltaHeadingRaw;
            DeltaHeading = (short)deltaHeadingSigned; // EQ10 format, no conversion needed for heading delta

            // Extract delta_y (bits 42-54, 13-bit signed) and convert from EQ13 format
            int deltaYRaw = (int)((firstChunk >> 42) & 0x1FFF);
            int deltaYSigned = (deltaYRaw >= 0x1000) ? deltaYRaw - 0x2000 : deltaYRaw;
            DeltaY = deltaYSigned / 64.0f; // EQ13 to float conversion

            // Extract y_pos (bits 0-18 of secondChunk, 19-bit signed) and convert from EQ19 format
            int yRaw = (int)(secondChunk & 0x7FFFF);
            int ySigned = (yRaw >= 0x40000) ? yRaw - 0x80000 : yRaw;
            Y = ySigned / 8.0f; // EQ19 to float conversion

            // Extract animation (bits 19-28 of secondChunk, 10-bit signed)
            int animationRaw = (int)((secondChunk >> 19) & 0x3FF);
            Animation = (short)((animationRaw >= 0x200) ? animationRaw - 0x400 : animationRaw);

            // Extract heading (bits 32-43 of secondChunk, 12-bit unsigned) and convert from EQ12 format
            int headingRaw = (int)((secondChunk >> 32) & 0xFFF);
            Heading = (ushort)headingRaw; // EQ12 format, conversion handled elsewhere if needed

            // Extract x_pos (bits 44-62 of secondChunk, 19-bit signed) and convert from EQ19 format
            int xRaw = (int)((secondChunk >> 44) & 0x7FFFF);
            int xSigned = (xRaw >= 0x40000) ? xRaw - 0x80000 : xRaw;
            X = xSigned / 8.0f; // EQ19 to float conversion

            // Extract z_pos (bits 0-18 of thirdChunk, 19-bit signed) and convert from EQ19 format
            int zRaw = (int)(thirdChunk & 0x7FFFF);
            int zSigned = (zRaw >= 0x40000) ? zRaw - 0x80000 : zRaw;
            Z = zSigned / 8.0f; // EQ19 to float conversion

            // Extract delta_z (bits 19-31 of thirdChunk, 13-bit signed) and convert from EQ13 format
            int deltaZRaw = (int)((thirdChunk >> 19) & 0x1FFF);
            int deltaZSigned = (deltaZRaw >= 0x1000) ? deltaZRaw - 0x2000 : deltaZRaw;
            DeltaZ = deltaZSigned / 64.0f; // EQ13 to float conversion
            //Y = Y / 8;
            //X = X / 8;
            //Z = Z / 8;
            //DeltaY = DeltaY / 8;
            //DeltaX = DeltaX / 8;
            //DeltaZ = DeltaZ / 8;
        }

        public byte[] Pack()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    Pack(bw);
                    return ms.ToArray();
                }
            }
        }
        public void Pack(BinaryWriter bw)
        {
            bw.Write(DeltaX);
            bw.Write((ushort)Heading);
            bw.Write((ushort)0);
            bw.Write(X);
            bw.Write(DeltaZ);       
            bw.Write(Z);        
            bw.Write(Y);
            bw.Write((short)Animation);
            bw.Write((short)0);
            bw.Write(DeltaY);
            bw.Write(DeltaHeading);
            bw.Write((short)0);
        }

        public override string ToString()
        {
            var ret = "bitfield UpdatePositionToServer {\n";
            ret += "\tDeltaX = ";
            try
            {
                ret += $"{DeltaX:F3},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDeltaHeading = ";
            try
            {
                ret += $"{Indentify(DeltaHeading)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDeltaY = ";
            try
            {
                ret += $"{DeltaY:F3},\n"; 
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tY = ";
            try
            {
                ret += $"{Y:F3},\n"; 
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAnimation = ";
            try
            {
                ret += $"{Indentify(Animation)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tHeading = ";
            try
            {
                ret += $"{Indentify(Heading)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tX = ";
            try
            {
                ret += $"{X:F3},\n"; 
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tZ = ";
            try
            {
                ret += $"{Z:F3}\n"; 
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDeltaZ = ";
            try
            {
                ret += $"{DeltaZ:F3}\n"; 
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}