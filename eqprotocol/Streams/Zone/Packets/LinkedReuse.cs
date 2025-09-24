using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LinkedSpellReuseTimer_Struct {
// uint32 timer_id; // Timer ID of the spell
// uint32 end_time; // timestamp of when it will be ready
// uint32 start_time; // timestamp of when it started
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LinkedReuse packet structure for EverQuest network communication.
	/// </summary>
	public struct LinkedReuse : IEQStruct {
		/// <summary>
		/// Gets or sets the timerid value.
		/// </summary>
		public uint TimerId { get; set; }

		/// <summary>
		/// Gets or sets the endtime value.
		/// </summary>
		public uint EndTime { get; set; }

		/// <summary>
		/// Gets or sets the starttime value.
		/// </summary>
		public uint StartTime { get; set; }

		/// <summary>
		/// Initializes a new instance of the LinkedReuse struct with specified field values.
		/// </summary>
		/// <param name="timer_id">The timerid value.</param>
		/// <param name="end_time">The endtime value.</param>
		/// <param name="start_time">The starttime value.</param>
		public LinkedReuse(uint timer_id, uint end_time, uint start_time) : this() {
			TimerId = timer_id;
			EndTime = end_time;
			StartTime = start_time;
		}

		/// <summary>
		/// Initializes a new instance of the LinkedReuse struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LinkedReuse(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LinkedReuse struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LinkedReuse(BinaryReader br) : this() {
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
			TimerId = br.ReadUInt32();
			EndTime = br.ReadUInt32();
			StartTime = br.ReadUInt32();
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
			bw.Write(TimerId);
			bw.Write(EndTime);
			bw.Write(StartTime);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LinkedReuse {\n";
			ret += "	TimerId = ";
			try {
				ret += $"{ Indentify(TimerId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	EndTime = ";
			try {
				ret += $"{ Indentify(EndTime) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	StartTime = ";
			try {
				ret += $"{ Indentify(StartTime) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}