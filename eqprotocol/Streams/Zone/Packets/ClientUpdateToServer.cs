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
        public short DeltaX;
        public short DeltaHeading;
        public short DeltaY;
        public int Y;
        public short Animation;
        public ushort Heading;
        public int X;
        public int Z;
        public short DeltaZ;

        public UpdatePositionToServer(short DeltaX, short DeltaHeading, short DeltaY, int Y, short Animation, ushort Heading, int X, int Z, short DeltaZ) : this()
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
            float deltaXFloat = br.ReadSingle();        // 0x000A: delta_x (0.0)
            DeltaX = (short)(deltaXFloat * 64.0f);      // Convert to EQ13 format            
            uint headingData = br.ReadUInt32();         // 0x000E: heading:12, padding:20 (1508)
            Heading = (ushort)(headingData & 0xFFF);    // Extract 12-bit heading
            float xFloat = br.ReadSingle();             // 0x0012: x_pos (136.25)
            X = (int)Math.Round(xFloat * 8.0f);         // Convert to EQ19 format with proper rounding
            float deltaZFloat = br.ReadSingle();        // 0x0016: delta_z (0.0)
            DeltaZ = (short)(deltaZFloat * 64.0f);      // Convert to EQ13 format
            float zFloat = br.ReadSingle();             // 0x001A: z_pos (49.375)
            Z = (int)Math.Round(zFloat * 8.0f);         // Convert to EQ19 format with proper rounding
            float yFloat = br.ReadSingle();             // 0x001E: y_pos (-992.5)
            Y = (int)Math.Round(yFloat * 8.0f);         // Convert to EQ19 format with proper rounding
            uint animData = br.ReadUInt32();            // animation + padding
            Animation = (short)(animData & 0x3FF);      // Extract 10-bit animation
            float deltaYFloat = br.ReadSingle();        // delta_y
            DeltaY = (short)(deltaYFloat * 64.0f);      // Convert to EQ13 format
            uint deltaHeadingData = br.ReadUInt32();    // delta_heading + padding
            int rawDeltaHeading = (int)(deltaHeadingData & 0x3FF);
            DeltaHeading = (short)(rawDeltaHeading >= 512 ? rawDeltaHeading - 1024 : rawDeltaHeading);
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
        }

        public override string ToString()
        {
            var ret = "bitfield UpdatePositionToServer {\n";
            ret += "\tDeltaX = ";
            try
            {
                ret += $"{(DeltaX / 64.0f):F3} {DeltaX:F3},\n"; // Convert EQ13 back to float
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
                ret += $"{(DeltaY / 64.0f):F3} {DeltaY:F3},\n"; // Convert EQ13 back to float
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tY = ";
            try
            {
                ret += $"{(Y / 8.0f):F3} {Y:F3},\n"; // Convert EQ19 back to float
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
                ret += $"{(X / 8.0f):F3} {X:F3},\n"; // Convert EQ19 back to float
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tZ = ";
            try
            {
                ret += $"{(Z / 8.0f):F3}, {Z:F3}\n"; // Convert EQ19 back to float
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDeltaZ = ";
            try
            {
                ret += $"{(DeltaZ / 64.0f):F3} {DeltaZ:F3}\n"; // Convert EQ13 back to float
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}