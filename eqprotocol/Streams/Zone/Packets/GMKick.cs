using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GMKick_Struct {
// char name[64];
// char gmname[64];
// uint8 unknown;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GMKick packet structure for EverQuest network communication.
	/// </summary>
	public struct GMKick : IEQStruct {
		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte[] Name { get; set; }

		/// <summary>
		/// Gets or sets the gmname value.
		/// </summary>
		public byte[] Gmname { get; set; }

		/// <summary>
		/// Gets or sets the unknown value.
		/// </summary>
		public byte Unknown { get; set; }

		/// <summary>
		/// Initializes a new instance of the GMKick struct with specified field values.
		/// </summary>
		/// <param name="name">The name value.</param>
		/// <param name="gmname">The gmname value.</param>
		/// <param name="unknown">The unknown value.</param>
		public GMKick(byte[] name, byte[] gmname, byte unknown) : this() {
			Name = name;
			Gmname = gmname;
			Unknown = unknown;
		}

		/// <summary>
		/// Initializes a new instance of the GMKick struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GMKick(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GMKick struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GMKick(BinaryReader br) : this() {
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
			// TODO: Array reading for Name - implement based on actual array size
			// Name = new byte[size];
			// TODO: Array reading for Gmname - implement based on actual array size
			// Gmname = new byte[size];
			Unknown = br.ReadByte();
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
			// TODO: Array writing for Name - implement based on actual array size
			// foreach(var item in Name) bw.Write(item);
			// TODO: Array writing for Gmname - implement based on actual array size
			// foreach(var item in Gmname) bw.Write(item);
			bw.Write(Unknown);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GMKick {\n";
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Gmname = ";
			try {
				ret += $"{ Indentify(Gmname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown = ";
			try {
				ret += $"{ Indentify(Unknown) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}