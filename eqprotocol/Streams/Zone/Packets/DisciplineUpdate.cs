using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Disciplines_Struct {
// uint32 values[MAX_PP_DISCIPLINES];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_DisciplineUpdate)
// {
// ENCODE_LENGTH_EXACT(Disciplines_Struct);
// SETUP_DIRECT_ENCODE(Disciplines_Struct, structs::Disciplines_Struct);
// 
// memcpy(&eq->values, &emu->values, sizeof(Disciplines_Struct));
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DisciplineUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct DisciplineUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the values value.
		/// </summary>
		public uint Values { get; set; }

		/// <summary>
		/// Initializes a new instance of the DisciplineUpdate struct with specified field values.
		/// </summary>
		/// <param name="values">The values value.</param>
		public DisciplineUpdate(uint values) : this() {
			Values = values;
		}

		/// <summary>
		/// Initializes a new instance of the DisciplineUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DisciplineUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DisciplineUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DisciplineUpdate(BinaryReader br) : this() {
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
			Values = br.ReadUInt32();
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
			bw.Write(Values);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DisciplineUpdate {\n";
			ret += "	Values = ";
			try {
				ret += $"{ Indentify(Values) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}