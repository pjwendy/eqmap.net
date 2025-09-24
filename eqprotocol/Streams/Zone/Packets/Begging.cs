using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BeggingResponse_Struct
// {
// /*00*/	uint32	Unknown00;
// /*04*/	uint32	Unknown04;
// /*08*/	uint32	Unknown08;
// /*12*/	uint32	Result;	// 0 = Fail, 1 = Plat, 2 = Gold, 3 = Silver, 4 = Copper
// /*16*/	uint32	Amount;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Begging packet structure for EverQuest network communication.
	/// </summary>
	public struct Begging : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown00 value.
		/// </summary>
		public uint Unknown00 { get; set; }

		/// <summary>
		/// Gets or sets the unknown04 value.
		/// </summary>
		public uint Unknown04 { get; set; }

		/// <summary>
		/// Gets or sets the unknown08 value.
		/// </summary>
		public uint Unknown08 { get; set; }

		/// <summary>
		/// Gets or sets the result value.
		/// </summary>
		public uint Result { get; set; }

		/// <summary>
		/// Gets or sets the amount value.
		/// </summary>
		public uint Amount { get; set; }

		/// <summary>
		/// Initializes a new instance of the Begging struct with specified field values.
		/// </summary>
		/// <param name="Unknown00">The unknown00 value.</param>
		/// <param name="Unknown04">The unknown04 value.</param>
		/// <param name="Unknown08">The unknown08 value.</param>
		/// <param name="Result">The result value.</param>
		/// <param name="Amount">The amount value.</param>
		public Begging(uint Unknown00, uint Unknown04, uint Unknown08, uint Result, uint Amount) : this() {
			Unknown00 = Unknown00;
			Unknown04 = Unknown04;
			Unknown08 = Unknown08;
			Result = Result;
			Amount = Amount;
		}

		/// <summary>
		/// Initializes a new instance of the Begging struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Begging(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Begging struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Begging(BinaryReader br) : this() {
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
			Unknown00 = br.ReadUInt32();
			Unknown04 = br.ReadUInt32();
			Unknown08 = br.ReadUInt32();
			Result = br.ReadUInt32();
			Amount = br.ReadUInt32();
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
			bw.Write(Unknown00);
			bw.Write(Unknown04);
			bw.Write(Unknown08);
			bw.Write(Result);
			bw.Write(Amount);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Begging {\n";
			ret += "	Unknown00 = ";
			try {
				ret += $"{ Indentify(Unknown00) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown04 = ";
			try {
				ret += $"{ Indentify(Unknown04) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown08 = ";
			try {
				ret += $"{ Indentify(Unknown08) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Result = ";
			try {
				ret += $"{ Indentify(Result) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Amount = ";
			try {
				ret += $"{ Indentify(Amount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}