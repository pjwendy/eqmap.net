using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SetRunMode_Struct {
// uint8 mode;                    //01=run  00=walk
// uint8 unknown[3];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SetRunMode packet structure for EverQuest network communication.
	/// </summary>
	public struct SetRunMode : IEQStruct {
		/// <summary>
		/// Gets or sets the mode value.
		/// </summary>
		public byte Mode { get; set; }

		/// <summary>
		/// Gets or sets the unknown value.
		/// </summary>
		public byte[] Unknown { get; set; }

		/// <summary>
		/// Initializes a new instance of the SetRunMode struct with specified field values.
		/// </summary>
		/// <param name="mode">The mode value.</param>
		/// <param name="unknown">The unknown value.</param>
		public SetRunMode(byte mode, byte[] unknown) : this() {
			Mode = mode;
			Unknown = unknown;
		}

		/// <summary>
		/// Initializes a new instance of the SetRunMode struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SetRunMode(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SetRunMode struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SetRunMode(BinaryReader br) : this() {
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
			Mode = br.ReadByte();
			// TODO: Array reading for Unknown - implement based on actual array size
			// Unknown = new byte[size];
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
			bw.Write(Mode);
			// TODO: Array writing for Unknown - implement based on actual array size
			// foreach(var item in Unknown) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SetRunMode {\n";
			ret += "	Mode = ";
			try {
				ret += $"{ Indentify(Mode) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown = ";
			try {
				ret += $"{ Indentify(Unknown) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}