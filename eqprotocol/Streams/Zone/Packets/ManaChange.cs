using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ManaChange_Struct
// {
// /*00*/	uint32	new_mana;		// New Mana AMount
// /*04*/	uint32	stamina;
// /*08*/	uint32	spell_id;
// /*12*/	uint8	keepcasting;	// won't stop the cast. Change mana while casting?
// /*13*/	uint8	padding[3];		// client doesn't read it, garbage data seems like
// /*16*/	int32	slot;			// -1 for normal usage slot for when we want silent interrupt? I think it does timer stuff or something. Linked Spell Reuse interrupt uses it
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ManaChange packet structure for EverQuest network communication.
	/// </summary>
	public struct ManaChange : IEQStruct {
		/// <summary>
		/// Gets or sets the newmana value.
		/// </summary>
		public uint NewMana { get; set; }

		/// <summary>
		/// Gets or sets the stamina value.
		/// </summary>
		public uint Stamina { get; set; }

		/// <summary>
		/// Gets or sets the spellid value.
		/// </summary>
		public uint SpellId { get; set; }

		/// <summary>
		/// Gets or sets the keepcasting value.
		/// </summary>
		public byte Keepcasting { get; set; }

		/// <summary>
		/// Gets or sets the padding value.
		/// </summary>
		public byte[] Padding { get; set; }

		/// <summary>
		/// Gets or sets the slot value.
		/// </summary>
		public int Slot { get; set; }

		/// <summary>
		/// Initializes a new instance of the ManaChange struct with specified field values.
		/// </summary>
		/// <param name="new_mana">The newmana value.</param>
		/// <param name="stamina">The stamina value.</param>
		/// <param name="spell_id">The spellid value.</param>
		/// <param name="keepcasting">The keepcasting value.</param>
		/// <param name="padding">The padding value.</param>
		/// <param name="slot">The slot value.</param>
		public ManaChange(uint new_mana, uint stamina, uint spell_id, byte keepcasting, byte[] padding, int slot) : this() {
			NewMana = new_mana;
			Stamina = stamina;
			SpellId = spell_id;
			Keepcasting = keepcasting;
			Padding = padding;
			Slot = slot;
		}

		/// <summary>
		/// Initializes a new instance of the ManaChange struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ManaChange(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ManaChange struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ManaChange(BinaryReader br) : this() {
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
			NewMana = br.ReadUInt32();
			Stamina = br.ReadUInt32();
			SpellId = br.ReadUInt32();
			Keepcasting = br.ReadByte();
			// TODO: Array reading for Padding - implement based on actual array size
			// Padding = new byte[size];
			Slot = br.ReadInt32();
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
			bw.Write(NewMana);
			bw.Write(Stamina);
			bw.Write(SpellId);
			bw.Write(Keepcasting);
			// TODO: Array writing for Padding - implement based on actual array size
			// foreach(var item in Padding) bw.Write(item);
			bw.Write(Slot);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ManaChange {\n";
			ret += "	NewMana = ";
			try {
				ret += $"{ Indentify(NewMana) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Stamina = ";
			try {
				ret += $"{ Indentify(Stamina) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpellId = ";
			try {
				ret += $"{ Indentify(SpellId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Keepcasting = ";
			try {
				ret += $"{ Indentify(Keepcasting) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Padding = ";
			try {
				ret += $"{ Indentify(Padding) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}