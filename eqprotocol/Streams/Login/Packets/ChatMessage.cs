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

//public struct ChannelMessage : IEQStruct
//{
//    public string To;
//    public string From;
//    uint unknown029;
//    public uint Language;
//    public uint ChanNum;
//    uint unknown030;
//    byte unknown031;
//    public uint LanguageSkill;
//    public string Message;

//    public ChannelMessage(string To, string From, uint Language, uint ChanNum, uint LanguageSkill, string Message) : this()
//    {
//        this.To = To;
//        this.From = From;
//        this.Language = Language;
//        this.ChanNum = ChanNum;
//        this.LanguageSkill = LanguageSkill;
//        this.Message = Message;
//    }

//    public ChannelMessage(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public ChannelMessage(BinaryReader br) : this()
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
//        To = br.ReadString(-1);
//        From = br.ReadString(-1);
//        unknown029 = br.ReadUInt32();
//        Language = br.ReadUInt32();
//        ChanNum = br.ReadUInt32();
//        unknown030 = br.ReadUInt32();
//        unknown031 = br.ReadByte();
//        LanguageSkill = br.ReadUInt32();
//        Message = br.ReadString(-1);
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
//        bw.Write(To.ToBytes(-1));
//        bw.Write(From.ToBytes(-1));
//        bw.Write(unknown029);
//        bw.Write(Language);
//        bw.Write(ChanNum);
//        bw.Write(unknown030);
//        bw.Write(unknown031);
//        bw.Write(LanguageSkill);
//        bw.Write(Message.ToBytes(-1));
//    }

//    public override string ToString()
//    {
//        var ret = "struct ChannelMessage {\n";
//        ret += "\tTo = ";
//        try
//        {
//            ret += $"{Indentify(To)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tFrom = ";
//        try
//        {
//            ret += $"{Indentify(From)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLanguage = ";
//        try
//        {
//            ret += $"{Indentify(Language)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tChanNum = ";
//        try
//        {
//            ret += $"{Indentify(ChanNum)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLanguageSkill = ";
//        try
//        {
//            ret += $"{Indentify(LanguageSkill)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tMessage = ";
//        try
//        {
//            ret += $"{Indentify(Message)}\n";
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
    /// Represents the ChatMessage packet structure for EverQuest network communication.
    /// </summary>
    public struct ChatMessage : IEQStruct
    {
        public string To;
        public string From;
        uint unknown029;
        public uint Language;
        public uint ChanNum;
        uint unknown030;
        byte unknown031;
        public uint LanguageSkill;
        public string Message;

        public ChatMessage(string To, string From, uint Language, uint ChanNum, uint LanguageSkill, string Message) : this()
        {
            this.To = To;
            this.From = From;
            this.Language = Language;
            this.ChanNum = ChanNum;
            this.LanguageSkill = LanguageSkill;
            this.Message = Message;
        }

        public ChatMessage(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public ChatMessage(BinaryReader br) : this()
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
            To = br.ReadString(-1);
            From = br.ReadString(-1);
            unknown029 = br.ReadUInt32();
            Language = br.ReadUInt32();
            ChanNum = br.ReadUInt32();
            unknown030 = br.ReadUInt32();
            unknown031 = br.ReadByte();
            LanguageSkill = br.ReadUInt32();
            Message = br.ReadString(-1);
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
            bw.Write(To.ToBytes(-1));
            bw.Write(From.ToBytes(-1));
            bw.Write(unknown029);
            bw.Write(Language);
            bw.Write(ChanNum);
            bw.Write(unknown030);
            bw.Write(unknown031);
            bw.Write(LanguageSkill);
            bw.Write(Message.ToBytes(-1));
        }

        public override string ToString()
        {
            var ret = "struct ChatMessage {\n";
            ret += "\tTo = ";
            try
            {
                ret += $"{Indentify(To)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tFrom = ";
            try
            {
                ret += $"{Indentify(From)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLanguage = ";
            try
            {
                ret += $"{Indentify(Language)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tChanNum = ";
            try
            {
                ret += $"{Indentify(ChanNum)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLanguageSkill = ";
            try
            {
                ret += $"{Indentify(LanguageSkill)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tMessage = ";
            try
            {
                ret += $"{Indentify(Message)}\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}