using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Who_All_Struct { // 76 length total
// /*000*/	char	whom[64];
// /*064*/	uint32	wrace;		// FF FF = no race
// 
// /*068*/	uint32	wclass;		// FF FF = no class
// /*072*/	uint32	lvllow;		// FF FF = no numbers
// /*076*/	uint32	lvlhigh;	// FF FF = no numbers
// /*080*/	uint32	gmlookup;	// FF FF = not doing /who all gm
// /*084*/	uint32	guildid;	// Also used for Buyer/Trader/LFG
// /*088*/	uint8	unknown088[64];
// /*156*/	uint32	type;		// 0 = /who 3 = /who all
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the XTargetAutoAddHaters packet structure for EverQuest network communication.
	/// </summary>
	public struct XTargetAutoAddHaters : IEQStruct {
		/// <summary>
		/// Gets or sets the whom value.
		/// </summary>
		public byte[] Whom { get; set; }

		/// <summary>
		/// Gets or sets the wrace value.
		/// </summary>
		public uint Wrace { get; set; }

		/// <summary>
		/// Gets or sets the wclass value.
		/// </summary>
		public uint Wclass { get; set; }

		/// <summary>
		/// Gets or sets the lvllow value.
		/// </summary>
		public uint Lvllow { get; set; }

		/// <summary>
		/// Gets or sets the lvlhigh value.
		/// </summary>
		public uint Lvlhigh { get; set; }

		/// <summary>
		/// Gets or sets the gmlookup value.
		/// </summary>
		public uint Gmlookup { get; set; }

		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint Guildid { get; set; }

		/// <summary>
		/// Gets or sets the unknown088 value.
		/// </summary>
		public byte[] Unknown088 { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Initializes a new instance of the XTargetAutoAddHaters struct with specified field values.
		/// </summary>
		/// <param name="whom">The whom value.</param>
		/// <param name="wrace">The wrace value.</param>
		/// <param name="wclass">The wclass value.</param>
		/// <param name="lvllow">The lvllow value.</param>
		/// <param name="lvlhigh">The lvlhigh value.</param>
		/// <param name="gmlookup">The gmlookup value.</param>
		/// <param name="guildid">The guildid value.</param>
		/// <param name="unknown088">The unknown088 value.</param>
		/// <param name="type">The type value.</param>
		public XTargetAutoAddHaters(byte[] whom, uint wrace, uint wclass, uint lvllow, uint lvlhigh, uint gmlookup, uint guildid, byte[] unknown088, uint type) : this() {
			Whom = whom;
			Wrace = wrace;
			Wclass = wclass;
			Lvllow = lvllow;
			Lvlhigh = lvlhigh;
			Gmlookup = gmlookup;
			Guildid = guildid;
			Unknown088 = unknown088;
			Type = type;
		}

		/// <summary>
		/// Initializes a new instance of the XTargetAutoAddHaters struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public XTargetAutoAddHaters(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the XTargetAutoAddHaters struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public XTargetAutoAddHaters(BinaryReader br) : this() {
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
			// TODO: Array reading for Whom - implement based on actual array size
			// Whom = new byte[size];
			Wrace = br.ReadUInt32();
			Wclass = br.ReadUInt32();
			Lvllow = br.ReadUInt32();
			Lvlhigh = br.ReadUInt32();
			Gmlookup = br.ReadUInt32();
			Guildid = br.ReadUInt32();
			// TODO: Array reading for Unknown088 - implement based on actual array size
			// Unknown088 = new byte[size];
			Type = br.ReadUInt32();
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
			// TODO: Array writing for Whom - implement based on actual array size
			// foreach(var item in Whom) bw.Write(item);
			bw.Write(Wrace);
			bw.Write(Wclass);
			bw.Write(Lvllow);
			bw.Write(Lvlhigh);
			bw.Write(Gmlookup);
			bw.Write(Guildid);
			// TODO: Array writing for Unknown088 - implement based on actual array size
			// foreach(var item in Unknown088) bw.Write(item);
			bw.Write(Type);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct XTargetAutoAddHaters {\n";
			ret += "	Whom = ";
			try {
				ret += $"{ Indentify(Whom) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Wrace = ";
			try {
				ret += $"{ Indentify(Wrace) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Wclass = ";
			try {
				ret += $"{ Indentify(Wclass) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Lvllow = ";
			try {
				ret += $"{ Indentify(Lvllow) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Lvlhigh = ";
			try {
				ret += $"{ Indentify(Lvlhigh) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Gmlookup = ";
			try {
				ret += $"{ Indentify(Gmlookup) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Guildid = ";
			try {
				ret += $"{ Indentify(Guildid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown088 = ";
			try {
				ret += $"{ Indentify(Unknown088) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}