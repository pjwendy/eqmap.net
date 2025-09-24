using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct EnduranceUpdate_Struct
// {
// /*00*/ uint32	cur_end;
// /*04*/ uint32	max_end;
// /*08*/ uint16	spawn_id;
// /*10*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the EnduranceUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct EnduranceUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the curend value.
		/// </summary>
		public uint CurEnd { get; set; }

		/// <summary>
		/// Gets or sets the maxend value.
		/// </summary>
		public uint MaxEnd { get; set; }

		/// <summary>
		/// Gets or sets the spawnid value.
		/// </summary>
		public ushort SpawnId { get; set; }

		/// <summary>
		/// Initializes a new instance of the EnduranceUpdate struct with specified field values.
		/// </summary>
		/// <param name="cur_end">The curend value.</param>
		/// <param name="max_end">The maxend value.</param>
		/// <param name="spawn_id">The spawnid value.</param>
		public EnduranceUpdate(uint cur_end, uint max_end, ushort spawn_id) : this() {
			CurEnd = cur_end;
			MaxEnd = max_end;
			SpawnId = spawn_id;
		}

		/// <summary>
		/// Initializes a new instance of the EnduranceUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public EnduranceUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the EnduranceUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public EnduranceUpdate(BinaryReader br) : this() {
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
			CurEnd = br.ReadUInt32();
			MaxEnd = br.ReadUInt32();
			SpawnId = br.ReadUInt16();
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
			bw.Write(CurEnd);
			bw.Write(MaxEnd);
			bw.Write(SpawnId);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct EnduranceUpdate {\n";
			ret += "	CurEnd = ";
			try {
				ret += $"{ Indentify(CurEnd) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MaxEnd = ";
			try {
				ret += $"{ Indentify(MaxEnd) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpawnId = ";
			try {
				ret += $"{ Indentify(SpawnId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}