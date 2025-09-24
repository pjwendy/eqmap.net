using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SpellEffect_Struct
// {
// /*000*/	uint32 EffectID;
// /*004*/	uint32 EntityID;
// /*008*/	uint32 EntityID2;	// EntityID again
// /*012*/	uint32 Duration;		// In Milliseconds
// /*016*/	uint32 FinishDelay;	// In Milliseconds - delay for final part of spell effect
// /*020*/	uint32 Unknown020;	// Seen 3000
// /*024*/ uint8 Unknown024;	// Seen 1 for SoD
// /*025*/ uint8 Unknown025;	// Seen 1 for Live
// /*026*/ uint16 Unknown026;	// Seen 1157 and 1177 - varies per char
// /*028*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SpellEffect packet structure for EverQuest network communication.
	/// </summary>
	public struct SpellEffect : IEQStruct {
		/// <summary>
		/// Gets or sets the effectid value.
		/// </summary>
		public uint Effectid { get; set; }

		/// <summary>
		/// Gets or sets the entityid value.
		/// </summary>
		public uint Entityid { get; set; }

		/// <summary>
		/// Gets or sets the entityid2 value.
		/// </summary>
		public uint Entityid2 { get; set; }

		/// <summary>
		/// Gets or sets the duration value.
		/// </summary>
		public uint Duration { get; set; }

		/// <summary>
		/// Gets or sets the finishdelay value.
		/// </summary>
		public uint Finishdelay { get; set; }

		/// <summary>
		/// Gets or sets the unknown020 value.
		/// </summary>
		public uint Unknown020 { get; set; }

		/// <summary>
		/// Gets or sets the unknown024 value.
		/// </summary>
		public byte Unknown024 { get; set; }

		/// <summary>
		/// Gets or sets the unknown025 value.
		/// </summary>
		public byte Unknown025 { get; set; }

		/// <summary>
		/// Gets or sets the unknown026 value.
		/// </summary>
		public ushort Unknown026 { get; set; }

		/// <summary>
		/// Initializes a new instance of the SpellEffect struct with specified field values.
		/// </summary>
		/// <param name="EffectID">The effectid value.</param>
		/// <param name="EntityID">The entityid value.</param>
		/// <param name="EntityID2">The entityid2 value.</param>
		/// <param name="Duration">The duration value.</param>
		/// <param name="FinishDelay">The finishdelay value.</param>
		/// <param name="Unknown020">The unknown020 value.</param>
		/// <param name="Unknown024">The unknown024 value.</param>
		/// <param name="Unknown025">The unknown025 value.</param>
		/// <param name="Unknown026">The unknown026 value.</param>
		public SpellEffect(uint EffectID, uint EntityID, uint EntityID2, uint Duration, uint FinishDelay, uint Unknown020, byte Unknown024, byte Unknown025, ushort Unknown026) : this() {
			Effectid = EffectID;
			Entityid = EntityID;
			Entityid2 = EntityID2;
			Duration = Duration;
			Finishdelay = FinishDelay;
			Unknown020 = Unknown020;
			Unknown024 = Unknown024;
			Unknown025 = Unknown025;
			Unknown026 = Unknown026;
		}

		/// <summary>
		/// Initializes a new instance of the SpellEffect struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SpellEffect(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SpellEffect struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SpellEffect(BinaryReader br) : this() {
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
			Effectid = br.ReadUInt32();
			Entityid = br.ReadUInt32();
			Entityid2 = br.ReadUInt32();
			Duration = br.ReadUInt32();
			Finishdelay = br.ReadUInt32();
			Unknown020 = br.ReadUInt32();
			Unknown024 = br.ReadByte();
			Unknown025 = br.ReadByte();
			Unknown026 = br.ReadUInt16();
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
			bw.Write(Effectid);
			bw.Write(Entityid);
			bw.Write(Entityid2);
			bw.Write(Duration);
			bw.Write(Finishdelay);
			bw.Write(Unknown020);
			bw.Write(Unknown024);
			bw.Write(Unknown025);
			bw.Write(Unknown026);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SpellEffect {\n";
			ret += "	Effectid = ";
			try {
				ret += $"{ Indentify(Effectid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Entityid = ";
			try {
				ret += $"{ Indentify(Entityid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Entityid2 = ";
			try {
				ret += $"{ Indentify(Entityid2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Duration = ";
			try {
				ret += $"{ Indentify(Duration) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Finishdelay = ";
			try {
				ret += $"{ Indentify(Finishdelay) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown020 = ";
			try {
				ret += $"{ Indentify(Unknown020) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown024 = ";
			try {
				ret += $"{ Indentify(Unknown024) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown025 = ";
			try {
				ret += $"{ Indentify(Unknown025) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown026 = ";
			try {
				ret += $"{ Indentify(Unknown026) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}