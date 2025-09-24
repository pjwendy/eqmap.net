using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LootingItem_Struct {
// /*000*/	uint32	lootee;
// /*004*/	uint32	looter;
// /*008*/	uint32	slot_id;
// /*012*/	int32	auto_loot;
// /*016*/	uint32	unknown16;
// /*020*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LootComplete packet structure for EverQuest network communication.
	/// </summary>
	public struct LootComplete : IEQStruct {
		/// <summary>
		/// Gets or sets the lootee value.
		/// </summary>
		public uint Lootee { get; set; }

		/// <summary>
		/// Gets or sets the looter value.
		/// </summary>
		public uint Looter { get; set; }

		/// <summary>
		/// Gets or sets the slotid value.
		/// </summary>
		public uint SlotId { get; set; }

		/// <summary>
		/// Gets or sets the autoloot value.
		/// </summary>
		public int AutoLoot { get; set; }

		/// <summary>
		/// Gets or sets the unknown16 value.
		/// </summary>
		public uint Unknown16 { get; set; }

		/// <summary>
		/// Initializes a new instance of the LootComplete struct with specified field values.
		/// </summary>
		/// <param name="lootee">The lootee value.</param>
		/// <param name="looter">The looter value.</param>
		/// <param name="slot_id">The slotid value.</param>
		/// <param name="auto_loot">The autoloot value.</param>
		/// <param name="unknown16">The unknown16 value.</param>
		public LootComplete(uint lootee, uint looter, uint slot_id, int auto_loot, uint unknown16) : this() {
			Lootee = lootee;
			Looter = looter;
			SlotId = slot_id;
			AutoLoot = auto_loot;
			Unknown16 = unknown16;
		}

		/// <summary>
		/// Initializes a new instance of the LootComplete struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LootComplete(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LootComplete struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LootComplete(BinaryReader br) : this() {
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
			Lootee = br.ReadUInt32();
			Looter = br.ReadUInt32();
			SlotId = br.ReadUInt32();
			AutoLoot = br.ReadInt32();
			Unknown16 = br.ReadUInt32();
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
			bw.Write(Lootee);
			bw.Write(Looter);
			bw.Write(SlotId);
			bw.Write(AutoLoot);
			bw.Write(Unknown16);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LootComplete {\n";
			ret += "	Lootee = ";
			try {
				ret += $"{ Indentify(Lootee) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Looter = ";
			try {
				ret += $"{ Indentify(Looter) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SlotId = ";
			try {
				ret += $"{ Indentify(SlotId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AutoLoot = ";
			try {
				ret += $"{ Indentify(AutoLoot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown16 = ";
			try {
				ret += $"{ Indentify(Unknown16) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}