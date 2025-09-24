using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GMTrainEnd_Struct
// {
// /*000*/ uint32 npcid;
// /*004*/ uint32 playerid;
// /*008*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GMEndTraining packet structure for EverQuest network communication.
	/// </summary>
	public struct GMEndTraining : IEQStruct {
		/// <summary>
		/// Gets or sets the npcid value.
		/// </summary>
		public uint Npcid { get; set; }

		/// <summary>
		/// Gets or sets the playerid value.
		/// </summary>
		public uint Playerid { get; set; }

		/// <summary>
		/// Initializes a new instance of the GMEndTraining struct with specified field values.
		/// </summary>
		/// <param name="npcid">The npcid value.</param>
		/// <param name="playerid">The playerid value.</param>
		public GMEndTraining(uint npcid, uint playerid) : this() {
			Npcid = npcid;
			Playerid = playerid;
		}

		/// <summary>
		/// Initializes a new instance of the GMEndTraining struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GMEndTraining(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GMEndTraining struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GMEndTraining(BinaryReader br) : this() {
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
			Npcid = br.ReadUInt32();
			Playerid = br.ReadUInt32();
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
			bw.Write(Npcid);
			bw.Write(Playerid);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GMEndTraining {\n";
			ret += "	Npcid = ";
			try {
				ret += $"{ Indentify(Npcid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Playerid = ";
			try {
				ret += $"{ Indentify(Playerid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}