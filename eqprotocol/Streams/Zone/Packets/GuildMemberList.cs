using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildMembers_Struct {	//just for display purposes, this is not actually used in the message encoding.
// char	player_name[1];		//variable length.
// uint32	count;				//network byte order
// GuildMemberEntry_Struct member[0];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_GuildMemberList)
// {
// //consume the packet
// EQApplicationPacket *in = *p;
// *p = nullptr;
// 
// //store away the emu struct
// unsigned char *__emu_buffer = in->pBuffer;
// Internal_GuildMembers_Struct *emu = (Internal_GuildMembers_Struct *)in->pBuffer;
// 
// //make a new EQ buffer.
// uint32 pnl = strlen(emu->player_name);
// uint32 length = sizeof(structs::GuildMembers_Struct) + pnl +
// emu->count*sizeof(structs::GuildMemberEntry_Struct)
// + emu->name_length + emu->note_length;
// in->pBuffer = new uint8[length];
// in->size = length;
// //no memset since we fill every byte.
// 
// uint8 *buffer;
// buffer = in->pBuffer;
// 
// //easier way to setup GuildMembers_Struct
// //set prefix name
// strcpy((char *)buffer, emu->player_name);
// buffer += pnl;
// *buffer = '\0';
// buffer++;
// 
// //add member count.
// *((uint32 *)buffer) = htonl(emu->count);
// buffer += sizeof(uint32);
// 
// if (emu->count > 0) {
// Internal_GuildMemberEntry_Struct *emu_e = emu->member;
// const char *emu_name = (const char *)(__emu_buffer +
// sizeof(Internal_GuildMembers_Struct)+ //skip header
// emu->count * sizeof(Internal_GuildMemberEntry_Struct)	//skip static length member data
// );
// const char *emu_note = (emu_name +
// emu->name_length + //skip name contents
// emu->count	//skip string terminators
// );
// 
// structs::GuildMemberEntry_Struct *e = (structs::GuildMemberEntry_Struct *) buffer;
// 
// uint32 r;
// for (r = 0; r < emu->count; r++, emu_e++) {
// 
// //the order we set things here must match the struct
// 
// //nice helper macro
// /*#define SlideStructString(field, str) \
// strcpy(e->field, str.c_str()); \
// e = (GuildMemberEntry_Struct *) ( ((uint8 *)e) + str.length() )*/
// #define SlideStructString(field, str) \
// { \
// int sl = strlen(str); \
// memcpy(e->field, str, sl+1); \
// e = (structs::GuildMemberEntry_Struct *) ( ((uint8 *)e) + sl ); \
// str += sl + 1; \
// }
// #define PutFieldN(field) e->field = htonl(emu_e->field)
// 
// SlideStructString(name, emu_name);
// PutFieldN(level);
// PutFieldN(banker);
// PutFieldN(class_);
// //Translate older ranks to new values* /
// switch (emu_e->rank) {
// case GUILD_SENIOR_MEMBER:
// case GUILD_MEMBER:
// case GUILD_JUNIOR_MEMBER:
// case GUILD_INITIATE:
// case GUILD_RECRUIT: {
// emu_e->rank = GUILD_MEMBER_TI;
// break;
// }
// case GUILD_OFFICER:
// case GUILD_SENIOR_OFFICER: {
// emu_e->rank = GUILD_OFFICER_TI;
// break;
// }
// case GUILD_LEADER: {
// emu_e->rank = GUILD_LEADER_TI;
// break;
// }
// default: {
// emu_e->rank = GUILD_RANK_NONE_TI;
// break;
// }
// }
// PutFieldN(rank);
// PutFieldN(time_last_on);
// PutFieldN(tribute_enable);
// PutFieldN(total_tribute);
// PutFieldN(last_tribute);
// e->unknown_one = htonl(1);
// SlideStructString(public_note, emu_note);
// e->zoneinstance = 0;
// e->zone_id = htons(emu_e->zone_id);
// 
// #undef SlideStructString
// #undef PutFieldN
// 
// e++;
// }
// }
// 
// delete[] __emu_buffer;
// dest->FastQueuePacket(&in, ack_req);
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GuildMemberList packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildMemberList : IEQStruct {
		/// <summary>
		/// Gets or sets the playername value.
		/// </summary>
		public byte PlayerName { get; set; }

		/// <summary>
		/// Gets or sets the count value.
		/// </summary>
		public uint Count { get; set; }

		/// <summary>
		/// Gets or sets the member value.
		/// </summary>
		public uint Member { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildMemberList struct with specified field values.
		/// </summary>
		/// <param name="player_name">The playername value.</param>
		/// <param name="count">The count value.</param>
		/// <param name="member">The member value.</param>
		public GuildMemberList(byte player_name, uint count, uint member) : this() {
			PlayerName = player_name;
			Count = count;
			Member = member;
		}

		/// <summary>
		/// Initializes a new instance of the GuildMemberList struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildMemberList(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildMemberList struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildMemberList(BinaryReader br) : this() {
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
			PlayerName = br.ReadByte();
			Count = br.ReadUInt32();
			Member = br.ReadUInt32();
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
			bw.Write(PlayerName);
			bw.Write(Count);
			bw.Write(Member);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildMemberList {\n";
			ret += "	PlayerName = ";
			try {
				ret += $"{ Indentify(PlayerName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Count = ";
			try {
				ret += $"{ Indentify(Count) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Member = ";
			try {
				ret += $"{ Indentify(Member) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}