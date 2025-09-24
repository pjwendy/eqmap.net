using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SelectTributeReq_Struct {
// uint32	client_id;	//? maybe action ID?
// uint32	tribute_id;
// uint32	unknown8;	//seen E3 00 00 00
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SenseHeading packet structure for EverQuest network communication.
	/// </summary>
	public struct SenseHeading : IEQStruct {
		/// <summary>
		/// Gets or sets the clientid value.
		/// </summary>
		public uint ClientId { get; set; }

		/// <summary>
		/// Gets or sets the tributeid value.
		/// </summary>
		public uint TributeId { get; set; }

		/// <summary>
		/// Gets or sets the unknown8 value.
		/// </summary>
		public uint Unknown8 { get; set; }

		/// <summary>
		/// Initializes a new instance of the SenseHeading struct with specified field values.
		/// </summary>
		/// <param name="client_id">The clientid value.</param>
		/// <param name="tribute_id">The tributeid value.</param>
		/// <param name="unknown8">The unknown8 value.</param>
		public SenseHeading(uint client_id, uint tribute_id, uint unknown8) : this() {
			ClientId = client_id;
			TributeId = tribute_id;
			Unknown8 = unknown8;
		}

		/// <summary>
		/// Initializes a new instance of the SenseHeading struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SenseHeading(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SenseHeading struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SenseHeading(BinaryReader br) : this() {
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
			ClientId = br.ReadUInt32();
			TributeId = br.ReadUInt32();
			Unknown8 = br.ReadUInt32();
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
			bw.Write(ClientId);
			bw.Write(TributeId);
			bw.Write(Unknown8);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SenseHeading {\n";
			ret += "	ClientId = ";
			try {
				ret += $"{ Indentify(ClientId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeId = ";
			try {
				ret += $"{ Indentify(TributeId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown8 = ";
			try {
				ret += $"{ Indentify(Unknown8) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}