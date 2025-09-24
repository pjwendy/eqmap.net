using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct FindPersonResult_Struct {
// FindPerson_Point dest;
// FindPerson_Point path[0];	//last element must be the same as dest
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the FindPersonReply packet structure for EverQuest network communication.
	/// </summary>
	public struct FindPersonReply : IEQStruct {
		/// <summary>
		/// Gets or sets the dest value.
		/// </summary>
		public uint Dest { get; set; }

		/// <summary>
		/// Gets or sets the path value.
		/// </summary>
		public uint Path { get; set; }

		/// <summary>
		/// Initializes a new instance of the FindPersonReply struct with specified field values.
		/// </summary>
		/// <param name="dest">The dest value.</param>
		/// <param name="path">The path value.</param>
		public FindPersonReply(uint dest, uint path) : this() {
			Dest = dest;
			Path = path;
		}

		/// <summary>
		/// Initializes a new instance of the FindPersonReply struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public FindPersonReply(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the FindPersonReply struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public FindPersonReply(BinaryReader br) : this() {
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
			Dest = br.ReadUInt32();
			Path = br.ReadUInt32();
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
			bw.Write(Dest);
			bw.Write(Path);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct FindPersonReply {\n";
			ret += "	Dest = ";
			try {
				ret += $"{ Indentify(Dest) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Path = ";
			try {
				ret += $"{ Indentify(Path) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}