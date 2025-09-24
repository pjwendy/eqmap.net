using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct CorpseDrag_Struct
// {
// /*000*/ char CorpseName[64];
// /*064*/ char DraggerName[64];
// /*128*/ uint8 Unknown128[24];
// /*152*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the CorpseDrag packet structure for EverQuest network communication.
	/// </summary>
	public struct CorpseDrag : IEQStruct {
		/// <summary>
		/// Gets or sets the corpsename value.
		/// </summary>
		public byte[] Corpsename { get; set; }

		/// <summary>
		/// Gets or sets the draggername value.
		/// </summary>
		public byte[] Draggername { get; set; }

		/// <summary>
		/// Gets or sets the unknown128 value.
		/// </summary>
		public byte[] Unknown128 { get; set; }

		/// <summary>
		/// Initializes a new instance of the CorpseDrag struct with specified field values.
		/// </summary>
		/// <param name="CorpseName">The corpsename value.</param>
		/// <param name="DraggerName">The draggername value.</param>
		/// <param name="Unknown128">The unknown128 value.</param>
		public CorpseDrag(byte[] CorpseName, byte[] DraggerName, byte[] Unknown128) : this() {
			Corpsename = CorpseName;
			Draggername = DraggerName;
			Unknown128 = Unknown128;
		}

		/// <summary>
		/// Initializes a new instance of the CorpseDrag struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public CorpseDrag(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the CorpseDrag struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public CorpseDrag(BinaryReader br) : this() {
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
			// TODO: Array reading for Corpsename - implement based on actual array size
			// Corpsename = new byte[size];
			// TODO: Array reading for Draggername - implement based on actual array size
			// Draggername = new byte[size];
			// TODO: Array reading for Unknown128 - implement based on actual array size
			// Unknown128 = new byte[size];
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
			// TODO: Array writing for Corpsename - implement based on actual array size
			// foreach(var item in Corpsename) bw.Write(item);
			// TODO: Array writing for Draggername - implement based on actual array size
			// foreach(var item in Draggername) bw.Write(item);
			// TODO: Array writing for Unknown128 - implement based on actual array size
			// foreach(var item in Unknown128) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct CorpseDrag {\n";
			ret += "	Corpsename = ";
			try {
				ret += $"{ Indentify(Corpsename) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Draggername = ";
			try {
				ret += $"{ Indentify(Draggername) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown128 = ";
			try {
				ret += $"{ Indentify(Unknown128) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}