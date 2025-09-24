using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GMZoneRequest_Struct {
// /*0000*/	char	charname[64];
// /*0064*/	uint32	zone_id;
// /*0068*/	float	x;
// /*0072*/	float	y;
// /*0076*/	float	z;
// /*0080*/	float	heading;
// /*0084*/	uint32	success;		// 0 if command failed, 1 if succeeded?
// /*0088*/
// //	/*072*/	int8	success;		// =0 client->server, =1 server->client, -X=specific error
// //	/*073*/	uint8	unknown0073[3]; // =0 ok, =ffffff error
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GMZoneRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct GMZoneRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the charname value.
		/// </summary>
		public byte[] Charname { get; set; }

		/// <summary>
		/// Gets or sets the zoneid value.
		/// </summary>
		public uint ZoneId { get; set; }

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
		/// Gets or sets the heading value.
		/// </summary>
		public float Heading { get; set; }

		/// <summary>
		/// Gets or sets the success value.
		/// </summary>
		public uint Success { get; set; }

		/// <summary>
		/// Initializes a new instance of the GMZoneRequest struct with specified field values.
		/// </summary>
		/// <param name="charname">The charname value.</param>
		/// <param name="zone_id">The zoneid value.</param>
		/// <param name="x">The x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="z">The z value.</param>
		/// <param name="heading">The heading value.</param>
		/// <param name="success">The success value.</param>
		public GMZoneRequest(byte[] charname, uint zone_id, float x, float y, float z, float heading, uint success) : this() {
			Charname = charname;
			ZoneId = zone_id;
			X = x;
			Y = y;
			Z = z;
			Heading = heading;
			Success = success;
		}

		/// <summary>
		/// Initializes a new instance of the GMZoneRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GMZoneRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GMZoneRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GMZoneRequest(BinaryReader br) : this() {
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
			ZoneId = br.ReadUInt32();
			X = br.ReadSingle();
			Y = br.ReadSingle();
			Z = br.ReadSingle();
			Heading = br.ReadSingle();
			Success = br.ReadUInt32();
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
			bw.Write(ZoneId);
			bw.Write(X);
			bw.Write(Y);
			bw.Write(Z);
			bw.Write(Heading);
			bw.Write(Success);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GMZoneRequest {\n";
			ret += "	Charname = ";
			try {
				ret += $"{ Indentify(Charname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneId = ";
			try {
				ret += $"{ Indentify(ZoneId) },\n";
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
			ret += "	Heading = ";
			try {
				ret += $"{ Indentify(Heading) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Success = ";
			try {
				ret += $"{ Indentify(Success) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}