using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ConsentResponse_Struct {
// char grantname[64];
// char ownername[64];
// uint8 permission;
// char zonename[64];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ConsentResponse packet structure for EverQuest network communication.
	/// </summary>
	public struct ConsentResponse : IEQStruct {
		/// <summary>
		/// Gets or sets the grantname value.
		/// </summary>
		public byte[] Grantname { get; set; }

		/// <summary>
		/// Gets or sets the ownername value.
		/// </summary>
		public byte[] Ownername { get; set; }

		/// <summary>
		/// Gets or sets the permission value.
		/// </summary>
		public byte Permission { get; set; }

		/// <summary>
		/// Gets or sets the zonename value.
		/// </summary>
		public byte[] Zonename { get; set; }

		/// <summary>
		/// Initializes a new instance of the ConsentResponse struct with specified field values.
		/// </summary>
		/// <param name="grantname">The grantname value.</param>
		/// <param name="ownername">The ownername value.</param>
		/// <param name="permission">The permission value.</param>
		/// <param name="zonename">The zonename value.</param>
		public ConsentResponse(byte[] grantname, byte[] ownername, byte permission, byte[] zonename) : this() {
			Grantname = grantname;
			Ownername = ownername;
			Permission = permission;
			Zonename = zonename;
		}

		/// <summary>
		/// Initializes a new instance of the ConsentResponse struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ConsentResponse(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ConsentResponse struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ConsentResponse(BinaryReader br) : this() {
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
			// TODO: Array reading for Grantname - implement based on actual array size
			// Grantname = new byte[size];
			// TODO: Array reading for Ownername - implement based on actual array size
			// Ownername = new byte[size];
			Permission = br.ReadByte();
			// TODO: Array reading for Zonename - implement based on actual array size
			// Zonename = new byte[size];
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
			// TODO: Array writing for Grantname - implement based on actual array size
			// foreach(var item in Grantname) bw.Write(item);
			// TODO: Array writing for Ownername - implement based on actual array size
			// foreach(var item in Ownername) bw.Write(item);
			bw.Write(Permission);
			// TODO: Array writing for Zonename - implement based on actual array size
			// foreach(var item in Zonename) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ConsentResponse {\n";
			ret += "	Grantname = ";
			try {
				ret += $"{ Indentify(Grantname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Ownername = ";
			try {
				ret += $"{ Indentify(Ownername) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Permission = ";
			try {
				ret += $"{ Indentify(Permission) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Zonename = ";
			try {
				ret += $"{ Indentify(Zonename) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}