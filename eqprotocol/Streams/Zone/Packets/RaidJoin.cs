using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct RaidGeneral_Struct {
// /*00*/	uint32		action;
// /*04*/	char		player_name[64];
// /*68*/	uint32		unknown68;
// /*72*/	char		leader_name[64];
// /*136*/	uint32		parameter;
// };

// ENCODE/DECODE Section:
// ENCODE(OP_RaidJoin)
// {
// EQApplicationPacket* inapp = *p;
// *p = nullptr;
// unsigned char* __emu_buffer = inapp->pBuffer;
// RaidCreate_Struct* emu = (RaidCreate_Struct*)__emu_buffer;
// 
// auto outapp = new EQApplicationPacket(OP_RaidUpdate, sizeof(structs::RaidGeneral_Struct));
// structs::RaidGeneral_Struct* general = (structs::RaidGeneral_Struct*)outapp->pBuffer;
// 
// general->action = raidCreate;
// general->parameter = RaidCommandAcceptInvite;
// strn0cpy(general->leader_name, emu->leader_name, sizeof(emu->leader_name));
// strn0cpy(general->player_name, emu->leader_name, sizeof(emu->leader_name));
// 
// dest->FastQueuePacket(&outapp);
// 
// safe_delete(inapp);
// 
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the RaidJoin packet structure for EverQuest network communication.
	/// </summary>
	public struct RaidJoin : IEQStruct {
		/// <summary>
		/// Gets or sets the action value.
		/// </summary>
		public uint Action { get; set; }

		/// <summary>
		/// Gets or sets the playername value.
		/// </summary>
		public byte[] PlayerName { get; set; }

		/// <summary>
		/// Gets or sets the unknown68 value.
		/// </summary>
		public uint Unknown68 { get; set; }

		/// <summary>
		/// Gets or sets the leadername value.
		/// </summary>
		public byte[] LeaderName { get; set; }

		/// <summary>
		/// Gets or sets the parameter value.
		/// </summary>
		public uint Parameter { get; set; }

		/// <summary>
		/// Initializes a new instance of the RaidJoin struct with specified field values.
		/// </summary>
		/// <param name="action">The action value.</param>
		/// <param name="player_name">The playername value.</param>
		/// <param name="unknown68">The unknown68 value.</param>
		/// <param name="leader_name">The leadername value.</param>
		/// <param name="parameter">The parameter value.</param>
		public RaidJoin(uint action, byte[] player_name, uint unknown68, byte[] leader_name, uint parameter) : this() {
			Action = action;
			PlayerName = player_name;
			Unknown68 = unknown68;
			LeaderName = leader_name;
			Parameter = parameter;
		}

		/// <summary>
		/// Initializes a new instance of the RaidJoin struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public RaidJoin(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the RaidJoin struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public RaidJoin(BinaryReader br) : this() {
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
			Action = br.ReadUInt32();
			// TODO: Array reading for PlayerName - implement based on actual array size
			// PlayerName = new byte[size];
			Unknown68 = br.ReadUInt32();
			// TODO: Array reading for LeaderName - implement based on actual array size
			// LeaderName = new byte[size];
			Parameter = br.ReadUInt32();
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
			bw.Write(Action);
			// TODO: Array writing for PlayerName - implement based on actual array size
			// foreach(var item in PlayerName) bw.Write(item);
			bw.Write(Unknown68);
			// TODO: Array writing for LeaderName - implement based on actual array size
			// foreach(var item in LeaderName) bw.Write(item);
			bw.Write(Parameter);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct RaidJoin {\n";
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PlayerName = ";
			try {
				ret += $"{ Indentify(PlayerName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown68 = ";
			try {
				ret += $"{ Indentify(Unknown68) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LeaderName = ";
			try {
				ret += $"{ Indentify(LeaderName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Parameter = ";
			try {
				ret += $"{ Indentify(Parameter) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}