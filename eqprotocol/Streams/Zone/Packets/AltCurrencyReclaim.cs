using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AltCurrencyReclaim_Struct {
// /*000*/ uint32 currency_id;
// /*004*/ uint32 unknown004;
// /*008*/ uint32 count;
// /*012*/ uint32 reclaim_flag; //1 = this is reclaim
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AltCurrencyReclaim packet structure for EverQuest network communication.
	/// </summary>
	public struct AltCurrencyReclaim : IEQStruct {
		/// <summary>
		/// Gets or sets the currencyid value.
		/// </summary>
		public uint CurrencyId { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public uint Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the count value.
		/// </summary>
		public uint Count { get; set; }

		/// <summary>
		/// Gets or sets the reclaimflag value.
		/// </summary>
		public uint ReclaimFlag { get; set; }

		/// <summary>
		/// Initializes a new instance of the AltCurrencyReclaim struct with specified field values.
		/// </summary>
		/// <param name="currency_id">The currencyid value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="count">The count value.</param>
		/// <param name="reclaim_flag">The reclaimflag value.</param>
		public AltCurrencyReclaim(uint currency_id, uint unknown004, uint count, uint reclaim_flag) : this() {
			CurrencyId = currency_id;
			Unknown004 = unknown004;
			Count = count;
			ReclaimFlag = reclaim_flag;
		}

		/// <summary>
		/// Initializes a new instance of the AltCurrencyReclaim struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AltCurrencyReclaim(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AltCurrencyReclaim struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AltCurrencyReclaim(BinaryReader br) : this() {
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
			CurrencyId = br.ReadUInt32();
			Unknown004 = br.ReadUInt32();
			Count = br.ReadUInt32();
			ReclaimFlag = br.ReadUInt32();
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
			bw.Write(CurrencyId);
			bw.Write(Unknown004);
			bw.Write(Count);
			bw.Write(ReclaimFlag);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AltCurrencyReclaim {\n";
			ret += "	CurrencyId = ";
			try {
				ret += $"{ Indentify(CurrencyId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Count = ";
			try {
				ret += $"{ Indentify(Count) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ReclaimFlag = ";
			try {
				ret += $"{ Indentify(ReclaimFlag) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}