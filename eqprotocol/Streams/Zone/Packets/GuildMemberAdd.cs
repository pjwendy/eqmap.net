using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildMemberAdd_Struct {
// /*000*/ uint32 guild_id;
// /*004*/ uint32 unknown04;
// /*008*/ uint32 unknown08;
// /*012*/ uint32 unknown12;
// /*016*/ uint32 level;
// /*020*/ uint32 class_;
// /*024*/ uint32 rank_;
// /*028*/ uint32 zone_id;
// /*032*/ uint32 last_on;
// /*036*/ char   player_name[64];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_GuildMemberAdd)
// {
// ENCODE_LENGTH_EXACT(GuildMemberAdd_Struct)
// SETUP_DIRECT_ENCODE(GuildMemberAdd_Struct, structs::GuildMemberAdd_Struct)
// 
// OUT(guild_id)
// OUT(level)
// OUT(class_)
// switch (emu->rank_) {
// case GUILD_SENIOR_MEMBER:
// case GUILD_MEMBER:
// case GUILD_JUNIOR_MEMBER:
// case GUILD_INITIATE:
// case GUILD_RECRUIT: {
// eq->rank_ = GUILD_MEMBER_TI;
// break;
// }
// case GUILD_OFFICER:
// case GUILD_SENIOR_OFFICER: {
// eq->rank_ = GUILD_OFFICER_TI;
// break;
// }
// case GUILD_LEADER: {
// eq->rank_ = GUILD_LEADER_TI;
// break;
// }
// default: {
// eq->rank_ = GUILD_RANK_NONE_TI;
// break;
// }
// }
// OUT(zone_id)
// OUT(last_on)
// OUT_str(player_name)
// 
// FINISH_ENCODE()
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GuildMemberAdd packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildMemberAdd : IEQStruct {
		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint GuildId { get; set; }

		/// <summary>
		/// Gets or sets the unknown04 value.
		/// </summary>
		public uint Unknown04 { get; set; }

		/// <summary>
		/// Gets or sets the unknown08 value.
		/// </summary>
		public uint Unknown08 { get; set; }

		/// <summary>
		/// Gets or sets the unknown12 value.
		/// </summary>
		public uint Unknown12 { get; set; }

		/// <summary>
		/// Gets or sets the level value.
		/// </summary>
		public uint Level { get; set; }

		/// <summary>
		/// Gets or sets the class value.
		/// </summary>
		public uint Class { get; set; }

		/// <summary>
		/// Gets or sets the rank value.
		/// </summary>
		public uint Rank { get; set; }

		/// <summary>
		/// Gets or sets the zoneid value.
		/// </summary>
		public uint ZoneId { get; set; }

		/// <summary>
		/// Gets or sets the laston value.
		/// </summary>
		public uint LastOn { get; set; }

		/// <summary>
		/// Gets or sets the playername value.
		/// </summary>
		public byte[] PlayerName { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildMemberAdd struct with specified field values.
		/// </summary>
		/// <param name="guild_id">The guildid value.</param>
		/// <param name="unknown04">The unknown04 value.</param>
		/// <param name="unknown08">The unknown08 value.</param>
		/// <param name="unknown12">The unknown12 value.</param>
		/// <param name="level">The level value.</param>
		/// <param name="class_">The class value.</param>
		/// <param name="rank_">The rank value.</param>
		/// <param name="zone_id">The zoneid value.</param>
		/// <param name="last_on">The laston value.</param>
		/// <param name="player_name">The playername value.</param>
		public GuildMemberAdd(uint guild_id, uint unknown04, uint unknown08, uint unknown12, uint level, uint class_, uint rank_, uint zone_id, uint last_on, byte[] player_name) : this() {
			GuildId = guild_id;
			Unknown04 = unknown04;
			Unknown08 = unknown08;
			Unknown12 = unknown12;
			Level = level;
			Class = class_;
			Rank = rank_;
			ZoneId = zone_id;
			LastOn = last_on;
			PlayerName = player_name;
		}

		/// <summary>
		/// Initializes a new instance of the GuildMemberAdd struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildMemberAdd(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildMemberAdd struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildMemberAdd(BinaryReader br) : this() {
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
			Unknown04 = br.ReadUInt32();
			Unknown08 = br.ReadUInt32();
			Unknown12 = br.ReadUInt32();
			Level = br.ReadUInt32();
			Class = br.ReadUInt32();
			Rank = br.ReadUInt32();
			ZoneId = br.ReadUInt32();
			LastOn = br.ReadUInt32();
			// TODO: Array reading for PlayerName - implement based on actual array size
			// PlayerName = new byte[size];
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
			bw.Write(Unknown04);
			bw.Write(Unknown08);
			bw.Write(Unknown12);
			bw.Write(Level);
			bw.Write(Class);
			bw.Write(Rank);
			bw.Write(ZoneId);
			bw.Write(LastOn);
			// TODO: Array writing for PlayerName - implement based on actual array size
			// foreach(var item in PlayerName) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildMemberAdd {\n";
			ret += "	GuildId = ";
			try {
				ret += $"{ Indentify(GuildId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown04 = ";
			try {
				ret += $"{ Indentify(Unknown04) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown08 = ";
			try {
				ret += $"{ Indentify(Unknown08) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown12 = ";
			try {
				ret += $"{ Indentify(Unknown12) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Level = ";
			try {
				ret += $"{ Indentify(Level) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Class = ";
			try {
				ret += $"{ Indentify(Class) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Rank = ";
			try {
				ret += $"{ Indentify(Rank) },\n";
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
			ret += "	PlayerName = ";
			try {
				ret += $"{ Indentify(PlayerName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}