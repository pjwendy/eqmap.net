using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ItemVerifyReply_Struct {
// /*000*/	int32	slot;		// Slot being Right Clicked
// /*004*/	uint32	spell;		// Spell ID to cast if different than item effect
// /*008*/	uint32	target;		// Target Entity ID
// /*012*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_ItemVerifyReply)
// {
// ENCODE_LENGTH_EXACT(ItemVerifyReply_Struct);
// SETUP_DIRECT_ENCODE(ItemVerifyReply_Struct, structs::ItemVerifyReply_Struct);
// 
// eq->slot = ServerToUFSlot(emu->slot);
// OUT(spell);
// OUT(target);
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ItemVerifyReply packet structure for EverQuest network communication.
	/// </summary>
	public struct ItemVerifyReply : IEQStruct {
		/// <summary>
		/// Gets or sets the slot value.
		/// </summary>
		public int Slot { get; set; }

		/// <summary>
		/// Gets or sets the spell value.
		/// </summary>
		public uint Spell { get; set; }

		/// <summary>
		/// Gets or sets the target value.
		/// </summary>
		public uint Target { get; set; }

		/// <summary>
		/// Initializes a new instance of the ItemVerifyReply struct with specified field values.
		/// </summary>
		/// <param name="slot">The slot value.</param>
		/// <param name="spell">The spell value.</param>
		/// <param name="target">The target value.</param>
		public ItemVerifyReply(int slot, uint spell, uint target) : this() {
			Slot = slot;
			Spell = spell;
			Target = target;
		}

		/// <summary>
		/// Initializes a new instance of the ItemVerifyReply struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ItemVerifyReply(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ItemVerifyReply struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ItemVerifyReply(BinaryReader br) : this() {
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
			Slot = br.ReadInt32();
			Spell = br.ReadUInt32();
			Target = br.ReadUInt32();
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
			bw.Write(Spell);
			bw.Write(Target);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ItemVerifyReply {\n";
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Spell = ";
			try {
				ret += $"{ Indentify(Spell) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Target = ";
			try {
				ret += $"{ Indentify(Target) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}