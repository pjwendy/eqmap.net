using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildTributeDonateItemReply_Struct {
// /*000*/ uint32	slot;
// /*004*/ uint32	quantity;
// /*008*/ uint32	unknown8;
// /*012*/	uint32	favor;
// };

// ENCODE/DECODE Section:
// DECODE(OP_GuildTributeDonateItem)
// {
// DECODE_LENGTH_EXACT(structs::GuildTributeDonateItemRequest_Struct);
// SETUP_DIRECT_DECODE(GuildTributeDonateItemRequest_Struct, structs::GuildTributeDonateItemRequest_Struct);
// 
// Log(Logs::Detail, Logs::Netcode, "UF::DECODE(OP_GuildTributeDonateItem)");
// 
// IN(quantity);
// IN(tribute_master_id);
// IN(guild_id);
// 
// emu->slot = UFToServerSlot(eq->slot);
// 
// FINISH_DIRECT_DECODE();
// }

namespace EQProtocol.Streams.Common.UnUsed_Packets {
	/// <summary>
	/// Represents the GuildTributeDonateItem packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildTributeDonateItem : IEQStruct {
		/// <summary>
		/// Gets or sets the slot value.
		/// </summary>
		public uint Slot { get; set; }

		/// <summary>
		/// Gets or sets the quantity value.
		/// </summary>
		public uint Quantity { get; set; }

		/// <summary>
		/// Gets or sets the unknown8 value.
		/// </summary>
		public uint Unknown8 { get; set; }

		/// <summary>
		/// Gets or sets the favor value.
		/// </summary>
		public uint Favor { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildTributeDonateItem struct with specified field values.
		/// </summary>
		/// <param name="slot">The slot value.</param>
		/// <param name="quantity">The quantity value.</param>
		/// <param name="unknown8">The unknown8 value.</param>
		/// <param name="favor">The favor value.</param>
		public GuildTributeDonateItem(uint slot, uint quantity, uint unknown8, uint favor) : this() {
			Slot = slot;
			Quantity = quantity;
			Unknown8 = unknown8;
			Favor = favor;
		}

		/// <summary>
		/// Initializes a new instance of the GuildTributeDonateItem struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildTributeDonateItem(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildTributeDonateItem struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildTributeDonateItem(BinaryReader br) : this() {
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
			Quantity = br.ReadUInt32();
			Unknown8 = br.ReadUInt32();
			Favor = br.ReadUInt32();
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
			bw.Write(Quantity);
			bw.Write(Unknown8);
			bw.Write(Favor);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildTributeDonateItem {\n";
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Quantity = ";
			try {
				ret += $"{ Indentify(Quantity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown8 = ";
			try {
				ret += $"{ Indentify(Unknown8) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Favor = ";
			try {
				ret += $"{ Indentify(Favor) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}