using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DisciplineTimer_Struct
// {
// /*00*/ uint32	TimerID;
// /*04*/ uint32	Duration;
// /*08*/ uint32	Unknown08;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DisciplineTimer packet structure for EverQuest network communication.
	/// </summary>
	public struct DisciplineTimer : IEQStruct {
		/// <summary>
		/// Gets or sets the timerid value.
		/// </summary>
		public uint Timerid { get; set; }

		/// <summary>
		/// Gets or sets the duration value.
		/// </summary>
		public uint Duration { get; set; }

		/// <summary>
		/// Gets or sets the unknown08 value.
		/// </summary>
		public uint Unknown08 { get; set; }

		/// <summary>
		/// Initializes a new instance of the DisciplineTimer struct with specified field values.
		/// </summary>
		/// <param name="TimerID">The timerid value.</param>
		/// <param name="Duration">The duration value.</param>
		/// <param name="Unknown08">The unknown08 value.</param>
		public DisciplineTimer(uint TimerID, uint Duration, uint Unknown08) : this() {
			Timerid = TimerID;
			Duration = Duration;
			Unknown08 = Unknown08;
		}

		/// <summary>
		/// Initializes a new instance of the DisciplineTimer struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DisciplineTimer(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DisciplineTimer struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DisciplineTimer(BinaryReader br) : this() {
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
			Timerid = br.ReadUInt32();
			Duration = br.ReadUInt32();
			Unknown08 = br.ReadUInt32();
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
			bw.Write(Timerid);
			bw.Write(Duration);
			bw.Write(Unknown08);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DisciplineTimer {\n";
			ret += "	Timerid = ";
			try {
				ret += $"{ Indentify(Timerid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Duration = ";
			try {
				ret += $"{ Indentify(Duration) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown08 = ";
			try {
				ret += $"{ Indentify(Unknown08) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}