using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildMemberRank_Struct {
// /*000*/ uint32 guild_id;
// /*004*/ uint32 unknown_004;
// /*008*/ uint32 rank_;
// /*012*/ char   player_name[64];
// /*076*/ uint32 alt_banker; //Banker/Alt bit 00 - none 10 - Alt 11 - Alt and Banker 01 - Banker.  Banker not functional for RoF2+
// };

// ENCODE/DECODE Section:
// ENCODE(OP_GuildMemberRankAltBanker)
// {
// ENCODE_LENGTH_EXACT(GuildMemberRank_Struct)
// SETUP_DIRECT_ENCODE(GuildMemberRank_Struct, structs::GuildMemberRank_Struct)
// 
// OUT(guild_id)
// OUT(alt_banker)
// OUT_str(player_name)
// 
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
// FINISH_ENCODE()
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GuildMemberRankAltBanker packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildMemberRankAltBanker : IEQStruct {
		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint GuildId { get; set; }

		/// <summary>
		/// Gets or sets the rank value.
		/// </summary>
		public uint Rank { get; set; }

		/// <summary>
		/// Gets or sets the playername value.
		/// </summary>
		public byte[] PlayerName { get; set; }

		/// <summary>
		/// Gets or sets the altbanker value.
		/// </summary>
		public uint AltBanker { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildMemberRankAltBanker struct with specified field values.
		/// </summary>
		/// <param name="guild_id">The guildid value.</param>
		/// <param name="rank_">The rank value.</param>
		/// <param name="player_name">The playername value.</param>
		/// <param name="alt_banker">The altbanker value.</param>
		public GuildMemberRankAltBanker(uint guild_id, uint rank_, byte[] player_name, uint alt_banker) : this() {
			GuildId = guild_id;
			Rank = rank_;
			PlayerName = player_name;
			AltBanker = alt_banker;
		}

		/// <summary>
		/// Initializes a new instance of the GuildMemberRankAltBanker struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildMemberRankAltBanker(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildMemberRankAltBanker struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildMemberRankAltBanker(BinaryReader br) : this() {
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
			Rank = br.ReadUInt32();
			// TODO: Array reading for PlayerName - implement based on actual array size
			// PlayerName = new byte[size];
			AltBanker = br.ReadUInt32();
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
			bw.Write(Rank);
			// TODO: Array writing for PlayerName - implement based on actual array size
			// foreach(var item in PlayerName) bw.Write(item);
			bw.Write(AltBanker);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildMemberRankAltBanker {\n";
			ret += "	GuildId = ";
			try {
				ret += $"{ Indentify(GuildId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Rank = ";
			try {
				ret += $"{ Indentify(Rank) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PlayerName = ";
			try {
				ret += $"{ Indentify(PlayerName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AltBanker = ";
			try {
				ret += $"{ Indentify(AltBanker) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}