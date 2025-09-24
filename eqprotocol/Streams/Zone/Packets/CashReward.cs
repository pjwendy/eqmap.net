using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct CashReward_Struct
// {
// /*000*/ uint32 copper;
// /*004*/ uint32 silver;
// /*008*/ uint32 gold;
// /*012*/ uint32 platinum;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the CashReward packet structure for EverQuest network communication.
	/// </summary>
	public struct CashReward : IEQStruct {
		/// <summary>
		/// Gets or sets the copper value.
		/// </summary>
		public uint Copper { get; set; }

		/// <summary>
		/// Gets or sets the silver value.
		/// </summary>
		public uint Silver { get; set; }

		/// <summary>
		/// Gets or sets the gold value.
		/// </summary>
		public uint Gold { get; set; }

		/// <summary>
		/// Gets or sets the platinum value.
		/// </summary>
		public uint Platinum { get; set; }

		/// <summary>
		/// Initializes a new instance of the CashReward struct with specified field values.
		/// </summary>
		/// <param name="copper">The copper value.</param>
		/// <param name="silver">The silver value.</param>
		/// <param name="gold">The gold value.</param>
		/// <param name="platinum">The platinum value.</param>
		public CashReward(uint copper, uint silver, uint gold, uint platinum) : this() {
			Copper = copper;
			Silver = silver;
			Gold = gold;
			Platinum = platinum;
		}

		/// <summary>
		/// Initializes a new instance of the CashReward struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public CashReward(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the CashReward struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public CashReward(BinaryReader br) : this() {
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
			Copper = br.ReadUInt32();
			Silver = br.ReadUInt32();
			Gold = br.ReadUInt32();
			Platinum = br.ReadUInt32();
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
			bw.Write(Copper);
			bw.Write(Silver);
			bw.Write(Gold);
			bw.Write(Platinum);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct CashReward {\n";
			ret += "	Copper = ";
			try {
				ret += $"{ Indentify(Copper) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Silver = ";
			try {
				ret += $"{ Indentify(Silver) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Gold = ";
			try {
				ret += $"{ Indentify(Gold) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Platinum = ";
			try {
				ret += $"{ Indentify(Platinum) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}