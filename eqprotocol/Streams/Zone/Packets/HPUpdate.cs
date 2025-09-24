using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SpawnHPUpdate_Struct
// {
// /*00*/ uint32	cur_hp;               // Id of spawn to update
// /*04*/ int32	max_hp;                 // Maximum hp of spawn
// /*08*/ int16	spawn_id;                 // Current hp of spawn
// /*10*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the HPUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct HPUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the curhp value.
		/// </summary>
		public uint CurHp { get; set; }

		/// <summary>
		/// Gets or sets the maxhp value.
		/// </summary>
		public int MaxHp { get; set; }

		/// <summary>
		/// Gets or sets the spawnid value.
		/// </summary>
		public short SpawnId { get; set; }

		/// <summary>
		/// Initializes a new instance of the HPUpdate struct with specified field values.
		/// </summary>
		/// <param name="cur_hp">The curhp value.</param>
		/// <param name="max_hp">The maxhp value.</param>
		/// <param name="spawn_id">The spawnid value.</param>
		public HPUpdate(uint cur_hp, int max_hp, short spawn_id) : this() {
			CurHp = cur_hp;
			MaxHp = max_hp;
			SpawnId = spawn_id;
		}

		/// <summary>
		/// Initializes a new instance of the HPUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public HPUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the HPUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public HPUpdate(BinaryReader br) : this() {
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
			CurHp = br.ReadUInt32();
			MaxHp = br.ReadInt32();
			SpawnId = br.ReadInt16();
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
			bw.Write(CurHp);
			bw.Write(MaxHp);
			bw.Write(SpawnId);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct HPUpdate {\n";
			ret += "	CurHp = ";
			try {
				ret += $"{ Indentify(CurHp) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MaxHp = ";
			try {
				ret += $"{ Indentify(MaxHp) },\n";
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