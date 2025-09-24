using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct sPickPocket_Struct {
// // Size 28 = coin/fail
// uint32 to;
// uint32 from;
// uint32 myskill;
// uint32 type;
// uint32 coin;
// char itemname[64];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the PickPocket packet structure for EverQuest network communication.
	/// </summary>
	public struct PickPocket : IEQStruct {
		/// <summary>
		/// Gets or sets the to value.
		/// </summary>
		public uint To { get; set; }

		/// <summary>
		/// Gets or sets the from value.
		/// </summary>
		public uint From { get; set; }

		/// <summary>
		/// Gets or sets the myskill value.
		/// </summary>
		public uint Myskill { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Gets or sets the coin value.
		/// </summary>
		public uint Coin { get; set; }

		/// <summary>
		/// Gets or sets the itemname value.
		/// </summary>
		public byte[] Itemname { get; set; }

		/// <summary>
		/// Initializes a new instance of the PickPocket struct with specified field values.
		/// </summary>
		/// <param name="to">The to value.</param>
		/// <param name="from">The from value.</param>
		/// <param name="myskill">The myskill value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="coin">The coin value.</param>
		/// <param name="itemname">The itemname value.</param>
		public PickPocket(uint to, uint from, uint myskill, uint type, uint coin, byte[] itemname) : this() {
			To = to;
			From = from;
			Myskill = myskill;
			Type = type;
			Coin = coin;
			Itemname = itemname;
		}

		/// <summary>
		/// Initializes a new instance of the PickPocket struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public PickPocket(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the PickPocket struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public PickPocket(BinaryReader br) : this() {
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
			To = br.ReadUInt32();
			From = br.ReadUInt32();
			Myskill = br.ReadUInt32();
			Type = br.ReadUInt32();
			Coin = br.ReadUInt32();
			// TODO: Array reading for Itemname - implement based on actual array size
			// Itemname = new byte[size];
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
			bw.Write(To);
			bw.Write(From);
			bw.Write(Myskill);
			bw.Write(Type);
			bw.Write(Coin);
			// TODO: Array writing for Itemname - implement based on actual array size
			// foreach(var item in Itemname) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct PickPocket {\n";
			ret += "	To = ";
			try {
				ret += $"{ Indentify(To) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	From = ";
			try {
				ret += $"{ Indentify(From) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Myskill = ";
			try {
				ret += $"{ Indentify(Myskill) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Coin = ";
			try {
				ret += $"{ Indentify(Coin) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Itemname = ";
			try {
				ret += $"{ Indentify(Itemname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}