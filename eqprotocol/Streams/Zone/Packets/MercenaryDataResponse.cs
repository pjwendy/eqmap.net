using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MercenaryMerchantList_Struct {
// /*0000*/	uint32	MercTypeCount;			// Number of Merc Types to follow
// /*0004*/	uint32	MercTypes[1];			// Count varies, but hard set to 3 for now - From dbstr_us.txt - Apprentice (330000100), Journeyman (330000200), Master (330000300)
// /*0016*/	uint32	MercCount;				// Number of MercenaryInfo_Struct to follow
// /*0020*/	MercenaryListEntry_Struct Mercs[0];	// Data for individual mercenaries in the Merchant List
// };

// ENCODE/DECODE Section:
// ENCODE(OP_MercenaryDataResponse)
// {
// //consume the packet
// EQApplicationPacket *in = *p;
// *p = nullptr;
// 
// //store away the emu struct
// unsigned char *__emu_buffer = in->pBuffer;
// MercenaryMerchantList_Struct *emu = (MercenaryMerchantList_Struct *)__emu_buffer;
// 
// char *Buffer = (char *)in->pBuffer;
// 
// int PacketSize = sizeof(structs::MercenaryMerchantList_Struct) - 4 + emu->MercTypeCount * 4;
// PacketSize += (sizeof(structs::MercenaryListEntry_Struct) - sizeof(structs::MercenaryStance_Struct)) * emu->MercCount;
// 
// uint32 r;
// uint32 k;
// for (r = 0; r < emu->MercCount; r++)
// {
// PacketSize += sizeof(structs::MercenaryStance_Struct) * emu->Mercs[r].StanceCount;
// }
// 
// auto outapp = new EQApplicationPacket(OP_MercenaryDataResponse, PacketSize);
// Buffer = (char *)outapp->pBuffer;
// 
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercTypeCount);
// 
// for (r = 0; r < emu->MercTypeCount; r++)
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercGrades[r]);
// }
// 
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercCount);
// 
// for (r = 0; r < emu->MercCount; r++)
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].MercID);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].MercType);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].MercSubType);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].PurchaseCost);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].UpkeepCost);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].AltCurrencyCost);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].AltCurrencyUpkeep);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].AltCurrencyType);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->Mercs[r].MercUnk01);
// VARSTRUCT_ENCODE_TYPE(int32, Buffer, emu->Mercs[r].TimeLeft);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].MerchantSlot);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].MercUnk02);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].StanceCount);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].MercUnk03);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->Mercs[r].MercUnk04);
// for (k = 0; k < emu->Mercs[r].StanceCount; k++)
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].Stances[k].StanceIndex);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->Mercs[r].Stances[k].Stance);
// }
// }
// 
// dest->FastQueuePacket(&outapp, ack_req);
// delete in;
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MercenaryDataResponse packet structure for EverQuest network communication.
	/// </summary>
	public struct MercenaryDataResponse : IEQStruct {
		/// <summary>
		/// Gets or sets the merctypecount value.
		/// </summary>
		public uint Merctypecount { get; set; }

		/// <summary>
		/// Gets or sets the merctypes value.
		/// </summary>
		public uint Merctypes { get; set; }

		/// <summary>
		/// Gets or sets the merccount value.
		/// </summary>
		public uint Merccount { get; set; }

		/// <summary>
		/// Gets or sets the mercs value.
		/// </summary>
		public uint Mercs { get; set; }

		/// <summary>
		/// Initializes a new instance of the MercenaryDataResponse struct with specified field values.
		/// </summary>
		/// <param name="MercTypeCount">The merctypecount value.</param>
		/// <param name="MercTypes">The merctypes value.</param>
		/// <param name="MercCount">The merccount value.</param>
		/// <param name="Mercs">The mercs value.</param>
		public MercenaryDataResponse(uint MercTypeCount, uint MercTypes, uint MercCount, uint Mercs) : this() {
			Merctypecount = MercTypeCount;
			Merctypes = MercTypes;
			Merccount = MercCount;
			Mercs = Mercs;
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryDataResponse struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MercenaryDataResponse(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryDataResponse struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MercenaryDataResponse(BinaryReader br) : this() {
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
			Merctypecount = br.ReadUInt32();
			Merctypes = br.ReadUInt32();
			Merccount = br.ReadUInt32();
			Mercs = br.ReadUInt32();
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
			bw.Write(Merctypecount);
			bw.Write(Merctypes);
			bw.Write(Merccount);
			bw.Write(Mercs);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct MercenaryDataResponse {\n";
			ret += "	Merctypecount = ";
			try {
				ret += $"{ Indentify(Merctypecount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Merctypes = ";
			try {
				ret += $"{ Indentify(Merctypes) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Merccount = ";
			try {
				ret += $"{ Indentify(Merccount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mercs = ";
			try {
				ret += $"{ Indentify(Mercs) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}