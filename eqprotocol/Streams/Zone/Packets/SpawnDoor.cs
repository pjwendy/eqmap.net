using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Door_Struct
// {
// /*0000*/ char    name[32];            // Filename of Door // Was 10char long before... added the 6 in the next unknown to it: Daeken M. BlackBlade
// /*0032*/ float   yPos;               // y loc
// /*0036*/ float   xPos;               // x loc
// /*0040*/ float   zPos;               // z loc
// /*0044*/ float	 heading;
// /*0048*/ uint32   incline;	// rotates the whole door
// /*0052*/ uint32   size;			// 100 is normal, smaller number = smaller model
// /*0054*/ uint8    unknown0054[4]; // 00 00 00 00
// /*0060*/ uint8   doorId;             // door's id #
// /*0061*/ uint8   opentype;
// /*0062*/ uint8  state_at_spawn;
// /*0063*/ uint8  invert_state;	// if this is 1, the door is normally open
// /*0064*/ uint32  door_param; // normally ff ff ff ff (-1)
// /*0068*/ uint32	unknown0068; // 00 00 00 00
// /*0072*/ uint32	unknown0072; // 00 00 00 00
// /*0076*/ uint8	unknown0076; // seen 1 or 0
// /*0077*/ uint8	unknown0077; // seen 1 (always?)
// /*0078*/ uint8	unknown0078; // seen 0 (always?)
// /*0079*/ uint8	unknown0079; // seen 1 (always?)
// /*0080*/ uint8	unknown0080; // seen 0 (always?)
// /*0081*/ uint8	unknown0081; // seen 1 or 0 or rarely 2C or 90 or ED or 2D or A1
// /*0082*/ uint8  unknown0082; // seen 0 or rarely FF or FE or 10 or 5A or 82
// /*0083*/ uint8  unknown0083; // seen 0 or rarely 02 or 7C
// /*0084*/ uint8  unknown0084[8]; // mostly 0s, the last 3 bytes are something tho
// /*0092*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_SpawnDoor)
// {
// SETUP_VAR_ENCODE(Door_Struct);
// int door_count = __packet->size / sizeof(Door_Struct);
// int total_length = door_count * sizeof(structs::Door_Struct);
// ALLOC_VAR_ENCODE(structs::Door_Struct, total_length);
// 
// int r;
// for (r = 0; r < door_count; r++) {
// strcpy(eq[r].name, emu[r].name);
// eq[r].xPos = emu[r].xPos;
// eq[r].yPos = emu[r].yPos;
// eq[r].zPos = emu[r].zPos;
// eq[r].heading = emu[r].heading;
// eq[r].incline = emu[r].incline;
// eq[r].size = emu[r].size;
// eq[r].doorId = emu[r].doorId;
// eq[r].opentype = emu[r].opentype;
// eq[r].state_at_spawn = emu[r].state_at_spawn;
// eq[r].invert_state = emu[r].invert_state;
// eq[r].door_param = emu[r].door_param;
// eq[r].unknown0076 = 0;
// eq[r].unknown0077 = 1; // Both must be 1 to allow clicking doors
// eq[r].unknown0078 = 0;
// eq[r].unknown0079 = 1; // Both must be 1 to allow clicking doors
// eq[r].unknown0080 = 0;
// eq[r].unknown0081 = 0;
// eq[r].unknown0082 = 0;
// }
// 
// FINISH_ENCODE();
// }

//public struct Door : IEQStruct
//{
//    public string Name;
//    public float[] Position;
//    public float Heading;
//    public uint Incline;
//    public uint Size;
//    public byte DoorID;
//    public byte OpenType;
//    public byte StateAtSpawn;
//    public byte InvertState;
//    public uint Param;

//    public Door(string Name, float[] Position, float Heading, uint Incline, uint Size, byte DoorID, byte OpenType, byte StateAtSpawn, byte InvertState, uint Param) : this()
//    {
//        this.Name = Name;
//        this.Position = Position;
//        this.Heading = Heading;
//        this.Incline = Incline;
//        this.Size = Size;
//        this.DoorID = DoorID;
//        this.OpenType = OpenType;
//        this.StateAtSpawn = StateAtSpawn;
//        this.InvertState = InvertState;
//        this.Param = Param;
//    }

