using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TimeOfDay_Struct {
// uint8	hour;
// uint8	minute;
// uint8	day;
// uint8	month;
// uint16	year;
// /*0006*/ uint16 unknown0016;            // Placeholder
// /*0008*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TimeOfDay packet structure for EverQuest network communication.
	/// </summary>
	public struct TimeOfDay : IEQStruct {
		/// <summary>
		/// Gets or sets the hour value.
		/// </summary>
		public byte Hour { get; set; }

		/// <summary>
		/// Gets or sets the minute value.
		/// </summary>
		public byte Minute { get; set; }

		/// <summary>
		/// Gets or sets the day value.
		/// </summary>
		public byte Day { get; set; }

		/// <summary>
		/// Gets or sets the month value.
		/// </summary>
		public byte Month { get; set; }

		/// <summary>
		/// Gets or sets the year value.
		/// </summary>
		public ushort Year { get; set; }

		/// <summary>
		/// Initializes a new instance of the TimeOfDay struct with specified field values.
		/// </summary>
		/// <param name="hour">The hour value.</param>
		/// <param name="minute">The minute value.</param>
		/// <param name="day">The day value.</param>
		/// <param name="month">The month value.</param>
		/// <param name="year">The year value.</param>
		public TimeOfDay(byte hour, byte minute, byte day, byte month, ushort year) : this() {
			Hour = hour;
			Minute = minute;
			Day = day;
			Month = month;
			Year = year;
		}

		/// <summary>
		/// Initializes a new instance of the TimeOfDay struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TimeOfDay(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TimeOfDay struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TimeOfDay(BinaryReader br) : this() {
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
			Hour = br.ReadByte();
			Minute = br.ReadByte();
			Day = br.ReadByte();
			Month = br.ReadByte();
			Year = br.ReadUInt16();
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
			bw.Write(Hour);
			bw.Write(Minute);
			bw.Write(Day);
			bw.Write(Month);
			bw.Write(Year);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TimeOfDay {\n";
			ret += "	Hour = ";
			try {
				ret += $"{ Indentify(Hour) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Minute = ";
			try {
				ret += $"{ Indentify(Minute) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Day = ";
			try {
				ret += $"{ Indentify(Day) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Month = ";
			try {
				ret += $"{ Indentify(Month) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Year = ";
			try {
				ret += $"{ Indentify(Year) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}