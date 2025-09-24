using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LFG_Appearance_Struct
// {
// /*0000*/ uint32 spawn_id;		// ID of the client
// /*0004*/ uint8 lfg;				// 1=LFG, 0=Not LFG
// /*0005*/ char unknown0005[3];	//
// /*0008*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LFGGetMatchesRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct LFGGetMatchesRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the spawnid value.
		/// </summary>
		public uint SpawnId { get; set; }

		/// <summary>
		/// Gets or sets the lfg value.
		/// </summary>
		public byte Lfg { get; set; }

		/// <summary>
		/// Initializes a new instance of the LFGGetMatchesRequest struct with specified field values.
		/// </summary>
		/// <param name="spawn_id">The spawnid value.</param>
		/// <param name="lfg">The lfg value.</param>
		public LFGGetMatchesRequest(uint spawn_id, byte lfg) : this() {
			SpawnId = spawn_id;
			Lfg = lfg;
		}

		/// <summary>
		/// Initializes a new instance of the LFGGetMatchesRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LFGGetMatchesRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LFGGetMatchesRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LFGGetMatchesRequest(BinaryReader br) : this() {
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
			Lfg = br.ReadByte();
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
			bw.Write(Lfg);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LFGGetMatchesRequest {\n";
			ret += "	SpawnId = ";
			try {
				ret += $"{ Indentify(SpawnId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Lfg = ";
			try {
				ret += $"{ Indentify(Lfg) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}