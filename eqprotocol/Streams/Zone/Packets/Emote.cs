using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Emote_Struct {
// /*0000*/	uint32 unknown01;
// /*0004*/	char message[1024]; // was 1024
// /*1028*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_Emote)
// {
// unsigned char *__eq_buffer = __packet->pBuffer;
// 
// std::string old_message = (char *)&__eq_buffer[4]; // unknown01 offset
// std::string new_message;
// UFToServerSayLink(new_message, old_message);
// 
// __packet->size = sizeof(Emote_Struct);
// __packet->pBuffer = new unsigned char[__packet->size];
// 
// char *InBuffer = (char *)__packet->pBuffer;
// 
// memcpy(InBuffer, __eq_buffer, 4);
// InBuffer += 4;
// strcpy(InBuffer, new_message.substr(0, 1023).c_str());
// InBuffer[1023] = '\0';
// 
// delete[] __eq_buffer;
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Emote packet structure for EverQuest network communication.
	/// </summary>
	public struct Emote : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown01 value.
		/// </summary>
		public uint Unknown01 { get; set; }

		/// <summary>
		/// Gets or sets the message value.
		/// </summary>
		public byte[] Message { get; set; }

		/// <summary>
		/// Initializes a new instance of the Emote struct with specified field values.
		/// </summary>
		/// <param name="unknown01">The unknown01 value.</param>
		/// <param name="message">The message value.</param>
		public Emote(uint unknown01, byte[] message) : this() {
			Unknown01 = unknown01;
			Message = message;
		}

		/// <summary>
		/// Initializes a new instance of the Emote struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Emote(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Emote struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Emote(BinaryReader br) : this() {
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
			Unknown01 = br.ReadUInt32();
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
			bw.Write(Unknown01);
			// TODO: Array writing for Message - implement based on actual array size
			// foreach(var item in Message) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Emote {\n";
			ret += "	Unknown01 = ";
			try {
				ret += $"{ Indentify(Unknown01) },\n";
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