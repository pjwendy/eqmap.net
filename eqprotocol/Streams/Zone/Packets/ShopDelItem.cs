using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Merchant_DelItem_Struct{
// /*000*/	uint32	npcid;			// Merchant NPC's entity id
// /*004*/	uint32	playerid;		// Player's entity id
// /*008*/	uint32	itemslot;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ShopDelItem packet structure for EverQuest network communication.
	/// </summary>
	public struct ShopDelItem : IEQStruct {
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
		/// Initializes a new instance of the ShopDelItem struct with specified field values.
		/// </summary>
		/// <param name="npcid">The npcid value.</param>
		/// <param name="playerid">The playerid value.</param>
		/// <param name="itemslot">The itemslot value.</param>
		public ShopDelItem(uint npcid, uint playerid, uint itemslot) : this() {
			Npcid = npcid;
			Playerid = playerid;
			Itemslot = itemslot;
		}

		/// <summary>
		/// Initializes a new instance of the ShopDelItem struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ShopDelItem(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ShopDelItem struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ShopDelItem(BinaryReader br) : this() {
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
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ShopDelItem {\n";
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
			
			return ret;
		}
	}
}