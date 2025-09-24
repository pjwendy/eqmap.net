using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildTributeFavorTimer_Struct {
// /*000*/ uint32 guild_id;
// /*004*/ uint32 guild_favor;
// /*008*/ uint32 tribute_timer;
// /*012*/ uint32 trophy_timer;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace EQProtocol.Streams.Common.UnUsed_Packets {
	/// <summary>
	/// Represents the GuildTributeFavorAndTimer packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildTributeFavorAndTimer : IEQStruct {
		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint GuildId { get; set; }

		/// <summary>
		/// Gets or sets the guildfavor value.
		/// </summary>
		public uint GuildFavor { get; set; }

		/// <summary>
		/// Gets or sets the tributetimer value.
		/// </summary>
		public uint TributeTimer { get; set; }

		/// <summary>
		/// Gets or sets the trophytimer value.
		/// </summary>
		public uint TrophyTimer { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildTributeFavorAndTimer struct with specified field values.
		/// </summary>
		/// <param name="guild_id">The guildid value.</param>
		/// <param name="guild_favor">The guildfavor value.</param>
		/// <param name="tribute_timer">The tributetimer value.</param>
		/// <param name="trophy_timer">The trophytimer value.</param>
		public GuildTributeFavorAndTimer(uint guild_id, uint guild_favor, uint tribute_timer, uint trophy_timer) : this() {
			GuildId = guild_id;
			GuildFavor = guild_favor;
			TributeTimer = tribute_timer;
			TrophyTimer = trophy_timer;
		}

		/// <summary>
		/// Initializes a new instance of the GuildTributeFavorAndTimer struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildTributeFavorAndTimer(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildTributeFavorAndTimer struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildTributeFavorAndTimer(BinaryReader br) : this() {
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
			GuildId = br.ReadUInt32();
			GuildFavor = br.ReadUInt32();
			TributeTimer = br.ReadUInt32();
			TrophyTimer = br.ReadUInt32();
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
			bw.Write(GuildId);
			bw.Write(GuildFavor);
			bw.Write(TributeTimer);
			bw.Write(TrophyTimer);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildTributeFavorAndTimer {\n";
			ret += "	GuildId = ";
			try {
				ret += $"{ Indentify(GuildId) },\n";
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
			ret += "	TrophyTimer = ";
			try {
				ret += $"{ Indentify(TrophyTimer) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}