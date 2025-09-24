using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Merchant_Sell_Struct {
// /*000*/	uint32	npcid;			// Merchant NPC's entity id
// /*004*/	uint32	playerid;		// Player's entity id
// /*008*/	uint32	itemslot;
// /*012*/	uint32	unknown12;
// /*016*/	uint32	quantity;
// /*020*/	uint32	Unknown020;
// /*024*/	uint32	price;
// /*028*/	uint32	pricehighorderbits;	// It appears the price is 64 bits in Underfoot+
// /*032*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_ShopPlayerBuy)
// {
// DECODE_LENGTH_EXACT(structs::Merchant_Sell_Struct);
// SETUP_DIRECT_DECODE(Merchant_Sell_Struct, structs::Merchant_Sell_Struct);
// 
// IN(npcid);
// IN(playerid);
// IN(itemslot);
// IN(quantity);
// IN(price);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ShopPlayerBuy packet structure for EverQuest network communication.
	/// </summary>
	public struct ShopPlayerBuy : IEQStruct {
		/// <summary>
		/// Gets or sets the npcid value.
		/// </summary>
		public uint Npcid { get; set; }

		/// <summary>
		/// Gets or sets the playerid value.
		/// </summary>
		public uint Playerid { get; set; }

		/// <summary>
		/// Gets or sets the itemslot value.
		/// </summary>
		public uint Itemslot { get; set; }

		/// <summary>
		/// Gets or sets the unknown12 value.
		/// </summary>
		public uint Unknown12 { get; set; }

		/// <summary>
		/// Gets or sets the quantity value.
		/// </summary>
		public uint Quantity { get; set; }

		/// <summary>
		/// Gets or sets the unknown020 value.
		/// </summary>
		public uint Unknown020 { get; set; }

		/// <summary>
		/// Gets or sets the price value.
		/// </summary>
		public uint Price { get; set; }

		/// <summary>
		/// Gets or sets the pricehighorderbits value.
		/// </summary>
		public uint Pricehighorderbits { get; set; }

		/// <summary>
		/// Initializes a new instance of the ShopPlayerBuy struct with specified field values.
		/// </summary>
		/// <param name="npcid">The npcid value.</param>
		/// <param name="playerid">The playerid value.</param>
		/// <param name="itemslot">The itemslot value.</param>
		/// <param name="unknown12">The unknown12 value.</param>
		/// <param name="quantity">The quantity value.</param>
		/// <param name="Unknown020">The unknown020 value.</param>
		/// <param name="price">The price value.</param>
		/// <param name="pricehighorderbits">The pricehighorderbits value.</param>
		public ShopPlayerBuy(uint npcid, uint playerid, uint itemslot, uint unknown12, uint quantity, uint Unknown020, uint price, uint pricehighorderbits) : this() {
			Npcid = npcid;
			Playerid = playerid;
			Itemslot = itemslot;
			Unknown12 = unknown12;
			Quantity = quantity;
			Unknown020 = Unknown020;
			Price = price;
			Pricehighorderbits = pricehighorderbits;
		}

		/// <summary>
		/// Initializes a new instance of the ShopPlayerBuy struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ShopPlayerBuy(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ShopPlayerBuy struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ShopPlayerBuy(BinaryReader br) : this() {
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
			Itemslot = br.ReadUInt32();
			Unknown12 = br.ReadUInt32();
			Quantity = br.ReadUInt32();
			Unknown020 = br.ReadUInt32();
			Price = br.ReadUInt32();
			Pricehighorderbits = br.ReadUInt32();
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
			bw.Write(Itemslot);
			bw.Write(Unknown12);
			bw.Write(Quantity);
			bw.Write(Unknown020);
			bw.Write(Price);
			bw.Write(Pricehighorderbits);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ShopPlayerBuy {\n";
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
			ret += "	Itemslot = ";
			try {
				ret += $"{ Indentify(Itemslot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown12 = ";
			try {
				ret += $"{ Indentify(Unknown12) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Quantity = ";
			try {
				ret += $"{ Indentify(Quantity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown020 = ";
			try {
				ret += $"{ Indentify(Unknown020) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Price = ";
			try {
				ret += $"{ Indentify(Price) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Pricehighorderbits = ";
			try {
				ret += $"{ Indentify(Pricehighorderbits) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}