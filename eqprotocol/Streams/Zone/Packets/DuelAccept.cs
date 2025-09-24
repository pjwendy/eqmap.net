using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Duel_Struct
// {
// uint32 duel_initiator;
// uint32 duel_target;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DuelAccept packet structure for EverQuest network communication.
	/// </summary>
	public struct DuelAccept : IEQStruct {
		/// <summary>
		/// Gets or sets the duelinitiator value.
		/// </summary>
		public uint DuelInitiator { get; set; }

		/// <summary>
		/// Gets or sets the dueltarget value.
		/// </summary>
		public uint DuelTarget { get; set; }

		/// <summary>
		/// Initializes a new instance of the DuelAccept struct with specified field values.
		/// </summary>
		/// <param name="duel_initiator">The duelinitiator value.</param>
		/// <param name="duel_target">The dueltarget value.</param>
		public DuelAccept(uint duel_initiator, uint duel_target) : this() {
			DuelInitiator = duel_initiator;
			DuelTarget = duel_target;
		}

		/// <summary>
		/// Initializes a new instance of the DuelAccept struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DuelAccept(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DuelAccept struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DuelAccept(BinaryReader br) : this() {
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
			DuelInitiator = br.ReadUInt32();
			DuelTarget = br.ReadUInt32();
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
			bw.Write(DuelInitiator);
			bw.Write(DuelTarget);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DuelAccept {\n";
			ret += "	DuelInitiator = ";
			try {
				ret += $"{ Indentify(DuelInitiator) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DuelTarget = ";
			try {
				ret += $"{ Indentify(DuelTarget) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}