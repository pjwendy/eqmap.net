using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;
using static EQProtocol.Streams.Zone.Constants;
using EQProtocol.Streams.Common;

// C++ Structure Definition:
// struct BandolierCreate_Struct
// {
// /*00*/	uint32 Action;		//0 for create
// /*04*/	uint8 Number;
// /*05*/	char Name[32];
// /*37*/	uint16 Unknown37;	//seen 0x93FD
// /*39*/	uint8 Unknown39;	//0
// };

// ENCODE/DECODE Section:
// Handler function not found.

//public struct Bandolier : IEQStruct
//{
//    public string Name;
//    public PotionBandolierItem[] Items;

//    public Bandolier(string Name, PotionBandolierItem[] Items) : this()
//    {
//        this.Name = Name;
//        this.Items = Items;
//    }

//    public Bandolier(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public Bandolier(BinaryReader br) : this()
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
//        Items = new PotionBandolierItem[BANDOLIER_ITEM_COUNT];
//        for (var i = 0; i < BANDOLIER_ITEM_COUNT; ++i)
//        {
//            Items[i] = new PotionBandolierItem(br);
//        }
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
//        for (var i = 0; i < BANDOLIER_ITEM_COUNT; ++i)
//        {
//            Items[i].Pack(bw);
//        }
//    }

//    public override string ToString()
//    {
//        var ret = "struct Bandolier {\n";
//        ret += "\tName = ";
//        try
//        {
//            ret += $"{Indentify(Name)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tItems = ";
//        try
//        {
//            ret += "{\n";
//            for (int i = 0, e = Items.Length; i < e; ++i)
//                ret += $"\t\t{Indentify(Items[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
//            ret += "\t}\n";
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
    /// Represents the Bandolier packet structure for EverQuest network communication.
    /// </summary>
    public struct Bandolier : IEQStruct
    {
        public string Name;
        public PotionBandolierItem[] Items;

        public Bandolier(string Name, PotionBandolierItem[] Items) : this()
        {
            this.Name = Name;
            this.Items = Items;
        }

        public Bandolier(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public Bandolier(BinaryReader br) : this()
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
            Items = new PotionBandolierItem[BANDOLIER_ITEM_COUNT];
            for (var i = 0; i < BANDOLIER_ITEM_COUNT; ++i)
            {
                Items[i] = new PotionBandolierItem(br);
            }
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
            for (var i = 0; i < BANDOLIER_ITEM_COUNT; ++i)
            {
                Items[i].Pack(bw);
            }
        }

        public override string ToString()
        {
            var ret = "struct Bandolier {\n";
            ret += "\tName = ";
            try
            {
                ret += $"{Indentify(Name)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tItems = ";
            try
            {
                ret += "{\n";
                for (int i = 0, e = Items.Length; i < e; ++i)
                    ret += $"\t\t{Indentify(Items[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
                ret += "\t}\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}