using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Surname_Struct
// {
// /*0000*/	char name[64];
// /*0064*/	uint32 unknown0064;
// /*0068*/	char lastname[32];
// /*0100*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SwapSpell packet structure for EverQuest network communication.
	/// </summary>
	public struct SwapSpell : IEQStruct {
		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte[] Name { get; set; }

		/// <summary>
		/// Gets or sets the lastname value.
		/// </summary>
		public byte[] Lastname { get; set; }

		/// <summary>
		/// Initializes a new instance of the SwapSpell struct with specified field values.
		/// </summary>
		/// <param name="name">The name value.</param>
		/// <param name="lastname">The lastname value.</param>
		public SwapSpell(byte[] name, byte[] lastname) : this() {
			Name = name;
			Lastname = lastname;
		}

		/// <summary>
		/// Initializes a new instance of the SwapSpell struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SwapSpell(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SwapSpell struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SwapSpell(BinaryReader br) : this() {
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
			// TODO: Array reading for Lastname - implement based on actual array size
			// Lastname = new byte[size];
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
			// TODO: Array writing for Lastname - implement based on actual array size
			// foreach(var item in Lastname) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SwapSpell {\n";
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Lastname = ";
			try {
				ret += $"{ Indentify(Lastname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}