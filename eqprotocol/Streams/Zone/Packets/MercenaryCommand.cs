using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MercenaryCommand_Struct {
// /*0000*/	uint32	MercCommand;	// Seen 0 (zone in with no merc or suspended), 1 (dismiss merc), 5 (normal state), 36 (zone in with merc)
// /*0004*/	int32	Option;			// Seen -1 (zone in with no merc), 0 (setting to passive stance), 1 (normal or setting to balanced stance)
// /*0008*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MercenaryCommand packet structure for EverQuest network communication.
	/// </summary>
	public struct MercenaryCommand : IEQStruct {
		/// <summary>
		/// Gets or sets the merccommand value.
		/// </summary>
		public uint Merccommand { get; set; }

		/// <summary>
		/// Gets or sets the option value.
		/// </summary>
		public int Option { get; set; }

		/// <summary>
		/// Initializes a new instance of the MercenaryCommand struct with specified field values.
		/// </summary>
		/// <param name="MercCommand">The merccommand value.</param>
		/// <param name="Option">The option value.</param>
		public MercenaryCommand(uint MercCommand, int Option) : this() {
			Merccommand = MercCommand;
			Option = Option;
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryCommand struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MercenaryCommand(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MercenaryCommand struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MercenaryCommand(BinaryReader br) : this() {
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
			Merccommand = br.ReadUInt32();
			Option = br.ReadInt32();
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
			bw.Write(Merccommand);
			bw.Write(Option);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct MercenaryCommand {\n";
			ret += "	Merccommand = ";
			try {
				ret += $"{ Indentify(Merccommand) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Option = ";
			try {
				ret += $"{ Indentify(Option) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}