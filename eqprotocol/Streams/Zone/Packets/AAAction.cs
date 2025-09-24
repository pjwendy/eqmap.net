using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct UseAA_Struct {
// uint32 begin;
// uint32 ability;
// uint32 end;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AAAction packet structure for EverQuest network communication.
	/// </summary>
	public struct AAAction : IEQStruct {
		/// <summary>
		/// Gets or sets the begin value.
		/// </summary>
		public uint Begin { get; set; }

		/// <summary>
		/// Gets or sets the ability value.
		/// </summary>
		public uint Ability { get; set; }

		/// <summary>
		/// Gets or sets the end value.
		/// </summary>
		public uint End { get; set; }

		/// <summary>
		/// Initializes a new instance of the AAAction struct with specified field values.
		/// </summary>
		/// <param name="begin">The begin value.</param>
		/// <param name="ability">The ability value.</param>
		/// <param name="end">The end value.</param>
		public AAAction(uint begin, uint ability, uint end) : this() {
			Begin = begin;
			Ability = ability;
			End = end;
		}

		/// <summary>
		/// Initializes a new instance of the AAAction struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AAAction(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AAAction struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AAAction(BinaryReader br) : this() {
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
			Begin = br.ReadUInt32();
			Ability = br.ReadUInt32();
			End = br.ReadUInt32();
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
			bw.Write(Begin);
			bw.Write(Ability);
			bw.Write(End);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AAAction {\n";
			ret += "	Begin = ";
			try {
				ret += $"{ Indentify(Begin) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Ability = ";
			try {
				ret += $"{ Indentify(Ability) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	End = ";
			try {
				ret += $"{ Indentify(End) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}