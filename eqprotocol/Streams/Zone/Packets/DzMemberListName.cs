using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DynamicZoneMemberListName_Struct
// {
// /*000*/ uint32 client_id;
// /*004*/ uint32 unknown004;
// /*008*/ uint32 add_name;   // padded bool, 0: remove name, 1: add name with unknown status
// /*012*/ char   name[64];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_DzMemberListName)
// {
// ENCODE_LENGTH_EXACT(DynamicZoneMemberListName_Struct);
// SETUP_DIRECT_ENCODE(DynamicZoneMemberListName_Struct, structs::DynamicZoneMemberListName_Struct);
// 
// OUT(client_id);
// OUT(add_name);
// strn0cpy(eq->name, emu->name, sizeof(eq->name));
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DzMemberListName packet structure for EverQuest network communication.
	/// </summary>
	public struct DzMemberListName : IEQStruct {
		/// <summary>
		/// Gets or sets the clientid value.
		/// </summary>
		public uint ClientId { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public uint Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the addname value.
		/// </summary>
		public uint AddName { get; set; }

		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte[] Name { get; set; }

		/// <summary>
		/// Initializes a new instance of the DzMemberListName struct with specified field values.
		/// </summary>
		/// <param name="client_id">The clientid value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="add_name">The addname value.</param>
		/// <param name="name">The name value.</param>
		public DzMemberListName(uint client_id, uint unknown004, uint add_name, byte[] name) : this() {
			ClientId = client_id;
			Unknown004 = unknown004;
			AddName = add_name;
			Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the DzMemberListName struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DzMemberListName(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DzMemberListName struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DzMemberListName(BinaryReader br) : this() {
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
			ClientId = br.ReadUInt32();
			Unknown004 = br.ReadUInt32();
			AddName = br.ReadUInt32();
			// TODO: Array reading for Name - implement based on actual array size
			// Name = new byte[size];
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
			bw.Write(ClientId);
			bw.Write(Unknown004);
			bw.Write(AddName);
			// TODO: Array writing for Name - implement based on actual array size
			// foreach(var item in Name) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DzMemberListName {\n";
			ret += "	ClientId = ";
			try {
				ret += $"{ Indentify(ClientId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AddName = ";
			try {
				ret += $"{ Indentify(AddName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}