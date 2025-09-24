using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AcceptNewTask_Struct {
// uint32  unknown00;
// uint32	task_id;		//set to 0 for 'decline'
// uint32	task_master_id;	//entity ID
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AcceptNewTask packet structure for EverQuest network communication.
	/// </summary>
	public struct AcceptNewTask : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown00 value.
		/// </summary>
		public uint Unknown00 { get; set; }

		/// <summary>
		/// Gets or sets the taskid value.
		/// </summary>
		public uint TaskId { get; set; }

		/// <summary>
		/// Gets or sets the taskmasterid value.
		/// </summary>
		public uint TaskMasterId { get; set; }

		/// <summary>
		/// Initializes a new instance of the AcceptNewTask struct with specified field values.
		/// </summary>
		/// <param name="unknown00">The unknown00 value.</param>
		/// <param name="task_id">The taskid value.</param>
		/// <param name="task_master_id">The taskmasterid value.</param>
		public AcceptNewTask(uint unknown00, uint task_id, uint task_master_id) : this() {
			Unknown00 = unknown00;
			TaskId = task_id;
			TaskMasterId = task_master_id;
		}

		/// <summary>
		/// Initializes a new instance of the AcceptNewTask struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AcceptNewTask(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AcceptNewTask struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AcceptNewTask(BinaryReader br) : this() {
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
			Unknown00 = br.ReadUInt32();
			TaskId = br.ReadUInt32();
			TaskMasterId = br.ReadUInt32();
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
			bw.Write(Unknown00);
			bw.Write(TaskId);
			bw.Write(TaskMasterId);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AcceptNewTask {\n";
			ret += "	Unknown00 = ";
			try {
				ret += $"{ Indentify(Unknown00) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TaskId = ";
			try {
				ret += $"{ Indentify(TaskId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TaskMasterId = ";
			try {
				ret += $"{ Indentify(TaskMasterId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}