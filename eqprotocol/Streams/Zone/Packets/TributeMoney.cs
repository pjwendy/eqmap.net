using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TributeMoney_Struct {
// uint32   platinum;
// uint32   tribute_master_id;
// int32   tribute_points;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TributeMoney packet structure for EverQuest network communication.
	/// </summary>
	public struct TributeMoney : IEQStruct {
		/// <summary>
		/// Gets or sets the platinum value.
		/// </summary>
		public uint Platinum { get; set; }

		/// <summary>
		/// Gets or sets the tributemasterid value.
		/// </summary>
		public uint TributeMasterId { get; set; }

		/// <summary>
		/// Gets or sets the tributepoints value.
		/// </summary>
		public int TributePoints { get; set; }

		/// <summary>
		/// Initializes a new instance of the TributeMoney struct with specified field values.
		/// </summary>
		/// <param name="platinum">The platinum value.</param>
		/// <param name="tribute_master_id">The tributemasterid value.</param>
		/// <param name="tribute_points">The tributepoints value.</param>
		public TributeMoney(uint platinum, uint tribute_master_id, int tribute_points) : this() {
			Platinum = platinum;
			TributeMasterId = tribute_master_id;
			TributePoints = tribute_points;
		}

		/// <summary>
		/// Initializes a new instance of the TributeMoney struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TributeMoney(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TributeMoney struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TributeMoney(BinaryReader br) : this() {
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
			Platinum = br.ReadUInt32();
			TributeMasterId = br.ReadUInt32();
			TributePoints = br.ReadInt32();
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
			bw.Write(Platinum);
			bw.Write(TributeMasterId);
			bw.Write(TributePoints);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TributeMoney {\n";
			ret += "	Platinum = ";
			try {
				ret += $"{ Indentify(Platinum) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeMasterId = ";
			try {
				ret += $"{ Indentify(TributeMasterId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributePoints = ";
			try {
				ret += $"{ Indentify(TributePoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}