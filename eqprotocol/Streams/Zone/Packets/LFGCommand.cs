using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LFG_Struct {
// /*000*/	uint32 unknown000;
// /*004*/	uint32 value; // 0x00 = off 0x01 = on
// /*008*/	uint32 unknown008;
// /*012*/	uint32 unknown012;
// /*016*/	char	name[64];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LFGCommand packet structure for EverQuest network communication.
	/// </summary>
	public struct LFGCommand : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the value value.
		/// </summary>
		public uint Value { get; set; }

		/// <summary>
		/// Gets or sets the unknown008 value.
		/// </summary>
		public uint Unknown008 { get; set; }

		/// <summary>
		/// Gets or sets the unknown012 value.
		/// </summary>
		public uint Unknown012 { get; set; }

		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte[] Name { get; set; }

		/// <summary>
		/// Initializes a new instance of the LFGCommand struct with specified field values.
		/// </summary>
		/// <param name="unknown000">The unknown000 value.</param>
		/// <param name="value">The value value.</param>
		/// <param name="unknown008">The unknown008 value.</param>
		/// <param name="unknown012">The unknown012 value.</param>
		/// <param name="name">The name value.</param>
		public LFGCommand(uint unknown000, uint value, uint unknown008, uint unknown012, byte[] name) : this() {
			Unknown000 = unknown000;
			Value = value;
			Unknown008 = unknown008;
			Unknown012 = unknown012;
			Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the LFGCommand struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LFGCommand(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LFGCommand struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LFGCommand(BinaryReader br) : this() {
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
			Value = br.ReadUInt32();
			Unknown008 = br.ReadUInt32();
			Unknown012 = br.ReadUInt32();
			// TODO: Array reading for Name - implement based on actual array size
			// Name = new byte[size];
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
			bw.Write(Value);
			bw.Write(Unknown008);
			bw.Write(Unknown012);
			// TODO: Array writing for Name - implement based on actual array size
			// foreach(var item in Name) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LFGCommand {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value = ";
			try {
				ret += $"{ Indentify(Value) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown008 = ";
			try {
				ret += $"{ Indentify(Unknown008) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown012 = ";
			try {
				ret += $"{ Indentify(Unknown012) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}