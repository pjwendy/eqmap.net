using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildMOTD_Struct{
// /*0000*/	uint32	unknown0;
// /*0004*/	char	name[64];
// /*0068*/	char	setby_name[64];
// /*0132*/	uint32	unknown132;
// /*0136*/	char	motd[0]; //was 512
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GuildMOTD packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildMOTD : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown0 value.
		/// </summary>
		public uint Unknown0 { get; set; }

		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte[] Name { get; set; }

		/// <summary>
		/// Gets or sets the setbyname value.
		/// </summary>
		public byte[] SetbyName { get; set; }

		/// <summary>
		/// Gets or sets the unknown132 value.
		/// </summary>
		public uint Unknown132 { get; set; }

		/// <summary>
		/// Gets or sets the motd value.
		/// </summary>
		public byte Motd { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildMOTD struct with specified field values.
		/// </summary>
		/// <param name="unknown0">The unknown0 value.</param>
		/// <param name="name">The name value.</param>
		/// <param name="setby_name">The setbyname value.</param>
		/// <param name="unknown132">The unknown132 value.</param>
		/// <param name="motd">The motd value.</param>
		public GuildMOTD(uint unknown0, byte[] name, byte[] setby_name, uint unknown132, byte motd) : this() {
			Unknown0 = unknown0;
			Name = name;
			SetbyName = setby_name;
			Unknown132 = unknown132;
			Motd = motd;
		}

		/// <summary>
		/// Initializes a new instance of the GuildMOTD struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildMOTD(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildMOTD struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildMOTD(BinaryReader br) : this() {
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
			Unknown0 = br.ReadUInt32();
			// TODO: Array reading for Name - implement based on actual array size
			// Name = new byte[size];
			// TODO: Array reading for SetbyName - implement based on actual array size
			// SetbyName = new byte[size];
			Unknown132 = br.ReadUInt32();
			Motd = br.ReadByte();
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
			bw.Write(Unknown0);
			// TODO: Array writing for Name - implement based on actual array size
			// foreach(var item in Name) bw.Write(item);
			// TODO: Array writing for SetbyName - implement based on actual array size
			// foreach(var item in SetbyName) bw.Write(item);
			bw.Write(Unknown132);
			bw.Write(Motd);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildMOTD {\n";
			ret += "	Unknown0 = ";
			try {
				ret += $"{ Indentify(Unknown0) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SetbyName = ";
			try {
				ret += $"{ Indentify(SetbyName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown132 = ";
			try {
				ret += $"{ Indentify(Unknown132) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Motd = ";
			try {
				ret += $"{ Indentify(Motd) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}