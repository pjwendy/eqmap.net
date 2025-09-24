using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Inspect_Struct {
// uint32 TargetID;
// uint32 PlayerID;
// };

// ENCODE/DECODE Section:
// DECODE(OP_InspectRequest)
// {
// DECODE_LENGTH_EXACT(structs::Inspect_Struct);
// SETUP_DIRECT_DECODE(Inspect_Struct, structs::Inspect_Struct);
// 
// IN(TargetID);
// IN(PlayerID);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the InspectRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct InspectRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the targetid value.
		/// </summary>
		public uint Targetid { get; set; }

		/// <summary>
		/// Gets or sets the playerid value.
		/// </summary>
		public uint Playerid { get; set; }

		/// <summary>
		/// Initializes a new instance of the InspectRequest struct with specified field values.
		/// </summary>
		/// <param name="TargetID">The targetid value.</param>
		/// <param name="PlayerID">The playerid value.</param>
		public InspectRequest(uint TargetID, uint PlayerID) : this() {
			Targetid = TargetID;
			Playerid = PlayerID;
		}

		/// <summary>
		/// Initializes a new instance of the InspectRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public InspectRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the InspectRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public InspectRequest(BinaryReader br) : this() {
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
			Targetid = br.ReadUInt32();
			Playerid = br.ReadUInt32();
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
			bw.Write(Targetid);
			bw.Write(Playerid);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct InspectRequest {\n";
			ret += "	Targetid = ";
			try {
				ret += $"{ Indentify(Targetid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Playerid = ";
			try {
				ret += $"{ Indentify(Playerid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}