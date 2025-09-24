using EQProtocol.Streams.Common;
using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition: Not found or not applicable

// ENCODE/DECODE Section:
// Handler function not found.

//    public LoginReply(uint AcctID, string Key, uint FailedAttempts) : this()
//    {
//        this.AcctID = AcctID;
//        this.Key = Key;
//        this.FailedAttempts = FailedAttempts;
//    }

//    public LoginReply(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public LoginReply(BinaryReader br) : this()
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
//        message = br.ReadByte();
//        unk1 = br.ReadByte();
//        unk2 = br.ReadByte();
//        unk3 = br.ReadByte();
//        unk4 = br.ReadByte();
//        unk5 = br.ReadByte();
//        unk6 = br.ReadByte();
//        unk7 = br.ReadByte();
//        AcctID = br.ReadUInt32();
//        Key = br.ReadString(11);
//        FailedAttempts = br.ReadUInt32();
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
//        bw.Write(message);
//        bw.Write(unk1);
//        bw.Write(unk2);
//        bw.Write(unk3);
//        bw.Write(unk4);
//        bw.Write(unk5);
//        bw.Write(unk6);
//        bw.Write(unk7);
//        bw.Write(AcctID);
//        bw.Write(Key.ToBytes(11));
//        bw.Write(FailedAttempts);
//    }

//    public override string ToString()
//    {
//        var ret = "struct LoginReply {\n";
//        ret += "\tAcctID = ";
//        try
//        {
//            ret += $"{Indentify(AcctID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tKey = ";
//        try
//        {
//            ret += $"{Indentify(Key)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tFailedAttempts = ";
//        try
//        {
//            ret += $"{Indentify(FailedAttempts)}\n";
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
    /// Represents the LoginAccepted packet structure for EverQuest network communication.
    /// </summary>
    public struct LoginAccepted : IEQStruct
    {
        byte message;
        byte unk1;
        byte unk2;
        byte unk3;
        byte unk4;
        byte unk5;
        byte unk6;
        byte unk7;
        public uint AcctID;
        public string Key;
        public uint FailedAttempts;

        public LoginAccepted(uint AcctID, string Key, uint FailedAttempts) : this()
        {
            this.AcctID = AcctID;
            this.Key = Key;
            this.FailedAttempts = FailedAttempts;
        }

        public LoginAccepted(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public LoginAccepted(BinaryReader br) : this()
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
            message = br.ReadByte();
            unk1 = br.ReadByte();
            unk2 = br.ReadByte();
            unk3 = br.ReadByte();
            unk4 = br.ReadByte();
            unk5 = br.ReadByte();
            unk6 = br.ReadByte();
            unk7 = br.ReadByte();
            AcctID = br.ReadUInt32();
            Key = br.ReadString(11);
            FailedAttempts = br.ReadUInt32();
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
            bw.Write(message);
            bw.Write(unk1);
            bw.Write(unk2);
            bw.Write(unk3);
            bw.Write(unk4);
            bw.Write(unk5);
            bw.Write(unk6);
            bw.Write(unk7);
            bw.Write(AcctID);
            bw.Write(Key.ToBytes(11));
            bw.Write(FailedAttempts);
        }

        public override string ToString()
        {
            var ret = "struct LoginAccepted {\n";
            ret += "\tAcctID = ";
            try
            {
                ret += $"{Indentify(AcctID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tKey = ";
            try
            {
                ret += $"{Indentify(Key)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tFailedAttempts = ";
            try
            {
                ret += $"{Indentify(FailedAttempts)}\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}