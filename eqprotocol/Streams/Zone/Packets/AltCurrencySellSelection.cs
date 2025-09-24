using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AltCurrencySelectItem_Struct {
// uint32 merchant_entity_id;
// uint32 slot_id;
// uint32 unknown008;
// uint32 unknown012;
// uint32 unknown016;
// uint32 unknown020;
// uint32 unknown024;
// uint32 unknown028;
// uint32 unknown032;
// uint32 unknown036;
// uint32 unknown040;
// uint32 unknown044;
// uint32 unknown048;
// uint32 unknown052;
// uint32 unknown056;
// uint32 unknown060;
// uint32 unknown064;
// uint32 unknown068;
// uint32 unknown072;
// uint32 unknown076;
// };

// ENCODE/DECODE Section:
// DECODE(OP_AltCurrencySellSelection)
// {
// DECODE_LENGTH_EXACT(structs::AltCurrencySelectItem_Struct);
// SETUP_DIRECT_DECODE(AltCurrencySelectItem_Struct, structs::AltCurrencySelectItem_Struct);
// 
// IN(merchant_entity_id);
// emu->slot_id = UFToServerSlot(eq->slot_id);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AltCurrencySellSelection packet structure for EverQuest network communication.
	/// </summary>
	public struct AltCurrencySellSelection : IEQStruct {
		/// <summary>
		/// Gets or sets the merchantentityid value.
		/// </summary>
		public uint MerchantEntityId { get; set; }

		/// <summary>
		/// Gets or sets the slotid value.
		/// </summary>
		public uint SlotId { get; set; }

		/// <summary>
		/// Gets or sets the unknown008 value.
		/// </summary>
		public uint Unknown008 { get; set; }

		/// <summary>
		/// Gets or sets the unknown012 value.
		/// </summary>
		public uint Unknown012 { get; set; }

		/// <summary>
		/// Gets or sets the unknown016 value.
		/// </summary>
		public uint Unknown016 { get; set; }

		/// <summary>
		/// Gets or sets the unknown020 value.
		/// </summary>
		public uint Unknown020 { get; set; }

		/// <summary>
		/// Gets or sets the unknown024 value.
		/// </summary>
		public uint Unknown024 { get; set; }

		/// <summary>
		/// Gets or sets the unknown028 value.
		/// </summary>
		public uint Unknown028 { get; set; }

		/// <summary>
		/// Gets or sets the unknown032 value.
		/// </summary>
		public uint Unknown032 { get; set; }

		/// <summary>
		/// Gets or sets the unknown036 value.
		/// </summary>
		public uint Unknown036 { get; set; }

		/// <summary>
		/// Gets or sets the unknown040 value.
		/// </summary>
		public uint Unknown040 { get; set; }

		/// <summary>
		/// Gets or sets the unknown044 value.
		/// </summary>
		public uint Unknown044 { get; set; }

		/// <summary>
		/// Gets or sets the unknown048 value.
		/// </summary>
		public uint Unknown048 { get; set; }

		/// <summary>
		/// Gets or sets the unknown052 value.
		/// </summary>
		public uint Unknown052 { get; set; }

		/// <summary>
		/// Gets or sets the unknown056 value.
		/// </summary>
		public uint Unknown056 { get; set; }

		/// <summary>
		/// Gets or sets the unknown060 value.
		/// </summary>
		public uint Unknown060 { get; set; }

		/// <summary>
		/// Gets or sets the unknown064 value.
		/// </summary>
		public uint Unknown064 { get; set; }

		/// <summary>
		/// Gets or sets the unknown068 value.
		/// </summary>
		public uint Unknown068 { get; set; }

		/// <summary>
		/// Gets or sets the unknown072 value.
		/// </summary>
		public uint Unknown072 { get; set; }

		/// <summary>
		/// Gets or sets the unknown076 value.
		/// </summary>
		public uint Unknown076 { get; set; }

		/// <summary>
		/// Initializes a new instance of the AltCurrencySellSelection struct with specified field values.
		/// </summary>
		/// <param name="merchant_entity_id">The merchantentityid value.</param>
		/// <param name="slot_id">The slotid value.</param>
		/// <param name="unknown008">The unknown008 value.</param>
		/// <param name="unknown012">The unknown012 value.</param>
		/// <param name="unknown016">The unknown016 value.</param>
		/// <param name="unknown020">The unknown020 value.</param>
		/// <param name="unknown024">The unknown024 value.</param>
		/// <param name="unknown028">The unknown028 value.</param>
		/// <param name="unknown032">The unknown032 value.</param>
		/// <param name="unknown036">The unknown036 value.</param>
		/// <param name="unknown040">The unknown040 value.</param>
		/// <param name="unknown044">The unknown044 value.</param>
		/// <param name="unknown048">The unknown048 value.</param>
		/// <param name="unknown052">The unknown052 value.</param>
		/// <param name="unknown056">The unknown056 value.</param>
		/// <param name="unknown060">The unknown060 value.</param>
		/// <param name="unknown064">The unknown064 value.</param>
		/// <param name="unknown068">The unknown068 value.</param>
		/// <param name="unknown072">The unknown072 value.</param>
		/// <param name="unknown076">The unknown076 value.</param>
		public AltCurrencySellSelection(uint merchant_entity_id, uint slot_id, uint unknown008, uint unknown012, uint unknown016, uint unknown020, uint unknown024, uint unknown028, uint unknown032, uint unknown036, uint unknown040, uint unknown044, uint unknown048, uint unknown052, uint unknown056, uint unknown060, uint unknown064, uint unknown068, uint unknown072, uint unknown076) : this() {
			MerchantEntityId = merchant_entity_id;
			SlotId = slot_id;
			Unknown008 = unknown008;
			Unknown012 = unknown012;
			Unknown016 = unknown016;
			Unknown020 = unknown020;
			Unknown024 = unknown024;
			Unknown028 = unknown028;
			Unknown032 = unknown032;
			Unknown036 = unknown036;
			Unknown040 = unknown040;
			Unknown044 = unknown044;
			Unknown048 = unknown048;
			Unknown052 = unknown052;
			Unknown056 = unknown056;
			Unknown060 = unknown060;
			Unknown064 = unknown064;
			Unknown068 = unknown068;
			Unknown072 = unknown072;
			Unknown076 = unknown076;
		}

		/// <summary>
		/// Initializes a new instance of the AltCurrencySellSelection struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AltCurrencySellSelection(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AltCurrencySellSelection struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AltCurrencySellSelection(BinaryReader br) : this() {
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
			Unknown008 = br.ReadUInt32();
			Unknown012 = br.ReadUInt32();
			Unknown016 = br.ReadUInt32();
			Unknown020 = br.ReadUInt32();
			Unknown024 = br.ReadUInt32();
			Unknown028 = br.ReadUInt32();
			Unknown032 = br.ReadUInt32();
			Unknown036 = br.ReadUInt32();
			Unknown040 = br.ReadUInt32();
			Unknown044 = br.ReadUInt32();
			Unknown048 = br.ReadUInt32();
			Unknown052 = br.ReadUInt32();
			Unknown056 = br.ReadUInt32();
			Unknown060 = br.ReadUInt32();
			Unknown064 = br.ReadUInt32();
			Unknown068 = br.ReadUInt32();
			Unknown072 = br.ReadUInt32();
			Unknown076 = br.ReadUInt32();
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
			bw.Write(Unknown008);
			bw.Write(Unknown012);
			bw.Write(Unknown016);
			bw.Write(Unknown020);
			bw.Write(Unknown024);
			bw.Write(Unknown028);
			bw.Write(Unknown032);
			bw.Write(Unknown036);
			bw.Write(Unknown040);
			bw.Write(Unknown044);
			bw.Write(Unknown048);
			bw.Write(Unknown052);
			bw.Write(Unknown056);
			bw.Write(Unknown060);
			bw.Write(Unknown064);
			bw.Write(Unknown068);
			bw.Write(Unknown072);
			bw.Write(Unknown076);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AltCurrencySellSelection {\n";
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
			ret += "	Unknown008 = ";
			try {
				ret += $"{ Indentify(Unknown008) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown012 = ";
			try {
				ret += $"{ Indentify(Unknown012) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown016 = ";
			try {
				ret += $"{ Indentify(Unknown016) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown020 = ";
			try {
				ret += $"{ Indentify(Unknown020) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown024 = ";
			try {
				ret += $"{ Indentify(Unknown024) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown028 = ";
			try {
				ret += $"{ Indentify(Unknown028) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown032 = ";
			try {
				ret += $"{ Indentify(Unknown032) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown036 = ";
			try {
				ret += $"{ Indentify(Unknown036) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown040 = ";
			try {
				ret += $"{ Indentify(Unknown040) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown044 = ";
			try {
				ret += $"{ Indentify(Unknown044) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown048 = ";
			try {
				ret += $"{ Indentify(Unknown048) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown052 = ";
			try {
				ret += $"{ Indentify(Unknown052) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown056 = ";
			try {
				ret += $"{ Indentify(Unknown056) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown060 = ";
			try {
				ret += $"{ Indentify(Unknown060) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown064 = ";
			try {
				ret += $"{ Indentify(Unknown064) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown068 = ";
			try {
				ret += $"{ Indentify(Unknown068) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown072 = ";
			try {
				ret += $"{ Indentify(Unknown072) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown076 = ";
			try {
				ret += $"{ Indentify(Unknown076) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}