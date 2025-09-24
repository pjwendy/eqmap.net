using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AltCurrencyPurchaseItem_Struct {
// /*000*/ uint32 merchant_entity_id;
// /*004*/ uint32 item_id;
// /*008*/ uint32 unknown008; //1
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AltCurrencyMerchantReply packet structure for EverQuest network communication.
	/// </summary>
	public struct AltCurrencyMerchantReply : IEQStruct {
		/// <summary>
		/// Gets or sets the merchantentityid value.
		/// </summary>
		public uint MerchantEntityId { get; set; }

		/// <summary>
		/// Gets or sets the itemid value.
		/// </summary>
		public uint ItemId { get; set; }

		/// <summary>
		/// Gets or sets the unknown008 value.
		/// </summary>
		public uint Unknown008 { get; set; }

		/// <summary>
		/// Initializes a new instance of the AltCurrencyMerchantReply struct with specified field values.
		/// </summary>
		/// <param name="merchant_entity_id">The merchantentityid value.</param>
		/// <param name="item_id">The itemid value.</param>
		/// <param name="unknown008">The unknown008 value.</param>
		public AltCurrencyMerchantReply(uint merchant_entity_id, uint item_id, uint unknown008) : this() {
			MerchantEntityId = merchant_entity_id;
			ItemId = item_id;
			Unknown008 = unknown008;
		}

		/// <summary>
		/// Initializes a new instance of the AltCurrencyMerchantReply struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AltCurrencyMerchantReply(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AltCurrencyMerchantReply struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AltCurrencyMerchantReply(BinaryReader br) : this() {
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
			ItemId = br.ReadUInt32();
			Unknown008 = br.ReadUInt32();
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
			bw.Write(ItemId);
			bw.Write(Unknown008);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AltCurrencyMerchantReply {\n";
			ret += "	MerchantEntityId = ";
			try {
				ret += $"{ Indentify(MerchantEntityId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ItemId = ";
			try {
				ret += $"{ Indentify(ItemId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown008 = ";
			try {
				ret += $"{ Indentify(Unknown008) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}