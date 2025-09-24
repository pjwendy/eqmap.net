using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MercenaryDataUpdate_Struct {
// /*0000*/	int32	MercStatus;					// Seen 0 with merc and -1 with no merc hired
// /*0004*/	uint32	MercCount;					// Seen 1 with 1 merc hired and 0 with no merc hired
// /*0008*/	MercenaryData_Struct MercData[0];	// Data for individual mercenaries in the Merchant List
// };

// ENCODE/DECODE Section:
// ENCODE(OP_MercenaryDataUpdate)
// {
// //consume the packet
// EQApplicationPacket *in = *p;
// *p = nullptr;
// 
// //store away the emu struct
// unsigned char *__emu_buffer = in->pBuffer;
// MercenaryDataUpdate_Struct *emu = (MercenaryDataUpdate_Struct *)__emu_buffer;
// 
// char *Buffer = (char *)in->pBuffer;
// 
// EQApplicationPacket *outapp;
// 
// uint32 PacketSize = 0;
// 
// // There are 2 different sized versions of this packet depending if a merc is hired or not
// if (emu->MercStatus >= 0)
// {
// PacketSize += sizeof(structs::MercenaryDataUpdate_Struct) + (sizeof(structs::MercenaryData_Struct) - sizeof(structs::MercenaryStance_Struct)) * emu->MercCount;
// 
// uint32 r;
// uint32 k;
// for (r = 0; r < emu->MercCount; r++)
// {
// PacketSize += sizeof(structs::MercenaryStance_Struct) * emu->MercData[r].StanceCount;
// }
// 
// outapp = new EQApplicationPacket(OP_MercenaryDataUpdate, PacketSize);
// Buffer = (char *)outapp->pBuffer;
// 
// VARSTRUCT_ENCODE_TYPE(int32, Buffer, emu->MercStatus);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercCount);
// 
// for (r = 0; r < emu->MercCount; r++)
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].MercID);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].MercType);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].MercSubType);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].PurchaseCost);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].UpkeepCost);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].AltCurrencyCost);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].AltCurrencyUpkeep);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].AltCurrencyType);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->MercData[r].MercUnk01);
// VARSTRUCT_ENCODE_TYPE(int32, Buffer, emu->MercData[r].TimeLeft);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].MerchantSlot);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].MercUnk02);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].StanceCount);
// VARSTRUCT_ENCODE_TYPE(int32, Buffer, emu->MercData[r].MercUnk03);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->MercData[r].MercUnk04);
// for (k = 0; k < emu->MercData[r].StanceCount; k++)
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].Stances[k].StanceIndex);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].Stances[k].Stance);
// }
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercData[r].MercUnk05);
// }
// }
// else
// {
// PacketSize += sizeof(structs::NoMercenaryHired_Struct);
// 
// outapp = new EQApplicationPacket(OP_MercenaryDataUpdate, PacketSize);
// Buffer = (char *)outapp->pBuffer;
// 
// VARSTRUCT_ENCODE_TYPE(int32, Buffer, emu->MercStatus);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->MercCount);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 1);
// }
// 
// dest->FastQueuePacket(&outapp, ack_req);
// delete in;
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MercenaryDataUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct MercenaryDataUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the mercstatus value.
		/// </summary>
		public int Mercstatus { get; set; }

		/// <summary>
		/// Gets or sets the merccount value.
		/// </summary>
		public uint Merccount { get; set; }

		/// <summary>
		/// Gets or sets the mercdata value.
		/// </summary>
		public uint Mercdata { get; set; }

		/// <summary>
		/// Initializes a new instance of the MercenaryDataUpdate struct with specified field values.
		/// </summary>
		/// <param name="MercStatus">The mercstatus value.</param>
		/// <param name="MercCount">The merccount value.</param>
		/// <param name="MercData">The mercdata value.</param>
		public MercenaryDataUpdate(int MercStatus, uint MercCount, uint MercData) : this() {
			Mercstatus = MercStatus;
			Merccount = MercCount;
			Mercdata = MercData;
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryDataUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MercenaryDataUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryDataUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MercenaryDataUpdate(BinaryReader br) : this() {
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
			Mercstatus = br.ReadInt32();
			Merccount = br.ReadUInt32();
			Mercdata = br.ReadUInt32();
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
			bw.Write(Mercstatus);
			bw.Write(Merccount);
			bw.Write(Mercdata);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct MercenaryDataUpdate {\n";
			ret += "	Mercstatus = ";
			try {
				ret += $"{ Indentify(Mercstatus) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Merccount = ";
			try {
				ret += $"{ Indentify(Merccount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mercdata = ";
			try {
				ret += $"{ Indentify(Mercdata) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}