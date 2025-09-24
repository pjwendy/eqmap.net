using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildTributeSelectReq_Struct {
// uint32 tribute_id;
// uint32 tier;
// uint32 tribute_id2;
// uint32 unknown12;    //seen A7 01 00 00
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace EQProtocol.Streams.Common.UnUsed_Packets {
	/// <summary>
	/// Represents the GuildModifyBenefits packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildModifyBenefits : IEQStruct {
		/// <summary>
		/// Gets or sets the tributeid value.
		/// </summary>
		public uint TributeId { get; set; }

		/// <summary>
		/// Gets or sets the tier value.
		/// </summary>
		public uint Tier { get; set; }

		/// <summary>
		/// Gets or sets the tributeid2 value.
		/// </summary>
		public uint TributeId2 { get; set; }

		/// <summary>
		/// Gets or sets the unknown12 value.
		/// </summary>
		public uint Unknown12 { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildModifyBenefits struct with specified field values.
		/// </summary>
		/// <param name="tribute_id">The tributeid value.</param>
		/// <param name="tier">The tier value.</param>
		/// <param name="tribute_id2">The tributeid2 value.</param>
		/// <param name="unknown12">The unknown12 value.</param>
		public GuildModifyBenefits(uint tribute_id, uint tier, uint tribute_id2, uint unknown12) : this() {
			TributeId = tribute_id;
			Tier = tier;
			TributeId2 = tribute_id2;
			Unknown12 = unknown12;
		}

		/// <summary>
		/// Initializes a new instance of the GuildModifyBenefits struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildModifyBenefits(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildModifyBenefits struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildModifyBenefits(BinaryReader br) : this() {
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
			TributeId = br.ReadUInt32();
			Tier = br.ReadUInt32();
			TributeId2 = br.ReadUInt32();
			Unknown12 = br.ReadUInt32();
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
			bw.Write(TributeId);
			bw.Write(Tier);
			bw.Write(TributeId2);
			bw.Write(Unknown12);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildModifyBenefits {\n";
			ret += "	TributeId = ";
			try {
				ret += $"{ Indentify(TributeId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Tier = ";
			try {
				ret += $"{ Indentify(Tier) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeId2 = ";
			try {
				ret += $"{ Indentify(TributeId2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown12 = ";
			try {
				ret += $"{ Indentify(Unknown12) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}