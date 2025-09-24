using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct CancelTrade_Struct {
// /*00*/	uint32 fromid;
// /*04*/	uint32 action;
// /*08*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_CancelTrade)
// {
// ENCODE_LENGTH_EXACT(CancelTrade_Struct);
// SETUP_DIRECT_ENCODE(CancelTrade_Struct, structs::CancelTrade_Struct);
// 
// OUT(fromid);
// OUT(action);
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the CancelTrade packet structure for EverQuest network communication.
	/// </summary>
	public struct CancelTrade : IEQStruct {
		/// <summary>
		/// Gets or sets the fromid value.
		/// </summary>
		public uint Fromid { get; set; }

		/// <summary>
		/// Gets or sets the action value.
		/// </summary>
		public uint Action { get; set; }

		/// <summary>
		/// Initializes a new instance of the CancelTrade struct with specified field values.
		/// </summary>
		/// <param name="fromid">The fromid value.</param>
		/// <param name="action">The action value.</param>
		public CancelTrade(uint fromid, uint action) : this() {
			Fromid = fromid;
			Action = action;
		}

		/// <summary>
		/// Initializes a new instance of the CancelTrade struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public CancelTrade(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the CancelTrade struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public CancelTrade(BinaryReader br) : this() {
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
			Fromid = br.ReadUInt32();
			Action = br.ReadUInt32();
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
			bw.Write(Fromid);
			bw.Write(Action);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct CancelTrade {\n";
			ret += "	Fromid = ";
			try {
				ret += $"{ Indentify(Fromid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}