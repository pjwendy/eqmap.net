using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MemorizeSpell_Struct {
// uint32 slot;     // Spot in the spell book/memorized slot
// uint32 spell_id; // Spell id (200 or c8 is minor healing, etc)
// uint32 scribing; // 1 if memorizing a spell, set to 0 if scribing to book, 2 if un-memming
// uint32 reduction; // lowers reuse
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MemorizeSpell packet structure for EverQuest network communication.
	/// </summary>
	public struct MemorizeSpell : IEQStruct {
		/// <summary>
		/// Gets or sets the slot value.
		/// </summary>
		public uint Slot { get; set; }

		/// <summary>
		/// Gets or sets the spellid value.
		/// </summary>
		public uint SpellId { get; set; }

		/// <summary>
		/// Gets or sets the scribing value.
		/// </summary>
		public uint Scribing { get; set; }

		/// <summary>
		/// Gets or sets the reduction value.
		/// </summary>
		public uint Reduction { get; set; }

		/// <summary>
		/// Initializes a new instance of the MemorizeSpell struct with specified field values.
		/// </summary>
		/// <param name="slot">The slot value.</param>
		/// <param name="spell_id">The spellid value.</param>
		/// <param name="scribing">The scribing value.</param>
		/// <param name="reduction">The reduction value.</param>
		public MemorizeSpell(uint slot, uint spell_id, uint scribing, uint reduction) : this() {
			Slot = slot;
			SpellId = spell_id;
			Scribing = scribing;
			Reduction = reduction;
		}

		/// <summary>
		/// Initializes a new instance of the MemorizeSpell struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MemorizeSpell(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MemorizeSpell struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MemorizeSpell(BinaryReader br) : this() {
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
			Slot = br.ReadUInt32();
			SpellId = br.ReadUInt32();
			Scribing = br.ReadUInt32();
			Reduction = br.ReadUInt32();
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
			bw.Write(Slot);
			bw.Write(SpellId);
			bw.Write(Scribing);
			bw.Write(Reduction);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct MemorizeSpell {\n";
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpellId = ";
			try {
				ret += $"{ Indentify(SpellId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Scribing = ";
			try {
				ret += $"{ Indentify(Scribing) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Reduction = ";
			try {
				ret += $"{ Indentify(Reduction) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}