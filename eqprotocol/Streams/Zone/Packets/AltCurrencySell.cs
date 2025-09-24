using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AltCurrencySellItem_Struct {
// /*000*/ uint32 merchant_entity_id;
// /*004*/ uint32 slot_id;
// /*008*/ uint32 charges;
// /*012*/ uint32 cost;
// };

// ENCODE/DECODE Section:
// DECODE(OP_AltCurrencySell)
// {
// DECODE_LENGTH_EXACT(structs::AltCurrencySellItem_Struct);
// SETUP_DIRECT_DECODE(AltCurrencySellItem_Struct, structs::AltCurrencySellItem_Struct);
// 
// IN(merchant_entity_id);
// emu->slot_id = UFToServerSlot(eq->slot_id);
// IN(charges);
// IN(cost);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AltCurrencySell packet structure for EverQuest network communication.
	/// </summary>
	public struct AltCurrencySell : IEQStruct {
		/// <summary>
		/// Gets or sets the merchantentityid value.
		/// </summary>
		public uint MerchantEntityId { get; set; }

		/// <summary>
		/// Gets or sets the slotid value.
		/// </summary>
		public uint SlotId { get; set; }

		/// <summary>
		/// Gets or sets the charges value.
		/// </summary>
		public uint Charges { get; set; }

		/// <summary>
		/// Gets or sets the cost value.
		/// </summary>
		public uint Cost { get; set; }

		/// <summary>
		/// Initializes a new instance of the AltCurrencySell struct with specified field values.
		/// </summary>
		/// <param name="merchant_entity_id">The merchantentityid value.</param>
		/// <param name="slot_id">The slotid value.</param>
		/// <param name="charges">The charges value.</param>
		/// <param name="cost">The cost value.</param>
		public AltCurrencySell(uint merchant_entity_id, uint slot_id, uint charges, uint cost) : this() {
			MerchantEntityId = merchant_entity_id;
			SlotId = slot_id;
			Charges = charges;
			Cost = cost;
		}

		/// <summary>
		/// Initializes a new instance of the AltCurrencySell struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AltCurrencySell(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AltCurrencySell struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AltCurrencySell(BinaryReader br) : this() {
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
			MerchantEntityId = br.ReadUInt32();
			SlotId = br.ReadUInt32();
			Charges = br.ReadUInt32();
			Cost = br.ReadUInt32();
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
			bw.Write(MerchantEntityId);
			bw.Write(SlotId);
			bw.Write(Charges);
			bw.Write(Cost);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AltCurrencySell {\n";
			ret += "	MerchantEntityId = ";
			try {
				ret += $"{ Indentify(MerchantEntityId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SlotId = ";
			try {
				ret += $"{ Indentify(SlotId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Charges = ";
			try {
				ret += $"{ Indentify(Charges) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Cost = ";
			try {
				ret += $"{ Indentify(Cost) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}