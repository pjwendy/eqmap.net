using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GroupMakeLeader_Struct
// {
// /*000*/ uint32 Unknown000;
// /*004*/ char CurrentLeader[64];
// /*068*/ char NewLeader[64];
// /*132*/ char Unknown072[324];
// /*456*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GroupMakeLeader packet structure for EverQuest network communication.
	/// </summary>
	public struct GroupMakeLeader : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the currentleader value.
		/// </summary>
		public byte[] Currentleader { get; set; }

		/// <summary>
		/// Gets or sets the newleader value.
		/// </summary>
		public byte[] Newleader { get; set; }

		/// <summary>
		/// Gets or sets the unknown072 value.
		/// </summary>
		public byte[] Unknown072 { get; set; }

		/// <summary>
		/// Initializes a new instance of the GroupMakeLeader struct with specified field values.
		/// </summary>
		/// <param name="Unknown000">The unknown000 value.</param>
		/// <param name="CurrentLeader">The currentleader value.</param>
		/// <param name="NewLeader">The newleader value.</param>
		/// <param name="Unknown072">The unknown072 value.</param>
		public GroupMakeLeader(uint Unknown000, byte[] CurrentLeader, byte[] NewLeader, byte[] Unknown072) : this() {
			Unknown000 = Unknown000;
			Currentleader = CurrentLeader;
			Newleader = NewLeader;
			Unknown072 = Unknown072;
		}

		/// <summary>
		/// Initializes a new instance of the GroupMakeLeader struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GroupMakeLeader(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GroupMakeLeader struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GroupMakeLeader(BinaryReader br) : this() {
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
			// TODO: Array reading for Currentleader - implement based on actual array size
			// Currentleader = new byte[size];
			// TODO: Array reading for Newleader - implement based on actual array size
			// Newleader = new byte[size];
			// TODO: Array reading for Unknown072 - implement based on actual array size
			// Unknown072 = new byte[size];
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
			// TODO: Array writing for Currentleader - implement based on actual array size
			// foreach(var item in Currentleader) bw.Write(item);
			// TODO: Array writing for Newleader - implement based on actual array size
			// foreach(var item in Newleader) bw.Write(item);
			// TODO: Array writing for Unknown072 - implement based on actual array size
			// foreach(var item in Unknown072) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GroupMakeLeader {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Currentleader = ";
			try {
				ret += $"{ Indentify(Currentleader) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Newleader = ";
			try {
				ret += $"{ Indentify(Newleader) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown072 = ";
			try {
				ret += $"{ Indentify(Unknown072) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}