using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AltCurrencyPopulate_Struct {
// /*000*/ uint32 opcode; //8 for populate
// /*004*/ uint32 count; //number of entries
// /*008*/ AltCurrencyPopulateEntry_Struct entries[0];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_AltCurrency)
// {
// EQApplicationPacket *in = *p;
// *p = nullptr;
// 
// unsigned char *emu_buffer = in->pBuffer;
// uint32 opcode = *((uint32*)emu_buffer);
// 
// if (opcode == AlternateCurrencyMode::Populate) {
// AltCurrencyPopulate_Struct *populate = (AltCurrencyPopulate_Struct*)emu_buffer;
// 
// auto outapp = new EQApplicationPacket(
// OP_AltCurrency, sizeof(structs::AltCurrencyPopulate_Struct) +
// sizeof(structs::AltCurrencyPopulateEntry_Struct) * populate->count);
// structs::AltCurrencyPopulate_Struct *out_populate = (structs::AltCurrencyPopulate_Struct*)outapp->pBuffer;
// 
// out_populate->opcode = populate->opcode;
// out_populate->count = populate->count;
// for (uint32 i = 0; i < populate->count; ++i) {
// out_populate->entries[i].currency_number = populate->entries[i].currency_number;
// out_populate->entries[i].currency_number2 = populate->entries[i].currency_number2;
// out_populate->entries[i].item_id = populate->entries[i].item_id;
// out_populate->entries[i].item_icon = populate->entries[i].item_icon;
// out_populate->entries[i].stack_size = populate->entries[i].stack_size;
// out_populate->entries[i].unknown00 = populate->entries[i].unknown00;
// }
// 
// dest->FastQueuePacket(&outapp, ack_req);
// }
// else {
// auto outapp = new EQApplicationPacket(OP_AltCurrency, sizeof(AltCurrencyUpdate_Struct));
// memcpy(outapp->pBuffer, emu_buffer, sizeof(AltCurrencyUpdate_Struct));
// dest->FastQueuePacket(&outapp, ack_req);
// }
// 
// //dest->FastQueuePacket(&outapp, ack_req);
// delete in;
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AltCurrency packet structure for EverQuest network communication.
	/// </summary>
	public struct AltCurrency : IEQStruct {
		/// <summary>
		/// Gets or sets the opcode value.
		/// </summary>
		public uint Opcode { get; set; }

		/// <summary>
		/// Gets or sets the count value.
		/// </summary>
		public uint Count { get; set; }

		/// <summary>
		/// Gets or sets the entries value.
		/// </summary>
		public uint Entries { get; set; }

		/// <summary>
		/// Initializes a new instance of the AltCurrency struct with specified field values.
		/// </summary>
		/// <param name="opcode">The opcode value.</param>
		/// <param name="count">The count value.</param>
		/// <param name="entries">The entries value.</param>
		public AltCurrency(uint opcode, uint count, uint entries) : this() {
			Opcode = opcode;
			Count = count;
			Entries = entries;
		}

		/// <summary>
		/// Initializes a new instance of the AltCurrency struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AltCurrency(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AltCurrency struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AltCurrency(BinaryReader br) : this() {
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
			Opcode = br.ReadUInt32();
			Count = br.ReadUInt32();
			Entries = br.ReadUInt32();
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
			bw.Write(Opcode);
			bw.Write(Count);
			bw.Write(Entries);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AltCurrency {\n";
			ret += "	Opcode = ";
			try {
				ret += $"{ Indentify(Opcode) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Count = ";
			try {
				ret += $"{ Indentify(Count) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Entries = ";
			try {
				ret += $"{ Indentify(Entries) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}