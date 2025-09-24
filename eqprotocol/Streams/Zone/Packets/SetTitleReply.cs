using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SetTitleReply_Struct {
// uint32	is_suffix;	//guessed: 0 = prefix, 1 = suffix
// char	title[32];
// uint32	entity_id;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SetTitleReply packet structure for EverQuest network communication.
	/// </summary>
	public struct SetTitleReply : IEQStruct {
		/// <summary>
		/// Gets or sets the issuffix value.
		/// </summary>
		public uint IsSuffix { get; set; }

		/// <summary>
		/// Gets or sets the title value.
		/// </summary>
		public byte[] Title { get; set; }

		/// <summary>
		/// Gets or sets the entityid value.
		/// </summary>
		public uint EntityId { get; set; }

		/// <summary>
		/// Initializes a new instance of the SetTitleReply struct with specified field values.
		/// </summary>
		/// <param name="is_suffix">The issuffix value.</param>
		/// <param name="title">The title value.</param>
		/// <param name="entity_id">The entityid value.</param>
		public SetTitleReply(uint is_suffix, byte[] title, uint entity_id) : this() {
			IsSuffix = is_suffix;
			Title = title;
			EntityId = entity_id;
		}

		/// <summary>
		/// Initializes a new instance of the SetTitleReply struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SetTitleReply(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SetTitleReply struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SetTitleReply(BinaryReader br) : this() {
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
			IsSuffix = br.ReadUInt32();
			// TODO: Array reading for Title - implement based on actual array size
			// Title = new byte[size];
			EntityId = br.ReadUInt32();
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
			bw.Write(IsSuffix);
			// TODO: Array writing for Title - implement based on actual array size
			// foreach(var item in Title) bw.Write(item);
			bw.Write(EntityId);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SetTitleReply {\n";
			ret += "	IsSuffix = ";
			try {
				ret += $"{ Indentify(IsSuffix) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Title = ";
			try {
				ret += $"{ Indentify(Title) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	EntityId = ";
			try {
				ret += $"{ Indentify(EntityId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}