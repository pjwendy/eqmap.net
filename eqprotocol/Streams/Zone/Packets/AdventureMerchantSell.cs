using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Adventure_Sell_Struct {
// /*000*/	uint32	unknown000;	//0x01 - Stack Size/Charges?
// /*004*/	uint32	npcid;
// /*008*/	uint32	slot;
// /*012*/	uint32	charges;
// /*016*/	uint32	sell_price;
// };

// ENCODE/DECODE Section:
// DECODE(OP_AdventureMerchantSell)
// {
// DECODE_LENGTH_EXACT(structs::Adventure_Sell_Struct);
// SETUP_DIRECT_DECODE(Adventure_Sell_Struct, structs::Adventure_Sell_Struct);
// 
// IN(npcid);
// emu->slot = UFToServerSlot(eq->slot);
// IN(charges);
// IN(sell_price);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AdventureMerchantSell packet structure for EverQuest network communication.
	/// </summary>
	public struct AdventureMerchantSell : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the npcid value.
		/// </summary>
		public uint Npcid { get; set; }

		/// <summary>
		/// Gets or sets the slot value.
		/// </summary>
		public uint Slot { get; set; }

		/// <summary>
		/// Gets or sets the charges value.
		/// </summary>
		public uint Charges { get; set; }

		/// <summary>
		/// Gets or sets the sellprice value.
		/// </summary>
		public uint SellPrice { get; set; }

		/// <summary>
		/// Initializes a new instance of the AdventureMerchantSell struct with specified field values.
		/// </summary>
		/// <param name="unknown000">The unknown000 value.</param>
		/// <param name="npcid">The npcid value.</param>
		/// <param name="slot">The slot value.</param>
		/// <param name="charges">The charges value.</param>
		/// <param name="sell_price">The sellprice value.</param>
		public AdventureMerchantSell(uint unknown000, uint npcid, uint slot, uint charges, uint sell_price) : this() {
			Unknown000 = unknown000;
			Npcid = npcid;
			Slot = slot;
			Charges = charges;
			SellPrice = sell_price;
		}

		/// <summary>
		/// Initializes a new instance of the AdventureMerchantSell struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AdventureMerchantSell(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AdventureMerchantSell struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AdventureMerchantSell(BinaryReader br) : this() {
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
			Npcid = br.ReadUInt32();
			Slot = br.ReadUInt32();
			Charges = br.ReadUInt32();
			SellPrice = br.ReadUInt32();
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
			bw.Write(Npcid);
			bw.Write(Slot);
			bw.Write(Charges);
			bw.Write(SellPrice);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AdventureMerchantSell {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Npcid = ";
			try {
				ret += $"{ Indentify(Npcid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Charges = ";
			try {
				ret += $"{ Indentify(Charges) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SellPrice = ";
			try {
				ret += $"{ Indentify(SellPrice) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}