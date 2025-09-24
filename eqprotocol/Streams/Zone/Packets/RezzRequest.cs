using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Resurrect_Struct	{
// uint32	unknown00;
// uint16	zone_id;
// uint16	instance_id;
// float	y;
// float	x;
// float	z;
// char	your_name[64];
// uint32	unknown88;
// char	rezzer_name[64];
// uint32	spellid;
// char	corpse_name[64];
// uint32	action;
// /* 228 */
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the RezzRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct RezzRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown00 value.
		/// </summary>
		public uint Unknown00 { get; set; }

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
		/// Gets or sets the yourname value.
		/// </summary>
		public byte[] YourName { get; set; }

		/// <summary>
		/// Gets or sets the unknown88 value.
		/// </summary>
		public uint Unknown88 { get; set; }

		/// <summary>
		/// Gets or sets the rezzername value.
		/// </summary>
		public byte[] RezzerName { get; set; }

		/// <summary>
		/// Gets or sets the spellid value.
		/// </summary>
		public uint Spellid { get; set; }

		/// <summary>
		/// Gets or sets the corpsename value.
		/// </summary>
		public byte[] CorpseName { get; set; }

		/// <summary>
		/// Gets or sets the action value.
		/// </summary>
		public uint Action { get; set; }

		/// <summary>
		/// Initializes a new instance of the RezzRequest struct with specified field values.
		/// </summary>
		/// <param name="unknown00">The unknown00 value.</param>
		/// <param name="zone_id">The zoneid value.</param>
		/// <param name="instance_id">The instanceid value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="x">The x value.</param>
		/// <param name="z">The z value.</param>
		/// <param name="your_name">The yourname value.</param>
		/// <param name="unknown88">The unknown88 value.</param>
		/// <param name="rezzer_name">The rezzername value.</param>
		/// <param name="spellid">The spellid value.</param>
		/// <param name="corpse_name">The corpsename value.</param>
		/// <param name="action">The action value.</param>
		public RezzRequest(uint unknown00, ushort zone_id, ushort instance_id, float y, float x, float z, byte[] your_name, uint unknown88, byte[] rezzer_name, uint spellid, byte[] corpse_name, uint action) : this() {
			Unknown00 = unknown00;
			ZoneId = zone_id;
			InstanceId = instance_id;
			Y = y;
			X = x;
			Z = z;
			YourName = your_name;
			Unknown88 = unknown88;
			RezzerName = rezzer_name;
			Spellid = spellid;
			CorpseName = corpse_name;
			Action = action;
		}

		/// <summary>
		/// Initializes a new instance of the RezzRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public RezzRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the RezzRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public RezzRequest(BinaryReader br) : this() {
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
			Unknown00 = br.ReadUInt32();
			ZoneId = br.ReadUInt16();
			InstanceId = br.ReadUInt16();
			Y = br.ReadSingle();
			X = br.ReadSingle();
			Z = br.ReadSingle();
			// TODO: Array reading for YourName - implement based on actual array size
			// YourName = new byte[size];
			Unknown88 = br.ReadUInt32();
			// TODO: Array reading for RezzerName - implement based on actual array size
			// RezzerName = new byte[size];
			Spellid = br.ReadUInt32();
			// TODO: Array reading for CorpseName - implement based on actual array size
			// CorpseName = new byte[size];
			Action = br.ReadUInt32();
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
			bw.Write(Unknown00);
			bw.Write(ZoneId);
			bw.Write(InstanceId);
			bw.Write(Y);
			bw.Write(X);
			bw.Write(Z);
			// TODO: Array writing for YourName - implement based on actual array size
			// foreach(var item in YourName) bw.Write(item);
			bw.Write(Unknown88);
			// TODO: Array writing for RezzerName - implement based on actual array size
			// foreach(var item in RezzerName) bw.Write(item);
			bw.Write(Spellid);
			// TODO: Array writing for CorpseName - implement based on actual array size
			// foreach(var item in CorpseName) bw.Write(item);
			bw.Write(Action);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct RezzRequest {\n";
			ret += "	Unknown00 = ";
			try {
				ret += $"{ Indentify(Unknown00) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
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
			ret += "	YourName = ";
			try {
				ret += $"{ Indentify(YourName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown88 = ";
			try {
				ret += $"{ Indentify(Unknown88) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	RezzerName = ";
			try {
				ret += $"{ Indentify(RezzerName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Spellid = ";
			try {
				ret += $"{ Indentify(Spellid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CorpseName = ";
			try {
				ret += $"{ Indentify(CorpseName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}