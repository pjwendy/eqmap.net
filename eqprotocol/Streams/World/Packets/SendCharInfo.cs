using EQProtocol.Streams.Common;
using EQProtocol.Streams.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct CharacterSelect_Struct
// {
// /*0000*/	uint32 CharCount;	//number of chars in this packet
// /*0004*/	uint32 TotalChars;	//total number of chars allowed?
// /*0008*/	CharacterSelectEntry_Struct Entries[0];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_SendCharInfo)
// {
// ENCODE_LENGTH_ATLEAST(CharacterSelect_Struct);
// SETUP_VAR_ENCODE(CharacterSelect_Struct);
// 
// // Zero-character count shunt
// if (emu->CharCount == 0) {
// ALLOC_VAR_ENCODE(structs::CharacterSelect_Struct, sizeof(structs::CharacterSelect_Struct));
// eq->CharCount = emu->CharCount;
// eq->TotalChars = emu->TotalChars;
// 
// if (eq->TotalChars > constants::CHARACTER_CREATION_LIMIT)
// eq->TotalChars = constants::CHARACTER_CREATION_LIMIT;
// 
// // Special Underfoot adjustment - field should really be 'AdditionalChars' or 'BonusChars'
// uint32 adjusted_total = eq->TotalChars - 8; // Yes, it rolls under for '< 8' - probably an int32 field
// eq->TotalChars = adjusted_total;
// 
// FINISH_ENCODE();
// return;
// }
// 
// unsigned char *emu_ptr = __emu_buffer;
// emu_ptr += sizeof(CharacterSelect_Struct);
// CharacterSelectEntry_Struct *emu_cse = (CharacterSelectEntry_Struct *)nullptr;
// 
// size_t names_length = 0;
// size_t character_count = 0;
// for (; character_count < emu->CharCount && character_count < constants::CHARACTER_CREATION_LIMIT; ++character_count) {
// emu_cse = (CharacterSelectEntry_Struct *)emu_ptr;
// names_length += strlen(emu_cse->Name);
// emu_ptr += sizeof(CharacterSelectEntry_Struct);
// }
// 
// size_t total_length = sizeof(structs::CharacterSelect_Struct)
// + character_count * sizeof(structs::CharacterSelectEntry_Struct)
// + names_length;
// 
// ALLOC_VAR_ENCODE(structs::CharacterSelect_Struct, total_length);
// structs::CharacterSelectEntry_Struct *eq_cse = (structs::CharacterSelectEntry_Struct *)nullptr;
// 
// eq->CharCount = character_count;
// eq->TotalChars = emu->TotalChars;
// 
// if (eq->TotalChars > constants::CHARACTER_CREATION_LIMIT)
// eq->TotalChars = constants::CHARACTER_CREATION_LIMIT;
// 
// // Special Underfoot adjustment - field should really be 'AdditionalChars' or 'BonusChars' in this client
// uint32 adjusted_total = eq->TotalChars - 8; // Yes, it rolls under for '< 8' - probably an int32 field
// eq->TotalChars = adjusted_total;
// 
// emu_ptr = __emu_buffer;
// emu_ptr += sizeof(CharacterSelect_Struct);
// 
// unsigned char *eq_ptr = __packet->pBuffer;
// eq_ptr += sizeof(structs::CharacterSelect_Struct);
// 
// for (int counter = 0; counter < character_count; ++counter) {
// emu_cse = (CharacterSelectEntry_Struct *)emu_ptr;
// eq_cse = (structs::CharacterSelectEntry_Struct *)eq_ptr; // base address
// 
// eq_cse->Level = emu_cse->Level;
// eq_cse->HairStyle = emu_cse->HairStyle;
// eq_cse->Gender = emu_cse->Gender;
// 
// strcpy(eq_cse->Name, emu_cse->Name);
// eq_ptr += strlen(emu_cse->Name);
// eq_cse = (structs::CharacterSelectEntry_Struct *)eq_ptr; // offset address (base + name length offset)
// eq_cse->Name[0] = '\0'; // (offset)eq_cse->Name[0] = (base)eq_cse->Name[strlen(emu_cse->Name)]
// 
// eq_cse->Beard = emu_cse->Beard;
// eq_cse->HairColor = emu_cse->HairColor;
// eq_cse->Face = emu_cse->Face;
// 
// for (int equip_index = EQ::textures::textureBegin; equip_index < EQ::textures::materialCount; equip_index++) {
// eq_cse->Equip[equip_index].Material = emu_cse->Equip[equip_index].Material;
// eq_cse->Equip[equip_index].Unknown1 = emu_cse->Equip[equip_index].Unknown1;
// eq_cse->Equip[equip_index].EliteMaterial = emu_cse->Equip[equip_index].EliteModel;
// eq_cse->Equip[equip_index].Color = emu_cse->Equip[equip_index].Color;
// }
// 
// eq_cse->PrimaryIDFile = emu_cse->PrimaryIDFile;
// eq_cse->SecondaryIDFile = emu_cse->SecondaryIDFile;
// eq_cse->Tutorial = emu_cse->Tutorial;
// eq_cse->Unknown15 = emu_cse->Unknown15;
// eq_cse->Deity = emu_cse->Deity;
// eq_cse->Zone = emu_cse->Zone;
// eq_cse->Unknown19 = emu_cse->Unknown19;
// eq_cse->Race = emu_cse->Race;
// eq_cse->GoHome = emu_cse->GoHome;
// eq_cse->Class = emu_cse->Class;
// eq_cse->EyeColor1 = emu_cse->EyeColor1;
// eq_cse->BeardColor = emu_cse->BeardColor;
// eq_cse->EyeColor2 = emu_cse->EyeColor2;
// eq_cse->DrakkinHeritage = emu_cse->DrakkinHeritage;
// eq_cse->DrakkinTattoo = emu_cse->DrakkinTattoo;
// eq_cse->DrakkinDetails = emu_cse->DrakkinDetails;
// 
// emu_ptr += sizeof(CharacterSelectEntry_Struct);
// eq_ptr += sizeof(structs::CharacterSelectEntry_Struct);
// }
// 
// FINISH_ENCODE();
// }
//public struct CharacterSelect : IEQStruct
//{
//    uint charCount;
//    uint totalChars;
//    public List<CharacterSelectEntry> Characters;

