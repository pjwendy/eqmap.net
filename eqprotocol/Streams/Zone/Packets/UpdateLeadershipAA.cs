using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct UpdateLeadershipAA_Struct {
// /*00*/	uint32	ability_id;
// /*04*/	uint32	new_rank;
// /*08*/	uint32	unknown08;
// /*12*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the UpdateLeadershipAA packet structure for EverQuest network communication.
	/// </summary>
	public struct UpdateLeadershipAA : IEQStruct {
		/// <summary>
		/// Gets or sets the abilityid value.
		/// </summary>
		public uint AbilityId { get; set; }

		/// <summary>
		/// Gets or sets the newrank value.
		/// </summary>
		public uint NewRank { get; set; }

		/// <summary>
		/// Gets or sets the unknown08 value.
		/// </summary>
		public uint Unknown08 { get; set; }

		/// <summary>
		/// Initializes a new instance of the UpdateLeadershipAA struct with specified field values.
		/// </summary>
		/// <param name="ability_id">The abilityid value.</param>
		/// <param name="new_rank">The newrank value.</param>
		/// <param name="unknown08">The unknown08 value.</param>
		public UpdateLeadershipAA(uint ability_id, uint new_rank, uint unknown08) : this() {
			AbilityId = ability_id;
			NewRank = new_rank;
			Unknown08 = unknown08;
		}

		/// <summary>
		/// Initializes a new instance of the UpdateLeadershipAA struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public UpdateLeadershipAA(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the UpdateLeadershipAA struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public UpdateLeadershipAA(BinaryReader br) : this() {
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
			AbilityId = br.ReadUInt32();
			NewRank = br.ReadUInt32();
			Unknown08 = br.ReadUInt32();
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
			bw.Write(AbilityId);
			bw.Write(NewRank);
			bw.Write(Unknown08);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct UpdateLeadershipAA {\n";
			ret += "	AbilityId = ";
			try {
				ret += $"{ Indentify(AbilityId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NewRank = ";
			try {
				ret += $"{ Indentify(NewRank) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown08 = ";
			try {
				ret += $"{ Indentify(Unknown08) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}