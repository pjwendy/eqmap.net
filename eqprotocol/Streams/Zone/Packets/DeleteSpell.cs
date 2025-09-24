using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DeleteSpell_Struct
// {
// /*000*/int16	spell_slot;
// /*002*/uint8	unknowndss002[2];
// /*004*/uint8	success;
// /*005*/uint8	unknowndss006[3];
// /*008*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DeleteSpell packet structure for EverQuest network communication.
	/// </summary>
	public struct DeleteSpell : IEQStruct {
		/// <summary>
		/// Gets or sets the spellslot value.
		/// </summary>
		public short SpellSlot { get; set; }

		/// <summary>
		/// Gets or sets the success value.
		/// </summary>
		public byte Success { get; set; }

		/// <summary>
		/// Initializes a new instance of the DeleteSpell struct with specified field values.
		/// </summary>
		/// <param name="spell_slot">The spellslot value.</param>
		/// <param name="success">The success value.</param>
		public DeleteSpell(short spell_slot, byte success) : this() {
			SpellSlot = spell_slot;
			Success = success;
		}

		/// <summary>
		/// Initializes a new instance of the DeleteSpell struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DeleteSpell(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DeleteSpell struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DeleteSpell(BinaryReader br) : this() {
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
			SpellSlot = br.ReadInt16();
			Success = br.ReadByte();
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
			bw.Write(SpellSlot);
			bw.Write(Success);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DeleteSpell {\n";
			ret += "	SpellSlot = ";
			try {
				ret += $"{ Indentify(SpellSlot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Success = ";
			try {
				ret += $"{ Indentify(Success) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}