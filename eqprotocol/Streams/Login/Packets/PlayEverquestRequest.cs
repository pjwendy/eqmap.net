using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition: Not found or not applicable

// ENCODE/DECODE Section:
// Handler function not found.

//public struct PlayRequest : IEQStruct
//{
//    // LoginBaseMessage structure (10 bytes, no padding with #pragma pack(1))
//    public uint Sequence;       // int32_t sequence (4 bytes)
//    public byte Compressed;     // bool compressed (1 byte)
//    public byte EncryptType;    // int8_t encrypt_type (1 byte)
//    public uint Unk3;           // int32_t unk3 (4 bytes)
//                                // PlayEverquestRequest specific
//    public uint ServerRuntimeID; // uint32 server_number (4 bytes)
//                                 // Total: 14 bytes

//    public PlayRequest(uint Sequence, uint ServerRuntimeID) : this()
//    {
//        this.Sequence = Sequence;
//        this.Compressed = 0;     // Not compressed
//        this.EncryptType = 0;    // No encryption
//        this.Unk3 = 0;          // Unused
//        this.ServerRuntimeID = ServerRuntimeID;
//    }

//    public PlayRequest(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public PlayRequest(BinaryReader br) : this()
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
//        Sequence = br.ReadUInt32();
//        Compressed = br.ReadByte();
//        EncryptType = br.ReadByte();
//        // No padding - #pragma pack(1) in server code
//        Unk3 = br.ReadUInt32();
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
//        bw.Write(Compressed);
//        bw.Write(EncryptType);
//        // No padding - #pragma pack(1) in server code
//        bw.Write(Unk3);
//        bw.Write(ServerRuntimeID);
//    }

//    public override string ToString()
//    {
//        var ret = "struct PlayRequest {\n";
//        ret += "\tSequence = ";
//        try
//        {
//            ret += $"{Indentify(Sequence)},\n";
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
    /// Represents the PlayEverquestRequest packet structure for EverQuest network communication.
    /// </summary>
    public struct PlayEverquestRequest : IEQStruct
    {
        // LoginBaseMessage structure (10 bytes, no padding with #pragma pack(1))
        public uint Sequence;       // int32_t sequence (4 bytes)
        public byte Compressed;     // bool compressed (1 byte)
        public byte EncryptType;    // int8_t encrypt_type (1 byte)
        public uint Unk3;           // int32_t unk3 (4 bytes)
                                    // PlayEverquestRequest specific
        public uint ServerRuntimeID; // uint32 server_number (4 bytes)
                                     // Total: 14 bytes

        public PlayEverquestRequest(uint Sequence, uint ServerRuntimeID) : this()
        {
            this.Sequence = Sequence;
            Compressed = 0;     // Not compressed
            EncryptType = 0;    // No encryption
            Unk3 = 0;          // Unused
            this.ServerRuntimeID = ServerRuntimeID;
        }

        public PlayEverquestRequest(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public PlayEverquestRequest(BinaryReader br) : this()
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
            Sequence = br.ReadUInt32();
            Compressed = br.ReadByte();
            EncryptType = br.ReadByte();
            // No padding - #pragma pack(1) in server code
            Unk3 = br.ReadUInt32();
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
            bw.Write(Compressed);
            bw.Write(EncryptType);
            // No padding - #pragma pack(1) in server code
            bw.Write(Unk3);
            bw.Write(ServerRuntimeID);
        }

        public override string ToString()
        {
            var ret = "struct PlayRequest {\n";
            ret += "\tSequence = ";
            try
            {
                ret += $"{Indentify(Sequence)},\n";
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