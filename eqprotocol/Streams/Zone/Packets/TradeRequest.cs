using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TradeRequest_Struct {
// /*00*/	uint32 to_mob_id;
// /*04*/	uint32 from_mob_id;
// /*08*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TradeRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct TradeRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the tomobid value.
		/// </summary>
		public uint ToMobId { get; set; }

		/// <summary>
		/// Gets or sets the frommobid value.
		/// </summary>
		public uint FromMobId { get; set; }

		/// <summary>
		/// Initializes a new instance of the TradeRequest struct with specified field values.
		/// </summary>
		/// <param name="to_mob_id">The tomobid value.</param>
		/// <param name="from_mob_id">The frommobid value.</param>
		public TradeRequest(uint to_mob_id, uint from_mob_id) : this() {
			ToMobId = to_mob_id;
			FromMobId = from_mob_id;
		}

		/// <summary>
		/// Initializes a new instance of the TradeRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TradeRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TradeRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TradeRequest(BinaryReader br) : this() {
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
			ToMobId = br.ReadUInt32();
			FromMobId = br.ReadUInt32();
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
			bw.Write(ToMobId);
			bw.Write(FromMobId);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TradeRequest {\n";
			ret += "	ToMobId = ";
			try {
				ret += $"{ Indentify(ToMobId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FromMobId = ";
			try {
				ret += $"{ Indentify(FromMobId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}