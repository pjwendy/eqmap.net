using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildManageBanker_Struct {
// uint32 unknown0;
// char myname[64];
// char member[64];
// uint32	enabled;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GuildManageBanker packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildManageBanker : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown0 value.
		/// </summary>
		public uint Unknown0 { get; set; }

		/// <summary>
		/// Gets or sets the myname value.
		/// </summary>
		public byte[] Myname { get; set; }

		/// <summary>
		/// Gets or sets the member value.
		/// </summary>
		public byte[] Member { get; set; }

		/// <summary>
		/// Gets or sets the enabled value.
		/// </summary>
		public uint Enabled { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildManageBanker struct with specified field values.
		/// </summary>
		/// <param name="unknown0">The unknown0 value.</param>
		/// <param name="myname">The myname value.</param>
		/// <param name="member">The member value.</param>
		/// <param name="enabled">The enabled value.</param>
		public GuildManageBanker(uint unknown0, byte[] myname, byte[] member, uint enabled) : this() {
			Unknown0 = unknown0;
			Myname = myname;
			Member = member;
			Enabled = enabled;
		}

		/// <summary>
		/// Initializes a new instance of the GuildManageBanker struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildManageBanker(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildManageBanker struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildManageBanker(BinaryReader br) : this() {
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
			// TODO: Array reading for Myname - implement based on actual array size
			// Myname = new byte[size];
			// TODO: Array reading for Member - implement based on actual array size
			// Member = new byte[size];
			Enabled = br.ReadUInt32();
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
			// TODO: Array writing for Myname - implement based on actual array size
			// foreach(var item in Myname) bw.Write(item);
			// TODO: Array writing for Member - implement based on actual array size
			// foreach(var item in Member) bw.Write(item);
			bw.Write(Enabled);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildManageBanker {\n";
			ret += "	Unknown0 = ";
			try {
				ret += $"{ Indentify(Unknown0) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Myname = ";
			try {
				ret += $"{ Indentify(Myname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Member = ";
			try {
				ret += $"{ Indentify(Member) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Enabled = ";
			try {
				ret += $"{ Indentify(Enabled) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}