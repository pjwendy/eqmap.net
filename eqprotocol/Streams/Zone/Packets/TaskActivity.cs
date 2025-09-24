using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TaskActivityComplete_Struct {
// uint32	TaskIndex;
// uint32	unknown2; // 0x00000002
// uint32	unknown3;
// uint32	ActivityID;
// uint32	unknown4; // 0x00000001
// uint32	unknown5; // 0x00000001
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TaskActivity packet structure for EverQuest network communication.
	/// </summary>
	public struct TaskActivity : IEQStruct {
		/// <summary>
		/// Gets or sets the taskindex value.
		/// </summary>
		public uint Taskindex { get; set; }

		/// <summary>
		/// Gets or sets the unknown2 value.
		/// </summary>
		public uint Unknown2 { get; set; }

		/// <summary>
		/// Gets or sets the unknown3 value.
		/// </summary>
		public uint Unknown3 { get; set; }

		/// <summary>
		/// Gets or sets the activityid value.
		/// </summary>
		public uint Activityid { get; set; }

		/// <summary>
		/// Gets or sets the unknown4 value.
		/// </summary>
		public uint Unknown4 { get; set; }

		/// <summary>
		/// Gets or sets the unknown5 value.
		/// </summary>
		public uint Unknown5 { get; set; }

		/// <summary>
		/// Initializes a new instance of the TaskActivity struct with specified field values.
		/// </summary>
		/// <param name="TaskIndex">The taskindex value.</param>
		/// <param name="unknown2">The unknown2 value.</param>
		/// <param name="unknown3">The unknown3 value.</param>
		/// <param name="ActivityID">The activityid value.</param>
		/// <param name="unknown4">The unknown4 value.</param>
		/// <param name="unknown5">The unknown5 value.</param>
		public TaskActivity(uint TaskIndex, uint unknown2, uint unknown3, uint ActivityID, uint unknown4, uint unknown5) : this() {
			Taskindex = TaskIndex;
			Unknown2 = unknown2;
			Unknown3 = unknown3;
			Activityid = ActivityID;
			Unknown4 = unknown4;
			Unknown5 = unknown5;
		}

		/// <summary>
		/// Initializes a new instance of the TaskActivity struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TaskActivity(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TaskActivity struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TaskActivity(BinaryReader br) : this() {
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
			Taskindex = br.ReadUInt32();
			Unknown2 = br.ReadUInt32();
			Unknown3 = br.ReadUInt32();
			Activityid = br.ReadUInt32();
			Unknown4 = br.ReadUInt32();
			Unknown5 = br.ReadUInt32();
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
			bw.Write(Taskindex);
			bw.Write(Unknown2);
			bw.Write(Unknown3);
			bw.Write(Activityid);
			bw.Write(Unknown4);
			bw.Write(Unknown5);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TaskActivity {\n";
			ret += "	Taskindex = ";
			try {
				ret += $"{ Indentify(Taskindex) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown2 = ";
			try {
				ret += $"{ Indentify(Unknown2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown3 = ";
			try {
				ret += $"{ Indentify(Unknown3) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Activityid = ";
			try {
				ret += $"{ Indentify(Activityid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown4 = ";
			try {
				ret += $"{ Indentify(Unknown4) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown5 = ";
			try {
				ret += $"{ Indentify(Unknown5) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}