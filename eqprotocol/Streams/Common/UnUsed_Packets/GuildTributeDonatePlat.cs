using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildTributeDonatePlatRequest_Struct {
// /*000*/ uint32 quantity;
// /*004*/ uint32 tribute_master_id;
// /*008*/ uint32 unknown08;
// /*012*/ uint32 guild_id;
// /*016*/ uint32 unknown16;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace EQProtocol.Streams.Common.UnUsed_Packets {
	/// <summary>
	/// Represents the GuildTributeDonatePlat packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildTributeDonatePlat : IEQStruct {
		/// <summary>
		/// Gets or sets the quantity value.
		/// </summary>
		public uint Quantity { get; set; }

		/// <summary>
		/// Gets or sets the tributemasterid value.
		/// </summary>
		public uint TributeMasterId { get; set; }

		/// <summary>
		/// Gets or sets the unknown08 value.
		/// </summary>
		public uint Unknown08 { get; set; }

		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint GuildId { get; set; }

		/// <summary>
		/// Gets or sets the unknown16 value.
		/// </summary>
		public uint Unknown16 { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildTributeDonatePlat struct with specified field values.
		/// </summary>
		/// <param name="quantity">The quantity value.</param>
		/// <param name="tribute_master_id">The tributemasterid value.</param>
		/// <param name="unknown08">The unknown08 value.</param>
		/// <param name="guild_id">The guildid value.</param>
		/// <param name="unknown16">The unknown16 value.</param>
		public GuildTributeDonatePlat(uint quantity, uint tribute_master_id, uint unknown08, uint guild_id, uint unknown16) : this() {
			Quantity = quantity;
			TributeMasterId = tribute_master_id;
			Unknown08 = unknown08;
			GuildId = guild_id;
			Unknown16 = unknown16;
		}

		/// <summary>
		/// Initializes a new instance of the GuildTributeDonatePlat struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildTributeDonatePlat(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildTributeDonatePlat struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildTributeDonatePlat(BinaryReader br) : this() {
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
			Quantity = br.ReadUInt32();
			TributeMasterId = br.ReadUInt32();
			Unknown08 = br.ReadUInt32();
			GuildId = br.ReadUInt32();
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
			bw.Write(Quantity);
			bw.Write(TributeMasterId);
			bw.Write(Unknown08);
			bw.Write(GuildId);
			bw.Write(Unknown16);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildTributeDonatePlat {\n";
			ret += "	Quantity = ";
			try {
				ret += $"{ Indentify(Quantity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeMasterId = ";
			try {
				ret += $"{ Indentify(TributeMasterId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown08 = ";
			try {
				ret += $"{ Indentify(Unknown08) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	GuildId = ";
			try {
				ret += $"{ Indentify(GuildId) },\n";
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