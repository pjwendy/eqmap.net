using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GroupRole_Struct
// {
// /*000*/	char	Name1[64];
// /*064*/	char	Name2[64];
// /*128*/	uint32	Unknown128;
// /*132*/	uint32	Unknown132;
// /*136*/	uint32	Unknown136;
// /*140*/	uint32	RoleNumber;
// /*144*/	uint8	Toggle;
// /*145*/	uint8	Unknown145[3];
// /*148*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GroupRoles packet structure for EverQuest network communication.
	/// </summary>
	public struct GroupRoles : IEQStruct {
		/// <summary>
		/// Gets or sets the name1 value.
		/// </summary>
		public byte[] Name1 { get; set; }

		/// <summary>
		/// Gets or sets the name2 value.
		/// </summary>
		public byte[] Name2 { get; set; }

		/// <summary>
		/// Gets or sets the unknown128 value.
		/// </summary>
		public uint Unknown128 { get; set; }

		/// <summary>
		/// Gets or sets the unknown132 value.
		/// </summary>
		public uint Unknown132 { get; set; }

		/// <summary>
		/// Gets or sets the unknown136 value.
		/// </summary>
		public uint Unknown136 { get; set; }

		/// <summary>
		/// Gets or sets the rolenumber value.
		/// </summary>
		public uint Rolenumber { get; set; }

		/// <summary>
		/// Gets or sets the toggle value.
		/// </summary>
		public byte Toggle { get; set; }

		/// <summary>
		/// Gets or sets the unknown145 value.
		/// </summary>
		public byte[] Unknown145 { get; set; }

		/// <summary>
		/// Initializes a new instance of the GroupRoles struct with specified field values.
		/// </summary>
		/// <param name="Name1">The name1 value.</param>
		/// <param name="Name2">The name2 value.</param>
		/// <param name="Unknown128">The unknown128 value.</param>
		/// <param name="Unknown132">The unknown132 value.</param>
		/// <param name="Unknown136">The unknown136 value.</param>
		/// <param name="RoleNumber">The rolenumber value.</param>
		/// <param name="Toggle">The toggle value.</param>
		/// <param name="Unknown145">The unknown145 value.</param>
		public GroupRoles(byte[] Name1, byte[] Name2, uint Unknown128, uint Unknown132, uint Unknown136, uint RoleNumber, byte Toggle, byte[] Unknown145) : this() {
			Name1 = Name1;
			Name2 = Name2;
			Unknown128 = Unknown128;
			Unknown132 = Unknown132;
			Unknown136 = Unknown136;
			Rolenumber = RoleNumber;
			Toggle = Toggle;
			Unknown145 = Unknown145;
		}

		/// <summary>
		/// Initializes a new instance of the GroupRoles struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GroupRoles(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GroupRoles struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GroupRoles(BinaryReader br) : this() {
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
			// TODO: Array reading for Name1 - implement based on actual array size
			// Name1 = new byte[size];
			// TODO: Array reading for Name2 - implement based on actual array size
			// Name2 = new byte[size];
			Unknown128 = br.ReadUInt32();
			Unknown132 = br.ReadUInt32();
			Unknown136 = br.ReadUInt32();
			Rolenumber = br.ReadUInt32();
			Toggle = br.ReadByte();
			// TODO: Array reading for Unknown145 - implement based on actual array size
			// Unknown145 = new byte[size];
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
			// TODO: Array writing for Name1 - implement based on actual array size
			// foreach(var item in Name1) bw.Write(item);
			// TODO: Array writing for Name2 - implement based on actual array size
			// foreach(var item in Name2) bw.Write(item);
			bw.Write(Unknown128);
			bw.Write(Unknown132);
			bw.Write(Unknown136);
			bw.Write(Rolenumber);
			bw.Write(Toggle);
			// TODO: Array writing for Unknown145 - implement based on actual array size
			// foreach(var item in Unknown145) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GroupRoles {\n";
			ret += "	Name1 = ";
			try {
				ret += $"{ Indentify(Name1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Name2 = ";
			try {
				ret += $"{ Indentify(Name2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown128 = ";
			try {
				ret += $"{ Indentify(Unknown128) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown132 = ";
			try {
				ret += $"{ Indentify(Unknown132) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown136 = ";
			try {
				ret += $"{ Indentify(Unknown136) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Rolenumber = ";
			try {
				ret += $"{ Indentify(Rolenumber) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Toggle = ";
			try {
				ret += $"{ Indentify(Toggle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown145 = ";
			try {
				ret += $"{ Indentify(Unknown145) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}