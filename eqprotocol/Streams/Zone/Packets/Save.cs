using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Save_Struct {
// uint8	unknown00[192];
// uint8	unknown0192[176];
// };

// ENCODE/DECODE Section:
// DECODE(OP_Save)
// {
// DECODE_LENGTH_EXACT(structs::Save_Struct);
// SETUP_DIRECT_DECODE(Save_Struct, structs::Save_Struct);
// 
// memcpy(emu->unknown00, eq->unknown00, sizeof(emu->unknown00));
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Save packet structure for EverQuest network communication.
	/// </summary>
	public struct Save : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown00 value.
		/// </summary>
		public byte[] Unknown00 { get; set; }

		/// <summary>
		/// Initializes a new instance of the Save struct with specified field values.
		/// </summary>
		/// <param name="unknown00">The unknown00 value.</param>
		public Save(byte[] unknown00) : this() {
			Unknown00 = unknown00;
		}

		/// <summary>
		/// Initializes a new instance of the Save struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Save(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Save struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Save(BinaryReader br) : this() {
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
			// TODO: Array reading for Unknown00 - implement based on actual array size
			// Unknown00 = new byte[size];
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
			// TODO: Array writing for Unknown00 - implement based on actual array size
			// foreach(var item in Unknown00) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Save {\n";
			ret += "	Unknown00 = ";
			try {
				ret += $"{ Indentify(Unknown00) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}