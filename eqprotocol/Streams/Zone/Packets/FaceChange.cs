using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct FaceChange_Struct {
// /*000*/	uint8	haircolor;
// /*001*/	uint8	beardcolor;
// /*002*/	uint8	eyecolor1;
// /*003*/	uint8	eyecolor2;
// /*004*/	uint8	hairstyle;
// /*005*/	uint8	beard;
// /*006*/	uint8	face;
// /*007*/ uint8   unused_padding;
// /*008*/ uint32	drakkin_heritage;
// /*012*/ uint32	drakkin_tattoo;
// /*016*/ uint32	drakkin_details;
// /*020*/ uint32  entity_id;
// /*024*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_FaceChange)
// {
// DECODE_LENGTH_EXACT(structs::FaceChange_Struct);
// SETUP_DIRECT_DECODE(FaceChange_Struct, structs::FaceChange_Struct);
// 
// IN(haircolor);
// IN(beardcolor);
// IN(eyecolor1);
// IN(eyecolor2);
// IN(hairstyle);
// IN(beard);
// IN(face);
// IN(drakkin_heritage);
// IN(drakkin_tattoo);
// IN(drakkin_details);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the FaceChange packet structure for EverQuest network communication.
	/// </summary>
	public struct FaceChange : IEQStruct {
		/// <summary>
		/// Gets or sets the haircolor value.
		/// </summary>
		public byte Haircolor { get; set; }

		/// <summary>
		/// Gets or sets the beardcolor value.
		/// </summary>
		public byte Beardcolor { get; set; }

		/// <summary>
		/// Gets or sets the eyecolor1 value.
		/// </summary>
		public byte Eyecolor1 { get; set; }

		/// <summary>
		/// Gets or sets the eyecolor2 value.
		/// </summary>
		public byte Eyecolor2 { get; set; }

		/// <summary>
		/// Gets or sets the hairstyle value.
		/// </summary>
		public byte Hairstyle { get; set; }

		/// <summary>
		/// Gets or sets the beard value.
		/// </summary>
		public byte Beard { get; set; }

		/// <summary>
		/// Gets or sets the face value.
		/// </summary>
		public byte Face { get; set; }

		/// <summary>
		/// Gets or sets the unusedpadding value.
		/// </summary>
		public byte UnusedPadding { get; set; }

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
		/// Gets or sets the entityid value.
		/// </summary>
		public uint EntityId { get; set; }

		/// <summary>
		/// Initializes a new instance of the FaceChange struct with specified field values.
		/// </summary>
		/// <param name="haircolor">The haircolor value.</param>
		/// <param name="beardcolor">The beardcolor value.</param>
		/// <param name="eyecolor1">The eyecolor1 value.</param>
		/// <param name="eyecolor2">The eyecolor2 value.</param>
		/// <param name="hairstyle">The hairstyle value.</param>
		/// <param name="beard">The beard value.</param>
		/// <param name="face">The face value.</param>
		/// <param name="unused_padding">The unusedpadding value.</param>
		/// <param name="drakkin_heritage">The drakkinheritage value.</param>
		/// <param name="drakkin_tattoo">The drakkintattoo value.</param>
		/// <param name="drakkin_details">The drakkindetails value.</param>
		/// <param name="entity_id">The entityid value.</param>
		public FaceChange(byte haircolor, byte beardcolor, byte eyecolor1, byte eyecolor2, byte hairstyle, byte beard, byte face, byte unused_padding, uint drakkin_heritage, uint drakkin_tattoo, uint drakkin_details, uint entity_id) : this() {
			Haircolor = haircolor;
			Beardcolor = beardcolor;
			Eyecolor1 = eyecolor1;
			Eyecolor2 = eyecolor2;
			Hairstyle = hairstyle;
			Beard = beard;
			Face = face;
			UnusedPadding = unused_padding;
			DrakkinHeritage = drakkin_heritage;
			DrakkinTattoo = drakkin_tattoo;
			DrakkinDetails = drakkin_details;
			EntityId = entity_id;
		}

		/// <summary>
		/// Initializes a new instance of the FaceChange struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public FaceChange(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the FaceChange struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public FaceChange(BinaryReader br) : this() {
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
			Haircolor = br.ReadByte();
			Beardcolor = br.ReadByte();
			Eyecolor1 = br.ReadByte();
			Eyecolor2 = br.ReadByte();
			Hairstyle = br.ReadByte();
			Beard = br.ReadByte();
			Face = br.ReadByte();
			UnusedPadding = br.ReadByte();
			DrakkinHeritage = br.ReadUInt32();
			DrakkinTattoo = br.ReadUInt32();
			DrakkinDetails = br.ReadUInt32();
			EntityId = br.ReadUInt32();
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
			bw.Write(Haircolor);
			bw.Write(Beardcolor);
			bw.Write(Eyecolor1);
			bw.Write(Eyecolor2);
			bw.Write(Hairstyle);
			bw.Write(Beard);
			bw.Write(Face);
			bw.Write(UnusedPadding);
			bw.Write(DrakkinHeritage);
			bw.Write(DrakkinTattoo);
			bw.Write(DrakkinDetails);
			bw.Write(EntityId);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct FaceChange {\n";
			ret += "	Haircolor = ";
			try {
				ret += $"{ Indentify(Haircolor) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Beardcolor = ";
			try {
				ret += $"{ Indentify(Beardcolor) },\n";
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
			ret += "	Hairstyle = ";
			try {
				ret += $"{ Indentify(Hairstyle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Beard = ";
			try {
				ret += $"{ Indentify(Beard) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Face = ";
			try {
				ret += $"{ Indentify(Face) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	UnusedPadding = ";
			try {
				ret += $"{ Indentify(UnusedPadding) },\n";
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
			ret += "	EntityId = ";
			try {
				ret += $"{ Indentify(EntityId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}