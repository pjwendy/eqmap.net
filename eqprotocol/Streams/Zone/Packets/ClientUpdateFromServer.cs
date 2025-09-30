using EQProtocol.Streams.Common;
using OpenEQ.Netcode;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
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
    /// This packet is sent from the server to update a client about another player's/NPC's position and movement.
    /// </summary>
    public struct ClientUpdateFromServer : IEQStruct
    {
        /// <summary>
        /// Packet sequence number for ordering and duplicate detection
        /// </summary>
        public uint Sequence;

        /// <summary>
        /// The spawn ID of the entity being updated (player or NPC)
        /// </summary>
        public ushort ID;

        /// <summary>
        /// The position and movement data for the entity
        /// </summary>
        public UpdatePositionFromServer Position;

        /// <summary>
        /// Initializes a new ClientUpdateFromServer with the specified ID and position data.
        /// </summary>
        /// <param name="ID">The spawn ID of the entity</param>
        /// <param name="Position">The position and movement data</param>
        public ClientUpdateFromServer(ushort ID, UpdatePositionFromServer Position) : this()
        {
            this.ID = ID;
            this.Position = Position;
        }

        /// <summary>
        /// Initializes a new ClientUpdateFromServer by unpacking data from a byte array.
        /// </summary>
        /// <param name="data">The binary data to unpack</param>
        /// <param name="offset">The offset in the data array to start unpacking from</param>
        public ClientUpdateFromServer(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }

        /// <summary>
        /// Initializes a new ClientUpdateFromServer by unpacking data from a BinaryReader.
        /// </summary>
        /// <param name="br">The BinaryReader to read data from</param>
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
    /// <summary>
    /// Represents position and movement data for entities in EverQuest.
    /// Contains absolute position coordinates and delta (movement) values for smooth interpolation.
    /// </summary>
    public struct UpdatePositionFromServer : IEQStruct
    {
        /// <summary>
        /// Change in X coordinate since last update (for smooth movement interpolation)
        /// </summary>
        public float DeltaX;

        /// <summary>
        /// Change in heading direction since last update
        /// </summary>
        public short DeltaHeading;

        /// <summary>
        /// Change in Y coordinate since last update (for smooth movement interpolation)
        /// </summary>
        public float DeltaY;

        /// <summary>
        /// Current Y coordinate (north-south position)
        /// </summary>
        public float Y;

        /// <summary>
        /// Current animation state/type being performed
        /// </summary>
        public short Animation;

        /// <summary>
        /// Current heading direction (0-4095, representing 0-360 degrees)
        /// </summary>
        public ushort Heading;

        /// <summary>
        /// Current X coordinate (east-west position)
        /// </summary>
        public float X;

        /// <summary>
        /// Current Z coordinate (vertical position/height)
        /// </summary>
        public float Z;

        /// <summary>
        /// Change in Z coordinate since last update (for smooth movement interpolation)
        /// </summary>
        public float DeltaZ;

        public UpdatePositionFromServer(float DeltaX, short DeltaHeading, float DeltaY, float Y, short Animation, ushort Heading, float X, float Z, float DeltaZ) : this()
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
            //ulong _databuf;
            //_databuf = br.ReadUInt64();
            //DeltaX = (short)(((int)(_databuf >> 12 & 0x1FFF) ^ 0x1000) - 0x1000);
            //DeltaHeading = (short)(((int)(_databuf >> 32 & 0x3FF) ^ 0x200) - 0x200);
            //DeltaY = (short)(((int)(_databuf >> 42 & 0x1FFF) ^ 0x1000) - 0x1000);
            //_databuf = br.ReadUInt64();
            //Y = ((int)(_databuf & 0x7FFFF) ^ 0x40000) - 0x40000;
            //Animation = (short)(((int)(_databuf >> 19 & 0x3FF) ^ 0x200) - 0x200);
            //Heading = (ushort)(_databuf >> 32 & 0xFFF);
            //X = ((int)(_databuf >> 44 & 0x7FFFF) ^ 0x40000) - 0x40000;
            //_databuf = br.ReadUInt32();
            //Z = ((int)(_databuf & 0x7FFFF) ^ 0x40000) - 0x40000;
            //DeltaZ = (short)(((int)(_databuf >> 19 & 0x1FFF) ^ 0x1000) - 0x1000);

            static int SignExtend(int v, int bits) => (v << (32 - bits)) >> (32 - bits);

            // 5 words = 20 bytes
            uint w0 = br.ReadUInt32(); // [12 pad][13 delta_x][7 pad]
            uint w1 = br.ReadUInt32(); // [10 delta_heading][13 delta_y][9 pad]
            uint w2 = br.ReadUInt32(); // [19 y_pos][10 animation][3 pad]
            uint w3 = br.ReadUInt32(); // [12 heading(u)][19 x_pos][1 pad]
            uint w4 = br.ReadUInt32(); // [19 z_pos][13 delta_z]

            // word 0: [12 pad][13 delta_x][7 pad]
            DeltaX = SignExtend((int)((w0 >> 12) & 0x1FFF), 13);

            // word 1: [10 delta_heading][13 delta_y][9 pad]
            DeltaHeading = (short)SignExtend((int)((w1 >> 0) & 0x03FF), 10);
            DeltaY = SignExtend((int)((w1 >> 10) & 0x1FFF), 13);

            // word 2: [19 y_pos][10 animation][3 pad]
            Y = SignExtend((int)((w2 >> 0) & 0x7FFFF), 19);
            Animation = (short)SignExtend((int)((w2 >> 19) & 0x03FF), 10);

            // word 3: [12 heading(u)][19 x_pos][1 pad]
            Heading = (ushort)((w3 >> 0) & 0x0FFF);             // unsigned 12-bit
            X = SignExtend((int)((w3 >> 12) & 0x7FFFF), 19);

            // word 4: [19 z_pos][13 delta_z]
            Z = SignExtend((int)((w4 >> 0) & 0x7FFFF), 19);
            DeltaZ = SignExtend((int)((w4 >> 19) & 0x1FFF), 13);

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

