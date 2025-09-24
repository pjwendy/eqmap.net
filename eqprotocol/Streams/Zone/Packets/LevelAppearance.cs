using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LevelAppearance_Struct { //Sends a little graphic on level up
// uint32	spawn_id;
// uint32	parm1;
// uint32	value1a;
// uint32	value1b;
// uint32	parm2;
// uint32	value2a;
// uint32	value2b;
// uint32	parm3;
// uint32	value3a;
// uint32	value3b;
// uint32	parm4;
// uint32	value4a;
// uint32	value4b;
// uint32	parm5;
// uint32	value5a;
// uint32	value5b;
// /*64*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LevelAppearance packet structure for EverQuest network communication.
	/// </summary>
	public struct LevelAppearance : IEQStruct {
		/// <summary>
		/// Gets or sets the spawnid value.
		/// </summary>
		public uint SpawnId { get; set; }

		/// <summary>
		/// Gets or sets the parm1 value.
		/// </summary>
		public uint Parm1 { get; set; }

		/// <summary>
		/// Gets or sets the value1a value.
		/// </summary>
		public uint Value1a { get; set; }

		/// <summary>
		/// Gets or sets the value1b value.
		/// </summary>
		public uint Value1b { get; set; }

		/// <summary>
		/// Gets or sets the parm2 value.
		/// </summary>
		public uint Parm2 { get; set; }

		/// <summary>
		/// Gets or sets the value2a value.
		/// </summary>
		public uint Value2a { get; set; }

		/// <summary>
		/// Gets or sets the value2b value.
		/// </summary>
		public uint Value2b { get; set; }

		/// <summary>
		/// Gets or sets the parm3 value.
		/// </summary>
		public uint Parm3 { get; set; }

		/// <summary>
		/// Gets or sets the value3a value.
		/// </summary>
		public uint Value3a { get; set; }

		/// <summary>
		/// Gets or sets the value3b value.
		/// </summary>
		public uint Value3b { get; set; }

		/// <summary>
		/// Gets or sets the parm4 value.
		/// </summary>
		public uint Parm4 { get; set; }

		/// <summary>
		/// Gets or sets the value4a value.
		/// </summary>
		public uint Value4a { get; set; }

		/// <summary>
		/// Gets or sets the value4b value.
		/// </summary>
		public uint Value4b { get; set; }

		/// <summary>
		/// Gets or sets the parm5 value.
		/// </summary>
		public uint Parm5 { get; set; }

		/// <summary>
		/// Gets or sets the value5a value.
		/// </summary>
		public uint Value5a { get; set; }

		/// <summary>
		/// Gets or sets the value5b value.
		/// </summary>
		public uint Value5b { get; set; }

		/// <summary>
		/// Initializes a new instance of the LevelAppearance struct with specified field values.
		/// </summary>
		/// <param name="spawn_id">The spawnid value.</param>
		/// <param name="parm1">The parm1 value.</param>
		/// <param name="value1a">The value1a value.</param>
		/// <param name="value1b">The value1b value.</param>
		/// <param name="parm2">The parm2 value.</param>
		/// <param name="value2a">The value2a value.</param>
		/// <param name="value2b">The value2b value.</param>
		/// <param name="parm3">The parm3 value.</param>
		/// <param name="value3a">The value3a value.</param>
		/// <param name="value3b">The value3b value.</param>
		/// <param name="parm4">The parm4 value.</param>
		/// <param name="value4a">The value4a value.</param>
		/// <param name="value4b">The value4b value.</param>
		/// <param name="parm5">The parm5 value.</param>
		/// <param name="value5a">The value5a value.</param>
		/// <param name="value5b">The value5b value.</param>
		public LevelAppearance(uint spawn_id, uint parm1, uint value1a, uint value1b, uint parm2, uint value2a, uint value2b, uint parm3, uint value3a, uint value3b, uint parm4, uint value4a, uint value4b, uint parm5, uint value5a, uint value5b) : this() {
			SpawnId = spawn_id;
			Parm1 = parm1;
			Value1a = value1a;
			Value1b = value1b;
			Parm2 = parm2;
			Value2a = value2a;
			Value2b = value2b;
			Parm3 = parm3;
			Value3a = value3a;
			Value3b = value3b;
			Parm4 = parm4;
			Value4a = value4a;
			Value4b = value4b;
			Parm5 = parm5;
			Value5a = value5a;
			Value5b = value5b;
		}

		/// <summary>
		/// Initializes a new instance of the LevelAppearance struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LevelAppearance(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LevelAppearance struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LevelAppearance(BinaryReader br) : this() {
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
			SpawnId = br.ReadUInt32();
			Parm1 = br.ReadUInt32();
			Value1a = br.ReadUInt32();
			Value1b = br.ReadUInt32();
			Parm2 = br.ReadUInt32();
			Value2a = br.ReadUInt32();
			Value2b = br.ReadUInt32();
			Parm3 = br.ReadUInt32();
			Value3a = br.ReadUInt32();
			Value3b = br.ReadUInt32();
			Parm4 = br.ReadUInt32();
			Value4a = br.ReadUInt32();
			Value4b = br.ReadUInt32();
			Parm5 = br.ReadUInt32();
			Value5a = br.ReadUInt32();
			Value5b = br.ReadUInt32();
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
			bw.Write(SpawnId);
			bw.Write(Parm1);
			bw.Write(Value1a);
			bw.Write(Value1b);
			bw.Write(Parm2);
			bw.Write(Value2a);
			bw.Write(Value2b);
			bw.Write(Parm3);
			bw.Write(Value3a);
			bw.Write(Value3b);
			bw.Write(Parm4);
			bw.Write(Value4a);
			bw.Write(Value4b);
			bw.Write(Parm5);
			bw.Write(Value5a);
			bw.Write(Value5b);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LevelAppearance {\n";
			ret += "	SpawnId = ";
			try {
				ret += $"{ Indentify(SpawnId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Parm1 = ";
			try {
				ret += $"{ Indentify(Parm1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value1a = ";
			try {
				ret += $"{ Indentify(Value1a) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value1b = ";
			try {
				ret += $"{ Indentify(Value1b) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Parm2 = ";
			try {
				ret += $"{ Indentify(Parm2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value2a = ";
			try {
				ret += $"{ Indentify(Value2a) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value2b = ";
			try {
				ret += $"{ Indentify(Value2b) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Parm3 = ";
			try {
				ret += $"{ Indentify(Parm3) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value3a = ";
			try {
				ret += $"{ Indentify(Value3a) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value3b = ";
			try {
				ret += $"{ Indentify(Value3b) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Parm4 = ";
			try {
				ret += $"{ Indentify(Parm4) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value4a = ";
			try {
				ret += $"{ Indentify(Value4a) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value4b = ";
			try {
				ret += $"{ Indentify(Value4b) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Parm5 = ";
			try {
				ret += $"{ Indentify(Parm5) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value5a = ";
			try {
				ret += $"{ Indentify(Value5a) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value5b = ";
			try {
				ret += $"{ Indentify(Value5b) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}