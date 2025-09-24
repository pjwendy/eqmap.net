using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct InspectBuffs_Struct {
// /*000*/ uint32 spell_id[BUFF_COUNT];
// /*120*/ int32 tics_remaining[BUFF_COUNT];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_InspectBuffs)
// {
// ENCODE_LENGTH_EXACT(InspectBuffs_Struct);
// SETUP_DIRECT_ENCODE(InspectBuffs_Struct, structs::InspectBuffs_Struct);
// 
// // we go over the internal 25 instead of the packet's since no entry is 0, which it will be already
// for (int i = 0; i < BUFF_COUNT; i++) {
// OUT(spell_id[i]);
// OUT(tics_remaining[i]);
// }
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the InspectBuffs packet structure for EverQuest network communication.
	/// </summary>
	public struct InspectBuffs : IEQStruct {
		/// <summary>
		/// Gets or sets the spellid value.
		/// </summary>
		public uint SpellId { get; set; }

		/// <summary>
		/// Gets or sets the ticsremaining value.
		/// </summary>
		public int TicsRemaining { get; set; }

		/// <summary>
		/// Initializes a new instance of the InspectBuffs struct with specified field values.
		/// </summary>
		/// <param name="spell_id">The spellid value.</param>
		/// <param name="tics_remaining">The ticsremaining value.</param>
		public InspectBuffs(uint spell_id, int tics_remaining) : this() {
			SpellId = spell_id;
			TicsRemaining = tics_remaining;
		}

		/// <summary>
		/// Initializes a new instance of the InspectBuffs struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public InspectBuffs(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the InspectBuffs struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public InspectBuffs(BinaryReader br) : this() {
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
			SpellId = br.ReadUInt32();
			TicsRemaining = br.ReadInt32();
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
			bw.Write(SpellId);
			bw.Write(TicsRemaining);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct InspectBuffs {\n";
			ret += "	SpellId = ";
			try {
				ret += $"{ Indentify(SpellId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TicsRemaining = ";
			try {
				ret += $"{ Indentify(TicsRemaining) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}