using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildCommand_Struct {
// char othername[64];
// char myname[64];
// uint16 guildeqid;
// uint8 unknown[2]; // for guildinvite all 0's, for remove 0=0x56, 2=0x02
// uint32 officer;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GuildRemove packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildRemove : IEQStruct {
		/// <summary>
		/// Gets or sets the othername value.
		/// </summary>
		public byte[] Othername { get; set; }

		/// <summary>
		/// Gets or sets the myname value.
		/// </summary>
		public byte[] Myname { get; set; }

		/// <summary>
		/// Gets or sets the guildeqid value.
		/// </summary>
		public ushort Guildeqid { get; set; }

		/// <summary>
		/// Gets or sets the unknown value.
		/// </summary>
		public byte[] Unknown { get; set; }

		/// <summary>
		/// Gets or sets the officer value.
		/// </summary>
		public uint Officer { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildRemove struct with specified field values.
		/// </summary>
		/// <param name="othername">The othername value.</param>
		/// <param name="myname">The myname value.</param>
		/// <param name="guildeqid">The guildeqid value.</param>
		/// <param name="unknown">The unknown value.</param>
		/// <param name="officer">The officer value.</param>
		public GuildRemove(byte[] othername, byte[] myname, ushort guildeqid, byte[] unknown, uint officer) : this() {
			Othername = othername;
			Myname = myname;
			Guildeqid = guildeqid;
			Unknown = unknown;
			Officer = officer;
		}

		/// <summary>
		/// Initializes a new instance of the GuildRemove struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildRemove(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildRemove struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildRemove(BinaryReader br) : this() {
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
			// TODO: Array reading for Othername - implement based on actual array size
			// Othername = new byte[size];
			// TODO: Array reading for Myname - implement based on actual array size
			// Myname = new byte[size];
			Guildeqid = br.ReadUInt16();
			// TODO: Array reading for Unknown - implement based on actual array size
			// Unknown = new byte[size];
			Officer = br.ReadUInt32();
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
			// TODO: Array writing for Othername - implement based on actual array size
			// foreach(var item in Othername) bw.Write(item);
			// TODO: Array writing for Myname - implement based on actual array size
			// foreach(var item in Myname) bw.Write(item);
			bw.Write(Guildeqid);
			// TODO: Array writing for Unknown - implement based on actual array size
			// foreach(var item in Unknown) bw.Write(item);
			bw.Write(Officer);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildRemove {\n";
			ret += "	Othername = ";
			try {
				ret += $"{ Indentify(Othername) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Myname = ";
			try {
				ret += $"{ Indentify(Myname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Guildeqid = ";
			try {
				ret += $"{ Indentify(Guildeqid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown = ";
			try {
				ret += $"{ Indentify(Unknown) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Officer = ";
			try {
				ret += $"{ Indentify(Officer) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}