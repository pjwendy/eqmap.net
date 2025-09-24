using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LoadSpellSet_Struct {
// uint8  spell[spells::SPELL_GEM_COUNT];      // 0 if no action
// uint32 unknown;	// there seems to be an extra field in this packet...
// };

// ENCODE/DECODE Section:
// DECODE(OP_LoadSpellSet)
// {
// DECODE_LENGTH_EXACT(structs::LoadSpellSet_Struct);
// SETUP_DIRECT_DECODE(LoadSpellSet_Struct, structs::LoadSpellSet_Struct);
// 
// for (unsigned int i = 0; i < EQ::spells::SPELL_GEM_COUNT; ++i)
// if (eq->spell[i] == 0)
// emu->spell[i] = 0xFFFFFFFF;
// else
// emu->spell[i] = eq->spell[i];
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LoadSpellSet packet structure for EverQuest network communication.
	/// </summary>
	public struct LoadSpellSet : IEQStruct {
		/// <summary>
		/// Gets or sets the spell value.
		/// </summary>
		public byte Spell { get; set; }

		/// <summary>
		/// Gets or sets the unknown value.
		/// </summary>
		public uint Unknown { get; set; }

		/// <summary>
		/// Initializes a new instance of the LoadSpellSet struct with specified field values.
		/// </summary>
		/// <param name="spell">The spell value.</param>
		/// <param name="unknown">The unknown value.</param>
		public LoadSpellSet(byte spell, uint unknown) : this() {
			Spell = spell;
			Unknown = unknown;
		}

		/// <summary>
		/// Initializes a new instance of the LoadSpellSet struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LoadSpellSet(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LoadSpellSet struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LoadSpellSet(BinaryReader br) : this() {
			Unpack(br);
		}

		/// <summary>
		/// Unpacks the struct data from a byte array.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public void Unpack(byte[] data, int offset = 0) {
			using(var ms = new MemoryStream(data, offset, data.Length - offset)) {
				using(var br = new BinaryReader(ms)) {
					Unpack(br);
				}
			}
		}

		/// <summary>
		/// Unpacks the struct data from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public void Unpack(BinaryReader br) {
			Spell = br.ReadByte();
			Unknown = br.ReadUInt32();
		}

		/// <summary>
		/// Packs the struct data into a byte array.
		/// </summary>
		/// <returns>A byte array containing the packed struct data.</returns>
		public byte[] Pack() {
			using(var ms = new MemoryStream()) {
				using(var bw = new BinaryWriter(ms)) {
					Pack(bw);
					return ms.ToArray();
				}
			}
		}

		/// <summary>
		/// Packs the struct data into a BinaryWriter.
		/// </summary>
		/// <param name="bw">The BinaryWriter to write data to.</param>
		public void Pack(BinaryWriter bw) {
			bw.Write(Spell);
			bw.Write(Unknown);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LoadSpellSet {\n";
			ret += "	Spell = ";
			try {
				ret += $"{ Indentify(Spell) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown = ";
			try {
				ret += $"{ Indentify(Unknown) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}