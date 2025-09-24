using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ApplyPoison_Struct {
// uint32 inventorySlot;
// uint32 success;
// };

// ENCODE/DECODE Section:
// DECODE(OP_ApplyPoison)
// {
// DECODE_LENGTH_EXACT(structs::ApplyPoison_Struct);
// SETUP_DIRECT_DECODE(ApplyPoison_Struct, structs::ApplyPoison_Struct);
// 
// emu->inventorySlot = UFToServerSlot(eq->inventorySlot);
// IN(success);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ApplyPoison packet structure for EverQuest network communication.
	/// </summary>
	public struct ApplyPoison : IEQStruct {
		/// <summary>
		/// Gets or sets the inventoryslot value.
		/// </summary>
		public uint Inventoryslot { get; set; }

		/// <summary>
		/// Gets or sets the success value.
		/// </summary>
		public uint Success { get; set; }

		/// <summary>
		/// Initializes a new instance of the ApplyPoison struct with specified field values.
		/// </summary>
		/// <param name="inventorySlot">The inventoryslot value.</param>
		/// <param name="success">The success value.</param>
		public ApplyPoison(uint inventorySlot, uint success) : this() {
			Inventoryslot = inventorySlot;
			Success = success;
		}

		/// <summary>
		/// Initializes a new instance of the ApplyPoison struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ApplyPoison(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ApplyPoison struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ApplyPoison(BinaryReader br) : this() {
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
			Inventoryslot = br.ReadUInt32();
			Success = br.ReadUInt32();
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
			bw.Write(Inventoryslot);
			bw.Write(Success);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ApplyPoison {\n";
			ret += "	Inventoryslot = ";
			try {
				ret += $"{ Indentify(Inventoryslot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Success = ";
			try {
				ret += $"{ Indentify(Success) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}