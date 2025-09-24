using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ColoredText_Struct {
// uint32 color;
// char msg[1]; //was 1
// /*0???*/ uint8  paddingXXX[3];          // always 0's
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ColoredText packet structure for EverQuest network communication.
	/// </summary>
	public struct ColoredText : IEQStruct {
		/// <summary>
		/// Gets or sets the color value.
		/// </summary>
		public uint Color { get; set; }

		/// <summary>
		/// Gets or sets the msg value.
		/// </summary>
		public byte Msg { get; set; }

		/// <summary>
		/// Gets or sets the paddingxxx value.
		/// </summary>
		public byte[] Paddingxxx { get; set; }

		/// <summary>
		/// Initializes a new instance of the ColoredText struct with specified field values.
		/// </summary>
		/// <param name="color">The color value.</param>
		/// <param name="msg">The msg value.</param>
		/// <param name="paddingXXX">The paddingxxx value.</param>
		public ColoredText(uint color, byte msg, byte[] paddingXXX) : this() {
			Color = color;
			Msg = msg;
			Paddingxxx = paddingXXX;
		}

		/// <summary>
		/// Initializes a new instance of the ColoredText struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ColoredText(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ColoredText struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ColoredText(BinaryReader br) : this() {
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
			Color = br.ReadUInt32();
			Msg = br.ReadByte();
			// TODO: Array reading for Paddingxxx - implement based on actual array size
			// Paddingxxx = new byte[size];
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
			bw.Write(Color);
			bw.Write(Msg);
			// TODO: Array writing for Paddingxxx - implement based on actual array size
			// foreach(var item in Paddingxxx) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ColoredText {\n";
			ret += "	Color = ";
			try {
				ret += $"{ Indentify(Color) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Msg = ";
			try {
				ret += $"{ Indentify(Msg) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Paddingxxx = ";
			try {
				ret += $"{ Indentify(Paddingxxx) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}