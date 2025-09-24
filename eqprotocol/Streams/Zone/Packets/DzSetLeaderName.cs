using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DynamicZoneLeaderName_Struct
// {
// /*000*/ uint32 client_id;
// /*004*/ uint32 unknown004;
// /*008*/ char   leader_name[64];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_DzSetLeaderName)
// {
// ENCODE_LENGTH_EXACT(DynamicZoneLeaderName_Struct);
// SETUP_DIRECT_ENCODE(DynamicZoneLeaderName_Struct, structs::DynamicZoneLeaderName_Struct);
// 
// OUT(client_id);
// strn0cpy(eq->leader_name, emu->leader_name, sizeof(eq->leader_name));
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DzSetLeaderName packet structure for EverQuest network communication.
	/// </summary>
	public struct DzSetLeaderName : IEQStruct {
		/// <summary>
		/// Gets or sets the clientid value.
		/// </summary>
		public uint ClientId { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public uint Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the leadername value.
		/// </summary>
		public byte[] LeaderName { get; set; }

		/// <summary>
		/// Initializes a new instance of the DzSetLeaderName struct with specified field values.
		/// </summary>
		/// <param name="client_id">The clientid value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="leader_name">The leadername value.</param>
		public DzSetLeaderName(uint client_id, uint unknown004, byte[] leader_name) : this() {
			ClientId = client_id;
			Unknown004 = unknown004;
			LeaderName = leader_name;
		}

		/// <summary>
		/// Initializes a new instance of the DzSetLeaderName struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DzSetLeaderName(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DzSetLeaderName struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DzSetLeaderName(BinaryReader br) : this() {
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
			ClientId = br.ReadUInt32();
			Unknown004 = br.ReadUInt32();
			// TODO: Array reading for LeaderName - implement based on actual array size
			// LeaderName = new byte[size];
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
			bw.Write(ClientId);
			bw.Write(Unknown004);
			// TODO: Array writing for LeaderName - implement based on actual array size
			// foreach(var item in LeaderName) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DzSetLeaderName {\n";
			ret += "	ClientId = ";
			try {
				ret += $"{ Indentify(ClientId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LeaderName = ";
			try {
				ret += $"{ Indentify(LeaderName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}