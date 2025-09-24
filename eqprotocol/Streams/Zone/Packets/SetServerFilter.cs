using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SetServerFilter_Struct {
// uint32 filters[34];		//see enum eqFilterType [31]
// };

// ENCODE/DECODE Section:
// DECODE(OP_SetServerFilter)
// {
// DECODE_LENGTH_EXACT(structs::SetServerFilter_Struct);
// SETUP_DIRECT_DECODE(SetServerFilter_Struct, structs::SetServerFilter_Struct);
// 
// int r;
// for (r = 0; r < 29; r++) {
// IN(filters[r]);
// }
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SetServerFilter packet structure for EverQuest network communication.
	/// </summary>
	public struct SetServerFilter : IEQStruct {
		/// <summary>
		/// Gets or sets the filters value.
		/// </summary>
		public uint[] Filters { get; set; }

		/// <summary>
		/// Initializes a new instance of the SetServerFilter struct with specified field values.
		/// </summary>
		/// <param name="filters">The filters value.</param>
		public SetServerFilter(uint[] filters) : this() {
			Filters = filters;
		}

		/// <summary>
		/// Initializes a new instance of the SetServerFilter struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SetServerFilter(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SetServerFilter struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SetServerFilter(BinaryReader br) : this() {
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
			// TODO: Array reading for Filters - implement based on actual array size
			// Filters = new uint[size];
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
			// TODO: Array writing for Filters - implement based on actual array size
			// foreach(var item in Filters) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SetServerFilter {\n";
			ret += "	Filters = ";
			try {
				ret += $"{ Indentify(Filters) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}