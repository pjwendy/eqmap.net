using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct RequestClientZoneChange_Struct {
// /*00*/	uint16	zone_id;
// /*02*/	uint16	instance_id;
// /*04*/	float	y;
// /*08*/	float	x;
// /*12*/	float	z;
// /*16*/	float	heading;
// /*20*/	uint32	type;	//unknown... values
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the RequestClientZoneChange packet structure for EverQuest network communication.
	/// </summary>
	public struct RequestClientZoneChange : IEQStruct {
		/// <summary>
		/// Gets or sets the zoneid value.
		/// </summary>
		public ushort ZoneId { get; set; }

		/// <summary>
		/// Gets or sets the instanceid value.
		/// </summary>
		public ushort InstanceId { get; set; }

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
		/// Gets or sets the heading value.
		/// </summary>
		public float Heading { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Initializes a new instance of the RequestClientZoneChange struct with specified field values.
		/// </summary>
		/// <param name="zone_id">The zoneid value.</param>
		/// <param name="instance_id">The instanceid value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="x">The x value.</param>
		/// <param name="z">The z value.</param>
		/// <param name="heading">The heading value.</param>
		/// <param name="type">The type value.</param>
		public RequestClientZoneChange(ushort zone_id, ushort instance_id, float y, float x, float z, float heading, uint type) : this() {
			ZoneId = zone_id;
			InstanceId = instance_id;
			Y = y;
			X = x;
			Z = z;
			Heading = heading;
			Type = type;
		}

		/// <summary>
		/// Initializes a new instance of the RequestClientZoneChange struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public RequestClientZoneChange(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the RequestClientZoneChange struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public RequestClientZoneChange(BinaryReader br) : this() {
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
			ZoneId = br.ReadUInt16();
			InstanceId = br.ReadUInt16();
			Y = br.ReadSingle();
			X = br.ReadSingle();
			Z = br.ReadSingle();
			Heading = br.ReadSingle();
			Type = br.ReadUInt32();
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
			bw.Write(ZoneId);
			bw.Write(InstanceId);
			bw.Write(Y);
			bw.Write(X);
			bw.Write(Z);
			bw.Write(Heading);
			bw.Write(Type);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct RequestClientZoneChange {\n";
			ret += "	ZoneId = ";
			try {
				ret += $"{ Indentify(ZoneId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	InstanceId = ";
			try {
				ret += $"{ Indentify(InstanceId) },\n";
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
			ret += "	Heading = ";
			try {
				ret += $"{ Indentify(Heading) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}