using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct moneyOnCorpseStruct {
// /*0000*/ uint8	response;		// 0 = someone else is, 1 = OK, 2 = not at this time
// /*0001*/ uint8	unknown1;		// = 0x5a
// /*0002*/ uint8	unknown2;		// = 0x40
// /*0003*/ uint8	unknown3;		// = 0
// /*0004*/ uint32	platinum;		// Platinum Pieces
// /*0008*/ uint32	gold;			// Gold Pieces
// 
// /*0012*/ uint32	silver;			// Silver Pieces
// /*0016*/ uint32	copper;			// Copper Pieces
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MoneyOnCorpse packet structure for EverQuest network communication.
	/// </summary>
	public struct MoneyOnCorpse : IEQStruct {
		/// <summary>
		/// Gets or sets the response value.
		/// </summary>
		public byte Response { get; set; }

		/// <summary>
		/// Gets or sets the unknown1 value.
		/// </summary>
		public byte Unknown1 { get; set; }

		/// <summary>
		/// Gets or sets the unknown2 value.
		/// </summary>
		public byte Unknown2 { get; set; }

		/// <summary>
		/// Gets or sets the unknown3 value.
		/// </summary>
		public byte Unknown3 { get; set; }

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
		/// Initializes a new instance of the MoneyOnCorpse struct with specified field values.
		/// </summary>
		/// <param name="response">The response value.</param>
		/// <param name="unknown1">The unknown1 value.</param>
		/// <param name="unknown2">The unknown2 value.</param>
		/// <param name="unknown3">The unknown3 value.</param>
		/// <param name="platinum">The platinum value.</param>
		/// <param name="gold">The gold value.</param>
		/// <param name="silver">The silver value.</param>
		/// <param name="copper">The copper value.</param>
		public MoneyOnCorpse(byte response, byte unknown1, byte unknown2, byte unknown3, uint platinum, uint gold, uint silver, uint copper) : this() {
			Response = response;
			Unknown1 = unknown1;
			Unknown2 = unknown2;
			Unknown3 = unknown3;
			Platinum = platinum;
			Gold = gold;
			Silver = silver;
			Copper = copper;
		}

		/// <summary>
		/// Initializes a new instance of the MoneyOnCorpse struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MoneyOnCorpse(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MoneyOnCorpse struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MoneyOnCorpse(BinaryReader br) : this() {
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
			Response = br.ReadByte();
			Unknown1 = br.ReadByte();
			Unknown2 = br.ReadByte();
			Unknown3 = br.ReadByte();
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
			bw.Write(Response);
			bw.Write(Unknown1);
			bw.Write(Unknown2);
			bw.Write(Unknown3);
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
			var ret = "struct MoneyOnCorpse {\n";
			ret += "	Response = ";
			try {
				ret += $"{ Indentify(Response) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown1 = ";
			try {
				ret += $"{ Indentify(Unknown1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown2 = ";
			try {
				ret += $"{ Indentify(Unknown2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown3 = ";
			try {
				ret += $"{ Indentify(Unknown3) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
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