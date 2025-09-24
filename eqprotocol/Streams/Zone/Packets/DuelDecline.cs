using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DuelResponse_Struct
// {
// uint32 target_id;
// uint32 entity_id;
// uint32 unknown;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DuelDecline packet structure for EverQuest network communication.
	/// </summary>
	public struct DuelDecline : IEQStruct {
		/// <summary>
		/// Gets or sets the targetid value.
		/// </summary>
		public uint TargetId { get; set; }

		/// <summary>
		/// Gets or sets the entityid value.
		/// </summary>
		public uint EntityId { get; set; }

		/// <summary>
		/// Gets or sets the unknown value.
		/// </summary>
		public uint Unknown { get; set; }

		/// <summary>
		/// Initializes a new instance of the DuelDecline struct with specified field values.
		/// </summary>
		/// <param name="target_id">The targetid value.</param>
		/// <param name="entity_id">The entityid value.</param>
		/// <param name="unknown">The unknown value.</param>
		public DuelDecline(uint target_id, uint entity_id, uint unknown) : this() {
			TargetId = target_id;
			EntityId = entity_id;
			Unknown = unknown;
		}

		/// <summary>
		/// Initializes a new instance of the DuelDecline struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DuelDecline(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DuelDecline struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DuelDecline(BinaryReader br) : this() {
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
			TargetId = br.ReadUInt32();
			EntityId = br.ReadUInt32();
			Unknown = br.ReadUInt32();
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
			bw.Write(TargetId);
			bw.Write(EntityId);
			bw.Write(Unknown);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DuelDecline {\n";
			ret += "	TargetId = ";
			try {
				ret += $"{ Indentify(TargetId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	EntityId = ";
			try {
				ret += $"{ Indentify(EntityId) },\n";
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