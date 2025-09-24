using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MerchantClick_Struct {
// /*000*/ uint32	npc_id;			// Merchant NPC's entity id
// /*004*/ uint32	player_id;
// /*008*/ uint32	command;		//1=open, 0=cancel/close
// /*012*/ float	rate;			//cost multiplier, dosent work anymore
// };

// ENCODE/DECODE Section:
// DECODE(OP_ShopRequest)
// {
// DECODE_LENGTH_EXACT(structs::MerchantClick_Struct);
// SETUP_DIRECT_DECODE(MerchantClick_Struct, structs::MerchantClick_Struct);
// 
// IN(npc_id);
// IN(player_id);
// IN(command);
// IN(rate);
// emu->tab_display = 0;
// emu->unknown020 = 0;
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ShopRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct ShopRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the npcid value.
		/// </summary>
		public uint NpcId { get; set; }

		/// <summary>
		/// Gets or sets the playerid value.
		/// </summary>
		public uint PlayerId { get; set; }

		/// <summary>
		/// Gets or sets the command value.
		/// </summary>
		public uint Command { get; set; }

		/// <summary>
		/// Gets or sets the rate value.
		/// </summary>
		public float Rate { get; set; }

		/// <summary>
		/// Initializes a new instance of the ShopRequest struct with specified field values.
		/// </summary>
		/// <param name="npc_id">The npcid value.</param>
		/// <param name="player_id">The playerid value.</param>
		/// <param name="command">The command value.</param>
		/// <param name="rate">The rate value.</param>
		public ShopRequest(uint npc_id, uint player_id, uint command, float rate) : this() {
			NpcId = npc_id;
			PlayerId = player_id;
			Command = command;
			Rate = rate;
		}

		/// <summary>
		/// Initializes a new instance of the ShopRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ShopRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ShopRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ShopRequest(BinaryReader br) : this() {
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
			NpcId = br.ReadUInt32();
			PlayerId = br.ReadUInt32();
			Command = br.ReadUInt32();
			Rate = br.ReadSingle();
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
			bw.Write(NpcId);
			bw.Write(PlayerId);
			bw.Write(Command);
			bw.Write(Rate);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ShopRequest {\n";
			ret += "	NpcId = ";
			try {
				ret += $"{ Indentify(NpcId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PlayerId = ";
			try {
				ret += $"{ Indentify(PlayerId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Command = ";
			try {
				ret += $"{ Indentify(Command) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Rate = ";
			try {
				ret += $"{ Indentify(Rate) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}