using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct FindableNPC_Struct
// {
// /*000*/	uint32	Action;		// 0 = Add, 1 = Remove
// /*004*/	uint32	EntityID;
// /*008*/	char	Name[64];
// /*072*/	char	LastName[32];
// /*104*/	uint32	Race;
// /*108*/	uint8	Class;
// /*109*/	uint8	Unknown109;	// Observed 0x16
// /*110*/	uint8	Unknown110;	// Observed 0x06
// /*111*/	uint8	Unknown111;	// Observed 0x24
// /*112*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

//public struct FindableNPC : IEQStruct
//{
//    public bool Remove;
//    public uint EntityID;
//    public string Name;
//    public string LastName;
//    public uint Race;
//    public byte Class;

//    public FindableNPC(bool Remove, uint EntityID, string Name, string LastName, uint Race, byte Class) : this()
//    {
//        this.Remove = Remove;
//        this.EntityID = EntityID;
//        this.Name = Name;
//        this.LastName = LastName;
//        this.Race = Race;
//        this.Class = Class;
//    }

//    public FindableNPC(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public FindableNPC(BinaryReader br) : this()
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
//        Remove = br.ReadUInt32() != 0;
//        EntityID = br.ReadUInt32();
//        Name = br.ReadString(64);
//        LastName = br.ReadString(32);
//        Race = br.ReadUInt32();
//        Class = br.ReadByte();
//        br.ReadBytes(3);
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
//        bw.Write((uint)(Remove ? 1 : 0));
//        bw.Write(EntityID);
//        bw.Write(Name.ToBytes(64));
//        bw.Write(LastName.ToBytes(32));
//        bw.Write(Race);
//        bw.Write(Class);
//        bw.Write(new byte[3]);
//    }

//    public override string ToString()
//    {
//        var ret = "struct FindableNPC {\n";
//        ret += "\tRemove = ";
//        try
//        {
//            ret += $"{Indentify(Remove)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tEntityID = ";
//        try
//        {
//            ret += $"{Indentify(EntityID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tName = ";
//        try
//        {
//            ret += $"{Indentify(Name)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLastName = ";
//        try
//        {
//            ret += $"{Indentify(LastName)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tRace = ";
//        try
//        {
//            ret += $"{Indentify(Race)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tClass = ";
//        try
//        {
//            ret += $"{Indentify(Class)},\n";
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
    /// Represents the SendFindableNPCs packet structure for EverQuest network communication.
    /// </summary>
    public struct SendFindableNPCs : IEQStruct
    {
        public bool Remove;
        public uint EntityID;
        public string Name;
        public string LastName;
        public uint Race;
        public byte Class;

        public SendFindableNPCs(bool Remove, uint EntityID, string Name, string LastName, uint Race, byte Class) : this()
        {
            this.Remove = Remove;
            this.EntityID = EntityID;
            this.Name = Name;
            this.LastName = LastName;
            this.Race = Race;
            this.Class = Class;
        }

        public SendFindableNPCs(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public SendFindableNPCs(BinaryReader br) : this()
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
            Remove = br.ReadUInt32() != 0;
            EntityID = br.ReadUInt32();
            Name = br.ReadString(64);
            LastName = br.ReadString(32);
            Race = br.ReadUInt32();
            Class = br.ReadByte();
            br.ReadBytes(3);
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
            bw.Write((uint)(Remove ? 1 : 0));
            bw.Write(EntityID);
            bw.Write(Name.ToBytes(64));
            bw.Write(LastName.ToBytes(32));
            bw.Write(Race);
            bw.Write(Class);
            bw.Write(new byte[3]);
        }

        public override string ToString()
        {
            var ret = "struct SendFindableNPCs {\n";
            ret += "\tRemove = ";
            try
            {
                ret += $"{Indentify(Remove)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tEntityID = ";
            try
            {
                ret += $"{Indentify(EntityID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tName = ";
            try
            {
                ret += $"{Indentify(Name)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLastName = ";
            try
            {
                ret += $"{Indentify(LastName)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tRace = ";
            try
            {
                ret += $"{Indentify(Race)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tClass = ";
            try
            {
                ret += $"{Indentify(Class)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}