using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct RandomReply_Struct {
// /* 00 */	uint32 low;
// /* 04 */	uint32 high;
// /* 08 */	uint32 result;
// /* 12 */	char name[64];
// /* 76 */
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the RandomReply packet structure for EverQuest network communication.
	/// </summary>
	public struct RandomReply : IEQStruct {
		/// <summary>
		/// Gets or sets the low value.
		/// </summary>
		public uint Low { get; set; }

		/// <summary>
		/// Gets or sets the high value.
		/// </summary>
		public uint High { get; set; }

		/// <summary>
		/// Gets or sets the result value.
		/// </summary>
		public uint Result { get; set; }

		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte[] Name { get; set; }

		/// <summary>
		/// Initializes a new instance of the RandomReply struct with specified field values.
		/// </summary>
		/// <param name="low">The low value.</param>
		/// <param name="high">The high value.</param>
		/// <param name="result">The result value.</param>
		/// <param name="name">The name value.</param>
		public RandomReply(uint low, uint high, uint result, byte[] name) : this() {
			Low = low;
			High = high;
			Result = result;
			Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the RandomReply struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public RandomReply(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the RandomReply struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public RandomReply(BinaryReader br) : this() {
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
			Low = br.ReadUInt32();
			High = br.ReadUInt32();
			Result = br.ReadUInt32();
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
			bw.Write(Low);
			bw.Write(High);
			bw.Write(Result);
			// TODO: Array writing for Name - implement based on actual array size
			// foreach(var item in Name) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct RandomReply {\n";
			ret += "	Low = ";
			try {
				ret += $"{ Indentify(Low) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	High = ";
			try {
				ret += $"{ Indentify(High) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Result = ";
			try {
				ret += $"{ Indentify(Result) },\n";
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