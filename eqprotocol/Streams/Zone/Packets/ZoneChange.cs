using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ZoneChange_Struct {
// /*000*/	char	char_name[64];     // Character Name
// /*064*/	uint16	zoneID;
// /*066*/	uint16	instanceID;
// /*068*/	float	y;
// /*072*/	float	x;
// /*076*/	float	z;
// /*080*/	uint32	zone_reason;	//0x0A == death, I think
// /*084*/	int32	success;		// =0 client->server, =1 server->client, -X=specific error
// /*088*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ZoneChange packet structure for EverQuest network communication.
	/// </summary>
	public struct ZoneChange : IEQStruct {
		/// <summary>
		/// Gets or sets the charname value.
		/// </summary>
		public byte[] CharName { get; set; }

		/// <summary>
		/// Gets or sets the zoneid value.
		/// </summary>
		public ushort Zoneid { get; set; }

		/// <summary>
		/// Gets or sets the instanceid value.
		/// </summary>
		public ushort Instanceid { get; set; }

		/// <summary>
		/// Gets or sets the y value.
		/// </summary>
		public float Y { get; set; }

		/// <summary>
		/// Gets or sets the x value.
		/// </summary>
		public float X { get; set; }

		/// <summary>
		/// Gets or sets the z value.
		/// </summary>
		public float Z { get; set; }

		/// <summary>
		/// Gets or sets the zonereason value.
		/// </summary>
		public uint ZoneReason { get; set; }

		/// <summary>
		/// Gets or sets the success value.
		/// </summary>
		public int Success { get; set; }

		/// <summary>
		/// Initializes a new instance of the ZoneChange struct with specified field values.
		/// </summary>
		/// <param name="char_name">The charname value.</param>
		/// <param name="zoneID">The zoneid value.</param>
		/// <param name="instanceID">The instanceid value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="x">The x value.</param>
		/// <param name="z">The z value.</param>
		/// <param name="zone_reason">The zonereason value.</param>
		/// <param name="success">The success value.</param>
		public ZoneChange(byte[] char_name, ushort zoneID, ushort instanceID, float y, float x, float z, uint zone_reason, int success) : this() {
			CharName = char_name;
			Zoneid = zoneID;
			Instanceid = instanceID;
			Y = y;
			X = x;
			Z = z;
			ZoneReason = zone_reason;
			Success = success;
		}

		/// <summary>
		/// Initializes a new instance of the ZoneChange struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ZoneChange(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ZoneChange struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ZoneChange(BinaryReader br) : this() {
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
			// TODO: Array reading for CharName - implement based on actual array size
			// CharName = new byte[size];
			Zoneid = br.ReadUInt16();
			Instanceid = br.ReadUInt16();
			Y = br.ReadSingle();
			X = br.ReadSingle();
			Z = br.ReadSingle();
			ZoneReason = br.ReadUInt32();
			Success = br.ReadInt32();
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
			// TODO: Array writing for CharName - implement based on actual array size
			// foreach(var item in CharName) bw.Write(item);
			bw.Write(Zoneid);
			bw.Write(Instanceid);
			bw.Write(Y);
			bw.Write(X);
			bw.Write(Z);
			bw.Write(ZoneReason);
			bw.Write(Success);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ZoneChange {\n";
			ret += "	CharName = ";
			try {
				ret += $"{ Indentify(CharName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Zoneid = ";
			try {
				ret += $"{ Indentify(Zoneid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Instanceid = ";
			try {
				ret += $"{ Indentify(Instanceid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Y = ";
			try {
				ret += $"{ Indentify(Y) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	X = ";
			try {
				ret += $"{ Indentify(X) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Z = ";
			try {
				ret += $"{ Indentify(Z) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneReason = ";
			try {
				ret += $"{ Indentify(ZoneReason) },\n";
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