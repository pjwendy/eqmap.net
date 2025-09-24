using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ChannelMessage_Struct
// {
// /*000*/	char	targetname[64];		// Tell recipient
// /*064*/	char	sender[64];			// The senders name (len might be wrong)
// /*128*/	uint32	language;			// Language
// /*132*/	uint32	chan_num;			// Channel
// /*136*/	uint32	cm_unknown4[2];		// ***Placeholder
// /*144*/	uint32	skill_in_language;	// The players skill in this language? might be wrong
// /*148*/	char	message[0];			// Variable length message
// };

// ENCODE/DECODE Section:
// DECODE(OP_ChannelMessage)
// {
// unsigned char *__eq_buffer = __packet->pBuffer;
// 
// char *InBuffer = (char *)__eq_buffer;
// 
// char Sender[64];
// char Target[64];
// 
// VARSTRUCT_DECODE_STRING(Sender, InBuffer);
// VARSTRUCT_DECODE_STRING(Target, InBuffer);
// 
// InBuffer += 4;
// 
// uint32 Language = VARSTRUCT_DECODE_TYPE(uint32, InBuffer);
// uint32 Channel = VARSTRUCT_DECODE_TYPE(uint32, InBuffer);
// 
// InBuffer += 5;
// 
// uint32 Skill = VARSTRUCT_DECODE_TYPE(uint32, InBuffer);
// 
// std::string old_message = InBuffer;
// std::string new_message;
// UFToServerSayLink(new_message, old_message);
// 
// //__packet->size = sizeof(ChannelMessage_Struct)+strlen(InBuffer) + 1;
// __packet->size = sizeof(ChannelMessage_Struct) + new_message.length() + 1;
// 
// __packet->pBuffer = new unsigned char[__packet->size];
// ChannelMessage_Struct *emu = (ChannelMessage_Struct *)__packet->pBuffer;
// 
// strn0cpy(emu->targetname, Target, sizeof(emu->targetname));
// strn0cpy(emu->sender, Target, sizeof(emu->sender));
// emu->language = Language;
// emu->chan_num = Channel;
// emu->skill_in_language = Skill;
// strcpy(emu->message, new_message.c_str());
// 
// delete[] __eq_buffer;
//// }
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

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the ChannelMessage packet structure for EverQuest network communication.
    /// </summary>
    public struct ChannelMessage : IEQStruct
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

        public ChannelMessage(string To, string From, uint Language, uint ChanNum, uint LanguageSkill, string Message) : this()
        {
            this.To = To;
            this.From = From;
            this.Language = Language;
            this.ChanNum = ChanNum;
            this.LanguageSkill = LanguageSkill;
            this.Message = Message;
        }

        public ChannelMessage(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public ChannelMessage(BinaryReader br) : this()
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
            var ret = "struct ChannelMessage {\n";
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