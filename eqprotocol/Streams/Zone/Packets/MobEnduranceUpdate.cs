using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MobManaUpdate_Struct
// {
// /*00*/ uint16	spawn_id;
// /*02*/ uint8		mana;		//Mana Percentage
// /*03*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MobEnduranceUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct MobEnduranceUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the spawnid value.
		/// </summary>
		public ushort SpawnId { get; set; }

		/// <summary>
		/// Gets or sets the mana value.
		/// </summary>
		public byte Mana { get; set; }

		/// <summary>
		/// Initializes a new instance of the MobEnduranceUpdate struct with specified field values.
		/// </summary>
		/// <param name="spawn_id">The spawnid value.</param>
		/// <param name="mana">The mana value.</param>
		public MobEnduranceUpdate(ushort spawn_id, byte mana) : this() {
			SpawnId = spawn_id;
			Mana = mana;
		}

		/// <summary>
		/// Initializes a new instance of the MobEnduranceUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MobEnduranceUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MobEnduranceUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MobEnduranceUpdate(BinaryReader br) : this() {
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
			SpawnId = br.ReadUInt16();
			Mana = br.ReadByte();
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
			bw.Write(Mana);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct MobEnduranceUpdate {\n";
			ret += "	SpawnId = ";
			try {
				ret += $"{ Indentify(SpawnId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mana = ";
			try {
				ret += $"{ Indentify(Mana) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}