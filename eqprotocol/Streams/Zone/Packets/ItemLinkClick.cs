using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct	ItemViewRequest_Struct {
// /*000*/	uint32	item_id;
// /*004*/	uint32	augments[5];
// /*024*/ uint32	link_hash;
// /*028*/	uint32	unknown028;	//seems to always be 4 on SoF client
// /*032*/	char	unknown032[12];	//probably includes loregroup & evolving info. see Client::MakeItemLink() in zone/inventory.cpp:469
// /*044*/	uint16	icon;
// /*046*/	char	unknown046[2];
// };

// ENCODE/DECODE Section:
// DECODE(OP_ItemLinkClick)
// {
// DECODE_LENGTH_EXACT(structs::ItemViewRequest_Struct);
// SETUP_DIRECT_DECODE(ItemViewRequest_Struct, structs::ItemViewRequest_Struct);
// MEMSET_IN(ItemViewRequest_Struct);
// 
// IN(item_id);
// int r;
// for (r = 0; r < 5; r++) {
// IN(augments[r]);
// }
// IN(link_hash);
// IN(icon);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ItemLinkClick packet structure for EverQuest network communication.
	/// </summary>
	public struct ItemLinkClick : IEQStruct {
		/// <summary>
		/// Gets or sets the itemid value.
		/// </summary>
		public uint ItemId { get; set; }

		/// <summary>
		/// Gets or sets the augments value.
		/// </summary>
		public uint[] Augments { get; set; }

		/// <summary>
		/// Gets or sets the linkhash value.
		/// </summary>
		public uint LinkHash { get; set; }

		/// <summary>
		/// Gets or sets the unknown028 value.
		/// </summary>
		public uint Unknown028 { get; set; }

		/// <summary>
		/// Gets or sets the unknown032 value.
		/// </summary>
		public byte[] Unknown032 { get; set; }

		/// <summary>
		/// Gets or sets the icon value.
		/// </summary>
		public ushort Icon { get; set; }

		/// <summary>
		/// Gets or sets the unknown046 value.
		/// </summary>
		public byte[] Unknown046 { get; set; }

		/// <summary>
		/// Initializes a new instance of the ItemLinkClick struct with specified field values.
		/// </summary>
		/// <param name="item_id">The itemid value.</param>
		/// <param name="augments">The augments value.</param>
		/// <param name="link_hash">The linkhash value.</param>
		/// <param name="unknown028">The unknown028 value.</param>
		/// <param name="unknown032">The unknown032 value.</param>
		/// <param name="icon">The icon value.</param>
		/// <param name="unknown046">The unknown046 value.</param>
		public ItemLinkClick(uint item_id, uint[] augments, uint link_hash, uint unknown028, byte[] unknown032, ushort icon, byte[] unknown046) : this() {
			ItemId = item_id;
			Augments = augments;
			LinkHash = link_hash;
			Unknown028 = unknown028;
			Unknown032 = unknown032;
			Icon = icon;
			Unknown046 = unknown046;
		}

		/// <summary>
		/// Initializes a new instance of the ItemLinkClick struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ItemLinkClick(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ItemLinkClick struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ItemLinkClick(BinaryReader br) : this() {
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
			ItemId = br.ReadUInt32();
			// TODO: Array reading for Augments - implement based on actual array size
			// Augments = new uint[size];
			LinkHash = br.ReadUInt32();
			Unknown028 = br.ReadUInt32();
			// TODO: Array reading for Unknown032 - implement based on actual array size
			// Unknown032 = new byte[size];
			Icon = br.ReadUInt16();
			// TODO: Array reading for Unknown046 - implement based on actual array size
			// Unknown046 = new byte[size];
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
			bw.Write(ItemId);
			// TODO: Array writing for Augments - implement based on actual array size
			// foreach(var item in Augments) bw.Write(item);
			bw.Write(LinkHash);
			bw.Write(Unknown028);
			// TODO: Array writing for Unknown032 - implement based on actual array size
			// foreach(var item in Unknown032) bw.Write(item);
			bw.Write(Icon);
			// TODO: Array writing for Unknown046 - implement based on actual array size
			// foreach(var item in Unknown046) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ItemLinkClick {\n";
			ret += "	ItemId = ";
			try {
				ret += $"{ Indentify(ItemId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Augments = ";
			try {
				ret += $"{ Indentify(Augments) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LinkHash = ";
			try {
				ret += $"{ Indentify(LinkHash) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown028 = ";
			try {
				ret += $"{ Indentify(Unknown028) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown032 = ";
			try {
				ret += $"{ Indentify(Unknown032) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Icon = ";
			try {
				ret += $"{ Indentify(Icon) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown046 = ";
			try {
				ret += $"{ Indentify(Unknown046) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}