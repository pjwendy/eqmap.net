using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BecomeNPC_Struct {
// uint32 id;
// int32 maxlevel;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GMBecomeNPC packet structure for EverQuest network communication.
	/// </summary>
	public struct GMBecomeNPC : IEQStruct {
		/// <summary>
		/// Gets or sets the id value.
		/// </summary>
		public uint Id { get; set; }

		/// <summary>
		/// Gets or sets the maxlevel value.
		/// </summary>
		public int Maxlevel { get; set; }

		/// <summary>
		/// Initializes a new instance of the GMBecomeNPC struct with specified field values.
		/// </summary>
		/// <param name="id">The id value.</param>
		/// <param name="maxlevel">The maxlevel value.</param>
		public GMBecomeNPC(uint id, int maxlevel) : this() {
			Id = id;
			Maxlevel = maxlevel;
		}

		/// <summary>
		/// Initializes a new instance of the GMBecomeNPC struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GMBecomeNPC(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GMBecomeNPC struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GMBecomeNPC(BinaryReader br) : this() {
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
			Maxlevel = br.ReadInt32();
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
			bw.Write(Maxlevel);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GMBecomeNPC {\n";
			ret += "	Id = ";
			try {
				ret += $"{ Indentify(Id) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Maxlevel = ";
			try {
				ret += $"{ Indentify(Maxlevel) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}