//    public CharacterSelect(List<CharacterSelectEntry> Characters) : this()
//    {
//        this.Characters = Characters;
//    }

//    public CharacterSelect(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public CharacterSelect(BinaryReader br) : this()
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
//        charCount = br.ReadUInt32();
//        totalChars = br.ReadUInt32();
//        Characters = new List<CharacterSelectEntry>();
//        for (var i = 0; i < charCount; ++i)
//        {
//            Characters.Add(new CharacterSelectEntry(br));
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
//        bw.Write(charCount);
//        bw.Write(totalChars);
//        for (var i = 0; i < charCount; ++i)
//        {
//            Characters[i].Pack(bw);
//        }
//    }

//    public override string ToString()
//    {
//        var ret = "struct CharacterSelect {\n";
//        ret += "\tCharacters = ";
//        try
//        {
//            ret += "{\n";
//            for (int i = 0, e = Characters.Count; i < e; ++i)
//                ret += $"\t\t{Indentify(Characters[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
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
	/// Represents the SendCharInfo packet structure for EverQuest network communication.
	/// </summary>
	public struct SendCharInfo : IEQStruct {
        /// <summary>
        /// The number of characters in this packet.
        /// </summary>
        uint charCount;

        /// <summary>
        /// The total number of characters allowed.
        /// </summary>
        uint totalChars;

        /// <summary>
        /// The list of character entries included in this packet.
        /// </summary>
        public List<CharacterSelectEntry> Characters;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSelect"/> struct with the specified character entries.
        /// </summary>
        /// <param name="Characters">The list of character entries.</param>
        public SendCharInfo(List<CharacterSelectEntry> Characters) : this()
        {
            this.Characters = Characters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSelect"/> struct from a byte array.
        /// </summary>
        /// <param name="data">The byte array containing the packet data.</param>
        /// <param name="offset">The offset in the array to start reading from.</param>
        public SendCharInfo(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSelect"/> struct from a <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br">The binary reader containing the packet data.</param>
        public SendCharInfo(BinaryReader br) : this()
        {
            Unpack(br);
        }

        /// <summary>
        /// Unpacks the packet data from a byte array.
        /// </summary>
        /// <param name="data">The byte array containing the packet data.</param>
        /// <param name="offset">The offset in the array to start reading from.</param>
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

        /// <summary>
        /// Unpacks the packet data from a <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br">The binary reader containing the packet data.</param>
        public void Unpack(BinaryReader br)
        {
            charCount = br.ReadUInt32();
            totalChars = br.ReadUInt32();
            Characters = new List<CharacterSelectEntry>();
            for (var i = 0; i < charCount; ++i)
            {
                Characters.Add(new CharacterSelectEntry(br));
            }
        }

        /// <summary>
        /// Packs the packet data into a byte array.
        /// </summary>
        /// <returns>A byte array containing the packed data.</returns>
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

        /// <summary>
        /// Packs the packet data into a <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="bw">The binary writer to write the packet data to.</param>
        public void Pack(BinaryWriter bw)
        {
            bw.Write(charCount);
            bw.Write(totalChars);
            for (var i = 0; i < charCount; ++i)
            {
                Characters[i].Pack(bw);
            }
        }

        /// <summary>
        /// Returns a string representation of the <see cref="CharacterSelect"/> struct.
        /// </summary>
        /// <returns>A string describing the contents of the struct.</returns>
        public override string ToString()
        {
            var ret = "struct CharacterSelect {\n";
            ret += "\tCharacters = ";
            try
            {
                ret += "{\n";
                for (int i = 0, e = Characters.Count; i < e; ++i)
                    ret += $"\t\t{Indentify(Characters[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
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