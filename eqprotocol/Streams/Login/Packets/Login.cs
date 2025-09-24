using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition: Not found or not applicable

// ENCODE/DECODE Section:
// Handler function not found.

//public struct Login : IEQStruct
//{
//    ushort unk1;
//    ushort unk2;
//    ushort unk3;
//    ushort unk4;
//    ushort unk5;

//    public Login(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public Login(BinaryReader br) : this()
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
//        unk1 = br.ReadUInt16();
//        unk2 = br.ReadUInt16();
//        unk3 = br.ReadUInt16();
//        unk4 = br.ReadUInt16();
//        unk5 = br.ReadUInt16();
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
//        bw.Write(unk1);
//        bw.Write(unk2);
//        bw.Write(unk3);
//        bw.Write(unk4);
//        bw.Write(unk5);
//    }

//    public override string ToString()
//    {
//        var ret = "struct Login {\n";
//        return ret + "}";
//    }
//}

namespace EQProtocol.Streams.Login.Packets {
    /// <summary>
    /// Represents the Login packet structure for EverQuest network communication.
    /// </summary>
    public struct Login : IEQStruct
    {
        ushort unk1;
        ushort unk2;
        ushort unk3;
        ushort unk4;
        ushort unk5;

        public Login(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public Login(BinaryReader br) : this()
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
            unk1 = br.ReadUInt16();
            unk2 = br.ReadUInt16();
            unk3 = br.ReadUInt16();
            unk4 = br.ReadUInt16();
            unk5 = br.ReadUInt16();
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
            bw.Write(unk1);
            bw.Write(unk2);
            bw.Write(unk3);
            bw.Write(unk4);
            bw.Write(unk5);
        }

        public override string ToString()
        {
            var ret = "struct Login {\n";
            return ret + "}";
        }
    }
}