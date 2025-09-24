using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GMName_Struct {
// char oldname[64];
// char gmname[64];
// char newname[64];
// uint8 badname;
// uint8 unknown[3];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GMSearchCorpse packet structure for EverQuest network communication.
	/// </summary>
	public struct GMSearchCorpse : IEQStruct {
		/// <summary>
		/// Gets or sets the oldname value.
		/// </summary>
		public byte[] Oldname { get; set; }

		/// <summary>
		/// Gets or sets the gmname value.
		/// </summary>
		public byte[] Gmname { get; set; }

		/// <summary>
		/// Gets or sets the newname value.
		/// </summary>
		public byte[] Newname { get; set; }

		/// <summary>
		/// Gets or sets the badname value.
		/// </summary>
		public byte Badname { get; set; }

		/// <summary>
		/// Gets or sets the unknown value.
		/// </summary>
		public byte[] Unknown { get; set; }

		/// <summary>
		/// Initializes a new instance of the GMSearchCorpse struct with specified field values.
		/// </summary>
		/// <param name="oldname">The oldname value.</param>
		/// <param name="gmname">The gmname value.</param>
		/// <param name="newname">The newname value.</param>
		/// <param name="badname">The badname value.</param>
		/// <param name="unknown">The unknown value.</param>
		public GMSearchCorpse(byte[] oldname, byte[] gmname, byte[] newname, byte badname, byte[] unknown) : this() {
			Oldname = oldname;
			Gmname = gmname;
			Newname = newname;
			Badname = badname;
			Unknown = unknown;
		}

		/// <summary>
		/// Initializes a new instance of the GMSearchCorpse struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GMSearchCorpse(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GMSearchCorpse struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GMSearchCorpse(BinaryReader br) : this() {
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
			// TODO: Array reading for Oldname - implement based on actual array size
			// Oldname = new byte[size];
			// TODO: Array reading for Gmname - implement based on actual array size
			// Gmname = new byte[size];
			// TODO: Array reading for Newname - implement based on actual array size
			// Newname = new byte[size];
			Badname = br.ReadByte();
			// TODO: Array reading for Unknown - implement based on actual array size
			// Unknown = new byte[size];
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
			// TODO: Array writing for Oldname - implement based on actual array size
			// foreach(var item in Oldname) bw.Write(item);
			// TODO: Array writing for Gmname - implement based on actual array size
			// foreach(var item in Gmname) bw.Write(item);
			// TODO: Array writing for Newname - implement based on actual array size
			// foreach(var item in Newname) bw.Write(item);
			bw.Write(Badname);
			// TODO: Array writing for Unknown - implement based on actual array size
			// foreach(var item in Unknown) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GMSearchCorpse {\n";
			ret += "	Oldname = ";
			try {
				ret += $"{ Indentify(Oldname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Gmname = ";
			try {
				ret += $"{ Indentify(Gmname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Newname = ";
			try {
				ret += $"{ Indentify(Newname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Badname = ";
			try {
				ret += $"{ Indentify(Badname) },\n";
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