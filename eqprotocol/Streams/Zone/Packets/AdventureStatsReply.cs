using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AdventureStats_Struct
// {
// /*000*/ AdventureStatsColumn_Struct success;
// /*024*/ AdventureStatsColumn_Struct failure;
// /*048*/	AdventureStatsColumn_Struct rank;
// /*072*/	AdventureStatsColumn_Struct rank2;
// /*096*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AdventureStatsReply packet structure for EverQuest network communication.
	/// </summary>
	public struct AdventureStatsReply : IEQStruct {
		/// <summary>
		/// Gets or sets the success value.
		/// </summary>
		public uint Success { get; set; }

		/// <summary>
		/// Gets or sets the failure value.
		/// </summary>
		public uint Failure { get; set; }

		/// <summary>
		/// Gets or sets the rank value.
		/// </summary>
		public uint Rank { get; set; }

		/// <summary>
		/// Gets or sets the rank2 value.
		/// </summary>
		public uint Rank2 { get; set; }

		/// <summary>
		/// Initializes a new instance of the AdventureStatsReply struct with specified field values.
		/// </summary>
		/// <param name="success">The success value.</param>
		/// <param name="failure">The failure value.</param>
		/// <param name="rank">The rank value.</param>
		/// <param name="rank2">The rank2 value.</param>
		public AdventureStatsReply(uint success, uint failure, uint rank, uint rank2) : this() {
			Success = success;
			Failure = failure;
			Rank = rank;
			Rank2 = rank2;
		}

		/// <summary>
		/// Initializes a new instance of the AdventureStatsReply struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AdventureStatsReply(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AdventureStatsReply struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AdventureStatsReply(BinaryReader br) : this() {
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
			Success = br.ReadUInt32();
			Failure = br.ReadUInt32();
			Rank = br.ReadUInt32();
			Rank2 = br.ReadUInt32();
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
			bw.Write(Success);
			bw.Write(Failure);
			bw.Write(Rank);
			bw.Write(Rank2);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AdventureStatsReply {\n";
			ret += "	Success = ";
			try {
				ret += $"{ Indentify(Success) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Failure = ";
			try {
				ret += $"{ Indentify(Failure) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Rank = ";
			try {
				ret += $"{ Indentify(Rank) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Rank2 = ";
			try {
				ret += $"{ Indentify(Rank2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}