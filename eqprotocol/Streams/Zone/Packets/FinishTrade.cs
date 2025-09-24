using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Parcel_Struct
// {
// /*000*/ uint32 npc_id;
// /*004*/ uint32 item_slot;
// /*008*/ uint32 quantity;
// /*012*/ uint32 money_flag;
// /*016*/ char   send_to[64];
// /*080*/ char   note[128];
// /*208*/ uint32 unknown_208;
// /*212*/ uint32 unknown_212;
// /*216*/ uint32 unknown_216;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the FinishTrade packet structure for EverQuest network communication.
	/// </summary>
	public struct FinishTrade : IEQStruct {
		/// <summary>
		/// Gets or sets the npcid value.
		/// </summary>
		public uint NpcId { get; set; }

		/// <summary>
		/// Gets or sets the itemslot value.
		/// </summary>
		public uint ItemSlot { get; set; }

		/// <summary>
		/// Gets or sets the quantity value.
		/// </summary>
		public uint Quantity { get; set; }

		/// <summary>
		/// Gets or sets the moneyflag value.
		/// </summary>
		public uint MoneyFlag { get; set; }

		/// <summary>
		/// Gets or sets the sendto value.
		/// </summary>
		public byte[] SendTo { get; set; }

		/// <summary>
		/// Gets or sets the note value.
		/// </summary>
		public byte[] Note { get; set; }

		/// <summary>
		/// Initializes a new instance of the FinishTrade struct with specified field values.
		/// </summary>
		/// <param name="npc_id">The npcid value.</param>
		/// <param name="item_slot">The itemslot value.</param>
		/// <param name="quantity">The quantity value.</param>
		/// <param name="money_flag">The moneyflag value.</param>
		/// <param name="send_to">The sendto value.</param>
		/// <param name="note">The note value.</param>
		public FinishTrade(uint npc_id, uint item_slot, uint quantity, uint money_flag, byte[] send_to, byte[] note) : this() {
			NpcId = npc_id;
			ItemSlot = item_slot;
			Quantity = quantity;
			MoneyFlag = money_flag;
			SendTo = send_to;
			Note = note;
		}

		/// <summary>
		/// Initializes a new instance of the FinishTrade struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public FinishTrade(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the FinishTrade struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public FinishTrade(BinaryReader br) : this() {
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
			NpcId = br.ReadUInt32();
			ItemSlot = br.ReadUInt32();
			Quantity = br.ReadUInt32();
			MoneyFlag = br.ReadUInt32();
			// TODO: Array reading for SendTo - implement based on actual array size
			// SendTo = new byte[size];
			// TODO: Array reading for Note - implement based on actual array size
			// Note = new byte[size];
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
			bw.Write(NpcId);
			bw.Write(ItemSlot);
			bw.Write(Quantity);
			bw.Write(MoneyFlag);
			// TODO: Array writing for SendTo - implement based on actual array size
			// foreach(var item in SendTo) bw.Write(item);
			// TODO: Array writing for Note - implement based on actual array size
			// foreach(var item in Note) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct FinishTrade {\n";
			ret += "	NpcId = ";
			try {
				ret += $"{ Indentify(NpcId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ItemSlot = ";
			try {
				ret += $"{ Indentify(ItemSlot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Quantity = ";
			try {
				ret += $"{ Indentify(Quantity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MoneyFlag = ";
			try {
				ret += $"{ Indentify(MoneyFlag) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SendTo = ";
			try {
				ret += $"{ Indentify(SendTo) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Note = ";
			try {
				ret += $"{ Indentify(Note) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}