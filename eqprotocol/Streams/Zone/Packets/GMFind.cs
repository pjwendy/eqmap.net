using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GMFind_Struct {
// char	charname[64];
// char	gmname[64];
// uint32	success;
// uint32	zoneID;
// float	x;
// float	y;
// float	z;
// uint32	unknown2;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GMFind packet structure for EverQuest network communication.
	/// </summary>
	public struct GMFind : IEQStruct {
		/// <summary>
		/// Gets or sets the charname value.
		/// </summary>
		public byte[] Charname { get; set; }

		/// <summary>
		/// Gets or sets the gmname value.
		/// </summary>
		public byte[] Gmname { get; set; }

		/// <summary>
		/// Gets or sets the success value.
		/// </summary>
		public uint Success { get; set; }

		/// <summary>
		/// Gets or sets the zoneid value.
		/// </summary>
		public uint Zoneid { get; set; }

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
		/// Gets or sets the unknown2 value.
		/// </summary>
		public uint Unknown2 { get; set; }

		/// <summary>
		/// Initializes a new instance of the GMFind struct with specified field values.
		/// </summary>
		/// <param name="charname">The charname value.</param>
		/// <param name="gmname">The gmname value.</param>
		/// <param name="success">The success value.</param>
		/// <param name="zoneID">The zoneid value.</param>
		/// <param name="x">The x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="z">The z value.</param>
		/// <param name="unknown2">The unknown2 value.</param>
		public GMFind(byte[] charname, byte[] gmname, uint success, uint zoneID, float x, float y, float z, uint unknown2) : this() {
			Charname = charname;
			Gmname = gmname;
			Success = success;
			Zoneid = zoneID;
			X = x;
			Y = y;
			Z = z;
			Unknown2 = unknown2;
		}

		/// <summary>
		/// Initializes a new instance of the GMFind struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GMFind(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GMFind struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GMFind(BinaryReader br) : this() {
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
			// TODO: Array reading for Charname - implement based on actual array size
			// Charname = new byte[size];
			// TODO: Array reading for Gmname - implement based on actual array size
			// Gmname = new byte[size];
			Success = br.ReadUInt32();
			Zoneid = br.ReadUInt32();
			X = br.ReadSingle();
			Y = br.ReadSingle();
			Z = br.ReadSingle();
			Unknown2 = br.ReadUInt32();
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
			// TODO: Array writing for Charname - implement based on actual array size
			// foreach(var item in Charname) bw.Write(item);
			// TODO: Array writing for Gmname - implement based on actual array size
			// foreach(var item in Gmname) bw.Write(item);
			bw.Write(Success);
			bw.Write(Zoneid);
			bw.Write(X);
			bw.Write(Y);
			bw.Write(Z);
			bw.Write(Unknown2);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GMFind {\n";
			ret += "	Charname = ";
			try {
				ret += $"{ Indentify(Charname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Gmname = ";
			try {
				ret += $"{ Indentify(Gmname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Success = ";
			try {
				ret += $"{ Indentify(Success) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Zoneid = ";
			try {
				ret += $"{ Indentify(Zoneid) },\n";
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
			ret += "	Unknown2 = ";
			try {
				ret += $"{ Indentify(Unknown2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}