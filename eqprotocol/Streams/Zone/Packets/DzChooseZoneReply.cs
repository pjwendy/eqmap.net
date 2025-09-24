using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DynamicZoneChooseZoneReply_Struct
// {
// /*000*/ uint32 unknown000;     // ff ff ff ff
// /*004*/ uint32 unknown004;     // seen 69 00 00 00
// /*008*/ uint32 unknown008;     // ff ff ff ff
// /*012*/ uint32 unknown_id1;    // from choose zone entry message
// /*016*/ uint16 dz_zone_id;     // dz_id pair
// /*018*/ uint16 dz_instance_id;
// /*020*/ uint32 dz_type;        // 1: Expedition, 2: Tutorial, 3: Task, 4: Mission, 5: Quest
// /*024*/ uint32 unknown_id2;    // from choose zone entry message
// /*028*/ uint32 unknown028;     // 00 00 00 00
// /*032*/ uint32 unknown032;     // always same as unknown044
// /*036*/ uint32 unknown036;
// /*040*/ uint32 unknown040;
// /*044*/ uint32 unknown044;     // always same as unknown032
// /*048*/ uint32 unknown048;     // seen 01 00 00 00 and 02 00 00 00
// };

// ENCODE/DECODE Section:
// DECODE(OP_DzChooseZoneReply)
// {
// DECODE_LENGTH_EXACT(structs::DynamicZoneChooseZoneReply_Struct);
// SETUP_DIRECT_DECODE(DynamicZoneChooseZoneReply_Struct, structs::DynamicZoneChooseZoneReply_Struct);
// 
// IN(unknown000);
// IN(unknown004);
// IN(unknown008);
// IN(unknown_id1);
// IN(dz_zone_id);
// IN(dz_instance_id);
// IN(dz_type);
// IN(unknown_id2);
// IN(unknown028);
// IN(unknown032);
// IN(unknown036);
// IN(unknown040);
// IN(unknown044);
// IN(unknown048);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DzChooseZoneReply packet structure for EverQuest network communication.
	/// </summary>
	public struct DzChooseZoneReply : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public uint Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the unknown008 value.
		/// </summary>
		public uint Unknown008 { get; set; }

		/// <summary>
		/// Gets or sets the dzzoneid value.
		/// </summary>
		public ushort DzZoneId { get; set; }

		/// <summary>
		/// Gets or sets the dzinstanceid value.
		/// </summary>
		public ushort DzInstanceId { get; set; }

		/// <summary>
		/// Gets or sets the dztype value.
		/// </summary>
		public uint DzType { get; set; }

		/// <summary>
		/// Gets or sets the unknown028 value.
		/// </summary>
		public uint Unknown028 { get; set; }

		/// <summary>
		/// Gets or sets the unknown032 value.
		/// </summary>
		public uint Unknown032 { get; set; }

		/// <summary>
		/// Gets or sets the unknown036 value.
		/// </summary>
		public uint Unknown036 { get; set; }

		/// <summary>
		/// Gets or sets the unknown040 value.
		/// </summary>
		public uint Unknown040 { get; set; }

		/// <summary>
		/// Gets or sets the unknown044 value.
		/// </summary>
		public uint Unknown044 { get; set; }

		/// <summary>
		/// Gets or sets the unknown048 value.
		/// </summary>
		public uint Unknown048 { get; set; }

		/// <summary>
		/// Initializes a new instance of the DzChooseZoneReply struct with specified field values.
		/// </summary>
		/// <param name="unknown000">The unknown000 value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="unknown008">The unknown008 value.</param>
		/// <param name="dz_zone_id">The dzzoneid value.</param>
		/// <param name="dz_instance_id">The dzinstanceid value.</param>
		/// <param name="dz_type">The dztype value.</param>
		/// <param name="unknown028">The unknown028 value.</param>
		/// <param name="unknown032">The unknown032 value.</param>
		/// <param name="unknown036">The unknown036 value.</param>
		/// <param name="unknown040">The unknown040 value.</param>
		/// <param name="unknown044">The unknown044 value.</param>
		/// <param name="unknown048">The unknown048 value.</param>
		public DzChooseZoneReply(uint unknown000, uint unknown004, uint unknown008, ushort dz_zone_id, ushort dz_instance_id, uint dz_type, uint unknown028, uint unknown032, uint unknown036, uint unknown040, uint unknown044, uint unknown048) : this() {
			Unknown000 = unknown000;
			Unknown004 = unknown004;
			Unknown008 = unknown008;
			DzZoneId = dz_zone_id;
			DzInstanceId = dz_instance_id;
			DzType = dz_type;
			Unknown028 = unknown028;
			Unknown032 = unknown032;
			Unknown036 = unknown036;
			Unknown040 = unknown040;
			Unknown044 = unknown044;
			Unknown048 = unknown048;
		}

		/// <summary>
		/// Initializes a new instance of the DzChooseZoneReply struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DzChooseZoneReply(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DzChooseZoneReply struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DzChooseZoneReply(BinaryReader br) : this() {
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
			Unknown004 = br.ReadUInt32();
			Unknown008 = br.ReadUInt32();
			DzZoneId = br.ReadUInt16();
			DzInstanceId = br.ReadUInt16();
			DzType = br.ReadUInt32();
			Unknown028 = br.ReadUInt32();
			Unknown032 = br.ReadUInt32();
			Unknown036 = br.ReadUInt32();
			Unknown040 = br.ReadUInt32();
			Unknown044 = br.ReadUInt32();
			Unknown048 = br.ReadUInt32();
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
			bw.Write(Unknown004);
			bw.Write(Unknown008);
			bw.Write(DzZoneId);
			bw.Write(DzInstanceId);
			bw.Write(DzType);
			bw.Write(Unknown028);
			bw.Write(Unknown032);
			bw.Write(Unknown036);
			bw.Write(Unknown040);
			bw.Write(Unknown044);
			bw.Write(Unknown048);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DzChooseZoneReply {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown008 = ";
			try {
				ret += $"{ Indentify(Unknown008) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DzZoneId = ";
			try {
				ret += $"{ Indentify(DzZoneId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DzInstanceId = ";
			try {
				ret += $"{ Indentify(DzInstanceId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DzType = ";
			try {
				ret += $"{ Indentify(DzType) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown028 = ";
			try {
				ret += $"{ Indentify(Unknown028) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown032 = ";
			try {
				ret += $"{ Indentify(Unknown032) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown036 = ";
			try {
				ret += $"{ Indentify(Unknown036) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown040 = ";
			try {
				ret += $"{ Indentify(Unknown040) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown044 = ";
			try {
				ret += $"{ Indentify(Unknown044) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown048 = ";
			try {
				ret += $"{ Indentify(Unknown048) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}