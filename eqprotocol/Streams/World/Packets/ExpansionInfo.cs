using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ExpansionInfo_Struct {
// /*000*/	char	Unknown000[64];
// /*064*/	uint32	Expansions;
// };

// ENCODE/DECODE Section:
// ENCODE(OP_ExpansionInfo)
// {
// ENCODE_LENGTH_EXACT(ExpansionInfo_Struct);
// SETUP_DIRECT_ENCODE(ExpansionInfo_Struct, structs::ExpansionInfo_Struct);
// 
// OUT(Expansions);
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ExpansionInfo packet structure for EverQuest network communication.
	/// </summary>
	public struct ExpansionInfo : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public byte[] Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the expansions value.
		/// </summary>
		public uint Expansions { get; set; }

		/// <summary>
		/// Initializes a new instance of the ExpansionInfo struct with specified field values.
		/// </summary>
		/// <param name="Unknown000">The unknown000 value.</param>
		/// <param name="Expansions">The expansions value.</param>
		public ExpansionInfo(byte[] Unknown000, uint Expansions) : this() {
			Unknown000 = Unknown000;
			Expansions = Expansions;
		}

		/// <summary>
		/// Initializes a new instance of the ExpansionInfo struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ExpansionInfo(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ExpansionInfo struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ExpansionInfo(BinaryReader br) : this() {
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
			// TODO: Array reading for Unknown000 - implement based on actual array size
			// Unknown000 = new byte[size];
			Expansions = br.ReadUInt32();
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
			// TODO: Array writing for Unknown000 - implement based on actual array size
			// foreach(var item in Unknown000) bw.Write(item);
			bw.Write(Expansions);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ExpansionInfo {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Expansions = ";
			try {
				ret += $"{ Indentify(Expansions) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}