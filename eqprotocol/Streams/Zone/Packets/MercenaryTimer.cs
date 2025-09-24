using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MercenaryStatus_Struct {
// /*0000*/	uint32	MercEntityID;	// Seen 0 (no merc spawned) or 615843841 and 22779137
// /*0004*/	uint32	UpdateInterval;	// Seen 900000 - Matches from 0x6537 packet (15 minutes in ms?)
// /*0008*/	uint32	MercUnk01;		// Seen 180000 - 3 minutes in milleseconds? Maybe next update interval?
// /*0012*/	uint32	MercState;		// Seen 5 (normal) or 1 (suspended)
// /*0016*/	uint32	SuspendedTime;	// Seen 0 (not suspended) or c9 c2 64 4f (suspended on Sat Mar 17 11:58:49 2012) - Unix Timestamp
// /*0020*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MercenaryTimer packet structure for EverQuest network communication.
	/// </summary>
	public struct MercenaryTimer : IEQStruct {
		/// <summary>
		/// Gets or sets the mercentityid value.
		/// </summary>
		public uint Mercentityid { get; set; }

		/// <summary>
		/// Gets or sets the updateinterval value.
		/// </summary>
		public uint Updateinterval { get; set; }

		/// <summary>
		/// Gets or sets the mercunk01 value.
		/// </summary>
		public uint Mercunk01 { get; set; }

		/// <summary>
		/// Gets or sets the mercstate value.
		/// </summary>
		public uint Mercstate { get; set; }

		/// <summary>
		/// Gets or sets the suspendedtime value.
		/// </summary>
		public uint Suspendedtime { get; set; }

		/// <summary>
		/// Initializes a new instance of the MercenaryTimer struct with specified field values.
		/// </summary>
		/// <param name="MercEntityID">The mercentityid value.</param>
		/// <param name="UpdateInterval">The updateinterval value.</param>
		/// <param name="MercUnk01">The mercunk01 value.</param>
		/// <param name="MercState">The mercstate value.</param>
		/// <param name="SuspendedTime">The suspendedtime value.</param>
		public MercenaryTimer(uint MercEntityID, uint UpdateInterval, uint MercUnk01, uint MercState, uint SuspendedTime) : this() {
			Mercentityid = MercEntityID;
			Updateinterval = UpdateInterval;
			Mercunk01 = MercUnk01;
			Mercstate = MercState;
			Suspendedtime = SuspendedTime;
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryTimer struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MercenaryTimer(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryTimer struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MercenaryTimer(BinaryReader br) : this() {
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
			Updateinterval = br.ReadUInt32();
			Mercunk01 = br.ReadUInt32();
			Mercstate = br.ReadUInt32();
			Suspendedtime = br.ReadUInt32();
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
			bw.Write(Updateinterval);
			bw.Write(Mercunk01);
			bw.Write(Mercstate);
			bw.Write(Suspendedtime);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct MercenaryTimer {\n";
			ret += "	Mercentityid = ";
			try {
				ret += $"{ Indentify(Mercentityid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Updateinterval = ";
			try {
				ret += $"{ Indentify(Updateinterval) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mercunk01 = ";
			try {
				ret += $"{ Indentify(Mercunk01) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mercstate = ";
			try {
				ret += $"{ Indentify(Mercstate) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Suspendedtime = ";
			try {
				ret += $"{ Indentify(Suspendedtime) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}