using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct IncreaseStat_Struct{
// /*0000*/	uint8	unknown0;
// /*0001*/	uint8	str;
// /*0002*/	uint8	sta;
// /*0003*/	uint8	agi;
// /*0004*/	uint8	dex;
// /*0005*/	uint8	int_;
// /*0006*/	uint8	wis;
// /*0007*/	uint8	cha;
// /*0008*/	uint8	fire;
// /*0009*/	uint8	cold;
// /*0010*/	uint8	magic;
// /*0011*/	uint8	poison;
// /*0012*/	uint8	disease;
// /*0013*/	char	unknown13[116];
// /*0129*/	uint8	str2;
// /*0130*/	uint8	sta2;
// /*0131*/	uint8	agi2;
// /*0132*/	uint8	dex2;
// /*0133*/	uint8	int_2;
// /*0134*/	uint8	wis2;
// /*0135*/	uint8	cha2;
// /*0136*/	uint8	fire2;
// /*0137*/	uint8	cold2;
// /*0138*/	uint8	magic2;
// /*0139*/	uint8	poison2;
// /*0140*/	uint8	disease2;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the IncreaseStats packet structure for EverQuest network communication.
	/// </summary>
	public struct IncreaseStats : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown0 value.
		/// </summary>
		public byte Unknown0 { get; set; }

		/// <summary>
		/// Gets or sets the str value.
		/// </summary>
		public byte Str { get; set; }

		/// <summary>
		/// Gets or sets the sta value.
		/// </summary>
		public byte Sta { get; set; }

		/// <summary>
		/// Gets or sets the agi value.
		/// </summary>
		public byte Agi { get; set; }

		/// <summary>
		/// Gets or sets the dex value.
		/// </summary>
		public byte Dex { get; set; }

		/// <summary>
		/// Gets or sets the int value.
		/// </summary>
		public byte Int { get; set; }

		/// <summary>
		/// Gets or sets the wis value.
		/// </summary>
		public byte Wis { get; set; }

		/// <summary>
		/// Gets or sets the cha value.
		/// </summary>
		public byte Cha { get; set; }

		/// <summary>
		/// Gets or sets the fire value.
		/// </summary>
		public byte Fire { get; set; }

		/// <summary>
		/// Gets or sets the cold value.
		/// </summary>
		public byte Cold { get; set; }

		/// <summary>
		/// Gets or sets the magic value.
		/// </summary>
		public byte Magic { get; set; }

		/// <summary>
		/// Gets or sets the poison value.
		/// </summary>
		public byte Poison { get; set; }

		/// <summary>
		/// Gets or sets the disease value.
		/// </summary>
		public byte Disease { get; set; }

		/// <summary>
		/// Gets or sets the unknown13 value.
		/// </summary>
		public byte[] Unknown13 { get; set; }

		/// <summary>
		/// Gets or sets the str2 value.
		/// </summary>
		public byte Str2 { get; set; }

		/// <summary>
		/// Gets or sets the sta2 value.
		/// </summary>
		public byte Sta2 { get; set; }

		/// <summary>
		/// Gets or sets the agi2 value.
		/// </summary>
		public byte Agi2 { get; set; }

		/// <summary>
		/// Gets or sets the dex2 value.
		/// </summary>
		public byte Dex2 { get; set; }

		/// <summary>
		/// Gets or sets the int2 value.
		/// </summary>
		public byte Int2 { get; set; }

		/// <summary>
		/// Gets or sets the wis2 value.
		/// </summary>
		public byte Wis2 { get; set; }

		/// <summary>
		/// Gets or sets the cha2 value.
		/// </summary>
		public byte Cha2 { get; set; }

		/// <summary>
		/// Gets or sets the fire2 value.
		/// </summary>
		public byte Fire2 { get; set; }

		/// <summary>
		/// Gets or sets the cold2 value.
		/// </summary>
		public byte Cold2 { get; set; }

		/// <summary>
		/// Gets or sets the magic2 value.
		/// </summary>
		public byte Magic2 { get; set; }

		/// <summary>
		/// Gets or sets the poison2 value.
		/// </summary>
		public byte Poison2 { get; set; }

		/// <summary>
		/// Gets or sets the disease2 value.
		/// </summary>
		public byte Disease2 { get; set; }

		/// <summary>
		/// Initializes a new instance of the IncreaseStats struct with specified field values.
		/// </summary>
		/// <param name="unknown0">The unknown0 value.</param>
		/// <param name="str">The str value.</param>
		/// <param name="sta">The sta value.</param>
		/// <param name="agi">The agi value.</param>
		/// <param name="dex">The dex value.</param>
		/// <param name="int_">The int value.</param>
		/// <param name="wis">The wis value.</param>
		/// <param name="cha">The cha value.</param>
		/// <param name="fire">The fire value.</param>
		/// <param name="cold">The cold value.</param>
		/// <param name="magic">The magic value.</param>
		/// <param name="poison">The poison value.</param>
		/// <param name="disease">The disease value.</param>
		/// <param name="unknown13">The unknown13 value.</param>
		/// <param name="str2">The str2 value.</param>
		/// <param name="sta2">The sta2 value.</param>
		/// <param name="agi2">The agi2 value.</param>
		/// <param name="dex2">The dex2 value.</param>
		/// <param name="int_2">The int2 value.</param>
		/// <param name="wis2">The wis2 value.</param>
		/// <param name="cha2">The cha2 value.</param>
		/// <param name="fire2">The fire2 value.</param>
		/// <param name="cold2">The cold2 value.</param>
		/// <param name="magic2">The magic2 value.</param>
		/// <param name="poison2">The poison2 value.</param>
		/// <param name="disease2">The disease2 value.</param>
		public IncreaseStats(byte unknown0, byte str, byte sta, byte agi, byte dex, byte int_, byte wis, byte cha, byte fire, byte cold, byte magic, byte poison, byte disease, byte[] unknown13, byte str2, byte sta2, byte agi2, byte dex2, byte int_2, byte wis2, byte cha2, byte fire2, byte cold2, byte magic2, byte poison2, byte disease2) : this() {
			Unknown0 = unknown0;
			Str = str;
			Sta = sta;
			Agi = agi;
			Dex = dex;
			Int = int_;
			Wis = wis;
			Cha = cha;
			Fire = fire;
			Cold = cold;
			Magic = magic;
			Poison = poison;
			Disease = disease;
			Unknown13 = unknown13;
			Str2 = str2;
			Sta2 = sta2;
			Agi2 = agi2;
			Dex2 = dex2;
			Int2 = int_2;
			Wis2 = wis2;
			Cha2 = cha2;
			Fire2 = fire2;
			Cold2 = cold2;
			Magic2 = magic2;
			Poison2 = poison2;
			Disease2 = disease2;
		}

		/// <summary>
		/// Initializes a new instance of the IncreaseStats struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public IncreaseStats(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the IncreaseStats struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public IncreaseStats(BinaryReader br) : this() {
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
			Unknown0 = br.ReadByte();
			Str = br.ReadByte();
			Sta = br.ReadByte();
			Agi = br.ReadByte();
			Dex = br.ReadByte();
			Int = br.ReadByte();
			Wis = br.ReadByte();
			Cha = br.ReadByte();
			Fire = br.ReadByte();
			Cold = br.ReadByte();
			Magic = br.ReadByte();
			Poison = br.ReadByte();
			Disease = br.ReadByte();
			// TODO: Array reading for Unknown13 - implement based on actual array size
			// Unknown13 = new byte[size];
			Str2 = br.ReadByte();
			Sta2 = br.ReadByte();
			Agi2 = br.ReadByte();
			Dex2 = br.ReadByte();
			Int2 = br.ReadByte();
			Wis2 = br.ReadByte();
			Cha2 = br.ReadByte();
			Fire2 = br.ReadByte();
			Cold2 = br.ReadByte();
			Magic2 = br.ReadByte();
			Poison2 = br.ReadByte();
			Disease2 = br.ReadByte();
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
			bw.Write(Unknown0);
			bw.Write(Str);
			bw.Write(Sta);
			bw.Write(Agi);
			bw.Write(Dex);
			bw.Write(Int);
			bw.Write(Wis);
			bw.Write(Cha);
			bw.Write(Fire);
			bw.Write(Cold);
			bw.Write(Magic);
			bw.Write(Poison);
			bw.Write(Disease);
			// TODO: Array writing for Unknown13 - implement based on actual array size
			// foreach(var item in Unknown13) bw.Write(item);
			bw.Write(Str2);
			bw.Write(Sta2);
			bw.Write(Agi2);
			bw.Write(Dex2);
			bw.Write(Int2);
			bw.Write(Wis2);
			bw.Write(Cha2);
			bw.Write(Fire2);
			bw.Write(Cold2);
			bw.Write(Magic2);
			bw.Write(Poison2);
			bw.Write(Disease2);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct IncreaseStats {\n";
			ret += "	Unknown0 = ";
			try {
				ret += $"{ Indentify(Unknown0) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Str = ";
			try {
				ret += $"{ Indentify(Str) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Sta = ";
			try {
				ret += $"{ Indentify(Sta) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Agi = ";
			try {
				ret += $"{ Indentify(Agi) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Dex = ";
			try {
				ret += $"{ Indentify(Dex) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Int = ";
			try {
				ret += $"{ Indentify(Int) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Wis = ";
			try {
				ret += $"{ Indentify(Wis) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Cha = ";
			try {
				ret += $"{ Indentify(Cha) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Fire = ";
			try {
				ret += $"{ Indentify(Fire) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Cold = ";
			try {
				ret += $"{ Indentify(Cold) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Magic = ";
			try {
				ret += $"{ Indentify(Magic) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Poison = ";
			try {
				ret += $"{ Indentify(Poison) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Disease = ";
			try {
				ret += $"{ Indentify(Disease) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown13 = ";
			try {
				ret += $"{ Indentify(Unknown13) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Str2 = ";
			try {
				ret += $"{ Indentify(Str2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Sta2 = ";
			try {
				ret += $"{ Indentify(Sta2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Agi2 = ";
			try {
				ret += $"{ Indentify(Agi2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Dex2 = ";
			try {
				ret += $"{ Indentify(Dex2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Int2 = ";
			try {
				ret += $"{ Indentify(Int2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Wis2 = ";
			try {
				ret += $"{ Indentify(Wis2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Cha2 = ";
			try {
				ret += $"{ Indentify(Cha2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Fire2 = ";
			try {
				ret += $"{ Indentify(Fire2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Cold2 = ";
			try {
				ret += $"{ Indentify(Cold2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Magic2 = ";
			try {
				ret += $"{ Indentify(Magic2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Poison2 = ";
			try {
				ret += $"{ Indentify(Poison2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Disease2 = ";
			try {
				ret += $"{ Indentify(Disease2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}