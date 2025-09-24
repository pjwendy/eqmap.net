using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ExpeditionLockoutTimers_Struct
// {
// /*000*/ uint32 client_id;
// /*004*/ uint32 count;
// /*008*/ ExpeditionLockoutTimerEntry_Struct timers[0];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_DzExpeditionLockoutTimers)
// {
// SETUP_VAR_ENCODE(ExpeditionLockoutTimers_Struct);
// 
// SerializeBuffer buf;
// buf.WriteUInt32(emu->client_id);
// buf.WriteUInt32(emu->count);
// for (uint32 i = 0; i < emu->count; ++i)
// {
// buf.WriteString(emu->timers[i].expedition_name);
// buf.WriteUInt32(emu->timers[i].seconds_remaining);
// buf.WriteInt32(emu->timers[i].event_type);
// buf.WriteString(emu->timers[i].event_name);
// }
// 
// __packet->size = buf.size();
// __packet->pBuffer = new unsigned char[__packet->size];
// memcpy(__packet->pBuffer, buf.buffer(), __packet->size);
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DzExpeditionLockoutTimers packet structure for EverQuest network communication.
	/// </summary>
	public struct DzExpeditionLockoutTimers : IEQStruct {
		/// <summary>
		/// Gets or sets the clientid value.
		/// </summary>
		public uint ClientId { get; set; }

		/// <summary>
		/// Gets or sets the count value.
		/// </summary>
		public uint Count { get; set; }

		/// <summary>
		/// Gets or sets the timers value.
		/// </summary>
		public uint Timers { get; set; }

		/// <summary>
		/// Initializes a new instance of the DzExpeditionLockoutTimers struct with specified field values.
		/// </summary>
		/// <param name="client_id">The clientid value.</param>
		/// <param name="count">The count value.</param>
		/// <param name="timers">The timers value.</param>
		public DzExpeditionLockoutTimers(uint client_id, uint count, uint timers) : this() {
			ClientId = client_id;
			Count = count;
			Timers = timers;
		}

		/// <summary>
		/// Initializes a new instance of the DzExpeditionLockoutTimers struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DzExpeditionLockoutTimers(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DzExpeditionLockoutTimers struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DzExpeditionLockoutTimers(BinaryReader br) : this() {
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
			Count = br.ReadUInt32();
			Timers = br.ReadUInt32();
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
			bw.Write(Count);
			bw.Write(Timers);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DzExpeditionLockoutTimers {\n";
			ret += "	ClientId = ";
			try {
				ret += $"{ Indentify(ClientId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Count = ";
			try {
				ret += $"{ Indentify(Count) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Timers = ";
			try {
				ret += $"{ Indentify(Timers) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}