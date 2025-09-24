using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TraderBuy_Struct {
// uint32 action;
// uint32 unknown_004;
// uint32 price;
// uint32 unknown_008;    // Probably high order bits of a 64 bit price.
// uint32 trader_id;
// char   item_name[64];
// uint32 unknown_076;
// uint32 item_id;
// uint32 already_sold;
// uint32 quantity;
// uint32 unknown_092;
// };

// ENCODE/DECODE Section:
// DECODE(OP_TraderBuy)
// {
// DECODE_LENGTH_EXACT(structs::TraderBuy_Struct);
// SETUP_DIRECT_DECODE(TraderBuy_Struct, structs::TraderBuy_Struct);
// LogTrading(
// "Decode OP_TraderBuy(UF) item_id <green>[{}] price <green>[{}] quantity <green>[{}] trader_id <green>[{}]",
// eq->item_id,
// eq->price,
// eq->quantity,
// eq->trader_id
// );
// 
// emu->action = BuyTraderItem;
// IN(price);
// IN(trader_id);
// IN(item_id);
// IN(quantity);
// IN(already_sold);
// strn0cpy(emu->item_name, eq->item_name, sizeof(eq->item_name));
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TraderBuy packet structure for EverQuest network communication.
	/// </summary>
	public struct TraderBuy : IEQStruct {
		/// <summary>
		/// Gets or sets the action value.
		/// </summary>
		public uint Action { get; set; }

		/// <summary>
		/// Gets or sets the price value.
		/// </summary>
		public uint Price { get; set; }

		/// <summary>
		/// Gets or sets the traderid value.
		/// </summary>
		public uint TraderId { get; set; }

		/// <summary>
		/// Gets or sets the itemname value.
		/// </summary>
		public byte[] ItemName { get; set; }

		/// <summary>
		/// Gets or sets the itemid value.
		/// </summary>
		public uint ItemId { get; set; }

		/// <summary>
		/// Gets or sets the alreadysold value.
		/// </summary>
		public uint AlreadySold { get; set; }

		/// <summary>
		/// Gets or sets the quantity value.
		/// </summary>
		public uint Quantity { get; set; }

		/// <summary>
		/// Initializes a new instance of the TraderBuy struct with specified field values.
		/// </summary>
		/// <param name="action">The action value.</param>
		/// <param name="price">The price value.</param>
		/// <param name="trader_id">The traderid value.</param>
		/// <param name="item_name">The itemname value.</param>
		/// <param name="item_id">The itemid value.</param>
		/// <param name="already_sold">The alreadysold value.</param>
		/// <param name="quantity">The quantity value.</param>
		public TraderBuy(uint action, uint price, uint trader_id, byte[] item_name, uint item_id, uint already_sold, uint quantity) : this() {
			Action = action;
			Price = price;
			TraderId = trader_id;
			ItemName = item_name;
			ItemId = item_id;
			AlreadySold = already_sold;
			Quantity = quantity;
		}

		/// <summary>
		/// Initializes a new instance of the TraderBuy struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TraderBuy(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TraderBuy struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TraderBuy(BinaryReader br) : this() {
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
			Action = br.ReadUInt32();
			Price = br.ReadUInt32();
			TraderId = br.ReadUInt32();
			// TODO: Array reading for ItemName - implement based on actual array size
			// ItemName = new byte[size];
			ItemId = br.ReadUInt32();
			AlreadySold = br.ReadUInt32();
			Quantity = br.ReadUInt32();
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
			bw.Write(Action);
			bw.Write(Price);
			bw.Write(TraderId);
			// TODO: Array writing for ItemName - implement based on actual array size
			// foreach(var item in ItemName) bw.Write(item);
			bw.Write(ItemId);
			bw.Write(AlreadySold);
			bw.Write(Quantity);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TraderBuy {\n";
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Price = ";
			try {
				ret += $"{ Indentify(Price) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TraderId = ";
			try {
				ret += $"{ Indentify(TraderId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ItemName = ";
			try {
				ret += $"{ Indentify(ItemName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ItemId = ";
			try {
				ret += $"{ Indentify(ItemId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AlreadySold = ";
			try {
				ret += $"{ Indentify(AlreadySold) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Quantity = ";
			try {
				ret += $"{ Indentify(Quantity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}