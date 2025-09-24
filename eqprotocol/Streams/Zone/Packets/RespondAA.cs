using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AATable_Struct {
// /*00*/ int32	aa_spent;	// Total AAs Spent
// /*04*/ int32	aa_assigned;	// Assigned: field in the AA window.
// /*08*/ int32	aa_spent3;	// Unknown. Same as aa_spent in observed packets.
// /*12*/ int32	unknown012;
// /*16*/ int32	unknown016;
// /*20*/ int32	unknown020;
// /*24*/ AA_Array aa_list[MAX_PP_AA_ARRAY];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_RespondAA)
// {
// SETUP_DIRECT_ENCODE(AATable_Struct, structs::AATable_Struct);
// 
// eq->aa_spent = emu->aa_spent;
// eq->aa_assigned = emu->aa_spent;
// eq->aa_spent3 = 0;
// eq->unknown012 = 0;
// eq->unknown016 = 0;
// eq->unknown020 = 0;
// 
// for (uint32 i = 0; i < MAX_PP_AA_ARRAY; ++i)
// {
// eq->aa_list[i].AA = emu->aa_list[i].AA;
// eq->aa_list[i].value = emu->aa_list[i].value;
// eq->aa_list[i].charges = emu->aa_list[i].charges;
// }
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the RespondAA packet structure for EverQuest network communication.
	/// </summary>
	public struct RespondAA : IEQStruct {
		/// <summary>
		/// Gets or sets the aaspent value.
		/// </summary>
		public int AaSpent { get; set; }

		/// <summary>
		/// Gets or sets the aaassigned value.
		/// </summary>
		public int AaAssigned { get; set; }

		/// <summary>
		/// Gets or sets the aaspent3 value.
		/// </summary>
		public int AaSpent3 { get; set; }

		/// <summary>
		/// Gets or sets the unknown012 value.
		/// </summary>
		public int Unknown012 { get; set; }

		/// <summary>
		/// Gets or sets the unknown016 value.
		/// </summary>
		public int Unknown016 { get; set; }

		/// <summary>
		/// Gets or sets the unknown020 value.
		/// </summary>
		public int Unknown020 { get; set; }

		/// <summary>
		/// Gets or sets the aalist value.
		/// </summary>
		public uint AaList { get; set; }

		/// <summary>
		/// Initializes a new instance of the RespondAA struct with specified field values.
		/// </summary>
		/// <param name="aa_spent">The aaspent value.</param>
		/// <param name="aa_assigned">The aaassigned value.</param>
		/// <param name="aa_spent3">The aaspent3 value.</param>
		/// <param name="unknown012">The unknown012 value.</param>
		/// <param name="unknown016">The unknown016 value.</param>
		/// <param name="unknown020">The unknown020 value.</param>
		/// <param name="aa_list">The aalist value.</param>
		public RespondAA(int aa_spent, int aa_assigned, int aa_spent3, int unknown012, int unknown016, int unknown020, uint aa_list) : this() {
			AaSpent = aa_spent;
			AaAssigned = aa_assigned;
			AaSpent3 = aa_spent3;
			Unknown012 = unknown012;
			Unknown016 = unknown016;
			Unknown020 = unknown020;
			AaList = aa_list;
		}

		/// <summary>
		/// Initializes a new instance of the RespondAA struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public RespondAA(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the RespondAA struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public RespondAA(BinaryReader br) : this() {
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
			AaSpent = br.ReadInt32();
			AaAssigned = br.ReadInt32();
			AaSpent3 = br.ReadInt32();
			Unknown012 = br.ReadInt32();
			Unknown016 = br.ReadInt32();
			Unknown020 = br.ReadInt32();
			AaList = br.ReadUInt32();
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
			bw.Write(AaSpent);
			bw.Write(AaAssigned);
			bw.Write(AaSpent3);
			bw.Write(Unknown012);
			bw.Write(Unknown016);
			bw.Write(Unknown020);
			bw.Write(AaList);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct RespondAA {\n";
			ret += "	AaSpent = ";
			try {
				ret += $"{ Indentify(AaSpent) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AaAssigned = ";
			try {
				ret += $"{ Indentify(AaAssigned) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AaSpent3 = ";
			try {
				ret += $"{ Indentify(AaSpent3) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown012 = ";
			try {
				ret += $"{ Indentify(Unknown012) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown016 = ";
			try {
				ret += $"{ Indentify(Unknown016) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown020 = ";
			try {
				ret += $"{ Indentify(Unknown020) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AaList = ";
			try {
				ret += $"{ Indentify(AaList) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}