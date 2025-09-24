using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AdventurePoints_Update_Struct {
// /*000*/	uint32				ldon_available_points;		// Total available points
// /*004*/ uint8				unkown_apu004[20];
// /*024*/	uint32				ldon_guk_points;		// Earned Deepest Guk points
// /*028*/	uint32				ldon_mirugal_points;		// Earned Mirugal' Mebagerie points
// /*032*/	uint32				ldon_mistmoore_points;		// Earned Mismoore Catacombs Points
// /*036*/	uint32				ldon_rujarkian_points;		// Earned Rujarkian Hills points
// /*040*/	uint32				ldon_takish_points;		// Earned Takish points
// /*044*/	uint8				unknown_apu042[216];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AdventurePointsUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct AdventurePointsUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the ldonavailablepoints value.
		/// </summary>
		public uint LdonAvailablePoints { get; set; }

		/// <summary>
		/// Gets or sets the unkownapu004 value.
		/// </summary>
		public byte[] UnkownApu004 { get; set; }

		/// <summary>
		/// Gets or sets the ldongukpoints value.
		/// </summary>
		public uint LdonGukPoints { get; set; }

		/// <summary>
		/// Gets or sets the ldonmirugalpoints value.
		/// </summary>
		public uint LdonMirugalPoints { get; set; }

		/// <summary>
		/// Gets or sets the ldonmistmoorepoints value.
		/// </summary>
		public uint LdonMistmoorePoints { get; set; }

		/// <summary>
		/// Gets or sets the ldonrujarkianpoints value.
		/// </summary>
		public uint LdonRujarkianPoints { get; set; }

		/// <summary>
		/// Gets or sets the ldontakishpoints value.
		/// </summary>
		public uint LdonTakishPoints { get; set; }

		/// <summary>
		/// Initializes a new instance of the AdventurePointsUpdate struct with specified field values.
		/// </summary>
		/// <param name="ldon_available_points">The ldonavailablepoints value.</param>
		/// <param name="unkown_apu004">The unkownapu004 value.</param>
		/// <param name="ldon_guk_points">The ldongukpoints value.</param>
		/// <param name="ldon_mirugal_points">The ldonmirugalpoints value.</param>
		/// <param name="ldon_mistmoore_points">The ldonmistmoorepoints value.</param>
		/// <param name="ldon_rujarkian_points">The ldonrujarkianpoints value.</param>
		/// <param name="ldon_takish_points">The ldontakishpoints value.</param>
		public AdventurePointsUpdate(uint ldon_available_points, byte[] unkown_apu004, uint ldon_guk_points, uint ldon_mirugal_points, uint ldon_mistmoore_points, uint ldon_rujarkian_points, uint ldon_takish_points) : this() {
			LdonAvailablePoints = ldon_available_points;
			UnkownApu004 = unkown_apu004;
			LdonGukPoints = ldon_guk_points;
			LdonMirugalPoints = ldon_mirugal_points;
			LdonMistmoorePoints = ldon_mistmoore_points;
			LdonRujarkianPoints = ldon_rujarkian_points;
			LdonTakishPoints = ldon_takish_points;
		}

		/// <summary>
		/// Initializes a new instance of the AdventurePointsUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AdventurePointsUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AdventurePointsUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AdventurePointsUpdate(BinaryReader br) : this() {
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
			LdonAvailablePoints = br.ReadUInt32();
			// TODO: Array reading for UnkownApu004 - implement based on actual array size
			// UnkownApu004 = new byte[size];
			LdonGukPoints = br.ReadUInt32();
			LdonMirugalPoints = br.ReadUInt32();
			LdonMistmoorePoints = br.ReadUInt32();
			LdonRujarkianPoints = br.ReadUInt32();
			LdonTakishPoints = br.ReadUInt32();
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
			bw.Write(LdonAvailablePoints);
			// TODO: Array writing for UnkownApu004 - implement based on actual array size
			// foreach(var item in UnkownApu004) bw.Write(item);
			bw.Write(LdonGukPoints);
			bw.Write(LdonMirugalPoints);
			bw.Write(LdonMistmoorePoints);
			bw.Write(LdonRujarkianPoints);
			bw.Write(LdonTakishPoints);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AdventurePointsUpdate {\n";
			ret += "	LdonAvailablePoints = ";
			try {
				ret += $"{ Indentify(LdonAvailablePoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	UnkownApu004 = ";
			try {
				ret += $"{ Indentify(UnkownApu004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LdonGukPoints = ";
			try {
				ret += $"{ Indentify(LdonGukPoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LdonMirugalPoints = ";
			try {
				ret += $"{ Indentify(LdonMirugalPoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LdonMistmoorePoints = ";
			try {
				ret += $"{ Indentify(LdonMistmoorePoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LdonRujarkianPoints = ";
			try {
				ret += $"{ Indentify(LdonRujarkianPoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LdonTakishPoints = ";
			try {
				ret += $"{ Indentify(LdonTakishPoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}