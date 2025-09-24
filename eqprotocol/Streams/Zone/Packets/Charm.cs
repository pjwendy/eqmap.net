using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Charm_Struct {
// /*00*/	uint32	owner_id;
// /*04*/	uint32	pet_id;
// /*08*/	uint32	command;    // 1: make pet, 0: release pet
// /*12*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Charm packet structure for EverQuest network communication.
	/// </summary>
	public struct Charm : IEQStruct {
		/// <summary>
		/// Gets or sets the ownerid value.
		/// </summary>
		public uint OwnerId { get; set; }

		/// <summary>
		/// Gets or sets the petid value.
		/// </summary>
		public uint PetId { get; set; }

		/// <summary>
		/// Gets or sets the command value.
		/// </summary>
		public uint Command { get; set; }

		/// <summary>
		/// Initializes a new instance of the Charm struct with specified field values.
		/// </summary>
		/// <param name="owner_id">The ownerid value.</param>
		/// <param name="pet_id">The petid value.</param>
		/// <param name="command">The command value.</param>
		public Charm(uint owner_id, uint pet_id, uint command) : this() {
			OwnerId = owner_id;
			PetId = pet_id;
			Command = command;
		}

		/// <summary>
		/// Initializes a new instance of the Charm struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Charm(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Charm struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Charm(BinaryReader br) : this() {
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
			OwnerId = br.ReadUInt32();
			PetId = br.ReadUInt32();
			Command = br.ReadUInt32();
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
			bw.Write(OwnerId);
			bw.Write(PetId);
			bw.Write(Command);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Charm {\n";
			ret += "	OwnerId = ";
			try {
				ret += $"{ Indentify(OwnerId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PetId = ";
			try {
				ret += $"{ Indentify(PetId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Command = ";
			try {
				ret += $"{ Indentify(Command) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}