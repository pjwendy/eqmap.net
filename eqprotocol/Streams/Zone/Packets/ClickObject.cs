using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ClickObject_Struct {
// /*00*/	uint32 drop_id;
// /*04*/	uint32 player_id;
// /*08*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ClickObject packet structure for EverQuest network communication.
	/// </summary>
	public struct ClickObject : IEQStruct {
		/// <summary>
		/// Gets or sets the dropid value.
		/// </summary>
		public uint DropId { get; set; }

		/// <summary>
		/// Gets or sets the playerid value.
		/// </summary>
		public uint PlayerId { get; set; }

		/// <summary>
		/// Initializes a new instance of the ClickObject struct with specified field values.
		/// </summary>
		/// <param name="drop_id">The dropid value.</param>
		/// <param name="player_id">The playerid value.</param>
		public ClickObject(uint drop_id, uint player_id) : this() {
			DropId = drop_id;
			PlayerId = player_id;
		}

		/// <summary>
		/// Initializes a new instance of the ClickObject struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ClickObject(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ClickObject struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ClickObject(BinaryReader br) : this() {
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
			DropId = br.ReadUInt32();
			PlayerId = br.ReadUInt32();
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
			bw.Write(DropId);
			bw.Write(PlayerId);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ClickObject {\n";
			ret += "	DropId = ";
			try {
				ret += $"{ Indentify(DropId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PlayerId = ";
			try {
				ret += $"{ Indentify(PlayerId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}