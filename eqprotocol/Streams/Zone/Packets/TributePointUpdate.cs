using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TributePoint_Struct {
// int32   tribute_points;
// uint32   unknown04;
// int32   career_tribute_points;
// uint32   unknown12;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TributePointUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct TributePointUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the tributepoints value.
		/// </summary>
		public int TributePoints { get; set; }

		/// <summary>
		/// Gets or sets the unknown04 value.
		/// </summary>
		public uint Unknown04 { get; set; }

		/// <summary>
		/// Gets or sets the careertributepoints value.
		/// </summary>
		public int CareerTributePoints { get; set; }

		/// <summary>
		/// Gets or sets the unknown12 value.
		/// </summary>
		public uint Unknown12 { get; set; }

		/// <summary>
		/// Initializes a new instance of the TributePointUpdate struct with specified field values.
		/// </summary>
		/// <param name="tribute_points">The tributepoints value.</param>
		/// <param name="unknown04">The unknown04 value.</param>
		/// <param name="career_tribute_points">The careertributepoints value.</param>
		/// <param name="unknown12">The unknown12 value.</param>
		public TributePointUpdate(int tribute_points, uint unknown04, int career_tribute_points, uint unknown12) : this() {
			TributePoints = tribute_points;
			Unknown04 = unknown04;
			CareerTributePoints = career_tribute_points;
			Unknown12 = unknown12;
		}

		/// <summary>
		/// Initializes a new instance of the TributePointUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TributePointUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TributePointUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TributePointUpdate(BinaryReader br) : this() {
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
			TributePoints = br.ReadInt32();
			Unknown04 = br.ReadUInt32();
			CareerTributePoints = br.ReadInt32();
			Unknown12 = br.ReadUInt32();
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
			bw.Write(TributePoints);
			bw.Write(Unknown04);
			bw.Write(CareerTributePoints);
			bw.Write(Unknown12);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TributePointUpdate {\n";
			ret += "	TributePoints = ";
			try {
				ret += $"{ Indentify(TributePoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown04 = ";
			try {
				ret += $"{ Indentify(Unknown04) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CareerTributePoints = ";
			try {
				ret += $"{ Indentify(CareerTributePoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown12 = ";
			try {
				ret += $"{ Indentify(Unknown12) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}