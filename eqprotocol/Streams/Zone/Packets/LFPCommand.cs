using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LFP_Struct {
// /*000*/	uint32	Unknown000;
// /*004*/	uint8	Action;
// /*005*/	uint8	MatchFilter;
// /*006*/	uint16	Unknown006;
// /*008*/	uint32	FromLevel;
// /*012*/	uint32	ToLevel;
// /*016*/	uint32	Classes;
// /*020*/	char	Comments[64];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LFPCommand packet structure for EverQuest network communication.
	/// </summary>
	public struct LFPCommand : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the action value.
		/// </summary>
		public byte Action { get; set; }

		/// <summary>
		/// Gets or sets the matchfilter value.
		/// </summary>
		public byte Matchfilter { get; set; }

		/// <summary>
		/// Gets or sets the unknown006 value.
		/// </summary>
		public ushort Unknown006 { get; set; }

		/// <summary>
		/// Gets or sets the fromlevel value.
		/// </summary>
		public uint Fromlevel { get; set; }

		/// <summary>
		/// Gets or sets the tolevel value.
		/// </summary>
		public uint Tolevel { get; set; }

		/// <summary>
		/// Gets or sets the classes value.
		/// </summary>
		public uint Classes { get; set; }

		/// <summary>
		/// Gets or sets the comments value.
		/// </summary>
		public byte[] Comments { get; set; }

		/// <summary>
		/// Initializes a new instance of the LFPCommand struct with specified field values.
		/// </summary>
		/// <param name="Unknown000">The unknown000 value.</param>
		/// <param name="Action">The action value.</param>
		/// <param name="MatchFilter">The matchfilter value.</param>
		/// <param name="Unknown006">The unknown006 value.</param>
		/// <param name="FromLevel">The fromlevel value.</param>
		/// <param name="ToLevel">The tolevel value.</param>
		/// <param name="Classes">The classes value.</param>
		/// <param name="Comments">The comments value.</param>
		public LFPCommand(uint Unknown000, byte Action, byte MatchFilter, ushort Unknown006, uint FromLevel, uint ToLevel, uint Classes, byte[] Comments) : this() {
			Unknown000 = Unknown000;
			Action = Action;
			Matchfilter = MatchFilter;
			Unknown006 = Unknown006;
			Fromlevel = FromLevel;
			Tolevel = ToLevel;
			Classes = Classes;
			Comments = Comments;
		}

		/// <summary>
		/// Initializes a new instance of the LFPCommand struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LFPCommand(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LFPCommand struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LFPCommand(BinaryReader br) : this() {
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
			Action = br.ReadByte();
			Matchfilter = br.ReadByte();
			Unknown006 = br.ReadUInt16();
			Fromlevel = br.ReadUInt32();
			Tolevel = br.ReadUInt32();
			Classes = br.ReadUInt32();
			// TODO: Array reading for Comments - implement based on actual array size
			// Comments = new byte[size];
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
			bw.Write(Action);
			bw.Write(Matchfilter);
			bw.Write(Unknown006);
			bw.Write(Fromlevel);
			bw.Write(Tolevel);
			bw.Write(Classes);
			// TODO: Array writing for Comments - implement based on actual array size
			// foreach(var item in Comments) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LFPCommand {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Matchfilter = ";
			try {
				ret += $"{ Indentify(Matchfilter) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown006 = ";
			try {
				ret += $"{ Indentify(Unknown006) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Fromlevel = ";
			try {
				ret += $"{ Indentify(Fromlevel) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Tolevel = ";
			try {
				ret += $"{ Indentify(Tolevel) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Classes = ";
			try {
				ret += $"{ Indentify(Classes) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Comments = ";
			try {
				ret += $"{ Indentify(Comments) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}