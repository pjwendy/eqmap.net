using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BindWound_Struct
// {
// /*000*/	uint16	to;			// TargetID
// /*002*/	uint16	unknown2;		// ***Placeholder
// /*004*/	uint16	type;
// /*006*/	uint16	unknown6;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the BindWound packet structure for EverQuest network communication.
	/// </summary>
	public struct BindWound : IEQStruct {
		/// <summary>
		/// Gets or sets the to value.
		/// </summary>
		public ushort To { get; set; }

		/// <summary>
		/// Gets or sets the unknown2 value.
		/// </summary>
		public ushort Unknown2 { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public ushort Type { get; set; }

		/// <summary>
		/// Gets or sets the unknown6 value.
		/// </summary>
		public ushort Unknown6 { get; set; }

		/// <summary>
		/// Initializes a new instance of the BindWound struct with specified field values.
		/// </summary>
		/// <param name="to">The to value.</param>
		/// <param name="unknown2">The unknown2 value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="unknown6">The unknown6 value.</param>
		public BindWound(ushort to, ushort unknown2, ushort type, ushort unknown6) : this() {
			To = to;
			Unknown2 = unknown2;
			Type = type;
			Unknown6 = unknown6;
		}

		/// <summary>
		/// Initializes a new instance of the BindWound struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public BindWound(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the BindWound struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public BindWound(BinaryReader br) : this() {
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
			To = br.ReadUInt16();
			Unknown2 = br.ReadUInt16();
			Type = br.ReadUInt16();
			Unknown6 = br.ReadUInt16();
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
			bw.Write(To);
			bw.Write(Unknown2);
			bw.Write(Type);
			bw.Write(Unknown6);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct BindWound {\n";
			ret += "	To = ";
			try {
				ret += $"{ Indentify(To) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown2 = ";
			try {
				ret += $"{ Indentify(Unknown2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown6 = ";
			try {
				ret += $"{ Indentify(Unknown6) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}