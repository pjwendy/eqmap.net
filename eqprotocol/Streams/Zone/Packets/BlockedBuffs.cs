using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BlockedBuffs_Struct {
// /*000*/	uint8	unknown000[80];
// /*080*/	uint8	unknown081;
// /*081*/	uint8	unknown082;
// /*082*/	uint8	unknown083;
// /*083*/	uint8	unknown084;
// /*084*/	uint8	unknown085;
// /*085*/	uint8	unknown086;
// /*086*/	uint8	unknown087;
// /*087*/	uint8	unknown088;
// /*088*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the BlockedBuffs packet structure for EverQuest network communication.
	/// </summary>
	public struct BlockedBuffs : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public byte[] Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the unknown081 value.
		/// </summary>
		public byte Unknown081 { get; set; }

		/// <summary>
		/// Gets or sets the unknown082 value.
		/// </summary>
		public byte Unknown082 { get; set; }

		/// <summary>
		/// Gets or sets the unknown083 value.
		/// </summary>
		public byte Unknown083 { get; set; }

		/// <summary>
		/// Gets or sets the unknown084 value.
		/// </summary>
		public byte Unknown084 { get; set; }

		/// <summary>
		/// Gets or sets the unknown085 value.
		/// </summary>
		public byte Unknown085 { get; set; }

		/// <summary>
		/// Gets or sets the unknown086 value.
		/// </summary>
		public byte Unknown086 { get; set; }

		/// <summary>
		/// Gets or sets the unknown087 value.
		/// </summary>
		public byte Unknown087 { get; set; }

		/// <summary>
		/// Gets or sets the unknown088 value.
		/// </summary>
		public byte Unknown088 { get; set; }

		/// <summary>
		/// Initializes a new instance of the BlockedBuffs struct with specified field values.
		/// </summary>
		/// <param name="unknown000">The unknown000 value.</param>
		/// <param name="unknown081">The unknown081 value.</param>
		/// <param name="unknown082">The unknown082 value.</param>
		/// <param name="unknown083">The unknown083 value.</param>
		/// <param name="unknown084">The unknown084 value.</param>
		/// <param name="unknown085">The unknown085 value.</param>
		/// <param name="unknown086">The unknown086 value.</param>
		/// <param name="unknown087">The unknown087 value.</param>
		/// <param name="unknown088">The unknown088 value.</param>
		public BlockedBuffs(byte[] unknown000, byte unknown081, byte unknown082, byte unknown083, byte unknown084, byte unknown085, byte unknown086, byte unknown087, byte unknown088) : this() {
			Unknown000 = unknown000;
			Unknown081 = unknown081;
			Unknown082 = unknown082;
			Unknown083 = unknown083;
			Unknown084 = unknown084;
			Unknown085 = unknown085;
			Unknown086 = unknown086;
			Unknown087 = unknown087;
			Unknown088 = unknown088;
		}

		/// <summary>
		/// Initializes a new instance of the BlockedBuffs struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public BlockedBuffs(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the BlockedBuffs struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public BlockedBuffs(BinaryReader br) : this() {
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
			Unknown081 = br.ReadByte();
			Unknown082 = br.ReadByte();
			Unknown083 = br.ReadByte();
			Unknown084 = br.ReadByte();
			Unknown085 = br.ReadByte();
			Unknown086 = br.ReadByte();
			Unknown087 = br.ReadByte();
			Unknown088 = br.ReadByte();
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
			bw.Write(Unknown081);
			bw.Write(Unknown082);
			bw.Write(Unknown083);
			bw.Write(Unknown084);
			bw.Write(Unknown085);
			bw.Write(Unknown086);
			bw.Write(Unknown087);
			bw.Write(Unknown088);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct BlockedBuffs {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown081 = ";
			try {
				ret += $"{ Indentify(Unknown081) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown082 = ";
			try {
				ret += $"{ Indentify(Unknown082) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown083 = ";
			try {
				ret += $"{ Indentify(Unknown083) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown084 = ";
			try {
				ret += $"{ Indentify(Unknown084) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown085 = ";
			try {
				ret += $"{ Indentify(Unknown085) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown086 = ";
			try {
				ret += $"{ Indentify(Unknown086) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown087 = ";
			try {
				ret += $"{ Indentify(Unknown087) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown088 = ";
			try {
				ret += $"{ Indentify(Unknown088) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}