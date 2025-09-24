using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AugmentItem_Struct {
// /*00*/	int16	container_slot;
// /*02*/	char	unknown02[2];
// /*04*/	int32	augment_slot;
// /*08*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_AugmentItem)
// {
// DECODE_LENGTH_EXACT(structs::AugmentItem_Struct);
// SETUP_DIRECT_DECODE(AugmentItem_Struct, structs::AugmentItem_Struct);
// 
// emu->container_slot = UFToServerSlot(eq->container_slot);
// emu->augment_slot = eq->augment_slot;
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AugmentItem packet structure for EverQuest network communication.
	/// </summary>
	public struct AugmentItem : IEQStruct {
		/// <summary>
		/// Gets or sets the containerslot value.
		/// </summary>
		public short ContainerSlot { get; set; }

		/// <summary>
		/// Gets or sets the unknown02 value.
		/// </summary>
		public byte[] Unknown02 { get; set; }

		/// <summary>
		/// Gets or sets the augmentslot value.
		/// </summary>
		public int AugmentSlot { get; set; }

		/// <summary>
		/// Initializes a new instance of the AugmentItem struct with specified field values.
		/// </summary>
		/// <param name="container_slot">The containerslot value.</param>
		/// <param name="unknown02">The unknown02 value.</param>
		/// <param name="augment_slot">The augmentslot value.</param>
		public AugmentItem(short container_slot, byte[] unknown02, int augment_slot) : this() {
			ContainerSlot = container_slot;
			Unknown02 = unknown02;
			AugmentSlot = augment_slot;
		}

		/// <summary>
		/// Initializes a new instance of the AugmentItem struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AugmentItem(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AugmentItem struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AugmentItem(BinaryReader br) : this() {
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
			ContainerSlot = br.ReadInt16();
			// TODO: Array reading for Unknown02 - implement based on actual array size
			// Unknown02 = new byte[size];
			AugmentSlot = br.ReadInt32();
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
			bw.Write(ContainerSlot);
			// TODO: Array writing for Unknown02 - implement based on actual array size
			// foreach(var item in Unknown02) bw.Write(item);
			bw.Write(AugmentSlot);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AugmentItem {\n";
			ret += "	ContainerSlot = ";
			try {
				ret += $"{ Indentify(ContainerSlot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown02 = ";
			try {
				ret += $"{ Indentify(Unknown02) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AugmentSlot = ";
			try {
				ret += $"{ Indentify(AugmentSlot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}