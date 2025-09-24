using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ExpeditionCommandSwap_Struct
// {
// /*000*/ uint32 unknown000;
// /*004*/ uint32 unknown004;
// /*008*/ char   add_player_name[64]; // swap to (player must confirm)
// /*072*/ char   rem_player_name[64]; // swap from
// };

// ENCODE/DECODE Section:
// DECODE(OP_DzSwapPlayer)
// {
// DECODE_LENGTH_EXACT(structs::ExpeditionCommandSwap_Struct);
// SETUP_DIRECT_DECODE(ExpeditionCommandSwap_Struct, structs::ExpeditionCommandSwap_Struct);
// 
// strn0cpy(emu->add_player_name, eq->add_player_name, sizeof(emu->add_player_name));
// strn0cpy(emu->rem_player_name, eq->rem_player_name, sizeof(emu->rem_player_name));
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DzSwapPlayer packet structure for EverQuest network communication.
	/// </summary>
	public struct DzSwapPlayer : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public uint Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the addplayername value.
		/// </summary>
		public byte[] AddPlayerName { get; set; }

		/// <summary>
		/// Gets or sets the remplayername value.
		/// </summary>
		public byte[] RemPlayerName { get; set; }

		/// <summary>
		/// Initializes a new instance of the DzSwapPlayer struct with specified field values.
		/// </summary>
		/// <param name="unknown000">The unknown000 value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="add_player_name">The addplayername value.</param>
		/// <param name="rem_player_name">The remplayername value.</param>
		public DzSwapPlayer(uint unknown000, uint unknown004, byte[] add_player_name, byte[] rem_player_name) : this() {
			Unknown000 = unknown000;
			Unknown004 = unknown004;
			AddPlayerName = add_player_name;
			RemPlayerName = rem_player_name;
		}

		/// <summary>
		/// Initializes a new instance of the DzSwapPlayer struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DzSwapPlayer(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DzSwapPlayer struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DzSwapPlayer(BinaryReader br) : this() {
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
			Unknown000 = br.ReadUInt32();
			Unknown004 = br.ReadUInt32();
			// TODO: Array reading for AddPlayerName - implement based on actual array size
			// AddPlayerName = new byte[size];
			// TODO: Array reading for RemPlayerName - implement based on actual array size
			// RemPlayerName = new byte[size];
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
			bw.Write(Unknown000);
			bw.Write(Unknown004);
			// TODO: Array writing for AddPlayerName - implement based on actual array size
			// foreach(var item in AddPlayerName) bw.Write(item);
			// TODO: Array writing for RemPlayerName - implement based on actual array size
			// foreach(var item in RemPlayerName) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DzSwapPlayer {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AddPlayerName = ";
			try {
				ret += $"{ Indentify(AddPlayerName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	RemPlayerName = ";
			try {
				ret += $"{ Indentify(RemPlayerName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}