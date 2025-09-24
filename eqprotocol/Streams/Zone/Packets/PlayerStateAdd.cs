using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct PlayerState_Struct {
// /*00*/	uint32 spawn_id;
// /*04*/	uint32 state;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the PlayerStateAdd packet structure for EverQuest network communication.
	/// </summary>
	public struct PlayerStateAdd : IEQStruct {
		/// <summary>
		/// Gets or sets the spawnid value.
		/// </summary>
		public uint SpawnId { get; set; }

		/// <summary>
		/// Gets or sets the state value.
		/// </summary>
		public uint State { get; set; }

		/// <summary>
		/// Initializes a new instance of the PlayerStateAdd struct with specified field values.
		/// </summary>
		/// <param name="spawn_id">The spawnid value.</param>
		/// <param name="state">The state value.</param>
		public PlayerStateAdd(uint spawn_id, uint state) : this() {
			SpawnId = spawn_id;
			State = state;
		}

		/// <summary>
		/// Initializes a new instance of the PlayerStateAdd struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public PlayerStateAdd(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the PlayerStateAdd struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public PlayerStateAdd(BinaryReader br) : this() {
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
			State = br.ReadUInt32();
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
			bw.Write(State);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct PlayerStateAdd {\n";
			ret += "	SpawnId = ";
			try {
				ret += $"{ Indentify(SpawnId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	State = ";
			try {
				ret += $"{ Indentify(State) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}