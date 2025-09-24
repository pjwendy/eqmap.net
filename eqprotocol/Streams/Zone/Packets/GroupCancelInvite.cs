using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GroupCancel_Struct {
// /*000*/	char	name1[64];
// /*064*/	char	name2[64];
// /*128*/	uint8	unknown128[20];
// /*148*/	uint32	toggle;
// /*152*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_GroupCancelInvite)
// {
// DECODE_LENGTH_EXACT(structs::GroupCancel_Struct);
// SETUP_DIRECT_DECODE(GroupCancel_Struct, structs::GroupCancel_Struct);
// 
// memcpy(emu->name1, eq->name1, sizeof(emu->name1));
// memcpy(emu->name2, eq->name2, sizeof(emu->name2));
// IN(toggle);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GroupCancelInvite packet structure for EverQuest network communication.
	/// </summary>
	public struct GroupCancelInvite : IEQStruct {
		/// <summary>
		/// Gets or sets the name1 value.
		/// </summary>
		public byte[] Name1 { get; set; }

		/// <summary>
		/// Gets or sets the name2 value.
		/// </summary>
		public byte[] Name2 { get; set; }

		/// <summary>
		/// Gets or sets the unknown128 value.
		/// </summary>
		public byte[] Unknown128 { get; set; }

		/// <summary>
		/// Gets or sets the toggle value.
		/// </summary>
		public uint Toggle { get; set; }

		/// <summary>
		/// Initializes a new instance of the GroupCancelInvite struct with specified field values.
		/// </summary>
		/// <param name="name1">The name1 value.</param>
		/// <param name="name2">The name2 value.</param>
		/// <param name="unknown128">The unknown128 value.</param>
		/// <param name="toggle">The toggle value.</param>
		public GroupCancelInvite(byte[] name1, byte[] name2, byte[] unknown128, uint toggle) : this() {
			Name1 = name1;
			Name2 = name2;
			Unknown128 = unknown128;
			Toggle = toggle;
		}

		/// <summary>
		/// Initializes a new instance of the GroupCancelInvite struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GroupCancelInvite(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GroupCancelInvite struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GroupCancelInvite(BinaryReader br) : this() {
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
			// TODO: Array reading for Unknown128 - implement based on actual array size
			// Unknown128 = new byte[size];
			Toggle = br.ReadUInt32();
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
			// TODO: Array writing for Unknown128 - implement based on actual array size
			// foreach(var item in Unknown128) bw.Write(item);
			bw.Write(Toggle);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GroupCancelInvite {\n";
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
			ret += "	Unknown128 = ";
			try {
				ret += $"{ Indentify(Unknown128) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Toggle = ";
			try {
				ret += $"{ Indentify(Toggle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}