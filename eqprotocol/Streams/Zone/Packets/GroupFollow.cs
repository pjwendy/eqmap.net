using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GroupFollow_Struct { // Underfoot Follow Struct
// /*0000*/	char	name1[64];	// inviter
// /*0064*/	char	name2[64];	// invitee
// /*0128*/	uint32	unknown0128;	// Seen 0
// /*0132*/	uint32	unknown0132;	// Group ID or member level?
// /*0136*/	uint32	unknown0136;	// Maybe Voice Chat Channel or Group ID?
// /*0140*/	uint32	unknown0140;	// Seen 0
// /*0144*/	uint32	unknown0144;	// Seen 0
// /*0148*/	uint32	unknown0148;
// /*0152*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_GroupFollow)
// {
// //EQApplicationPacket *in = __packet;
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] Received incoming OP_GroupFollow");
// //Log.Hex(Logs::Netcode, in->pBuffer, in->size);
// DECODE_LENGTH_EXACT(structs::GroupFollow_Struct);
// SETUP_DIRECT_DECODE(GroupGeneric_Struct, structs::GroupFollow_Struct);
// 
// memcpy(emu->name1, eq->name1, sizeof(emu->name1));
// memcpy(emu->name2, eq->name2, sizeof(emu->name2));
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GroupFollow packet structure for EverQuest network communication.
	/// </summary>
	public struct GroupFollow : IEQStruct {
		/// <summary>
		/// Gets or sets the name1 value.
		/// </summary>
		public byte[] Name1 { get; set; }

		/// <summary>
		/// Gets or sets the name2 value.
		/// </summary>
		public byte[] Name2 { get; set; }

		/// <summary>
		/// Initializes a new instance of the GroupFollow struct with specified field values.
		/// </summary>
		/// <param name="name1">The name1 value.</param>
		/// <param name="name2">The name2 value.</param>
		public GroupFollow(byte[] name1, byte[] name2) : this() {
			Name1 = name1;
			Name2 = name2;
		}

		/// <summary>
		/// Initializes a new instance of the GroupFollow struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GroupFollow(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GroupFollow struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GroupFollow(BinaryReader br) : this() {
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
			// TODO: Array reading for Name1 - implement based on actual array size
			// Name1 = new byte[size];
			// TODO: Array reading for Name2 - implement based on actual array size
			// Name2 = new byte[size];
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
			// TODO: Array writing for Name1 - implement based on actual array size
			// foreach(var item in Name1) bw.Write(item);
			// TODO: Array writing for Name2 - implement based on actual array size
			// foreach(var item in Name2) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GroupFollow {\n";
			ret += "	Name1 = ";
			try {
				ret += $"{ Indentify(Name1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Name2 = ";
			try {
				ret += $"{ Indentify(Name2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}