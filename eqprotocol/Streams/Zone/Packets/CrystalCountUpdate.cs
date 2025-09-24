using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct CrystalCountUpdate_Struct
// {
// /*000*/	uint32	CurrentRadiantCrystals;
// /*004*/	uint32	CurrentEbonCrystals;
// /*008*/	uint32	CareerRadiantCrystals;
// /*012*/	uint32	CareerEbonCrystals;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the CrystalCountUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct CrystalCountUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the currentradiantcrystals value.
		/// </summary>
		public uint Currentradiantcrystals { get; set; }

		/// <summary>
		/// Gets or sets the currenteboncrystals value.
		/// </summary>
		public uint Currenteboncrystals { get; set; }

		/// <summary>
		/// Gets or sets the careerradiantcrystals value.
		/// </summary>
		public uint Careerradiantcrystals { get; set; }

		/// <summary>
		/// Gets or sets the careereboncrystals value.
		/// </summary>
		public uint Careereboncrystals { get; set; }

		/// <summary>
		/// Initializes a new instance of the CrystalCountUpdate struct with specified field values.
		/// </summary>
		/// <param name="CurrentRadiantCrystals">The currentradiantcrystals value.</param>
		/// <param name="CurrentEbonCrystals">The currenteboncrystals value.</param>
		/// <param name="CareerRadiantCrystals">The careerradiantcrystals value.</param>
		/// <param name="CareerEbonCrystals">The careereboncrystals value.</param>
		public CrystalCountUpdate(uint CurrentRadiantCrystals, uint CurrentEbonCrystals, uint CareerRadiantCrystals, uint CareerEbonCrystals) : this() {
			Currentradiantcrystals = CurrentRadiantCrystals;
			Currenteboncrystals = CurrentEbonCrystals;
			Careerradiantcrystals = CareerRadiantCrystals;
			Careereboncrystals = CareerEbonCrystals;
		}

		/// <summary>
		/// Initializes a new instance of the CrystalCountUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public CrystalCountUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the CrystalCountUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public CrystalCountUpdate(BinaryReader br) : this() {
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
			Currentradiantcrystals = br.ReadUInt32();
			Currenteboncrystals = br.ReadUInt32();
			Careerradiantcrystals = br.ReadUInt32();
			Careereboncrystals = br.ReadUInt32();
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
			bw.Write(Currentradiantcrystals);
			bw.Write(Currenteboncrystals);
			bw.Write(Careerradiantcrystals);
			bw.Write(Careereboncrystals);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct CrystalCountUpdate {\n";
			ret += "	Currentradiantcrystals = ";
			try {
				ret += $"{ Indentify(Currentradiantcrystals) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Currenteboncrystals = ";
			try {
				ret += $"{ Indentify(Currenteboncrystals) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Careerradiantcrystals = ";
			try {
				ret += $"{ Indentify(Careerradiantcrystals) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Careereboncrystals = ";
			try {
				ret += $"{ Indentify(Careereboncrystals) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}