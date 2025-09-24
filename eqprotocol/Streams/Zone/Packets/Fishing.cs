using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct FindPersonRequest_Struct
// {
// /*000*/	uint32	unknown000;
// /*004*/	uint32	npc_id;
// /*008*/	FindPerson_Point client_pos;
// /*020*/	uint32	unknown020;
// /*024*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Fishing packet structure for EverQuest network communication.
	/// </summary>
	public struct Fishing : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the npcid value.
		/// </summary>
		public uint NpcId { get; set; }

		/// <summary>
		/// Gets or sets the clientpos value.
		/// </summary>
		public uint ClientPos { get; set; }

		/// <summary>
		/// Gets or sets the unknown020 value.
		/// </summary>
		public uint Unknown020 { get; set; }

		/// <summary>
		/// Initializes a new instance of the Fishing struct with specified field values.
		/// </summary>
		/// <param name="unknown000">The unknown000 value.</param>
		/// <param name="npc_id">The npcid value.</param>
		/// <param name="client_pos">The clientpos value.</param>
		/// <param name="unknown020">The unknown020 value.</param>
		public Fishing(uint unknown000, uint npc_id, uint client_pos, uint unknown020) : this() {
			Unknown000 = unknown000;
			NpcId = npc_id;
			ClientPos = client_pos;
			Unknown020 = unknown020;
		}

		/// <summary>
		/// Initializes a new instance of the Fishing struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Fishing(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Fishing struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Fishing(BinaryReader br) : this() {
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
			NpcId = br.ReadUInt32();
			ClientPos = br.ReadUInt32();
			Unknown020 = br.ReadUInt32();
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
			bw.Write(NpcId);
			bw.Write(ClientPos);
			bw.Write(Unknown020);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Fishing {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NpcId = ";
			try {
				ret += $"{ Indentify(NpcId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ClientPos = ";
			try {
				ret += $"{ Indentify(ClientPos) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown020 = ";
			try {
				ret += $"{ Indentify(Unknown020) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}