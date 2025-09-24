using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AdventureRequestResponse_Struct{
// uint32 unknown000;
// char text[2048];
// uint32 timetoenter;
// uint32 timeleft;
// uint32 risk;
// float x;
// float y;
// float z;
// uint32 showcompass;
// uint32 unknown2080;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AdventureData packet structure for EverQuest network communication.
	/// </summary>
	public struct AdventureData : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the text value.
		/// </summary>
		public byte[] Text { get; set; }

		/// <summary>
		/// Gets or sets the timetoenter value.
		/// </summary>
		public uint Timetoenter { get; set; }

		/// <summary>
		/// Gets or sets the timeleft value.
		/// </summary>
		public uint Timeleft { get; set; }

		/// <summary>
		/// Gets or sets the risk value.
		/// </summary>
		public uint Risk { get; set; }

		/// <summary>
		/// Gets or sets the x value.
		/// </summary>
		public float X { get; set; }

		/// <summary>
		/// Gets or sets the y value.
		/// </summary>
		public float Y { get; set; }

		/// <summary>
		/// Gets or sets the z value.
		/// </summary>
		public float Z { get; set; }

		/// <summary>
		/// Gets or sets the showcompass value.
		/// </summary>
		public uint Showcompass { get; set; }

		/// <summary>
		/// Initializes a new instance of the AdventureData struct with specified field values.
		/// </summary>
		/// <param name="unknown000">The unknown000 value.</param>
		/// <param name="text">The text value.</param>
		/// <param name="timetoenter">The timetoenter value.</param>
		/// <param name="timeleft">The timeleft value.</param>
		/// <param name="risk">The risk value.</param>
		/// <param name="x">The x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="z">The z value.</param>
		/// <param name="showcompass">The showcompass value.</param>
		public AdventureData(uint unknown000, byte[] text, uint timetoenter, uint timeleft, uint risk, float x, float y, float z, uint showcompass) : this() {
			Unknown000 = unknown000;
			Text = text;
			Timetoenter = timetoenter;
			Timeleft = timeleft;
			Risk = risk;
			X = x;
			Y = y;
			Z = z;
			Showcompass = showcompass;
		}

		/// <summary>
		/// Initializes a new instance of the AdventureData struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AdventureData(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AdventureData struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AdventureData(BinaryReader br) : this() {
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
			Unknown000 = br.ReadUInt32();
			// TODO: Array reading for Text - implement based on actual array size
			// Text = new byte[size];
			Timetoenter = br.ReadUInt32();
			Timeleft = br.ReadUInt32();
			Risk = br.ReadUInt32();
			X = br.ReadSingle();
			Y = br.ReadSingle();
			Z = br.ReadSingle();
			Showcompass = br.ReadUInt32();
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
			bw.Write(Unknown000);
			// TODO: Array writing for Text - implement based on actual array size
			// foreach(var item in Text) bw.Write(item);
			bw.Write(Timetoenter);
			bw.Write(Timeleft);
			bw.Write(Risk);
			bw.Write(X);
			bw.Write(Y);
			bw.Write(Z);
			bw.Write(Showcompass);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AdventureData {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Text = ";
			try {
				ret += $"{ Indentify(Text) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Timetoenter = ";
			try {
				ret += $"{ Indentify(Timetoenter) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Timeleft = ";
			try {
				ret += $"{ Indentify(Timeleft) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Risk = ";
			try {
				ret += $"{ Indentify(Risk) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	X = ";
			try {
				ret += $"{ Indentify(X) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Y = ";
			try {
				ret += $"{ Indentify(Y) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Z = ";
			try {
				ret += $"{ Indentify(Z) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Showcompass = ";
			try {
				ret += $"{ Indentify(Showcompass) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}