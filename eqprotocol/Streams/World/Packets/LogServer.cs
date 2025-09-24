using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LogServer_Struct {
// // Op_Code OP_LOGSERVER
// /*000*/	uint32	unknown000;
// /*004*/	uint8	enable_pvp;
// /*005*/	uint8	unknown005;
// /*006*/	uint8	unknown006;
// /*007*/	uint8	unknown007;
// /*008*/	uint8	enable_FV;
// /*009*/	uint8	unknown009;
// /*010*/	uint8	unknown010;
// /*011*/	uint8	unknown011;
// /*012*/	uint32	unknown012;	// htonl(1) on live
// /*016*/	uint32	unknown016;	// htonl(1) on live
// /*020*/	uint8	unknown020[12];
// /*032*/ uint32	unknown032;
// /*036*/	char	worldshortname[32];
// /*068*/	uint8	unknown064[32];
// /*100*/	char	unknown096[16];	// 'pacman' on live
// /*116*/	char	unknown112[16];	// '64.37,148,36' on live
// /*132*/	uint8	unknown128[48];
// /*180*/	uint32	unknown176;	// htonl(0x00002695)
// /*184*/	char	unknown180[80];	// 'eqdataexceptions@mail.station.sony.com' on live
// /*264*/	uint8	unknown260;	// 0x01 on live
// /*265*/	uint8	enablevoicemacros;
// /*266*/	uint8	enablemail;
// /*267*/	uint8	unknown263[41];
// /*308*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_LogServer)
// {
// ENCODE_LENGTH_EXACT(LogServer_Struct);
// SETUP_DIRECT_ENCODE(LogServer_Struct, structs::LogServer_Struct);
// 
// strcpy(eq->worldshortname, emu->worldshortname);
// 
// OUT(enablevoicemacros);
// OUT(enablemail);
// OUT(enable_pvp);
// OUT(enable_FV);
// 
// eq->unknown016 = 1;
// eq->unknown020[0] = 1;
// 
// // These next two need to be set like this for the Tutorial Button to work.
// eq->unknown263[0] = 0;
// eq->unknown263[2] = 1;
// eq->unknown263[4] = 1;
// eq->unknown263[5] = 1;
// eq->unknown263[6] = 1;
// eq->unknown263[9] = 8;
// eq->unknown263[19] = 0x80;
// eq->unknown263[20] = 0x3f;
// eq->unknown263[23] = 0x80;
// eq->unknown263[24] = 0x3f;
// eq->unknown263[33] = 1;
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LogServer packet structure for EverQuest network communication.
	/// </summary>
	public struct LogServer : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the enablepvp value.
		/// </summary>
		public byte EnablePvp { get; set; }

		/// <summary>
		/// Gets or sets the unknown005 value.
		/// </summary>
		public byte Unknown005 { get; set; }

		/// <summary>
		/// Gets or sets the unknown006 value.
		/// </summary>
		public byte Unknown006 { get; set; }

		/// <summary>
		/// Gets or sets the unknown007 value.
		/// </summary>
		public byte Unknown007 { get; set; }

		/// <summary>
		/// Gets or sets the enablefv value.
		/// </summary>
		public byte EnableFV { get; set; }

		/// <summary>
		/// Gets or sets the unknown009 value.
		/// </summary>
		public byte Unknown009 { get; set; }

		/// <summary>
		/// Gets or sets the unknown010 value.
		/// </summary>
		public byte Unknown010 { get; set; }

		/// <summary>
		/// Gets or sets the unknown011 value.
		/// </summary>
		public byte Unknown011 { get; set; }

		/// <summary>
		/// Gets or sets the unknown012 value.
		/// </summary>
		public uint Unknown012 { get; set; }

		/// <summary>
		/// Gets or sets the unknown016 value.
		/// </summary>
		public uint Unknown016 { get; set; }

		/// <summary>
		/// Gets or sets the unknown020 value.
		/// </summary>
		public byte[] Unknown020 { get; set; }

		/// <summary>
		/// Gets or sets the unknown032 value.
		/// </summary>
		public uint Unknown032 { get; set; }

		/// <summary>
		/// Gets or sets the worldshortname value.
		/// </summary>
		public byte[] Worldshortname { get; set; }

		/// <summary>
		/// Gets or sets the unknown064 value.
		/// </summary>
		public byte[] Unknown064 { get; set; }

		/// <summary>
		/// Gets or sets the unknown096 value.
		/// </summary>
		public byte[] Unknown096 { get; set; }

		/// <summary>
		/// Gets or sets the unknown112 value.
		/// </summary>
		public byte[] Unknown112 { get; set; }

		/// <summary>
		/// Gets or sets the unknown128 value.
		/// </summary>
		public byte[] Unknown128 { get; set; }

		/// <summary>
		/// Gets or sets the unknown176 value.
		/// </summary>
		public uint Unknown176 { get; set; }

		/// <summary>
		/// Gets or sets the unknown180 value.
		/// </summary>
		public byte[] Unknown180 { get; set; }

		/// <summary>
		/// Gets or sets the unknown260 value.
		/// </summary>
		public byte Unknown260 { get; set; }

		/// <summary>
		/// Gets or sets the enablevoicemacros value.
		/// </summary>
		public byte Enablevoicemacros { get; set; }

		/// <summary>
		/// Gets or sets the enablemail value.
		/// </summary>
		public byte Enablemail { get; set; }

		/// <summary>
		/// Gets or sets the unknown263 value.
		/// </summary>
		public byte[] Unknown263 { get; set; }

		/// <summary>
		/// Initializes a new instance of the LogServer struct with specified field values.
		/// </summary>
		/// <param name="unknown000">The unknown000 value.</param>
		/// <param name="enable_pvp">The enablepvp value.</param>
		/// <param name="unknown005">The unknown005 value.</param>
		/// <param name="unknown006">The unknown006 value.</param>
		/// <param name="unknown007">The unknown007 value.</param>
		/// <param name="enable_FV">The enablefv value.</param>
		/// <param name="unknown009">The unknown009 value.</param>
		/// <param name="unknown010">The unknown010 value.</param>
		/// <param name="unknown011">The unknown011 value.</param>
		/// <param name="unknown012">The unknown012 value.</param>
		/// <param name="unknown016">The unknown016 value.</param>
		/// <param name="unknown020">The unknown020 value.</param>
		/// <param name="unknown032">The unknown032 value.</param>
		/// <param name="worldshortname">The worldshortname value.</param>
		/// <param name="unknown064">The unknown064 value.</param>
		/// <param name="unknown096">The unknown096 value.</param>
		/// <param name="unknown112">The unknown112 value.</param>
		/// <param name="unknown128">The unknown128 value.</param>
		/// <param name="unknown176">The unknown176 value.</param>
		/// <param name="unknown180">The unknown180 value.</param>
		/// <param name="unknown260">The unknown260 value.</param>
		/// <param name="enablevoicemacros">The enablevoicemacros value.</param>
		/// <param name="enablemail">The enablemail value.</param>
		/// <param name="unknown263">The unknown263 value.</param>
		public LogServer(uint unknown000, byte enable_pvp, byte unknown005, byte unknown006, byte unknown007, byte enable_FV, byte unknown009, byte unknown010, byte unknown011, uint unknown012, uint unknown016, byte[] unknown020, uint unknown032, byte[] worldshortname, byte[] unknown064, byte[] unknown096, byte[] unknown112, byte[] unknown128, uint unknown176, byte[] unknown180, byte unknown260, byte enablevoicemacros, byte enablemail, byte[] unknown263) : this() {
			Unknown000 = unknown000;
			EnablePvp = enable_pvp;
			Unknown005 = unknown005;
			Unknown006 = unknown006;
			Unknown007 = unknown007;
			EnableFV = enable_FV;
			Unknown009 = unknown009;
			Unknown010 = unknown010;
			Unknown011 = unknown011;
			Unknown012 = unknown012;
			Unknown016 = unknown016;
			Unknown020 = unknown020;
			Unknown032 = unknown032;
			Worldshortname = worldshortname;
			Unknown064 = unknown064;
			Unknown096 = unknown096;
			Unknown112 = unknown112;
			Unknown128 = unknown128;
			Unknown176 = unknown176;
			Unknown180 = unknown180;
			Unknown260 = unknown260;
			Enablevoicemacros = enablevoicemacros;
			Enablemail = enablemail;
			Unknown263 = unknown263;
		}

		/// <summary>
		/// Initializes a new instance of the LogServer struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LogServer(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LogServer struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LogServer(BinaryReader br) : this() {
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
			Unknown000 = br.ReadUInt32();
			EnablePvp = br.ReadByte();
			Unknown005 = br.ReadByte();
			Unknown006 = br.ReadByte();
			Unknown007 = br.ReadByte();
			EnableFV = br.ReadByte();
			Unknown009 = br.ReadByte();
			Unknown010 = br.ReadByte();
			Unknown011 = br.ReadByte();
			Unknown012 = br.ReadUInt32();
			Unknown016 = br.ReadUInt32();
			// TODO: Array reading for Unknown020 - implement based on actual array size
			// Unknown020 = new byte[size];
			Unknown032 = br.ReadUInt32();
			// TODO: Array reading for Worldshortname - implement based on actual array size
			// Worldshortname = new byte[size];
			// TODO: Array reading for Unknown064 - implement based on actual array size
			// Unknown064 = new byte[size];
			// TODO: Array reading for Unknown096 - implement based on actual array size
			// Unknown096 = new byte[size];
			// TODO: Array reading for Unknown112 - implement based on actual array size
			// Unknown112 = new byte[size];
			// TODO: Array reading for Unknown128 - implement based on actual array size
			// Unknown128 = new byte[size];
			Unknown176 = br.ReadUInt32();
			// TODO: Array reading for Unknown180 - implement based on actual array size
			// Unknown180 = new byte[size];
			Unknown260 = br.ReadByte();
			Enablevoicemacros = br.ReadByte();
			Enablemail = br.ReadByte();
			// TODO: Array reading for Unknown263 - implement based on actual array size
			// Unknown263 = new byte[size];
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
			bw.Write(Unknown000);
			bw.Write(EnablePvp);
			bw.Write(Unknown005);
			bw.Write(Unknown006);
			bw.Write(Unknown007);
			bw.Write(EnableFV);
			bw.Write(Unknown009);
			bw.Write(Unknown010);
			bw.Write(Unknown011);
			bw.Write(Unknown012);
			bw.Write(Unknown016);
			// TODO: Array writing for Unknown020 - implement based on actual array size
			// foreach(var item in Unknown020) bw.Write(item);
			bw.Write(Unknown032);
			// TODO: Array writing for Worldshortname - implement based on actual array size
			// foreach(var item in Worldshortname) bw.Write(item);
			// TODO: Array writing for Unknown064 - implement based on actual array size
			// foreach(var item in Unknown064) bw.Write(item);
			// TODO: Array writing for Unknown096 - implement based on actual array size
			// foreach(var item in Unknown096) bw.Write(item);
			// TODO: Array writing for Unknown112 - implement based on actual array size
			// foreach(var item in Unknown112) bw.Write(item);
			// TODO: Array writing for Unknown128 - implement based on actual array size
			// foreach(var item in Unknown128) bw.Write(item);
			bw.Write(Unknown176);
			// TODO: Array writing for Unknown180 - implement based on actual array size
			// foreach(var item in Unknown180) bw.Write(item);
			bw.Write(Unknown260);
			bw.Write(Enablevoicemacros);
			bw.Write(Enablemail);
			// TODO: Array writing for Unknown263 - implement based on actual array size
			// foreach(var item in Unknown263) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LogServer {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	EnablePvp = ";
			try {
				ret += $"{ Indentify(EnablePvp) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown005 = ";
			try {
				ret += $"{ Indentify(Unknown005) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown006 = ";
			try {
				ret += $"{ Indentify(Unknown006) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown007 = ";
			try {
				ret += $"{ Indentify(Unknown007) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	EnableFV = ";
			try {
				ret += $"{ Indentify(EnableFV) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown009 = ";
			try {
				ret += $"{ Indentify(Unknown009) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown010 = ";
			try {
				ret += $"{ Indentify(Unknown010) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown011 = ";
			try {
				ret += $"{ Indentify(Unknown011) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown012 = ";
			try {
				ret += $"{ Indentify(Unknown012) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown016 = ";
			try {
				ret += $"{ Indentify(Unknown016) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown020 = ";
			try {
				ret += $"{ Indentify(Unknown020) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown032 = ";
			try {
				ret += $"{ Indentify(Unknown032) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Worldshortname = ";
			try {
				ret += $"{ Indentify(Worldshortname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown064 = ";
			try {
				ret += $"{ Indentify(Unknown064) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown096 = ";
			try {
				ret += $"{ Indentify(Unknown096) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown112 = ";
			try {
				ret += $"{ Indentify(Unknown112) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown128 = ";
			try {
				ret += $"{ Indentify(Unknown128) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown176 = ";
			try {
				ret += $"{ Indentify(Unknown176) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown180 = ";
			try {
				ret += $"{ Indentify(Unknown180) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown260 = ";
			try {
				ret += $"{ Indentify(Unknown260) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Enablevoicemacros = ";
			try {
				ret += $"{ Indentify(Enablevoicemacros) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Enablemail = ";
			try {
				ret += $"{ Indentify(Enablemail) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown263 = ";
			try {
				ret += $"{ Indentify(Unknown263) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}