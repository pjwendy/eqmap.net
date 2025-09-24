using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SimpleMessage_Struct{
// uint32	string_id;
// uint32	color;
// uint32	unknown8;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SimpleMessage packet structure for EverQuest network communication.
	/// </summary>
	public struct SimpleMessage : IEQStruct {
		/// <summary>
		/// Gets or sets the stringid value.
		/// </summary>
		public uint StringId { get; set; }

		/// <summary>
		/// Gets or sets the color value.
		/// </summary>
		public uint Color { get; set; }

		/// <summary>
		/// Gets or sets the unknown8 value.
		/// </summary>
		public uint Unknown8 { get; set; }

		/// <summary>
		/// Initializes a new instance of the SimpleMessage struct with specified field values.
		/// </summary>
		/// <param name="string_id">The stringid value.</param>
		/// <param name="color">The color value.</param>
		/// <param name="unknown8">The unknown8 value.</param>
		public SimpleMessage(uint string_id, uint color, uint unknown8) : this() {
			StringId = string_id;
			Color = color;
			Unknown8 = unknown8;
		}

		/// <summary>
		/// Initializes a new instance of the SimpleMessage struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SimpleMessage(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SimpleMessage struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SimpleMessage(BinaryReader br) : this() {
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
			StringId = br.ReadUInt32();
			Color = br.ReadUInt32();
			Unknown8 = br.ReadUInt32();
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
			bw.Write(StringId);
			bw.Write(Color);
			bw.Write(Unknown8);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SimpleMessage {\n";
			ret += "	StringId = ";
			try {
				ret += $"{ Indentify(StringId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Color = ";
			try {
				ret += $"{ Indentify(Color) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown8 = ";
			try {
				ret += $"{ Indentify(Unknown8) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}