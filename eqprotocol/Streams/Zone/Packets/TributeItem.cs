using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TributeItem_Struct {
// uint32	slot;
// uint32	quantity;
// uint32	tribute_master_id;
// int32	tribute_points;
// };

// ENCODE/DECODE Section:
// DECODE(OP_TributeItem)
// {
// DECODE_LENGTH_EXACT(structs::TributeItem_Struct);
// SETUP_DIRECT_DECODE(TributeItem_Struct, structs::TributeItem_Struct);
// 
// emu->slot = UFToServerSlot(eq->slot);
// IN(quantity);
// IN(tribute_master_id);
// IN(tribute_points);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TributeItem packet structure for EverQuest network communication.
	/// </summary>
	public struct TributeItem : IEQStruct {
		/// <summary>
		/// Gets or sets the slot value.
		/// </summary>
		public uint Slot { get; set; }

		/// <summary>
		/// Gets or sets the quantity value.
		/// </summary>
		public uint Quantity { get; set; }

		/// <summary>
		/// Gets or sets the tributemasterid value.
		/// </summary>
		public uint TributeMasterId { get; set; }

		/// <summary>
		/// Gets or sets the tributepoints value.
		/// </summary>
		public int TributePoints { get; set; }

		/// <summary>
		/// Initializes a new instance of the TributeItem struct with specified field values.
		/// </summary>
		/// <param name="slot">The slot value.</param>
		/// <param name="quantity">The quantity value.</param>
		/// <param name="tribute_master_id">The tributemasterid value.</param>
		/// <param name="tribute_points">The tributepoints value.</param>
		public TributeItem(uint slot, uint quantity, uint tribute_master_id, int tribute_points) : this() {
			Slot = slot;
			Quantity = quantity;
			TributeMasterId = tribute_master_id;
			TributePoints = tribute_points;
		}

		/// <summary>
		/// Initializes a new instance of the TributeItem struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TributeItem(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TributeItem struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TributeItem(BinaryReader br) : this() {
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
			Slot = br.ReadUInt32();
			Quantity = br.ReadUInt32();
			TributeMasterId = br.ReadUInt32();
			TributePoints = br.ReadInt32();
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
			bw.Write(Slot);
			bw.Write(Quantity);
			bw.Write(TributeMasterId);
			bw.Write(TributePoints);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TributeItem {\n";
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Quantity = ";
			try {
				ret += $"{ Indentify(Quantity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeMasterId = ";
			try {
				ret += $"{ Indentify(TributeMasterId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributePoints = ";
			try {
				ret += $"{ Indentify(TributePoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}