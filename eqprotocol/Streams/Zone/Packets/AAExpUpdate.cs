using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AltAdvStats_Struct {
// /*000*/  uint32 experience;
// /*004*/  uint16 unspent;
// /*006*/  uint16	unknown006;
// /*008*/  uint8	percentage;
// /*009*/  uint8	unknown009[3];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AAExpUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct AAExpUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the experience value.
		/// </summary>
		public uint Experience { get; set; }

		/// <summary>
		/// Gets or sets the unspent value.
		/// </summary>
		public ushort Unspent { get; set; }

		/// <summary>
		/// Gets or sets the unknown006 value.
		/// </summary>
		public ushort Unknown006 { get; set; }

		/// <summary>
		/// Gets or sets the percentage value.
		/// </summary>
		public byte Percentage { get; set; }

		/// <summary>
		/// Gets or sets the unknown009 value.
		/// </summary>
		public byte[] Unknown009 { get; set; }

		/// <summary>
		/// Initializes a new instance of the AAExpUpdate struct with specified field values.
		/// </summary>
		/// <param name="experience">The experience value.</param>
		/// <param name="unspent">The unspent value.</param>
		/// <param name="unknown006">The unknown006 value.</param>
		/// <param name="percentage">The percentage value.</param>
		/// <param name="unknown009">The unknown009 value.</param>
		public AAExpUpdate(uint experience, ushort unspent, ushort unknown006, byte percentage, byte[] unknown009) : this() {
			Experience = experience;
			Unspent = unspent;
			Unknown006 = unknown006;
			Percentage = percentage;
			Unknown009 = unknown009;
		}

		/// <summary>
		/// Initializes a new instance of the AAExpUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AAExpUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AAExpUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AAExpUpdate(BinaryReader br) : this() {
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
			Experience = br.ReadUInt32();
			Unspent = br.ReadUInt16();
			Unknown006 = br.ReadUInt16();
			Percentage = br.ReadByte();
			// TODO: Array reading for Unknown009 - implement based on actual array size
			// Unknown009 = new byte[size];
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
			bw.Write(Experience);
			bw.Write(Unspent);
			bw.Write(Unknown006);
			bw.Write(Percentage);
			// TODO: Array writing for Unknown009 - implement based on actual array size
			// foreach(var item in Unknown009) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AAExpUpdate {\n";
			ret += "	Experience = ";
			try {
				ret += $"{ Indentify(Experience) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unspent = ";
			try {
				ret += $"{ Indentify(Unspent) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown006 = ";
			try {
				ret += $"{ Indentify(Unknown006) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Percentage = ";
			try {
				ret += $"{ Indentify(Percentage) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown009 = ";
			try {
				ret += $"{ Indentify(Unknown009) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}