using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ClearObject_Struct
// {
// /*000*/	uint8	Clear;	// If this is not set to non-zero there is a random chance of a client crash.
// /*001*/	uint8	Unknown001[7];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ClearObject packet structure for EverQuest network communication.
	/// </summary>
	public struct ClearObject : IEQStruct {
		/// <summary>
		/// Gets or sets the clear value.
		/// </summary>
		public byte Clear { get; set; }

		/// <summary>
		/// Gets or sets the unknown001 value.
		/// </summary>
		public byte[] Unknown001 { get; set; }

		/// <summary>
		/// Initializes a new instance of the ClearObject struct with specified field values.
		/// </summary>
		/// <param name="Clear">The clear value.</param>
		/// <param name="Unknown001">The unknown001 value.</param>
		public ClearObject(byte Clear, byte[] Unknown001) : this() {
			Clear = Clear;
			Unknown001 = Unknown001;
		}

		/// <summary>
		/// Initializes a new instance of the ClearObject struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ClearObject(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ClearObject struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ClearObject(BinaryReader br) : this() {
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
			Clear = br.ReadByte();
			// TODO: Array reading for Unknown001 - implement based on actual array size
			// Unknown001 = new byte[size];
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
			bw.Write(Clear);
			// TODO: Array writing for Unknown001 - implement based on actual array size
			// foreach(var item in Unknown001) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ClearObject {\n";
			ret += "	Clear = ";
			try {
				ret += $"{ Indentify(Clear) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown001 = ";
			try {
				ret += $"{ Indentify(Unknown001) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}