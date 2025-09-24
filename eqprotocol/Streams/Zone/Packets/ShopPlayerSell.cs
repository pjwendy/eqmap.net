using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Merchant_Purchase_Struct {
// /*000*/	uint32	npcid;			// Merchant NPC's entity id
// /*004*/	uint32	itemslot;		// Player's entity id
// /*008*/	uint32	quantity;
// /*012*/	uint32	price;
// };

// ENCODE/DECODE Section:
// DECODE(OP_ShopPlayerSell)
// {
// DECODE_LENGTH_EXACT(structs::Merchant_Purchase_Struct);
// SETUP_DIRECT_DECODE(Merchant_Purchase_Struct, structs::Merchant_Purchase_Struct);
// 
// IN(npcid);
// emu->itemslot = UFToServerSlot(eq->itemslot);
// IN(quantity);
// IN(price);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ShopPlayerSell packet structure for EverQuest network communication.
	/// </summary>
	public struct ShopPlayerSell : IEQStruct {
		/// <summary>
		/// Gets or sets the npcid value.
		/// </summary>
		public uint Npcid { get; set; }

		/// <summary>
		/// Gets or sets the itemslot value.
		/// </summary>
		public uint Itemslot { get; set; }

		/// <summary>
		/// Gets or sets the quantity value.
		/// </summary>
		public uint Quantity { get; set; }

		/// <summary>
		/// Gets or sets the price value.
		/// </summary>
		public uint Price { get; set; }

		/// <summary>
		/// Initializes a new instance of the ShopPlayerSell struct with specified field values.
		/// </summary>
		/// <param name="npcid">The npcid value.</param>
		/// <param name="itemslot">The itemslot value.</param>
		/// <param name="quantity">The quantity value.</param>
		/// <param name="price">The price value.</param>
		public ShopPlayerSell(uint npcid, uint itemslot, uint quantity, uint price) : this() {
			Npcid = npcid;
			Itemslot = itemslot;
			Quantity = quantity;
			Price = price;
		}

		/// <summary>
		/// Initializes a new instance of the ShopPlayerSell struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ShopPlayerSell(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ShopPlayerSell struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ShopPlayerSell(BinaryReader br) : this() {
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
			Itemslot = br.ReadUInt32();
			Quantity = br.ReadUInt32();
			Price = br.ReadUInt32();
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
			bw.Write(Itemslot);
			bw.Write(Quantity);
			bw.Write(Price);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ShopPlayerSell {\n";
			ret += "	Npcid = ";
			try {
				ret += $"{ Indentify(Npcid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Itemslot = ";
			try {
				ret += $"{ Indentify(Itemslot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Quantity = ";
			try {
				ret += $"{ Indentify(Quantity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Price = ";
			try {
				ret += $"{ Indentify(Price) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}