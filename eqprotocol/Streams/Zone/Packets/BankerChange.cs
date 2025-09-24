using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BankerChange_Struct {
// uint32	platinum;
// uint32	gold;
// uint32	silver;
// uint32	copper;
// uint32	platinum_bank;
// uint32	gold_bank;
// uint32	silver_bank;
// uint32	copper_bank;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the BankerChange packet structure for EverQuest network communication.
	/// </summary>
	public struct BankerChange : IEQStruct {
		/// <summary>
		/// Gets or sets the platinum value.
		/// </summary>
		public uint Platinum { get; set; }

		/// <summary>
		/// Gets or sets the gold value.
		/// </summary>
		public uint Gold { get; set; }

		/// <summary>
		/// Gets or sets the silver value.
		/// </summary>
		public uint Silver { get; set; }

		/// <summary>
		/// Gets or sets the copper value.
		/// </summary>
		public uint Copper { get; set; }

		/// <summary>
		/// Gets or sets the platinumbank value.
		/// </summary>
		public uint PlatinumBank { get; set; }

		/// <summary>
		/// Gets or sets the goldbank value.
		/// </summary>
		public uint GoldBank { get; set; }

		/// <summary>
		/// Gets or sets the silverbank value.
		/// </summary>
		public uint SilverBank { get; set; }

		/// <summary>
		/// Gets or sets the copperbank value.
		/// </summary>
		public uint CopperBank { get; set; }

		/// <summary>
		/// Initializes a new instance of the BankerChange struct with specified field values.
		/// </summary>
		/// <param name="platinum">The platinum value.</param>
		/// <param name="gold">The gold value.</param>
		/// <param name="silver">The silver value.</param>
		/// <param name="copper">The copper value.</param>
		/// <param name="platinum_bank">The platinumbank value.</param>
		/// <param name="gold_bank">The goldbank value.</param>
		/// <param name="silver_bank">The silverbank value.</param>
		/// <param name="copper_bank">The copperbank value.</param>
		public BankerChange(uint platinum, uint gold, uint silver, uint copper, uint platinum_bank, uint gold_bank, uint silver_bank, uint copper_bank) : this() {
			Platinum = platinum;
			Gold = gold;
			Silver = silver;
			Copper = copper;
			PlatinumBank = platinum_bank;
			GoldBank = gold_bank;
			SilverBank = silver_bank;
			CopperBank = copper_bank;
		}

		/// <summary>
		/// Initializes a new instance of the BankerChange struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public BankerChange(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the BankerChange struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public BankerChange(BinaryReader br) : this() {
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
			Platinum = br.ReadUInt32();
			Gold = br.ReadUInt32();
			Silver = br.ReadUInt32();
			Copper = br.ReadUInt32();
			PlatinumBank = br.ReadUInt32();
			GoldBank = br.ReadUInt32();
			SilverBank = br.ReadUInt32();
			CopperBank = br.ReadUInt32();
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
			bw.Write(Platinum);
			bw.Write(Gold);
			bw.Write(Silver);
			bw.Write(Copper);
			bw.Write(PlatinumBank);
			bw.Write(GoldBank);
			bw.Write(SilverBank);
			bw.Write(CopperBank);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct BankerChange {\n";
			ret += "	Platinum = ";
			try {
				ret += $"{ Indentify(Platinum) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Gold = ";
			try {
				ret += $"{ Indentify(Gold) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Silver = ";
			try {
				ret += $"{ Indentify(Silver) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Copper = ";
			try {
				ret += $"{ Indentify(Copper) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PlatinumBank = ";
			try {
				ret += $"{ Indentify(PlatinumBank) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	GoldBank = ";
			try {
				ret += $"{ Indentify(GoldBank) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SilverBank = ";
			try {
				ret += $"{ Indentify(SilverBank) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CopperBank = ";
			try {
				ret += $"{ Indentify(CopperBank) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}