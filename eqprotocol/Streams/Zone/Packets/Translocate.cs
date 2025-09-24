using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Translocate_Struct {
// /*000*/	uint32	ZoneID;
// /*004*/	uint32	SpellID;
// /*008*/	uint32	unknown008; //Heading ?
// /*012*/	char	Caster[64];
// /*076*/	float	y;
// /*080*/	float	x;
// /*084*/	float	z;
// /*088*/	uint32	Complete;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Translocate packet structure for EverQuest network communication.
	/// </summary>
	public struct Translocate : IEQStruct {
		/// <summary>
		/// Gets or sets the zoneid value.
		/// </summary>
		public uint Zoneid { get; set; }

		/// <summary>
		/// Gets or sets the spellid value.
		/// </summary>
		public uint Spellid { get; set; }

		/// <summary>
		/// Gets or sets the unknown008 value.
		/// </summary>
		public uint Unknown008 { get; set; }

		/// <summary>
		/// Gets or sets the caster value.
		/// </summary>
		public byte[] Caster { get; set; }

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
		/// Gets or sets the complete value.
		/// </summary>
		public uint Complete { get; set; }

		/// <summary>
		/// Initializes a new instance of the Translocate struct with specified field values.
		/// </summary>
		/// <param name="ZoneID">The zoneid value.</param>
		/// <param name="SpellID">The spellid value.</param>
		/// <param name="unknown008">The unknown008 value.</param>
		/// <param name="Caster">The caster value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="x">The x value.</param>
		/// <param name="z">The z value.</param>
		/// <param name="Complete">The complete value.</param>
		public Translocate(uint ZoneID, uint SpellID, uint unknown008, byte[] Caster, float y, float x, float z, uint Complete) : this() {
			Zoneid = ZoneID;
			Spellid = SpellID;
			Unknown008 = unknown008;
			Caster = Caster;
			Y = y;
			X = x;
			Z = z;
			Complete = Complete;
		}

		/// <summary>
		/// Initializes a new instance of the Translocate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Translocate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Translocate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Translocate(BinaryReader br) : this() {
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
			Zoneid = br.ReadUInt32();
			Spellid = br.ReadUInt32();
			Unknown008 = br.ReadUInt32();
			// TODO: Array reading for Caster - implement based on actual array size
			// Caster = new byte[size];
			Y = br.ReadSingle();
			X = br.ReadSingle();
			Z = br.ReadSingle();
			Complete = br.ReadUInt32();
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
			bw.Write(Zoneid);
			bw.Write(Spellid);
			bw.Write(Unknown008);
			// TODO: Array writing for Caster - implement based on actual array size
			// foreach(var item in Caster) bw.Write(item);
			bw.Write(Y);
			bw.Write(X);
			bw.Write(Z);
			bw.Write(Complete);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Translocate {\n";
			ret += "	Zoneid = ";
			try {
				ret += $"{ Indentify(Zoneid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Spellid = ";
			try {
				ret += $"{ Indentify(Spellid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown008 = ";
			try {
				ret += $"{ Indentify(Unknown008) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Caster = ";
			try {
				ret += $"{ Indentify(Caster) },\n";
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
			ret += "	Complete = ";
			try {
				ret += $"{ Indentify(Complete) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}