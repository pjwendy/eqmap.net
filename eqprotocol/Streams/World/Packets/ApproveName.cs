using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct CharCreate_Struct
// {
// /*0000*/	uint32	class_;
// /*0004*/	uint32	haircolor;
// /*0008*/	uint32	beard;
// /*0012*/	uint32	beardcolor;
// /*0016*/	uint32	gender;
// /*0020*/	uint32	race;
// /*0024*/	uint32	start_zone;
// /*0028*/	uint32	hairstyle;
// /*0032*/	uint32	deity;
// /*0036*/	uint32	STR;
// /*0040*/	uint32	STA;
// /*0044*/	uint32	AGI;
// /*0048*/	uint32	DEX;
// /*0052*/	uint32	WIS;
// /*0056*/	uint32	INT;
// /*0060*/	uint32	CHA;
// /*0064*/	uint32	face;		// Could be unknown0076
// /*0068*/	uint32	eyecolor1;	//its possiable we could have these switched
// /*0073*/	uint32	eyecolor2;	//since setting one sets the other we really can't check
// /*0076*/	uint32	tutorial;
// /*0080*/	uint32	drakkin_heritage;
// /*0084*/	uint32	drakkin_tattoo;
// /*0088*/	uint32	drakkin_details;
// /*0092*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ApproveName packet structure for EverQuest network communication.
	/// </summary>
	public struct ApproveName : IEQStruct {
		/// <summary>
		/// Gets or sets the class value.
		/// </summary>
		public uint Class { get; set; }

		/// <summary>
		/// Gets or sets the haircolor value.
		/// </summary>
		public uint Haircolor { get; set; }

		/// <summary>
		/// Gets or sets the beard value.
		/// </summary>
		public uint Beard { get; set; }

		/// <summary>
		/// Gets or sets the beardcolor value.
		/// </summary>
		public uint Beardcolor { get; set; }

		/// <summary>
		/// Gets or sets the gender value.
		/// </summary>
		public uint Gender { get; set; }

		/// <summary>
		/// Gets or sets the race value.
		/// </summary>
		public uint Race { get; set; }

		/// <summary>
		/// Gets or sets the startzone value.
		/// </summary>
		public uint StartZone { get; set; }

		/// <summary>
		/// Gets or sets the hairstyle value.
		/// </summary>
		public uint Hairstyle { get; set; }

		/// <summary>
		/// Gets or sets the deity value.
		/// </summary>
		public uint Deity { get; set; }

		/// <summary>
		/// Gets or sets the str value.
		/// </summary>
		public uint STR { get; set; }

		/// <summary>
		/// Gets or sets the sta value.
		/// </summary>
		public uint STA { get; set; }

		/// <summary>
		/// Gets or sets the agi value.
		/// </summary>
		public uint AGI { get; set; }

		/// <summary>
		/// Gets or sets the dex value.
		/// </summary>
		public uint DEX { get; set; }

		/// <summary>
		/// Gets or sets the wis value.
		/// </summary>
		public uint WIS { get; set; }

		/// <summary>
		/// Gets or sets the int value.
		/// </summary>
		public uint INT { get; set; }

		/// <summary>
		/// Gets or sets the cha value.
		/// </summary>
		public uint CHA { get; set; }

		/// <summary>
		/// Gets or sets the face value.
		/// </summary>
		public uint Face { get; set; }

		/// <summary>
		/// Gets or sets the eyecolor1 value.
		/// </summary>
		public uint Eyecolor1 { get; set; }

		/// <summary>
		/// Gets or sets the eyecolor2 value.
		/// </summary>
		public uint Eyecolor2 { get; set; }

		/// <summary>
		/// Gets or sets the tutorial value.
		/// </summary>
		public uint Tutorial { get; set; }

		/// <summary>
		/// Gets or sets the drakkinheritage value.
		/// </summary>
		public uint DrakkinHeritage { get; set; }

		/// <summary>
		/// Gets or sets the drakkintattoo value.
		/// </summary>
		public uint DrakkinTattoo { get; set; }

		/// <summary>
		/// Gets or sets the drakkindetails value.
		/// </summary>
		public uint DrakkinDetails { get; set; }

		/// <summary>
		/// Initializes a new instance of the ApproveName struct with specified field values.
		/// </summary>
		/// <param name="class_">The class value.</param>
		/// <param name="haircolor">The haircolor value.</param>
		/// <param name="beard">The beard value.</param>
		/// <param name="beardcolor">The beardcolor value.</param>
		/// <param name="gender">The gender value.</param>
		/// <param name="race">The race value.</param>
		/// <param name="start_zone">The startzone value.</param>
		/// <param name="hairstyle">The hairstyle value.</param>
		/// <param name="deity">The deity value.</param>
		/// <param name="STR">The str value.</param>
		/// <param name="STA">The sta value.</param>
		/// <param name="AGI">The agi value.</param>
		/// <param name="DEX">The dex value.</param>
		/// <param name="WIS">The wis value.</param>
		/// <param name="INT">The int value.</param>
		/// <param name="CHA">The cha value.</param>
		/// <param name="face">The face value.</param>
		/// <param name="eyecolor1">The eyecolor1 value.</param>
		/// <param name="eyecolor2">The eyecolor2 value.</param>
		/// <param name="tutorial">The tutorial value.</param>
		/// <param name="drakkin_heritage">The drakkinheritage value.</param>
		/// <param name="drakkin_tattoo">The drakkintattoo value.</param>
		/// <param name="drakkin_details">The drakkindetails value.</param>
		public ApproveName(uint class_, uint haircolor, uint beard, uint beardcolor, uint gender, uint race, uint start_zone, uint hairstyle, uint deity, uint STR, uint STA, uint AGI, uint DEX, uint WIS, uint INT, uint CHA, uint face, uint eyecolor1, uint eyecolor2, uint tutorial, uint drakkin_heritage, uint drakkin_tattoo, uint drakkin_details) : this() {
			Class = class_;
			Haircolor = haircolor;
			Beard = beard;
			Beardcolor = beardcolor;
			Gender = gender;
			Race = race;
			StartZone = start_zone;
			Hairstyle = hairstyle;
			Deity = deity;
			STR = STR;
			STA = STA;
			AGI = AGI;
			DEX = DEX;
			WIS = WIS;
			INT = INT;
			CHA = CHA;
			Face = face;
			Eyecolor1 = eyecolor1;
			Eyecolor2 = eyecolor2;
			Tutorial = tutorial;
			DrakkinHeritage = drakkin_heritage;
			DrakkinTattoo = drakkin_tattoo;
			DrakkinDetails = drakkin_details;
		}

		/// <summary>
		/// Initializes a new instance of the ApproveName struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ApproveName(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ApproveName struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ApproveName(BinaryReader br) : this() {
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
			Class = br.ReadUInt32();
			Haircolor = br.ReadUInt32();
			Beard = br.ReadUInt32();
			Beardcolor = br.ReadUInt32();
			Gender = br.ReadUInt32();
			Race = br.ReadUInt32();
			StartZone = br.ReadUInt32();
			Hairstyle = br.ReadUInt32();
			Deity = br.ReadUInt32();
			STR = br.ReadUInt32();
			STA = br.ReadUInt32();
			AGI = br.ReadUInt32();
			DEX = br.ReadUInt32();
			WIS = br.ReadUInt32();
			INT = br.ReadUInt32();
			CHA = br.ReadUInt32();
			Face = br.ReadUInt32();
			Eyecolor1 = br.ReadUInt32();
			Eyecolor2 = br.ReadUInt32();
			Tutorial = br.ReadUInt32();
			DrakkinHeritage = br.ReadUInt32();
			DrakkinTattoo = br.ReadUInt32();
			DrakkinDetails = br.ReadUInt32();
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
			bw.Write(Class);
			bw.Write(Haircolor);
			bw.Write(Beard);
			bw.Write(Beardcolor);
			bw.Write(Gender);
			bw.Write(Race);
			bw.Write(StartZone);
			bw.Write(Hairstyle);
			bw.Write(Deity);
			bw.Write(STR);
			bw.Write(STA);
			bw.Write(AGI);
			bw.Write(DEX);
			bw.Write(WIS);
			bw.Write(INT);
			bw.Write(CHA);
			bw.Write(Face);
			bw.Write(Eyecolor1);
			bw.Write(Eyecolor2);
			bw.Write(Tutorial);
			bw.Write(DrakkinHeritage);
			bw.Write(DrakkinTattoo);
			bw.Write(DrakkinDetails);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ApproveName {\n";
			ret += "	Class = ";
			try {
				ret += $"{ Indentify(Class) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Haircolor = ";
			try {
				ret += $"{ Indentify(Haircolor) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Beard = ";
			try {
				ret += $"{ Indentify(Beard) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Beardcolor = ";
			try {
				ret += $"{ Indentify(Beardcolor) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Gender = ";
			try {
				ret += $"{ Indentify(Gender) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Race = ";
			try {
				ret += $"{ Indentify(Race) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	StartZone = ";
			try {
				ret += $"{ Indentify(StartZone) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Hairstyle = ";
			try {
				ret += $"{ Indentify(Hairstyle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Deity = ";
			try {
				ret += $"{ Indentify(Deity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	STR = ";
			try {
				ret += $"{ Indentify(STR) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	STA = ";
			try {
				ret += $"{ Indentify(STA) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AGI = ";
			try {
				ret += $"{ Indentify(AGI) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DEX = ";
			try {
				ret += $"{ Indentify(DEX) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	WIS = ";
			try {
				ret += $"{ Indentify(WIS) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	INT = ";
			try {
				ret += $"{ Indentify(INT) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CHA = ";
			try {
				ret += $"{ Indentify(CHA) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Face = ";
			try {
				ret += $"{ Indentify(Face) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Eyecolor1 = ";
			try {
				ret += $"{ Indentify(Eyecolor1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Eyecolor2 = ";
			try {
				ret += $"{ Indentify(Eyecolor2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Tutorial = ";
			try {
				ret += $"{ Indentify(Tutorial) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DrakkinHeritage = ";
			try {
				ret += $"{ Indentify(DrakkinHeritage) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DrakkinTattoo = ";
			try {
				ret += $"{ Indentify(DrakkinTattoo) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DrakkinDetails = ";
			try {
				ret += $"{ Indentify(DrakkinDetails) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}