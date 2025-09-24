using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MercenaryMerchantShopRequest_Struct {
// /*0000*/	uint32	MercMerchantID;			// Entity ID of the Mercenary Merchant
// /*0004*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MercenaryDataRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct MercenaryDataRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the mercmerchantid value.
		/// </summary>
		public uint Mercmerchantid { get; set; }

		/// <summary>
		/// Initializes a new instance of the MercenaryDataRequest struct with specified field values.
		/// </summary>
		/// <param name="MercMerchantID">The mercmerchantid value.</param>
		public MercenaryDataRequest(uint MercMerchantID) : this() {
			Mercmerchantid = MercMerchantID;
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryDataRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MercenaryDataRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryDataRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MercenaryDataRequest(BinaryReader br) : this() {
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
			Mercmerchantid = br.ReadUInt32();
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
			bw.Write(Mercmerchantid);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct MercenaryDataRequest {\n";
			ret += "	Mercmerchantid = ";
			try {
				ret += $"{ Indentify(Mercmerchantid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}