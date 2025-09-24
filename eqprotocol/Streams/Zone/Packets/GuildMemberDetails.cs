using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildMemberDetails_Struct {
// /*000*/ uint32 guild_id;
// /*004*/ char   player_name[64];
// /*068*/ uint32 zone_id;
// /*072*/ uint32 last_on;
// /*076*/ uint32 offline_mode; //1 Offline
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GuildMemberDetails packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildMemberDetails : IEQStruct {
		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint GuildId { get; set; }

		/// <summary>
		/// Gets or sets the playername value.
		/// </summary>
		public byte[] PlayerName { get; set; }

		/// <summary>
		/// Gets or sets the zoneid value.
		/// </summary>
		public uint ZoneId { get; set; }

		/// <summary>
		/// Gets or sets the laston value.
		/// </summary>
		public uint LastOn { get; set; }

		/// <summary>
		/// Gets or sets the offlinemode value.
		/// </summary>
		public uint OfflineMode { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildMemberDetails struct with specified field values.
		/// </summary>
		/// <param name="guild_id">The guildid value.</param>
		/// <param name="player_name">The playername value.</param>
		/// <param name="zone_id">The zoneid value.</param>
		/// <param name="last_on">The laston value.</param>
		/// <param name="offline_mode">The offlinemode value.</param>
		public GuildMemberDetails(uint guild_id, byte[] player_name, uint zone_id, uint last_on, uint offline_mode) : this() {
			GuildId = guild_id;
			PlayerName = player_name;
			ZoneId = zone_id;
			LastOn = last_on;
			OfflineMode = offline_mode;
		}

		/// <summary>
		/// Initializes a new instance of the GuildMemberDetails struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildMemberDetails(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildMemberDetails struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildMemberDetails(BinaryReader br) : this() {
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
			// TODO: Array reading for PlayerName - implement based on actual array size
			// PlayerName = new byte[size];
			ZoneId = br.ReadUInt32();
			LastOn = br.ReadUInt32();
			OfflineMode = br.ReadUInt32();
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
			// TODO: Array writing for PlayerName - implement based on actual array size
			// foreach(var item in PlayerName) bw.Write(item);
			bw.Write(ZoneId);
			bw.Write(LastOn);
			bw.Write(OfflineMode);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildMemberDetails {\n";
			ret += "	GuildId = ";
			try {
				ret += $"{ Indentify(GuildId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PlayerName = ";
			try {
				ret += $"{ Indentify(PlayerName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneId = ";
			try {
				ret += $"{ Indentify(ZoneId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LastOn = ";
			try {
				ret += $"{ Indentify(LastOn) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	OfflineMode = ";
			try {
				ret += $"{ Indentify(OfflineMode) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}