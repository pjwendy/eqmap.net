using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildRenameGuild_Struct {
// /*000*/ uint32 guild_id;
// /*004*/ char   new_guild_name[64];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GuildRenameGuild packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildRenameGuild : IEQStruct {
		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint GuildId { get; set; }

		/// <summary>
		/// Gets or sets the newguildname value.
		/// </summary>
		public byte[] NewGuildName { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildRenameGuild struct with specified field values.
		/// </summary>
		/// <param name="guild_id">The guildid value.</param>
		/// <param name="new_guild_name">The newguildname value.</param>
		public GuildRenameGuild(uint guild_id, byte[] new_guild_name) : this() {
			GuildId = guild_id;
			NewGuildName = new_guild_name;
		}

		/// <summary>
		/// Initializes a new instance of the GuildRenameGuild struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildRenameGuild(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildRenameGuild struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildRenameGuild(BinaryReader br) : this() {
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
			// TODO: Array reading for NewGuildName - implement based on actual array size
			// NewGuildName = new byte[size];
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
			// TODO: Array writing for NewGuildName - implement based on actual array size
			// foreach(var item in NewGuildName) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildRenameGuild {\n";
			ret += "	GuildId = ";
			try {
				ret += $"{ Indentify(GuildId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NewGuildName = ";
			try {
				ret += $"{ Indentify(NewGuildName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}