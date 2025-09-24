using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TradeBusy_Struct {
// /*00*/	uint32 to_mob_id;
// /*04*/	uint32 from_mob_id;
// /*08*/	uint8 type;			// Seen 01
// /*09*/	uint8 unknown09;	// Seen EF (239)
// /*10*/	uint8 unknown10;	// Seen FF (255)
// /*11*/	uint8 unknown11;	// Seen FF (255)
// /*12*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TradeBusy packet structure for EverQuest network communication.
	/// </summary>
	public struct TradeBusy : IEQStruct {
		/// <summary>
		/// Gets or sets the tomobid value.
		/// </summary>
		public uint ToMobId { get; set; }

		/// <summary>
		/// Gets or sets the frommobid value.
		/// </summary>
		public uint FromMobId { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public byte Type { get; set; }

		/// <summary>
		/// Gets or sets the unknown09 value.
		/// </summary>
		public byte Unknown09 { get; set; }

		/// <summary>
		/// Gets or sets the unknown10 value.
		/// </summary>
		public byte Unknown10 { get; set; }

		/// <summary>
		/// Gets or sets the unknown11 value.
		/// </summary>
		public byte Unknown11 { get; set; }

		/// <summary>
		/// Initializes a new instance of the TradeBusy struct with specified field values.
		/// </summary>
		/// <param name="to_mob_id">The tomobid value.</param>
		/// <param name="from_mob_id">The frommobid value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="unknown09">The unknown09 value.</param>
		/// <param name="unknown10">The unknown10 value.</param>
		/// <param name="unknown11">The unknown11 value.</param>
		public TradeBusy(uint to_mob_id, uint from_mob_id, byte type, byte unknown09, byte unknown10, byte unknown11) : this() {
			ToMobId = to_mob_id;
			FromMobId = from_mob_id;
			Type = type;
			Unknown09 = unknown09;
			Unknown10 = unknown10;
			Unknown11 = unknown11;
		}

		/// <summary>
		/// Initializes a new instance of the TradeBusy struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TradeBusy(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TradeBusy struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TradeBusy(BinaryReader br) : this() {
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
			ToMobId = br.ReadUInt32();
			FromMobId = br.ReadUInt32();
			Type = br.ReadByte();
			Unknown09 = br.ReadByte();
			Unknown10 = br.ReadByte();
			Unknown11 = br.ReadByte();
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
			bw.Write(ToMobId);
			bw.Write(FromMobId);
			bw.Write(Type);
			bw.Write(Unknown09);
			bw.Write(Unknown10);
			bw.Write(Unknown11);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TradeBusy {\n";
			ret += "	ToMobId = ";
			try {
				ret += $"{ Indentify(ToMobId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FromMobId = ";
			try {
				ret += $"{ Indentify(FromMobId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown09 = ";
			try {
				ret += $"{ Indentify(Unknown09) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown10 = ";
			try {
				ret += $"{ Indentify(Unknown10) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown11 = ";
			try {
				ret += $"{ Indentify(Unknown11) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}