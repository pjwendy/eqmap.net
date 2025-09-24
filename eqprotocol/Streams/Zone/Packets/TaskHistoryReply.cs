using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TaskHistoryReplyHeader_Struct {
// uint32	TaskID;
// uint32	ActivityCount;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TaskHistoryReply packet structure for EverQuest network communication.
	/// </summary>
	public struct TaskHistoryReply : IEQStruct {
		/// <summary>
		/// Gets or sets the taskid value.
		/// </summary>
		public uint Taskid { get; set; }

		/// <summary>
		/// Gets or sets the activitycount value.
		/// </summary>
		public uint Activitycount { get; set; }

		/// <summary>
		/// Initializes a new instance of the TaskHistoryReply struct with specified field values.
		/// </summary>
		/// <param name="TaskID">The taskid value.</param>
		/// <param name="ActivityCount">The activitycount value.</param>
		public TaskHistoryReply(uint TaskID, uint ActivityCount) : this() {
			Taskid = TaskID;
			Activitycount = ActivityCount;
		}

		/// <summary>
		/// Initializes a new instance of the TaskHistoryReply struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TaskHistoryReply(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TaskHistoryReply struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TaskHistoryReply(BinaryReader br) : this() {
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
			Taskid = br.ReadUInt32();
			Activitycount = br.ReadUInt32();
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
			bw.Write(Taskid);
			bw.Write(Activitycount);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TaskHistoryReply {\n";
			ret += "	Taskid = ";
			try {
				ret += $"{ Indentify(Taskid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Activitycount = ";
			try {
				ret += $"{ Indentify(Activitycount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}