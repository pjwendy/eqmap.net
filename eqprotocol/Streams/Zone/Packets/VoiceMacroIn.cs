using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct VoiceMacroIn_Struct {
// /*000*/	char	Unknown000[64];
// /*064*/	uint32	Type;	// 1 = Tell, 2 = Group, 3 = Raid
// /*068*/	char	Target[64];
// /*132*/	uint32	Unknown132;	// Seems to be 0x0000000c always
// /*136*/	uint32	MacroNumber;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the VoiceMacroIn packet structure for EverQuest network communication.
	/// </summary>
	public struct VoiceMacroIn : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public byte[] Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Gets or sets the target value.
		/// </summary>
		public byte[] Target { get; set; }

		/// <summary>
		/// Gets or sets the unknown132 value.
		/// </summary>
		public uint Unknown132 { get; set; }

		/// <summary>
		/// Gets or sets the macronumber value.
		/// </summary>
		public uint Macronumber { get; set; }

		/// <summary>
		/// Initializes a new instance of the VoiceMacroIn struct with specified field values.
		/// </summary>
		/// <param name="Unknown000">The unknown000 value.</param>
		/// <param name="Type">The type value.</param>
		/// <param name="Target">The target value.</param>
		/// <param name="Unknown132">The unknown132 value.</param>
		/// <param name="MacroNumber">The macronumber value.</param>
		public VoiceMacroIn(byte[] Unknown000, uint Type, byte[] Target, uint Unknown132, uint MacroNumber) : this() {
			Unknown000 = Unknown000;
			Type = Type;
			Target = Target;
			Unknown132 = Unknown132;
			Macronumber = MacroNumber;
		}

		/// <summary>
		/// Initializes a new instance of the VoiceMacroIn struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public VoiceMacroIn(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the VoiceMacroIn struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public VoiceMacroIn(BinaryReader br) : this() {
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
			// TODO: Array reading for Unknown000 - implement based on actual array size
			// Unknown000 = new byte[size];
			Type = br.ReadUInt32();
			// TODO: Array reading for Target - implement based on actual array size
			// Target = new byte[size];
			Unknown132 = br.ReadUInt32();
			Macronumber = br.ReadUInt32();
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
			// TODO: Array writing for Unknown000 - implement based on actual array size
			// foreach(var item in Unknown000) bw.Write(item);
			bw.Write(Type);
			// TODO: Array writing for Target - implement based on actual array size
			// foreach(var item in Target) bw.Write(item);
			bw.Write(Unknown132);
			bw.Write(Macronumber);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct VoiceMacroIn {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Target = ";
			try {
				ret += $"{ Indentify(Target) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown132 = ";
			try {
				ret += $"{ Indentify(Unknown132) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Macronumber = ";
			try {
				ret += $"{ Indentify(Macronumber) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}