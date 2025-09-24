using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GMToggle_Struct {
// uint8 unknown0[64];
// uint32 toggle;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GMToggle packet structure for EverQuest network communication.
	/// </summary>
	public struct GMToggle : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown0 value.
		/// </summary>
		public byte[] Unknown0 { get; set; }

		/// <summary>
		/// Gets or sets the toggle value.
		/// </summary>
		public uint Toggle { get; set; }

		/// <summary>
		/// Initializes a new instance of the GMToggle struct with specified field values.
		/// </summary>
		/// <param name="unknown0">The unknown0 value.</param>
		/// <param name="toggle">The toggle value.</param>
		public GMToggle(byte[] unknown0, uint toggle) : this() {
			Unknown0 = unknown0;
			Toggle = toggle;
		}

		/// <summary>
		/// Initializes a new instance of the GMToggle struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GMToggle(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GMToggle struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GMToggle(BinaryReader br) : this() {
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
			// TODO: Array reading for Unknown0 - implement based on actual array size
			// Unknown0 = new byte[size];
			Toggle = br.ReadUInt32();
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
			// TODO: Array writing for Unknown0 - implement based on actual array size
			// foreach(var item in Unknown0) bw.Write(item);
			bw.Write(Toggle);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GMToggle {\n";
			ret += "	Unknown0 = ";
			try {
				ret += $"{ Indentify(Unknown0) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Toggle = ";
			try {
				ret += $"{ Indentify(Toggle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}