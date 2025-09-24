using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DeleteItem_Struct {
// /*0000*/ uint32	from_slot;
// /*0004*/ uint32	to_slot;
// /*0008*/ uint32	number_in_stack;
// /*0012*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_DeleteItem)
// {
// DECODE_LENGTH_EXACT(structs::DeleteItem_Struct);
// SETUP_DIRECT_DECODE(DeleteItem_Struct, structs::DeleteItem_Struct);
// 
// emu->from_slot = UFToServerSlot(eq->from_slot);
// emu->to_slot = UFToServerSlot(eq->to_slot);
// IN(number_in_stack);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DeleteItem packet structure for EverQuest network communication.
	/// </summary>
	public struct DeleteItem : IEQStruct {
		/// <summary>
		/// Gets or sets the fromslot value.
		/// </summary>
		public uint FromSlot { get; set; }

		/// <summary>
		/// Gets or sets the toslot value.
		/// </summary>
		public uint ToSlot { get; set; }

		/// <summary>
		/// Gets or sets the numberinstack value.
		/// </summary>
		public uint NumberInStack { get; set; }

		/// <summary>
		/// Initializes a new instance of the DeleteItem struct with specified field values.
		/// </summary>
		/// <param name="from_slot">The fromslot value.</param>
		/// <param name="to_slot">The toslot value.</param>
		/// <param name="number_in_stack">The numberinstack value.</param>
		public DeleteItem(uint from_slot, uint to_slot, uint number_in_stack) : this() {
			FromSlot = from_slot;
			ToSlot = to_slot;
			NumberInStack = number_in_stack;
		}

		/// <summary>
		/// Initializes a new instance of the DeleteItem struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DeleteItem(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DeleteItem struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DeleteItem(BinaryReader br) : this() {
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
			FromSlot = br.ReadUInt32();
			ToSlot = br.ReadUInt32();
			NumberInStack = br.ReadUInt32();
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
			bw.Write(FromSlot);
			bw.Write(ToSlot);
			bw.Write(NumberInStack);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DeleteItem {\n";
			ret += "	FromSlot = ";
			try {
				ret += $"{ Indentify(FromSlot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ToSlot = ";
			try {
				ret += $"{ Indentify(ToSlot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NumberInStack = ";
			try {
				ret += $"{ Indentify(NumberInStack) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}