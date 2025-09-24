using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct PetCommandState_Struct {
// /*00*/	uint32	button_id;
// /*04*/	uint32	state;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the PetCommandState packet structure for EverQuest network communication.
	/// </summary>
	public struct PetCommandState : IEQStruct {
		/// <summary>
		/// Gets or sets the buttonid value.
		/// </summary>
		public uint ButtonId { get; set; }

		/// <summary>
		/// Gets or sets the state value.
		/// </summary>
		public uint State { get; set; }

		/// <summary>
		/// Initializes a new instance of the PetCommandState struct with specified field values.
		/// </summary>
		/// <param name="button_id">The buttonid value.</param>
		/// <param name="state">The state value.</param>
		public PetCommandState(uint button_id, uint state) : this() {
			ButtonId = button_id;
			State = state;
		}

		/// <summary>
		/// Initializes a new instance of the PetCommandState struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public PetCommandState(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the PetCommandState struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public PetCommandState(BinaryReader br) : this() {
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
			ButtonId = br.ReadUInt32();
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
			bw.Write(ButtonId);
			bw.Write(State);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct PetCommandState {\n";
			ret += "	ButtonId = ";
			try {
				ret += $"{ Indentify(ButtonId) },\n";
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