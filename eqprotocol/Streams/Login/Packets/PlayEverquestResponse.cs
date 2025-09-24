using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition: Not found or not applicable

// ENCODE/DECODE Section:
// Handler function not found.

//public struct PlayResponse : IEQStruct
//{
//    public byte Sequence;
//    uint unk1;
//    uint unk2;
//    byte unk3;
//    public bool Allowed;
//    public ushort Message;
//    ushort unk4;
//    byte unk5;
//    public uint ServerRuntimeID;

//    public PlayResponse(byte Sequence, bool Allowed, ushort Message, uint ServerRuntimeID) : this()
//    {
//        this.Sequence = Sequence;
//        this.Allowed = Allowed;
//        this.Message = Message;
//        this.ServerRuntimeID = ServerRuntimeID;
//    }

//    public PlayResponse(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public PlayResponse(BinaryReader br) : this()
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
//        Sequence = br.ReadByte();
//        unk1 = br.ReadUInt32();
//        unk2 = br.ReadUInt32();
//        unk3 = br.ReadByte();
//        Allowed = br.ReadByte() != 0;
//        Message = br.ReadUInt16();
//        unk4 = br.ReadUInt16();
//        unk5 = br.ReadByte();
//        ServerRuntimeID = br.ReadUInt32();
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
//        bw.Write(Sequence);
//        bw.Write(unk1);
//        bw.Write(unk2);
//        bw.Write(unk3);
//        bw.Write((byte)(Allowed ? 1 : 0));
//        bw.Write(Message);
//        bw.Write(unk4);
//        bw.Write(unk5);
//        bw.Write(ServerRuntimeID);
//    }

//    public override string ToString()
//    {
//        var ret = "struct PlayResponse {\n";
//        ret += "\tSequence = ";
//        try
//        {
//            ret += $"{Indentify(Sequence)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAllowed = ";
//        try
//        {
//            ret += $"{Indentify(Allowed)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tMessage = ";
//        try
//        {
//            ret += $"{Indentify(Message)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tServerRuntimeID = ";
//        try
//        {
//            ret += $"{Indentify(ServerRuntimeID)}\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        return ret + "}";
//    }
//}

namespace EQProtocol.Streams.Login.Packets {
    /// <summary>
    /// Represents the PlayEverquestResponse packet structure for EverQuest network communication.
    /// </summary>
    public struct PlayEverquestResponse : IEQStruct
    {
        public byte Sequence;
        uint unk1;
        uint unk2;
        byte unk3;
        public bool Allowed;
        public ushort Message;
        ushort unk4;
        byte unk5;
        public uint ServerRuntimeID;

        public PlayEverquestResponse(byte Sequence, bool Allowed, ushort Message, uint ServerRuntimeID) : this()
        {
            this.Sequence = Sequence;
            this.Allowed = Allowed;
            this.Message = Message;
            this.ServerRuntimeID = ServerRuntimeID;
        }

        public PlayEverquestResponse(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public PlayEverquestResponse(BinaryReader br) : this()
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
            Sequence = br.ReadByte();
            unk1 = br.ReadUInt32();
            unk2 = br.ReadUInt32();
            unk3 = br.ReadByte();
            Allowed = br.ReadByte() != 0;
            Message = br.ReadUInt16();
            unk4 = br.ReadUInt16();
            unk5 = br.ReadByte();
            ServerRuntimeID = br.ReadUInt32();
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
            bw.Write(Sequence);
            bw.Write(unk1);
            bw.Write(unk2);
            bw.Write(unk3);
            bw.Write((byte)(Allowed ? 1 : 0));
            bw.Write(Message);
            bw.Write(unk4);
            bw.Write(unk5);
            bw.Write(ServerRuntimeID);
        }

        public override string ToString()
        {
            var ret = "struct PlayEverquestResponse {\n";
            ret += "\tSequence = ";
            try
            {
                ret += $"{Indentify(Sequence)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAllowed = ";
            try
            {
                ret += $"{Indentify(Allowed)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tMessage = ";
            try
            {
                ret += $"{Indentify(Message)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tServerRuntimeID = ";
            try
            {
                ret += $"{Indentify(ServerRuntimeID)}\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}