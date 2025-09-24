using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LFPGetMatchesRequest_Struct {
// /*000*/	uint32	Unknown000;
// /*004*/	uint32	FromLevel;
// /*008*/	uint32	ToLevel;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LFPGetMatchesRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct LFPGetMatchesRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the fromlevel value.
		/// </summary>
		public uint Fromlevel { get; set; }

		/// <summary>
		/// Gets or sets the tolevel value.
		/// </summary>
		public uint Tolevel { get; set; }

		/// <summary>
		/// Initializes a new instance of the LFPGetMatchesRequest struct with specified field values.
		/// </summary>
		/// <param name="Unknown000">The unknown000 value.</param>
		/// <param name="FromLevel">The fromlevel value.</param>
		/// <param name="ToLevel">The tolevel value.</param>
		public LFPGetMatchesRequest(uint Unknown000, uint FromLevel, uint ToLevel) : this() {
			Unknown000 = Unknown000;
			Fromlevel = FromLevel;
			Tolevel = ToLevel;
		}

		/// <summary>
		/// Initializes a new instance of the LFPGetMatchesRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LFPGetMatchesRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LFPGetMatchesRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LFPGetMatchesRequest(BinaryReader br) : this() {
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
			Fromlevel = br.ReadUInt32();
			Tolevel = br.ReadUInt32();
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
			bw.Write(Fromlevel);
			bw.Write(Tolevel);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LFPGetMatchesRequest {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Fromlevel = ";
			try {
				ret += $"{ Indentify(Fromlevel) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Tolevel = ";
			try {
				ret += $"{ Indentify(Tolevel) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}