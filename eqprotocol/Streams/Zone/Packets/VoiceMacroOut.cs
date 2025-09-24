using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct VoiceMacroOut_Struct {
// /*000*/	char	From[64];
// /*064*/	uint32	Type;	// 1 = Tell, 2 = Group, 3 = Raid
// /*068*/	uint32	Unknown068;
// /*072*/	uint32	Voice;
// /*076*/	uint32	MacroNumber;
// /*080*/	char	Unknown080[60];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the VoiceMacroOut packet structure for EverQuest network communication.
	/// </summary>
	public struct VoiceMacroOut : IEQStruct {
		/// <summary>
		/// Gets or sets the from value.
		/// </summary>
		public byte[] From { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Gets or sets the unknown068 value.
		/// </summary>
		public uint Unknown068 { get; set; }

		/// <summary>
		/// Gets or sets the voice value.
		/// </summary>
		public uint Voice { get; set; }

		/// <summary>
		/// Gets or sets the macronumber value.
		/// </summary>
		public uint Macronumber { get; set; }

		/// <summary>
		/// Gets or sets the unknown080 value.
		/// </summary>
		public byte[] Unknown080 { get; set; }

		/// <summary>
		/// Initializes a new instance of the VoiceMacroOut struct with specified field values.
		/// </summary>
		/// <param name="From">The from value.</param>
		/// <param name="Type">The type value.</param>
		/// <param name="Unknown068">The unknown068 value.</param>
		/// <param name="Voice">The voice value.</param>
		/// <param name="MacroNumber">The macronumber value.</param>
		/// <param name="Unknown080">The unknown080 value.</param>
		public VoiceMacroOut(byte[] From, uint Type, uint Unknown068, uint Voice, uint MacroNumber, byte[] Unknown080) : this() {
			From = From;
			Type = Type;
			Unknown068 = Unknown068;
			Voice = Voice;
			Macronumber = MacroNumber;
			Unknown080 = Unknown080;
		}

		/// <summary>
		/// Initializes a new instance of the VoiceMacroOut struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public VoiceMacroOut(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the VoiceMacroOut struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public VoiceMacroOut(BinaryReader br) : this() {
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
			// TODO: Array reading for From - implement based on actual array size
			// From = new byte[size];
			Type = br.ReadUInt32();
			Unknown068 = br.ReadUInt32();
			Voice = br.ReadUInt32();
			Macronumber = br.ReadUInt32();
			// TODO: Array reading for Unknown080 - implement based on actual array size
			// Unknown080 = new byte[size];
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
			// TODO: Array writing for From - implement based on actual array size
			// foreach(var item in From) bw.Write(item);
			bw.Write(Type);
			bw.Write(Unknown068);
			bw.Write(Voice);
			bw.Write(Macronumber);
			// TODO: Array writing for Unknown080 - implement based on actual array size
			// foreach(var item in Unknown080) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct VoiceMacroOut {\n";
			ret += "	From = ";
			try {
				ret += $"{ Indentify(From) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown068 = ";
			try {
				ret += $"{ Indentify(Unknown068) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Voice = ";
			try {
				ret += $"{ Indentify(Voice) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Macronumber = ";
			try {
				ret += $"{ Indentify(Macronumber) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown080 = ";
			try {
				ret += $"{ Indentify(Unknown080) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}