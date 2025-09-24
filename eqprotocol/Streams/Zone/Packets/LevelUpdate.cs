using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LevelUpdate_Struct
// {
// /*00*/ uint32 level;                  // New level
// /*04*/ uint32 level_old;              // Old level
// /*08*/ uint32 exp;                    // Current Experience
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LevelUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct LevelUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the level value.
		/// </summary>
		public uint Level { get; set; }

		/// <summary>
		/// Gets or sets the levelold value.
		/// </summary>
		public uint LevelOld { get; set; }

		/// <summary>
		/// Gets or sets the exp value.
		/// </summary>
		public uint Exp { get; set; }

		/// <summary>
		/// Initializes a new instance of the LevelUpdate struct with specified field values.
		/// </summary>
		/// <param name="level">The level value.</param>
		/// <param name="level_old">The levelold value.</param>
		/// <param name="exp">The exp value.</param>
		public LevelUpdate(uint level, uint level_old, uint exp) : this() {
			Level = level;
			LevelOld = level_old;
			Exp = exp;
		}

		/// <summary>
		/// Initializes a new instance of the LevelUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LevelUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LevelUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LevelUpdate(BinaryReader br) : this() {
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
			Level = br.ReadUInt32();
			LevelOld = br.ReadUInt32();
			Exp = br.ReadUInt32();
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
			bw.Write(Level);
			bw.Write(LevelOld);
			bw.Write(Exp);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LevelUpdate {\n";
			ret += "	Level = ";
			try {
				ret += $"{ Indentify(Level) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LevelOld = ";
			try {
				ret += $"{ Indentify(LevelOld) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Exp = ";
			try {
				ret += $"{ Indentify(Exp) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}