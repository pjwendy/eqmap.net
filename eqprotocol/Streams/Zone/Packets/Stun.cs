using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Stun_Struct { // 8 bytes total
// /*000*/	uint32	duration; // Duration of stun
// /*004*/	uint8	unknown004; // seen 0
// /*005*/	uint8	unknown005; // seen 163
// /*006*/	uint8	unknown006; // seen 67
// /*007*/	uint8	unknown007; // seen 0
// /*008*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_Stun)
// {
// ENCODE_LENGTH_EXACT(Stun_Struct);
// SETUP_DIRECT_ENCODE(Stun_Struct, structs::Stun_Struct);
// 
// OUT(duration);
// eq->unknown005 = 163;
// eq->unknown006 = 67;
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Stun packet structure for EverQuest network communication.
	/// </summary>
	public struct Stun : IEQStruct {
		/// <summary>
		/// Gets or sets the duration value.
		/// </summary>
		public uint Duration { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public byte Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the unknown005 value.
		/// </summary>
		public byte Unknown005 { get; set; }

		/// <summary>
		/// Gets or sets the unknown006 value.
		/// </summary>
		public byte Unknown006 { get; set; }

		/// <summary>
		/// Gets or sets the unknown007 value.
		/// </summary>
		public byte Unknown007 { get; set; }

		/// <summary>
		/// Initializes a new instance of the Stun struct with specified field values.
		/// </summary>
		/// <param name="duration">The duration value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="unknown005">The unknown005 value.</param>
		/// <param name="unknown006">The unknown006 value.</param>
		/// <param name="unknown007">The unknown007 value.</param>
		public Stun(uint duration, byte unknown004, byte unknown005, byte unknown006, byte unknown007) : this() {
			Duration = duration;
			Unknown004 = unknown004;
			Unknown005 = unknown005;
			Unknown006 = unknown006;
			Unknown007 = unknown007;
		}

		/// <summary>
		/// Initializes a new instance of the Stun struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Stun(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Stun struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Stun(BinaryReader br) : this() {
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
			Duration = br.ReadUInt32();
			Unknown004 = br.ReadByte();
			Unknown005 = br.ReadByte();
			Unknown006 = br.ReadByte();
			Unknown007 = br.ReadByte();
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
			bw.Write(Duration);
			bw.Write(Unknown004);
			bw.Write(Unknown005);
			bw.Write(Unknown006);
			bw.Write(Unknown007);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Stun {\n";
			ret += "	Duration = ";
			try {
				ret += $"{ Indentify(Duration) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown005 = ";
			try {
				ret += $"{ Indentify(Unknown005) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown006 = ";
			try {
				ret += $"{ Indentify(Unknown006) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown007 = ";
			try {
				ret += $"{ Indentify(Unknown007) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}