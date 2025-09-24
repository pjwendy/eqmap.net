using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Consume_Struct
// {
// /*0000*/ uint32	slot;
// /*0004*/ uint32	auto_consumed; // 0xffffffff when auto eating e7030000 when right click
// /*0008*/ uint8	c_unknown1[4];
// /*0012*/ uint8	type; // 0x01=Food 0x02=Water
// /*0013*/ uint8	unknown13[3];
// /*0016*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_Consume)
// {
// DECODE_LENGTH_EXACT(structs::Consume_Struct);
// SETUP_DIRECT_DECODE(Consume_Struct, structs::Consume_Struct);
// 
// emu->slot = UFToServerSlot(eq->slot);
// IN(auto_consumed);
// IN(type);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Consume packet structure for EverQuest network communication.
	/// </summary>
	public struct Consume : IEQStruct {
		/// <summary>
		/// Gets or sets the slot value.
		/// </summary>
		public uint Slot { get; set; }

		/// <summary>
		/// Gets or sets the autoconsumed value.
		/// </summary>
		public uint AutoConsumed { get; set; }

		/// <summary>
		/// Gets or sets the cunknown1 value.
		/// </summary>
		public byte[] CUnknown1 { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public byte Type { get; set; }

		/// <summary>
		/// Gets or sets the unknown13 value.
		/// </summary>
		public byte[] Unknown13 { get; set; }

		/// <summary>
		/// Initializes a new instance of the Consume struct with specified field values.
		/// </summary>
		/// <param name="slot">The slot value.</param>
		/// <param name="auto_consumed">The autoconsumed value.</param>
		/// <param name="c_unknown1">The cunknown1 value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="unknown13">The unknown13 value.</param>
		public Consume(uint slot, uint auto_consumed, byte[] c_unknown1, byte type, byte[] unknown13) : this() {
			Slot = slot;
			AutoConsumed = auto_consumed;
			CUnknown1 = c_unknown1;
			Type = type;
			Unknown13 = unknown13;
		}

		/// <summary>
		/// Initializes a new instance of the Consume struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Consume(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Consume struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Consume(BinaryReader br) : this() {
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
			Slot = br.ReadUInt32();
			AutoConsumed = br.ReadUInt32();
			// TODO: Array reading for CUnknown1 - implement based on actual array size
			// CUnknown1 = new byte[size];
			Type = br.ReadByte();
			// TODO: Array reading for Unknown13 - implement based on actual array size
			// Unknown13 = new byte[size];
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
			bw.Write(Slot);
			bw.Write(AutoConsumed);
			// TODO: Array writing for CUnknown1 - implement based on actual array size
			// foreach(var item in CUnknown1) bw.Write(item);
			bw.Write(Type);
			// TODO: Array writing for Unknown13 - implement based on actual array size
			// foreach(var item in Unknown13) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Consume {\n";
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AutoConsumed = ";
			try {
				ret += $"{ Indentify(AutoConsumed) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CUnknown1 = ";
			try {
				ret += $"{ Indentify(CUnknown1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown13 = ";
			try {
				ret += $"{ Indentify(Unknown13) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}