using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ManaUpdate_Struct
// {
// /*00*/ uint32	cur_mana;
// /*04*/ uint32	max_mana;
// /*08*/ uint16	spawn_id;
// /*10*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ManaUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct ManaUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the curmana value.
		/// </summary>
		public uint CurMana { get; set; }

		/// <summary>
		/// Gets or sets the maxmana value.
		/// </summary>
		public uint MaxMana { get; set; }

		/// <summary>
		/// Gets or sets the spawnid value.
		/// </summary>
		public ushort SpawnId { get; set; }

		/// <summary>
		/// Initializes a new instance of the ManaUpdate struct with specified field values.
		/// </summary>
		/// <param name="cur_mana">The curmana value.</param>
		/// <param name="max_mana">The maxmana value.</param>
		/// <param name="spawn_id">The spawnid value.</param>
		public ManaUpdate(uint cur_mana, uint max_mana, ushort spawn_id) : this() {
			CurMana = cur_mana;
			MaxMana = max_mana;
			SpawnId = spawn_id;
		}

		/// <summary>
		/// Initializes a new instance of the ManaUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ManaUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ManaUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ManaUpdate(BinaryReader br) : this() {
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
			CurMana = br.ReadUInt32();
			MaxMana = br.ReadUInt32();
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
			bw.Write(CurMana);
			bw.Write(MaxMana);
			bw.Write(SpawnId);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ManaUpdate {\n";
			ret += "	CurMana = ";
			try {
				ret += $"{ Indentify(CurMana) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MaxMana = ";
			try {
				ret += $"{ Indentify(MaxMana) },\n";
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