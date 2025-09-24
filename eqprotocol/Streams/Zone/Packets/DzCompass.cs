using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DynamicZoneCompassEntry_Struct
// {
// /*000*/ uint16 dz_zone_id;      // target dz id pair
// /*002*/ uint16 dz_instance_id;
// /*004*/ uint32 dz_type;         // 1: Expedition, 2: Tutorial (purple), 3: Task, 4: Mission, 5: Quest (green)
// /*008*/ uint32 dz_switch_id;
// /*012*/ float y;
// /*016*/ float x;
// /*020*/ float z;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DzCompass packet structure for EverQuest network communication.
	/// </summary>
	public struct DzCompass : IEQStruct {
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
		/// Gets or sets the dzswitchid value.
		/// </summary>
		public uint DzSwitchId { get; set; }

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
		/// Initializes a new instance of the DzCompass struct with specified field values.
		/// </summary>
		/// <param name="dz_zone_id">The dzzoneid value.</param>
		/// <param name="dz_instance_id">The dzinstanceid value.</param>
		/// <param name="dz_type">The dztype value.</param>
		/// <param name="dz_switch_id">The dzswitchid value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="x">The x value.</param>
		/// <param name="z">The z value.</param>
		public DzCompass(ushort dz_zone_id, ushort dz_instance_id, uint dz_type, uint dz_switch_id, float y, float x, float z) : this() {
			DzZoneId = dz_zone_id;
			DzInstanceId = dz_instance_id;
			DzType = dz_type;
			DzSwitchId = dz_switch_id;
			Y = y;
			X = x;
			Z = z;
		}

		/// <summary>
		/// Initializes a new instance of the DzCompass struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DzCompass(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DzCompass struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DzCompass(BinaryReader br) : this() {
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
			DzZoneId = br.ReadUInt16();
			DzInstanceId = br.ReadUInt16();
			DzType = br.ReadUInt32();
			DzSwitchId = br.ReadUInt32();
			Y = br.ReadSingle();
			X = br.ReadSingle();
			Z = br.ReadSingle();
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
			bw.Write(DzZoneId);
			bw.Write(DzInstanceId);
			bw.Write(DzType);
			bw.Write(DzSwitchId);
			bw.Write(Y);
			bw.Write(X);
			bw.Write(Z);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DzCompass {\n";
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
			ret += "	DzSwitchId = ";
			try {
				ret += $"{ Indentify(DzSwitchId) },\n";
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
			
			return ret;
		}
	}
}