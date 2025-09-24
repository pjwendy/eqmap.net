using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ItemVerifyRequest_Struct {
// /*000*/	int32	slot;		// Slot being Right Clicked
// /*004*/	uint32	target;		// Target Entity ID
// /*008*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_ItemVerifyRequest)
// {
// DECODE_LENGTH_EXACT(structs::ItemVerifyRequest_Struct);
// SETUP_DIRECT_DECODE(ItemVerifyRequest_Struct, structs::ItemVerifyRequest_Struct);
// 
// emu->slot = UFToServerSlot(eq->slot);
// IN(target);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ItemVerifyRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct ItemVerifyRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the slot value.
		/// </summary>
		public int Slot { get; set; }

		/// <summary>
		/// Gets or sets the target value.
		/// </summary>
		public uint Target { get; set; }

		/// <summary>
		/// Initializes a new instance of the ItemVerifyRequest struct with specified field values.
		/// </summary>
		/// <param name="slot">The slot value.</param>
		/// <param name="target">The target value.</param>
		public ItemVerifyRequest(int slot, uint target) : this() {
			Slot = slot;
			Target = target;
		}

		/// <summary>
		/// Initializes a new instance of the ItemVerifyRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ItemVerifyRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ItemVerifyRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ItemVerifyRequest(BinaryReader br) : this() {
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
			Slot = br.ReadInt32();
			Target = br.ReadUInt32();
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
			bw.Write(Target);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ItemVerifyRequest {\n";
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Target = ";
			try {
				ret += $"{ Indentify(Target) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}