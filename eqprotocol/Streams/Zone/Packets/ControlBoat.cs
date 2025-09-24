using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Stamina_Struct {
// /*00*/ uint32 food;                     // (low more hungry 127-0)
// /*02*/ uint32 water;                    // (low more thirsty 127-0)
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ControlBoat packet structure for EverQuest network communication.
	/// </summary>
	public struct ControlBoat : IEQStruct {
		/// <summary>
		/// Gets or sets the food value.
		/// </summary>
		public uint Food { get; set; }

		/// <summary>
		/// Gets or sets the water value.
		/// </summary>
		public uint Water { get; set; }

		/// <summary>
		/// Initializes a new instance of the ControlBoat struct with specified field values.
		/// </summary>
		/// <param name="food">The food value.</param>
		/// <param name="water">The water value.</param>
		public ControlBoat(uint food, uint water) : this() {
			Food = food;
			Water = water;
		}

		/// <summary>
		/// Initializes a new instance of the ControlBoat struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ControlBoat(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ControlBoat struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ControlBoat(BinaryReader br) : this() {
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
			Food = br.ReadUInt32();
			Water = br.ReadUInt32();
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
			bw.Write(Food);
			bw.Write(Water);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ControlBoat {\n";
			ret += "	Food = ";
			try {
				ret += $"{ Indentify(Food) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Water = ";
			try {
				ret += $"{ Indentify(Water) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}