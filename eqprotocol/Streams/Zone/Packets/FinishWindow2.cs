using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ClientError_Struct
// {
// /*00001*/	char	type;
// /*00001*/	char	unknown0001[69];
// /*00069*/	char	character_name[64];
// /*00134*/	char	unknown134[192];
// /*00133*/	char	message[31994];
// /*32136*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the FinishWindow2 packet structure for EverQuest network communication.
	/// </summary>
	public struct FinishWindow2 : IEQStruct {
		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public byte Type { get; set; }

		/// <summary>
		/// Gets or sets the charactername value.
		/// </summary>
		public byte[] CharacterName { get; set; }

		/// <summary>
		/// Gets or sets the unknown134 value.
		/// </summary>
		public byte[] Unknown134 { get; set; }

		/// <summary>
		/// Gets or sets the message value.
		/// </summary>
		public byte[] Message { get; set; }

		/// <summary>
		/// Initializes a new instance of the FinishWindow2 struct with specified field values.
		/// </summary>
		/// <param name="type">The type value.</param>
		/// <param name="character_name">The charactername value.</param>
		/// <param name="unknown134">The unknown134 value.</param>
		/// <param name="message">The message value.</param>
		public FinishWindow2(byte type, byte[] character_name, byte[] unknown134, byte[] message) : this() {
			Type = type;
			CharacterName = character_name;
			Unknown134 = unknown134;
			Message = message;
		}

		/// <summary>
		/// Initializes a new instance of the FinishWindow2 struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public FinishWindow2(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the FinishWindow2 struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public FinishWindow2(BinaryReader br) : this() {
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
			Type = br.ReadByte();
			// TODO: Array reading for CharacterName - implement based on actual array size
			// CharacterName = new byte[size];
			// TODO: Array reading for Unknown134 - implement based on actual array size
			// Unknown134 = new byte[size];
			// TODO: Array reading for Message - implement based on actual array size
			// Message = new byte[size];
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
			bw.Write(Type);
			// TODO: Array writing for CharacterName - implement based on actual array size
			// foreach(var item in CharacterName) bw.Write(item);
			// TODO: Array writing for Unknown134 - implement based on actual array size
			// foreach(var item in Unknown134) bw.Write(item);
			// TODO: Array writing for Message - implement based on actual array size
			// foreach(var item in Message) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct FinishWindow2 {\n";
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CharacterName = ";
			try {
				ret += $"{ Indentify(CharacterName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown134 = ";
			try {
				ret += $"{ Indentify(Unknown134) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Message = ";
			try {
				ret += $"{ Indentify(Message) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}