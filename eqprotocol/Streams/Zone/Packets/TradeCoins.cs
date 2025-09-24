using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TradeCoin_Struct{
// uint32	trader;
// uint8	slot;
// uint16	unknown5;
// uint8	unknown7;
// uint32	amount;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TradeCoins packet structure for EverQuest network communication.
	/// </summary>
	public struct TradeCoins : IEQStruct {
		/// <summary>
		/// Gets or sets the trader value.
		/// </summary>
		public uint Trader { get; set; }

		/// <summary>
		/// Gets or sets the slot value.
		/// </summary>
		public byte Slot { get; set; }

		/// <summary>
		/// Gets or sets the unknown5 value.
		/// </summary>
		public ushort Unknown5 { get; set; }

		/// <summary>
		/// Gets or sets the unknown7 value.
		/// </summary>
		public byte Unknown7 { get; set; }

		/// <summary>
		/// Gets or sets the amount value.
		/// </summary>
		public uint Amount { get; set; }

		/// <summary>
		/// Initializes a new instance of the TradeCoins struct with specified field values.
		/// </summary>
		/// <param name="trader">The trader value.</param>
		/// <param name="slot">The slot value.</param>
		/// <param name="unknown5">The unknown5 value.</param>
		/// <param name="unknown7">The unknown7 value.</param>
		/// <param name="amount">The amount value.</param>
		public TradeCoins(uint trader, byte slot, ushort unknown5, byte unknown7, uint amount) : this() {
			Trader = trader;
			Slot = slot;
			Unknown5 = unknown5;
			Unknown7 = unknown7;
			Amount = amount;
		}

		/// <summary>
		/// Initializes a new instance of the TradeCoins struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TradeCoins(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TradeCoins struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TradeCoins(BinaryReader br) : this() {
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
			Trader = br.ReadUInt32();
			Slot = br.ReadByte();
			Unknown5 = br.ReadUInt16();
			Unknown7 = br.ReadByte();
			Amount = br.ReadUInt32();
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
			bw.Write(Trader);
			bw.Write(Slot);
			bw.Write(Unknown5);
			bw.Write(Unknown7);
			bw.Write(Amount);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TradeCoins {\n";
			ret += "	Trader = ";
			try {
				ret += $"{ Indentify(Trader) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown5 = ";
			try {
				ret += $"{ Indentify(Unknown5) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown7 = ";
			try {
				ret += $"{ Indentify(Unknown7) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Amount = ";
			try {
				ret += $"{ Indentify(Amount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}