//    public Door(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public Door(BinaryReader br) : this()
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
//        Name = br.ReadString(32);
//        Position = new float[3];
//        for (var i = 0; i < 3; ++i)
//        {
//            Position[i] = br.ReadSingle();
//        }
//        Heading = br.ReadSingle();
//        Incline = br.ReadUInt32();
//        Size = br.ReadUInt32();
//        br.ReadBytes(4);
//        DoorID = br.ReadByte();
//        OpenType = br.ReadByte();
//        StateAtSpawn = br.ReadByte();
//        InvertState = br.ReadByte();
//        Param = br.ReadUInt32();
//        br.ReadBytes(24);
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
//        bw.Write(Name.ToBytes(32));
//        for (var i = 0; i < 3; ++i)
//        {
//            bw.Write(Position[i]);
//        }
//        bw.Write(Heading);
//        bw.Write(Incline);
//        bw.Write(Size);
//        bw.Write(new byte[4]);
//        bw.Write(DoorID);
//        bw.Write(OpenType);
//        bw.Write(StateAtSpawn);
//        bw.Write(InvertState);
//        bw.Write(Param);
//        bw.Write(new byte[24]);
//    }

//    public override string ToString()
//    {
//        var ret = "struct Door {\n";
//        ret += "\tName = ";
//        try
//        {
//            ret += $"{Indentify(Name)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPosition = ";
//        try
//        {
//            ret += "{\n";
//            for (int i = 0, e = Position.Length; i < e; ++i)
//                ret += $"\t\t{Indentify(Position[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
//            ret += "\t},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tHeading = ";
//        try
//        {
//            ret += $"{Indentify(Heading)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tIncline = ";
//        try
//        {
//            ret += $"{Indentify(Incline)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tSize = ";
//        try
//        {
//            ret += $"{Indentify(Size)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tDoorID = ";
//        try
//        {
//            ret += $"{Indentify(DoorID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tOpenType = ";
//        try
//        {
//            ret += $"{Indentify(OpenType)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tStateAtSpawn = ";
//        try
//        {
//            ret += $"{Indentify(StateAtSpawn)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tInvertState = ";
//        try
//        {
//            ret += $"{Indentify(InvertState)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tParam = ";
//        try
//        {
//            ret += $"{Indentify(Param)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        return ret + "}";
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the SpawnDoor packet structure for EverQuest network communication.
    /// </summary>
    public struct SpawnDoor : IEQStruct
    {
        public string Name;
        public float[] Position;
        public float Heading;
        public uint Incline;
        public uint Size;
        public byte DoorID;
        public byte OpenType;
        public byte StateAtSpawn;
        public byte InvertState;
        public uint Param;

        public SpawnDoor(string Name, float[] Position, float Heading, uint Incline, uint Size, byte DoorID, byte OpenType, byte StateAtSpawn, byte InvertState, uint Param) : this()
        {
            this.Name = Name;
            this.Position = Position;
            this.Heading = Heading;
            this.Incline = Incline;
            this.Size = Size;
            this.DoorID = DoorID;
            this.OpenType = OpenType;
            this.StateAtSpawn = StateAtSpawn;
            this.InvertState = InvertState;
            this.Param = Param;
        }

        public SpawnDoor(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public SpawnDoor(BinaryReader br) : this()
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
            Name = br.ReadString(32);
            Position = new float[3];
            for (var i = 0; i < 3; ++i)
            {
                Position[i] = br.ReadSingle();
            }
            Heading = br.ReadSingle();
            Incline = br.ReadUInt32();
            Size = br.ReadUInt32();
            br.ReadBytes(4);
            DoorID = br.ReadByte();
            OpenType = br.ReadByte();
            StateAtSpawn = br.ReadByte();
            InvertState = br.ReadByte();
            Param = br.ReadUInt32();
            br.ReadBytes(24);
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
            bw.Write(Name.ToBytes(32));
            for (var i = 0; i < 3; ++i)
            {
                bw.Write(Position[i]);
            }
            bw.Write(Heading);
            bw.Write(Incline);
            bw.Write(Size);
            bw.Write(new byte[4]);
            bw.Write(DoorID);
            bw.Write(OpenType);
            bw.Write(StateAtSpawn);
            bw.Write(InvertState);
            bw.Write(Param);
            bw.Write(new byte[24]);
        }

        public override string ToString()
        {
            var ret = "struct SpawnDoor {\n";
            ret += "\tName = ";
            try
            {
                ret += $"{Indentify(Name)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPosition = ";
            try
            {
                ret += "{\n";
                for (int i = 0, e = Position.Length; i < e; ++i)
                    ret += $"\t\t{Indentify(Position[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
                ret += "\t},\n";
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
            ret += "\tIncline = ";
            try
            {
                ret += $"{Indentify(Incline)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tSize = ";
            try
            {
                ret += $"{Indentify(Size)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDoorID = ";
            try
            {
                ret += $"{Indentify(DoorID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tOpenType = ";
            try
            {
                ret += $"{Indentify(OpenType)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tStateAtSpawn = ";
            try
            {
                ret += $"{Indentify(StateAtSpawn)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tInvertState = ";
            try
            {
                ret += $"{Indentify(InvertState)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tParam = ";
            try
            {
                ret += $"{Indentify(Param)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}