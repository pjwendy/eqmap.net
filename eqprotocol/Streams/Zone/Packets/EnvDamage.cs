using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct EnvDamage2_Struct {
// /*0000*/	uint32 id;
// /*0004*/	uint16 unknown4;
// /*0006*/	uint32 damage;
// /*0010*/	float unknown10;	// New to Underfoot - Seen 1
// /*0014*/	uint8 unknown14[12];
// /*0026*/	uint8 dmgtype;		// FA = Lava; FC = Falling
// /*0027*/	uint8 unknown27[4];
// /*0031*/	uint16 unknown31;	// New to Underfoot - Seen 66
// /*0033*/	uint16 constant;		// Always FFFF
// /*0035*/	uint16 unknown35;
// /*0037*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_EnvDamage)
// {
// DECODE_LENGTH_EXACT(structs::EnvDamage2_Struct);
// SETUP_DIRECT_DECODE(EnvDamage2_Struct, structs::EnvDamage2_Struct);
// 
// IN(id);
// IN(damage);
// IN(dmgtype);
// emu->constant = 0xFFFF;
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the EnvDamage packet structure for EverQuest network communication.
	/// </summary>
	public struct EnvDamage : IEQStruct {
		/// <summary>
		/// Gets or sets the id value.
		/// </summary>
		public uint Id { get; set; }

		/// <summary>
		/// Gets or sets the unknown4 value.
		/// </summary>
		public ushort Unknown4 { get; set; }

		/// <summary>
		/// Gets or sets the damage value.
		/// </summary>
		public uint Damage { get; set; }

		/// <summary>
		/// Gets or sets the unknown10 value.
		/// </summary>
		public float Unknown10 { get; set; }

		/// <summary>
		/// Gets or sets the unknown14 value.
		/// </summary>
		public byte[] Unknown14 { get; set; }

		/// <summary>
		/// Gets or sets the dmgtype value.
		/// </summary>
		public byte Dmgtype { get; set; }

		/// <summary>
		/// Gets or sets the unknown27 value.
		/// </summary>
		public byte[] Unknown27 { get; set; }

		/// <summary>
		/// Gets or sets the unknown31 value.
		/// </summary>
		public ushort Unknown31 { get; set; }

		/// <summary>
		/// Gets or sets the constant value.
		/// </summary>
		public ushort Constant { get; set; }

		/// <summary>
		/// Gets or sets the unknown35 value.
		/// </summary>
		public ushort Unknown35 { get; set; }

		/// <summary>
		/// Initializes a new instance of the EnvDamage struct with specified field values.
		/// </summary>
		/// <param name="id">The id value.</param>
		/// <param name="unknown4">The unknown4 value.</param>
		/// <param name="damage">The damage value.</param>
		/// <param name="unknown10">The unknown10 value.</param>
		/// <param name="unknown14">The unknown14 value.</param>
		/// <param name="dmgtype">The dmgtype value.</param>
		/// <param name="unknown27">The unknown27 value.</param>
		/// <param name="unknown31">The unknown31 value.</param>
		/// <param name="constant">The constant value.</param>
		/// <param name="unknown35">The unknown35 value.</param>
		public EnvDamage(uint id, ushort unknown4, uint damage, float unknown10, byte[] unknown14, byte dmgtype, byte[] unknown27, ushort unknown31, ushort constant, ushort unknown35) : this() {
			Id = id;
			Unknown4 = unknown4;
			Damage = damage;
			Unknown10 = unknown10;
			Unknown14 = unknown14;
			Dmgtype = dmgtype;
			Unknown27 = unknown27;
			Unknown31 = unknown31;
			Constant = constant;
			Unknown35 = unknown35;
		}

		/// <summary>
		/// Initializes a new instance of the EnvDamage struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public EnvDamage(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the EnvDamage struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public EnvDamage(BinaryReader br) : this() {
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
			Id = br.ReadUInt32();
			Unknown4 = br.ReadUInt16();
			Damage = br.ReadUInt32();
			Unknown10 = br.ReadSingle();
			// TODO: Array reading for Unknown14 - implement based on actual array size
			// Unknown14 = new byte[size];
			Dmgtype = br.ReadByte();
			// TODO: Array reading for Unknown27 - implement based on actual array size
			// Unknown27 = new byte[size];
			Unknown31 = br.ReadUInt16();
			Constant = br.ReadUInt16();
			Unknown35 = br.ReadUInt16();
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
			bw.Write(Id);
			bw.Write(Unknown4);
			bw.Write(Damage);
			bw.Write(Unknown10);
			// TODO: Array writing for Unknown14 - implement based on actual array size
			// foreach(var item in Unknown14) bw.Write(item);
			bw.Write(Dmgtype);
			// TODO: Array writing for Unknown27 - implement based on actual array size
			// foreach(var item in Unknown27) bw.Write(item);
			bw.Write(Unknown31);
			bw.Write(Constant);
			bw.Write(Unknown35);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct EnvDamage {\n";
			ret += "	Id = ";
			try {
				ret += $"{ Indentify(Id) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown4 = ";
			try {
				ret += $"{ Indentify(Unknown4) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Damage = ";
			try {
				ret += $"{ Indentify(Damage) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown10 = ";
			try {
				ret += $"{ Indentify(Unknown10) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown14 = ";
			try {
				ret += $"{ Indentify(Unknown14) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Dmgtype = ";
			try {
				ret += $"{ Indentify(Dmgtype) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown27 = ";
			try {
				ret += $"{ Indentify(Unknown27) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown31 = ";
			try {
				ret += $"{ Indentify(Unknown31) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Constant = ";
			try {
				ret += $"{ Indentify(Constant) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown35 = ";
			try {
				ret += $"{ Indentify(Unknown35) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}