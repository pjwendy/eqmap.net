using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TributeInfo_Struct {
// uint32	active;		//0 == inactive, 1 == active
// uint32	tributes[MAX_PLAYER_TRIBUTES];	//-1 == NONE
// uint32	tiers[MAX_PLAYER_TRIBUTES];		//all 00's
// uint32	tribute_master_id;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the VetClaimRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct VetClaimRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the active value.
		/// </summary>
		public uint Active { get; set; }

		/// <summary>
		/// Gets or sets the tributes value.
		/// </summary>
		public uint Tributes { get; set; }

		/// <summary>
		/// Gets or sets the tiers value.
		/// </summary>
		public uint Tiers { get; set; }

		/// <summary>
		/// Gets or sets the tributemasterid value.
		/// </summary>
		public uint TributeMasterId { get; set; }

		/// <summary>
		/// Initializes a new instance of the VetClaimRequest struct with specified field values.
		/// </summary>
		/// <param name="active">The active value.</param>
		/// <param name="tributes">The tributes value.</param>
		/// <param name="tiers">The tiers value.</param>
		/// <param name="tribute_master_id">The tributemasterid value.</param>
		public VetClaimRequest(uint active, uint tributes, uint tiers, uint tribute_master_id) : this() {
			Active = active;
			Tributes = tributes;
			Tiers = tiers;
			TributeMasterId = tribute_master_id;
		}

		/// <summary>
		/// Initializes a new instance of the VetClaimRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public VetClaimRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the VetClaimRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public VetClaimRequest(BinaryReader br) : this() {
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
			Active = br.ReadUInt32();
			Tributes = br.ReadUInt32();
			Tiers = br.ReadUInt32();
			TributeMasterId = br.ReadUInt32();
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
			bw.Write(Active);
			bw.Write(Tributes);
			bw.Write(Tiers);
			bw.Write(TributeMasterId);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct VetClaimRequest {\n";
			ret += "	Active = ";
			try {
				ret += $"{ Indentify(Active) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Tributes = ";
			try {
				ret += $"{ Indentify(Tributes) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Tiers = ";
			try {
				ret += $"{ Indentify(Tiers) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeMasterId = ";
			try {
				ret += $"{ Indentify(TributeMasterId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}