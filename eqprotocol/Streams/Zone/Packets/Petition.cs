using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct PetitionUpdate_Struct {
// uint32 petnumber;    // Petition Number
// uint32 color;		// 0x00 = green, 0x01 = yellow, 0x02 = red
// uint32 status;
// time_t senttime;    // 4 has to be 0x1F
// char accountid[32];
// char gmsenttoo[64];
// int32 quetotal;
// char charname[64];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Petition packet structure for EverQuest network communication.
	/// </summary>
	public struct Petition : IEQStruct {
		/// <summary>
		/// Gets or sets the petnumber value.
		/// </summary>
		public uint Petnumber { get; set; }

		/// <summary>
		/// Gets or sets the color value.
		/// </summary>
		public uint Color { get; set; }

		/// <summary>
		/// Gets or sets the status value.
		/// </summary>
		public uint Status { get; set; }

		/// <summary>
		/// Gets or sets the senttime value.
		/// </summary>
		public uint Senttime { get; set; }

		/// <summary>
		/// Gets or sets the accountid value.
		/// </summary>
		public byte[] Accountid { get; set; }

		/// <summary>
		/// Gets or sets the gmsenttoo value.
		/// </summary>
		public byte[] Gmsenttoo { get; set; }

		/// <summary>
		/// Gets or sets the quetotal value.
		/// </summary>
		public int Quetotal { get; set; }

		/// <summary>
		/// Gets or sets the charname value.
		/// </summary>
		public byte[] Charname { get; set; }

		/// <summary>
		/// Initializes a new instance of the Petition struct with specified field values.
		/// </summary>
		/// <param name="petnumber">The petnumber value.</param>
		/// <param name="color">The color value.</param>
		/// <param name="status">The status value.</param>
		/// <param name="senttime">The senttime value.</param>
		/// <param name="accountid">The accountid value.</param>
		/// <param name="gmsenttoo">The gmsenttoo value.</param>
		/// <param name="quetotal">The quetotal value.</param>
		/// <param name="charname">The charname value.</param>
		public Petition(uint petnumber, uint color, uint status, uint senttime, byte[] accountid, byte[] gmsenttoo, int quetotal, byte[] charname) : this() {
			Petnumber = petnumber;
			Color = color;
			Status = status;
			Senttime = senttime;
			Accountid = accountid;
			Gmsenttoo = gmsenttoo;
			Quetotal = quetotal;
			Charname = charname;
		}

		/// <summary>
		/// Initializes a new instance of the Petition struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Petition(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Petition struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Petition(BinaryReader br) : this() {
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
			Petnumber = br.ReadUInt32();
			Color = br.ReadUInt32();
			Status = br.ReadUInt32();
			Senttime = br.ReadUInt32();
			// TODO: Array reading for Accountid - implement based on actual array size
			// Accountid = new byte[size];
			// TODO: Array reading for Gmsenttoo - implement based on actual array size
			// Gmsenttoo = new byte[size];
			Quetotal = br.ReadInt32();
			// TODO: Array reading for Charname - implement based on actual array size
			// Charname = new byte[size];
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
			bw.Write(Petnumber);
			bw.Write(Color);
			bw.Write(Status);
			bw.Write(Senttime);
			// TODO: Array writing for Accountid - implement based on actual array size
			// foreach(var item in Accountid) bw.Write(item);
			// TODO: Array writing for Gmsenttoo - implement based on actual array size
			// foreach(var item in Gmsenttoo) bw.Write(item);
			bw.Write(Quetotal);
			// TODO: Array writing for Charname - implement based on actual array size
			// foreach(var item in Charname) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Petition {\n";
			ret += "	Petnumber = ";
			try {
				ret += $"{ Indentify(Petnumber) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Color = ";
			try {
				ret += $"{ Indentify(Color) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Status = ";
			try {
				ret += $"{ Indentify(Status) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Senttime = ";
			try {
				ret += $"{ Indentify(Senttime) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Accountid = ";
			try {
				ret += $"{ Indentify(Accountid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Gmsenttoo = ";
			try {
				ret += $"{ Indentify(Gmsenttoo) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Quetotal = ";
			try {
				ret += $"{ Indentify(Quetotal) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Charname = ";
			try {
				ret += $"{ Indentify(Charname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}