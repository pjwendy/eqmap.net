using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TributeAbility_Struct {
// uint32	tribute_id;	//backwards byte order!
// uint32	tier_count;	//backwards byte order!
// TributeLevel_Struct tiers[MAX_TRIBUTE_TIERS];
// char	name[0];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TributeInfo packet structure for EverQuest network communication.
	/// </summary>
	public struct TributeInfo : IEQStruct {
		/// <summary>
		/// Gets or sets the tributeid value.
		/// </summary>
		public uint TributeId { get; set; }

		/// <summary>
		/// Gets or sets the tiercount value.
		/// </summary>
		public uint TierCount { get; set; }

		/// <summary>
		/// Gets or sets the tiers value.
		/// </summary>
		public uint Tiers { get; set; }

		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte Name { get; set; }

		/// <summary>
		/// Initializes a new instance of the TributeInfo struct with specified field values.
		/// </summary>
		/// <param name="tribute_id">The tributeid value.</param>
		/// <param name="tier_count">The tiercount value.</param>
		/// <param name="tiers">The tiers value.</param>
		/// <param name="name">The name value.</param>
		public TributeInfo(uint tribute_id, uint tier_count, uint tiers, byte name) : this() {
			TributeId = tribute_id;
			TierCount = tier_count;
			Tiers = tiers;
			Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the TributeInfo struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TributeInfo(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TributeInfo struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TributeInfo(BinaryReader br) : this() {
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
			TributeId = br.ReadUInt32();
			TierCount = br.ReadUInt32();
			Tiers = br.ReadUInt32();
			Name = br.ReadByte();
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
			bw.Write(TributeId);
			bw.Write(TierCount);
			bw.Write(Tiers);
			bw.Write(Name);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TributeInfo {\n";
			ret += "	TributeId = ";
			try {
				ret += $"{ Indentify(TributeId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TierCount = ";
			try {
				ret += $"{ Indentify(TierCount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Tiers = ";
			try {
				ret += $"{ Indentify(Tiers) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}