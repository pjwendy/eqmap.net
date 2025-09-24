using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildTributeSendActive_Struct {
// /*000*/ uint32 not_used;
// /*004*/ uint32 guild_favor;
// /*008*/ uint32 tribute_timer;
// /*012*/ uint32 tribute_enabled;
// /*016*/ char   unknown16[8];
// /*024*/ uint32 tribute_id_1;
// /*028*/ uint32 tribute_id_2;
// /*032*/ uint32 tribute_id_1_tier;
// /*036*/ uint32 tribute_id_2_tier;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace EQProtocol.Streams.Common.UnUsed_Packets {
	/// <summary>
	/// Represents the GuildSendActiveTributes packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildSendActiveTributes : IEQStruct {
		/// <summary>
		/// Gets or sets the notused value.
		/// </summary>
		public uint NotUsed { get; set; }

		/// <summary>
		/// Gets or sets the guildfavor value.
		/// </summary>
		public uint GuildFavor { get; set; }

		/// <summary>
		/// Gets or sets the tributetimer value.
		/// </summary>
		public uint TributeTimer { get; set; }

		/// <summary>
		/// Gets or sets the tributeenabled value.
		/// </summary>
		public uint TributeEnabled { get; set; }

		/// <summary>
		/// Gets or sets the unknown16 value.
		/// </summary>
		public byte[] Unknown16 { get; set; }

		/// <summary>
		/// Gets or sets the tributeid1 value.
		/// </summary>
		public uint TributeId1 { get; set; }

		/// <summary>
		/// Gets or sets the tributeid2 value.
		/// </summary>
		public uint TributeId2 { get; set; }

		/// <summary>
		/// Gets or sets the tributeid1tier value.
		/// </summary>
		public uint TributeId1Tier { get; set; }

		/// <summary>
		/// Gets or sets the tributeid2tier value.
		/// </summary>
		public uint TributeId2Tier { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildSendActiveTributes struct with specified field values.
		/// </summary>
		/// <param name="not_used">The notused value.</param>
		/// <param name="guild_favor">The guildfavor value.</param>
		/// <param name="tribute_timer">The tributetimer value.</param>
		/// <param name="tribute_enabled">The tributeenabled value.</param>
		/// <param name="unknown16">The unknown16 value.</param>
		/// <param name="tribute_id_1">The tributeid1 value.</param>
		/// <param name="tribute_id_2">The tributeid2 value.</param>
		/// <param name="tribute_id_1_tier">The tributeid1tier value.</param>
		/// <param name="tribute_id_2_tier">The tributeid2tier value.</param>
		public GuildSendActiveTributes(uint not_used, uint guild_favor, uint tribute_timer, uint tribute_enabled, byte[] unknown16, uint tribute_id_1, uint tribute_id_2, uint tribute_id_1_tier, uint tribute_id_2_tier) : this() {
			NotUsed = not_used;
			GuildFavor = guild_favor;
			TributeTimer = tribute_timer;
			TributeEnabled = tribute_enabled;
			Unknown16 = unknown16;
			TributeId1 = tribute_id_1;
			TributeId2 = tribute_id_2;
			TributeId1Tier = tribute_id_1_tier;
			TributeId2Tier = tribute_id_2_tier;
		}

		/// <summary>
		/// Initializes a new instance of the GuildSendActiveTributes struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildSendActiveTributes(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildSendActiveTributes struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildSendActiveTributes(BinaryReader br) : this() {
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
			NotUsed = br.ReadUInt32();
			GuildFavor = br.ReadUInt32();
			TributeTimer = br.ReadUInt32();
			TributeEnabled = br.ReadUInt32();
			// TODO: Array reading for Unknown16 - implement based on actual array size
			// Unknown16 = new byte[size];
			TributeId1 = br.ReadUInt32();
			TributeId2 = br.ReadUInt32();
			TributeId1Tier = br.ReadUInt32();
			TributeId2Tier = br.ReadUInt32();
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
			bw.Write(NotUsed);
			bw.Write(GuildFavor);
			bw.Write(TributeTimer);
			bw.Write(TributeEnabled);
			// TODO: Array writing for Unknown16 - implement based on actual array size
			// foreach(var item in Unknown16) bw.Write(item);
			bw.Write(TributeId1);
			bw.Write(TributeId2);
			bw.Write(TributeId1Tier);
			bw.Write(TributeId2Tier);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildSendActiveTributes {\n";
			ret += "	NotUsed = ";
			try {
				ret += $"{ Indentify(NotUsed) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	GuildFavor = ";
			try {
				ret += $"{ Indentify(GuildFavor) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeTimer = ";
			try {
				ret += $"{ Indentify(TributeTimer) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeEnabled = ";
			try {
				ret += $"{ Indentify(TributeEnabled) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown16 = ";
			try {
				ret += $"{ Indentify(Unknown16) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeId1 = ";
			try {
				ret += $"{ Indentify(TributeId1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeId2 = ";
			try {
				ret += $"{ Indentify(TributeId2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeId1Tier = ";
			try {
				ret += $"{ Indentify(TributeId1Tier) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeId2Tier = ";
			try {
				ret += $"{ Indentify(TributeId2Tier) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}