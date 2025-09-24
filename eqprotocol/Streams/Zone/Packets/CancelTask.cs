using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct CancelTask_Struct {
// uint32 SequenceNumber;
// uint32 unknown4; // Only seen 0x00000002
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the CancelTask packet structure for EverQuest network communication.
	/// </summary>
	public struct CancelTask : IEQStruct {
		/// <summary>
		/// Gets or sets the sequencenumber value.
		/// </summary>
		public uint Sequencenumber { get; set; }

		/// <summary>
		/// Gets or sets the unknown4 value.
		/// </summary>
		public uint Unknown4 { get; set; }

		/// <summary>
		/// Initializes a new instance of the CancelTask struct with specified field values.
		/// </summary>
		/// <param name="SequenceNumber">The sequencenumber value.</param>
		/// <param name="unknown4">The unknown4 value.</param>
		public CancelTask(uint SequenceNumber, uint unknown4) : this() {
			Sequencenumber = SequenceNumber;
			Unknown4 = unknown4;
		}

		/// <summary>
		/// Initializes a new instance of the CancelTask struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public CancelTask(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the CancelTask struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public CancelTask(BinaryReader br) : this() {
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
			Sequencenumber = br.ReadUInt32();
			Unknown4 = br.ReadUInt32();
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
			bw.Write(Sequencenumber);
			bw.Write(Unknown4);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct CancelTask {\n";
			ret += "	Sequencenumber = ";
			try {
				ret += $"{ Indentify(Sequencenumber) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown4 = ";
			try {
				ret += $"{ Indentify(Unknown4) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}