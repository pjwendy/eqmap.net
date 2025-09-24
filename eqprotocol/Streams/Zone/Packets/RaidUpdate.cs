using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct RaidAddMember_Struct {
// /*000*/ RaidGeneral_Struct raidGen; //param = (group num-1); 0xFFFFFFFF = no group
// /*136*/ uint8 _class;
// /*137*/	uint8 level;
// /*138*/	uint8 isGroupLeader;
// /*139*/	uint8 flags[5]; //no idea if these are needed...
// };

// ENCODE/DECODE Section:
// ENCODE(OP_RaidUpdate)
// {
// EQApplicationPacket* inapp = *p;
// *p = nullptr;
// unsigned char* __emu_buffer = inapp->pBuffer;
// RaidGeneral_Struct* raid_gen = (RaidGeneral_Struct*)__emu_buffer;
// 
// switch (raid_gen->action)
// {
// case raidAdd:
// {
// RaidAddMember_Struct* emu = (RaidAddMember_Struct*)__emu_buffer;
// 
// auto outapp = new EQApplicationPacket(OP_RaidUpdate, sizeof(structs::RaidAddMember_Struct));
// structs::RaidAddMember_Struct* eq = (structs::RaidAddMember_Struct*)outapp->pBuffer;
// 
// OUT(raidGen.action);
// OUT(raidGen.parameter);
// OUT_str(raidGen.leader_name);
// OUT_str(raidGen.player_name);
// OUT(_class);
// OUT(level);
// OUT(isGroupLeader);
// OUT(flags[0]);
// OUT(flags[1]);
// OUT(flags[2]);
// OUT(flags[3]);
// OUT(flags[4]);
// 
// dest->FastQueuePacket(&outapp);
// break;
// }
// case raidSetMotd:
// {
// RaidMOTD_Struct* emu = (RaidMOTD_Struct*)__emu_buffer;
// 
// auto outapp = new EQApplicationPacket(OP_RaidUpdate, sizeof(structs::RaidMOTD_Struct));
// structs::RaidMOTD_Struct* eq = (structs::RaidMOTD_Struct*)outapp->pBuffer;
// 
// OUT(general.action);
// OUT_str(general.player_name);
// OUT_str(general.leader_name);
// OUT_str(motd);
// 
// dest->FastQueuePacket(&outapp);
// break;
// }
// case raidSetLeaderAbilities:
// case raidMakeLeader:
// {
// RaidLeadershipUpdate_Struct* emu = (RaidLeadershipUpdate_Struct*)__emu_buffer;
// 
// auto outapp = new EQApplicationPacket(OP_RaidUpdate, sizeof(structs::RaidLeadershipUpdate_Struct));
// structs::RaidLeadershipUpdate_Struct* eq = (structs::RaidLeadershipUpdate_Struct*)outapp->pBuffer;
// 
// OUT(action);
// OUT_str(player_name);
// OUT_str(leader_name);
// memcpy(&eq->raid, &emu->raid, sizeof(RaidLeadershipAA_Struct));
// 
// dest->FastQueuePacket(&outapp);
// break;
// }
// case raidSetNote:
// {
// auto emu = (RaidNote_Struct*)__emu_buffer;
// 
// auto outapp = new EQApplicationPacket(OP_RaidUpdate, sizeof(structs::RaidNote_Struct));
// auto eq = (structs::RaidNote_Struct*)outapp->pBuffer;
// 
// OUT(general.action);
// OUT_str(general.leader_name);
// OUT_str(general.player_name);
// OUT_str(note);
// 
// dest->FastQueuePacket(&outapp);
// break;
// }
// case raidNoRaid:
// {
// dest->QueuePacket(inapp);
// break;
// }
// default:
// {
// RaidGeneral_Struct* emu = (RaidGeneral_Struct*)__emu_buffer;
// 
// auto outapp = new EQApplicationPacket(OP_RaidUpdate, sizeof(structs::RaidGeneral_Struct));
// structs::RaidGeneral_Struct* eq = (structs::RaidGeneral_Struct*)outapp->pBuffer;
// 
// OUT(action);
// OUT(parameter);
// OUT_str(leader_name);
// OUT_str(player_name);
// 
// dest->FastQueuePacket(&outapp);
// break;
// }
// }
// safe_delete(inapp);
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the RaidUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct RaidUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the raidgen value.
		/// </summary>
		public uint Raidgen { get; set; }

		/// <summary>
		/// Gets or sets the class value.
		/// </summary>
		public byte Class { get; set; }

		/// <summary>
		/// Gets or sets the level value.
		/// </summary>
		public byte Level { get; set; }

		/// <summary>
		/// Gets or sets the isgroupleader value.
		/// </summary>
		public byte Isgroupleader { get; set; }

		/// <summary>
		/// Gets or sets the flags value.
		/// </summary>
		public byte[] Flags { get; set; }

		/// <summary>
		/// Initializes a new instance of the RaidUpdate struct with specified field values.
		/// </summary>
		/// <param name="raidGen">The raidgen value.</param>
		/// <param name="_class">The class value.</param>
		/// <param name="level">The level value.</param>
		/// <param name="isGroupLeader">The isgroupleader value.</param>
		/// <param name="flags">The flags value.</param>
		public RaidUpdate(uint raidGen, byte _class, byte level, byte isGroupLeader, byte[] flags) : this() {
			Raidgen = raidGen;
			Class = _class;
			Level = level;
			Isgroupleader = isGroupLeader;
			Flags = flags;
		}

		/// <summary>
		/// Initializes a new instance of the RaidUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public RaidUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the RaidUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public RaidUpdate(BinaryReader br) : this() {
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
			Raidgen = br.ReadUInt32();
			Class = br.ReadByte();
			Level = br.ReadByte();
			Isgroupleader = br.ReadByte();
			// TODO: Array reading for Flags - implement based on actual array size
			// Flags = new byte[size];
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
			bw.Write(Raidgen);
			bw.Write(Class);
			bw.Write(Level);
			bw.Write(Isgroupleader);
			// TODO: Array writing for Flags - implement based on actual array size
			// foreach(var item in Flags) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct RaidUpdate {\n";
			ret += "	Raidgen = ";
			try {
				ret += $"{ Indentify(Raidgen) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Class = ";
			try {
				ret += $"{ Indentify(Class) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Level = ";
			try {
				ret += $"{ Indentify(Level) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Isgroupleader = ";
			try {
				ret += $"{ Indentify(Isgroupleader) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Flags = ";
			try {
				ret += $"{ Indentify(Flags) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}