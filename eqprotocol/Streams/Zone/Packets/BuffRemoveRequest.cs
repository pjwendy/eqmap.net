using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BuffRemoveRequest_Struct
// {
// /*00*/ uint32 SlotID;
// /*04*/ uint32 EntityID;
// /*08*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_BuffRemoveRequest)
// {
// // This is to cater for the fact that short buff box buffs start at 30 as opposed to 25 in prior clients.
// //
// DECODE_LENGTH_EXACT(structs::BuffRemoveRequest_Struct);
// SETUP_DIRECT_DECODE(BuffRemoveRequest_Struct, structs::BuffRemoveRequest_Struct);
// 
// emu->SlotID = UFToServerBuffSlot(eq->SlotID);
// 
// IN(EntityID);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the BuffRemoveRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct BuffRemoveRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the slotid value.
		/// </summary>
		public uint Slotid { get; set; }

		/// <summary>
		/// Gets or sets the entityid value.
		/// </summary>
		public uint Entityid { get; set; }

		/// <summary>
		/// Initializes a new instance of the BuffRemoveRequest struct with specified field values.
		/// </summary>
		/// <param name="SlotID">The slotid value.</param>
		/// <param name="EntityID">The entityid value.</param>
		public BuffRemoveRequest(uint SlotID, uint EntityID) : this() {
			Slotid = SlotID;
			Entityid = EntityID;
		}

		/// <summary>
		/// Initializes a new instance of the BuffRemoveRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public BuffRemoveRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the BuffRemoveRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public BuffRemoveRequest(BinaryReader br) : this() {
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
			Slotid = br.ReadUInt32();
			Entityid = br.ReadUInt32();
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
			bw.Write(Slotid);
			bw.Write(Entityid);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct BuffRemoveRequest {\n";
			ret += "	Slotid = ";
			try {
				ret += $"{ Indentify(Slotid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Entityid = ";
			try {
				ret += $"{ Indentify(Entityid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}