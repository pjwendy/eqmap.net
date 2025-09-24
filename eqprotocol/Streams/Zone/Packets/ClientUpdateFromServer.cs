using EQProtocol.Streams.Common;
using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
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
    public struct ClientUpdateFromServer : IEQStruct
    {
        public uint Sequence;
        public ushort ID;
        public UpdatePositionFromServer Position;

        public ClientUpdateFromServer(ushort ID, UpdatePositionFromServer Position) : this()
        {
            this.ID = ID;
            this.Position = Position;
        }

        public ClientUpdateFromServer(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public ClientUpdateFromServer(BinaryReader br) : this()
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
            ID = br.ReadUInt16();       
            Position = new UpdatePositionFromServer(br);
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
            var ret = "struct ClientUpdateFromServer {\n";
            ret += "\tID = ";
            try
            {
                ret += $"{Indentify(ID)},\n";
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
    public struct UpdatePositionFromServer : IEQStruct
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

        public UpdatePositionFromServer(short DeltaX, short DeltaHeading, short DeltaY, int Y, short Animation, ushort Heading, int X, int Z, short DeltaZ) : this()
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

        public UpdatePositionFromServer(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public UpdatePositionFromServer(BinaryReader br) : this()
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
        // struct PlayerPositionUpdateServer_Struct
        // {
        //    /*0000*/
        //    uint16 spawn_id;
        //    /*0002*/
        //    signed padding0000 : 12;    // ***Placeholder
        //    signed delta_x : 13;        // change in x
        //    signed padding0005 : 7; // ***Placeholder
        //    /*0006*/
        //    signed delta_heading : 10;  // change in heading
        //    signed delta_y : 13;        // change in y
        //    signed padding0006 : 9; // ***Placeholder
        //    /*0010*/
        //    signed y_pos : 19;          // y coord
        //    signed animation : 10;      // animation
        //    signed padding0010 : 3; // ***Placeholder
        //    /*0014*/
        //    unsigned heading : 12;      // heading
        //    signed x_pos : 19;          // x coord
        //    signed padding0014 : 1; // ***Placeholder
        //    /*0018*/
        //    signed z_pos : 19;          // z coord
        //    signed delta_z : 13;        // change in z
        //    /*0022*/
        // };        
            ulong _databuf;
            _databuf = br.ReadUInt64();
            DeltaX = (short)(((int)(_databuf >> 12 & 0x1FFF) ^ 0x1000) - 0x1000);
            DeltaHeading = (short)(((int)(_databuf >> 32 & 0x3FF) ^ 0x200) - 0x200);
            DeltaY = (short)(((int)(_databuf >> 42 & 0x1FFF) ^ 0x1000) - 0x1000);
            _databuf = br.ReadUInt64();
            Y = ((int)(_databuf & 0x7FFFF) ^ 0x40000) - 0x40000;
            Animation = (short)(((int)(_databuf >> 19 & 0x3FF) ^ 0x200) - 0x200);
            Heading = (ushort)(_databuf >> 32 & 0xFFF);
            X = ((int)(_databuf >> 44 & 0x7FFFF) ^ 0x40000) - 0x40000;
            _databuf = br.ReadUInt32();
            Z = ((int)(_databuf & 0x7FFFF) ^ 0x40000) - 0x40000;
            DeltaZ = (short)(((int)(_databuf >> 19 & 0x1FFF) ^ 0x1000) - 0x1000);
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
            var ret = "bitfield UpdatePositionFromServer {\n";
            ret += "\tDeltaX = ";
            try
            {
                ret += $"{Indentify(DeltaX)},\n";
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
                ret += $"{Indentify(DeltaY)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tY = ";
            try
            {
                ret += $"{Indentify(Y)},\n";
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
                ret += $"{Indentify(X)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tZ = ";
            try
            {
                ret += $"{Indentify(Z)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDeltaZ = ";
            try
            {
                ret += $"{Indentify(DeltaZ)}\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}

