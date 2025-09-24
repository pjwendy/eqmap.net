using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ExpeditionExpireWarning
// {
// /*000*/ uint32 client_id;
// /*004*/ uint32 unknown004;
// /*008*/ uint32 minutes_remaining;
// };

// ENCODE/DECODE Section:
// ENCODE(OP_DzExpeditionEndsWarning)
// {
// ENCODE_LENGTH_EXACT(ExpeditionExpireWarning);
// SETUP_DIRECT_ENCODE(ExpeditionExpireWarning, structs::ExpeditionExpireWarning);
// 
// OUT(minutes_remaining);
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DzExpeditionEndsWarning packet structure for EverQuest network communication.
	/// </summary>
	public struct DzExpeditionEndsWarning : IEQStruct {
		/// <summary>
		/// Gets or sets the clientid value.
		/// </summary>
		public uint ClientId { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public uint Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the minutesremaining value.
		/// </summary>
		public uint MinutesRemaining { get; set; }

		/// <summary>
		/// Initializes a new instance of the DzExpeditionEndsWarning struct with specified field values.
		/// </summary>
		/// <param name="client_id">The clientid value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="minutes_remaining">The minutesremaining value.</param>
		public DzExpeditionEndsWarning(uint client_id, uint unknown004, uint minutes_remaining) : this() {
			ClientId = client_id;
			Unknown004 = unknown004;
			MinutesRemaining = minutes_remaining;
		}

		/// <summary>
		/// Initializes a new instance of the DzExpeditionEndsWarning struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DzExpeditionEndsWarning(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DzExpeditionEndsWarning struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DzExpeditionEndsWarning(BinaryReader br) : this() {
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
			ClientId = br.ReadUInt32();
			Unknown004 = br.ReadUInt32();
			MinutesRemaining = br.ReadUInt32();
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
			bw.Write(ClientId);
			bw.Write(Unknown004);
			bw.Write(MinutesRemaining);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DzExpeditionEndsWarning {\n";
			ret += "	ClientId = ";
			try {
				ret += $"{ Indentify(ClientId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MinutesRemaining = ";
			try {
				ret += $"{ Indentify(MinutesRemaining) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}