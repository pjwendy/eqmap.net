using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct PetCommand_Struct {
// /*000*/ uint32	command;
// /*004*/ uint32	target;
// };

// ENCODE/DECODE Section:
// DECODE(OP_PetCommands)
// {
// DECODE_LENGTH_EXACT(structs::PetCommand_Struct);
// SETUP_DIRECT_DECODE(PetCommand_Struct, structs::PetCommand_Struct);
// 
// IN(command);
// IN(target);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the PetCommands packet structure for EverQuest network communication.
	/// </summary>
	public struct PetCommands : IEQStruct {
		/// <summary>
		/// Gets or sets the command value.
		/// </summary>
		public uint Command { get; set; }

		/// <summary>
		/// Gets or sets the target value.
		/// </summary>
		public uint Target { get; set; }

		/// <summary>
		/// Initializes a new instance of the PetCommands struct with specified field values.
		/// </summary>
		/// <param name="command">The command value.</param>
		/// <param name="target">The target value.</param>
		public PetCommands(uint command, uint target) : this() {
			Command = command;
			Target = target;
		}

		/// <summary>
		/// Initializes a new instance of the PetCommands struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public PetCommands(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the PetCommands struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public PetCommands(BinaryReader br) : this() {
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
			Command = br.ReadUInt32();
			Target = br.ReadUInt32();
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
			bw.Write(Command);
			bw.Write(Target);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct PetCommands {\n";
			ret += "	Command = ";
			try {
				ret += $"{ Indentify(Command) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Target = ";
			try {
				ret += $"{ Indentify(Target) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}