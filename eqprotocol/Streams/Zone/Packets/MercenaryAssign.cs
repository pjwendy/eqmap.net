using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MercenaryAssign_Struct {
// /*0000*/	uint32	MercEntityID;	// Seen 0 (no merc spawned) or 615843841 and 22779137
// /*0004*/	uint32	MercUnk01;		//
// /*0008*/	uint32	MercUnk02;		//
// /*0012*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MercenaryAssign packet structure for EverQuest network communication.
	/// </summary>
	public struct MercenaryAssign : IEQStruct {
		/// <summary>
		/// Gets or sets the mercentityid value.
		/// </summary>
		public uint Mercentityid { get; set; }

		/// <summary>
		/// Gets or sets the mercunk01 value.
		/// </summary>
		public uint Mercunk01 { get; set; }

		/// <summary>
		/// Gets or sets the mercunk02 value.
		/// </summary>
		public uint Mercunk02 { get; set; }

		/// <summary>
		/// Initializes a new instance of the MercenaryAssign struct with specified field values.
		/// </summary>
		/// <param name="MercEntityID">The mercentityid value.</param>
		/// <param name="MercUnk01">The mercunk01 value.</param>
		/// <param name="MercUnk02">The mercunk02 value.</param>
		public MercenaryAssign(uint MercEntityID, uint MercUnk01, uint MercUnk02) : this() {
			Mercentityid = MercEntityID;
			Mercunk01 = MercUnk01;
			Mercunk02 = MercUnk02;
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryAssign struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MercenaryAssign(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryAssign struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MercenaryAssign(BinaryReader br) : this() {
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
			Mercentityid = br.ReadUInt32();
			Mercunk01 = br.ReadUInt32();
			Mercunk02 = br.ReadUInt32();
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
			bw.Write(Mercentityid);
			bw.Write(Mercunk01);
			bw.Write(Mercunk02);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct MercenaryAssign {\n";
			ret += "	Mercentityid = ";
			try {
				ret += $"{ Indentify(Mercentityid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mercunk01 = ";
			try {
				ret += $"{ Indentify(Mercunk01) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mercunk02 = ";
			try {
				ret += $"{ Indentify(Mercunk02) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}