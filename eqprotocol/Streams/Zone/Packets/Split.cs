using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Split_Struct
// {
// uint32	platinum;
// uint32	gold;
// uint32	silver;
// uint32	copper;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Split packet structure for EverQuest network communication.
	/// </summary>
	public struct Split : IEQStruct {
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
		/// Initializes a new instance of the Split struct with specified field values.
		/// </summary>
		/// <param name="platinum">The platinum value.</param>
		/// <param name="gold">The gold value.</param>
		/// <param name="silver">The silver value.</param>
		/// <param name="copper">The copper value.</param>
		public Split(uint platinum, uint gold, uint silver, uint copper) : this() {
			Platinum = platinum;
			Gold = gold;
			Silver = silver;
			Copper = copper;
		}

		/// <summary>
		/// Initializes a new instance of the Split struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Split(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Split struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Split(BinaryReader br) : this() {
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
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Split {\n";
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
			
			return ret;
		}
	}
}