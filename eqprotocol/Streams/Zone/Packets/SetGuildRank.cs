using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildSetRank_Struct
// {
// /*00*/	uint32	unknown00;
// /*04*/	uint32	unknown04;
// /*08*/	uint32	rank;
// /*72*/	char	member_name[64];
// /*76*/	uint32	banker;
// };

// ENCODE/DECODE Section:
// ENCODE(OP_SetGuildRank)
// {
// ENCODE_LENGTH_EXACT(GuildSetRank_Struct);
// SETUP_DIRECT_ENCODE(GuildSetRank_Struct, structs::GuildSetRank_Struct);
// 
// eq->unknown00 = 0;
// eq->unknown04 = 0;
// 
// switch (emu->rank) {
// case GUILD_SENIOR_MEMBER:
// case GUILD_MEMBER:
// case GUILD_JUNIOR_MEMBER:
// case GUILD_INITIATE:
// case GUILD_RECRUIT: {
// emu->rank = GUILD_MEMBER_TI;
// break;
// }
// case GUILD_OFFICER:
// case GUILD_SENIOR_OFFICER: {
// emu->rank = GUILD_OFFICER_TI;
// break;
// }
// case GUILD_LEADER: {
// emu->rank = GUILD_LEADER_TI;
// break;
// }
// default: {
// emu->rank = GUILD_RANK_NONE_TI;
// break;
// }
// }
// 
// memcpy(eq->member_name, emu->member_name, sizeof(eq->member_name));
// OUT(banker);
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SetGuildRank packet structure for EverQuest network communication.
	/// </summary>
	public struct SetGuildRank : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown00 value.
		/// </summary>
		public uint Unknown00 { get; set; }

		/// <summary>
		/// Gets or sets the unknown04 value.
		/// </summary>
		public uint Unknown04 { get; set; }

		/// <summary>
		/// Gets or sets the rank value.
		/// </summary>
		public uint Rank { get; set; }

		/// <summary>
		/// Gets or sets the membername value.
		/// </summary>
		public byte[] MemberName { get; set; }

		/// <summary>
		/// Gets or sets the banker value.
		/// </summary>
		public uint Banker { get; set; }

		/// <summary>
		/// Initializes a new instance of the SetGuildRank struct with specified field values.
		/// </summary>
		/// <param name="unknown00">The unknown00 value.</param>
		/// <param name="unknown04">The unknown04 value.</param>
		/// <param name="rank">The rank value.</param>
		/// <param name="member_name">The membername value.</param>
		/// <param name="banker">The banker value.</param>
		public SetGuildRank(uint unknown00, uint unknown04, uint rank, byte[] member_name, uint banker) : this() {
			Unknown00 = unknown00;
			Unknown04 = unknown04;
			Rank = rank;
			MemberName = member_name;
			Banker = banker;
		}

		/// <summary>
		/// Initializes a new instance of the SetGuildRank struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SetGuildRank(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SetGuildRank struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SetGuildRank(BinaryReader br) : this() {
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
			Unknown00 = br.ReadUInt32();
			Unknown04 = br.ReadUInt32();
			Rank = br.ReadUInt32();
			// TODO: Array reading for MemberName - implement based on actual array size
			// MemberName = new byte[size];
			Banker = br.ReadUInt32();
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
			bw.Write(Unknown00);
			bw.Write(Unknown04);
			bw.Write(Rank);
			// TODO: Array writing for MemberName - implement based on actual array size
			// foreach(var item in MemberName) bw.Write(item);
			bw.Write(Banker);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SetGuildRank {\n";
			ret += "	Unknown00 = ";
			try {
				ret += $"{ Indentify(Unknown00) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown04 = ";
			try {
				ret += $"{ Indentify(Unknown04) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Rank = ";
			try {
				ret += $"{ Indentify(Rank) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MemberName = ";
			try {
				ret += $"{ Indentify(MemberName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Banker = ";
			try {
				ret += $"{ Indentify(Banker) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}