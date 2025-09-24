using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ZonePlayerToBind_Struct {
// /*000*/	uint16	bind_zone_id;
// /*002*/	uint16	bind_instance_id;
// /*004*/	float	x;
// /*008*/	float	y;
// /*012*/	float	z;
// /*016*/	float	heading;
// /*020*/	char	zone_name[1];  // Or "Bind Location"
// /*000*/	uint8	unknown021;	// Seen 1 - Maybe 0 would be to force a rezone and 1 is just respawn
// /*000*/	uint32	unknown022;	// Seen 32 or 59
// /*000*/	uint32	unknown023;	// Seen 0
// /*000*/	uint32	unknown024;	// Seen 21 or 43
// };

// ENCODE/DECODE Section:
// ENCODE(OP_ZonePlayerToBind)
// {
// SETUP_VAR_ENCODE(ZonePlayerToBind_Struct);
// ALLOC_LEN_ENCODE(sizeof(structs::ZonePlayerToBind_Struct) + strlen(emu->zone_name));
// 
// __packet->SetWritePosition(0);
// __packet->WriteUInt16(emu->bind_zone_id);
// __packet->WriteUInt16(emu->bind_instance_id);
// __packet->WriteFloat(emu->x);
// __packet->WriteFloat(emu->y);
// __packet->WriteFloat(emu->z);
// __packet->WriteFloat(emu->heading);
// __packet->WriteString(emu->zone_name);
// __packet->WriteUInt8(1); // save items
// __packet->WriteUInt32(0); // hp
// __packet->WriteUInt32(0); // mana
// __packet->WriteUInt32(0); // endurance
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ZonePlayerToBind packet structure for EverQuest network communication.
	/// </summary>
	public struct ZonePlayerToBind : IEQStruct {
		/// <summary>
		/// Gets or sets the bindzoneid value.
		/// </summary>
		public ushort BindZoneId { get; set; }

		/// <summary>
		/// Gets or sets the bindinstanceid value.
		/// </summary>
		public ushort BindInstanceId { get; set; }

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
		/// Gets or sets the zonename value.
		/// </summary>
		public byte ZoneName { get; set; }

		/// <summary>
		/// Gets or sets the unknown021 value.
		/// </summary>
		public byte Unknown021 { get; set; }

		/// <summary>
		/// Gets or sets the unknown022 value.
		/// </summary>
		public uint Unknown022 { get; set; }

		/// <summary>
		/// Gets or sets the unknown023 value.
		/// </summary>
		public uint Unknown023 { get; set; }

		/// <summary>
		/// Gets or sets the unknown024 value.
		/// </summary>
		public uint Unknown024 { get; set; }

		/// <summary>
		/// Initializes a new instance of the ZonePlayerToBind struct with specified field values.
		/// </summary>
		/// <param name="bind_zone_id">The bindzoneid value.</param>
		/// <param name="bind_instance_id">The bindinstanceid value.</param>
		/// <param name="x">The x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="z">The z value.</param>
		/// <param name="heading">The heading value.</param>
		/// <param name="zone_name">The zonename value.</param>
		/// <param name="unknown021">The unknown021 value.</param>
		/// <param name="unknown022">The unknown022 value.</param>
		/// <param name="unknown023">The unknown023 value.</param>
		/// <param name="unknown024">The unknown024 value.</param>
		public ZonePlayerToBind(ushort bind_zone_id, ushort bind_instance_id, float x, float y, float z, float heading, byte zone_name, byte unknown021, uint unknown022, uint unknown023, uint unknown024) : this() {
			BindZoneId = bind_zone_id;
			BindInstanceId = bind_instance_id;
			X = x;
			Y = y;
			Z = z;
			Heading = heading;
			ZoneName = zone_name;
			Unknown021 = unknown021;
			Unknown022 = unknown022;
			Unknown023 = unknown023;
			Unknown024 = unknown024;
		}

		/// <summary>
		/// Initializes a new instance of the ZonePlayerToBind struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ZonePlayerToBind(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ZonePlayerToBind struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ZonePlayerToBind(BinaryReader br) : this() {
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
			BindZoneId = br.ReadUInt16();
			BindInstanceId = br.ReadUInt16();
			X = br.ReadSingle();
			Y = br.ReadSingle();
			Z = br.ReadSingle();
			Heading = br.ReadSingle();
			ZoneName = br.ReadByte();
			Unknown021 = br.ReadByte();
			Unknown022 = br.ReadUInt32();
			Unknown023 = br.ReadUInt32();
			Unknown024 = br.ReadUInt32();
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
			bw.Write(BindZoneId);
			bw.Write(BindInstanceId);
			bw.Write(X);
			bw.Write(Y);
			bw.Write(Z);
			bw.Write(Heading);
			bw.Write(ZoneName);
			bw.Write(Unknown021);
			bw.Write(Unknown022);
			bw.Write(Unknown023);
			bw.Write(Unknown024);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ZonePlayerToBind {\n";
			ret += "	BindZoneId = ";
			try {
				ret += $"{ Indentify(BindZoneId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	BindInstanceId = ";
			try {
				ret += $"{ Indentify(BindInstanceId) },\n";
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
			ret += "	ZoneName = ";
			try {
				ret += $"{ Indentify(ZoneName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown021 = ";
			try {
				ret += $"{ Indentify(Unknown021) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown022 = ";
			try {
				ret += $"{ Indentify(Unknown022) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown023 = ";
			try {
				ret += $"{ Indentify(Unknown023) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown024 = ";
			try {
				ret += $"{ Indentify(Unknown024) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}