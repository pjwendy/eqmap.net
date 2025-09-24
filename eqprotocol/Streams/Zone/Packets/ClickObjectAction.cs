using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ClickObjectAction_Struct {
// /*00*/	uint32	player_id;	// Entity Id of player who clicked object
// /*04*/	uint32	drop_id;	// Zone-specified unique object identifier
// /*08*/	uint32	open;		// 1=opening, 0=closing
// /*12*/	uint32	type;		// See object.h, "Object Types"
// /*16*/	uint32	unknown16;	//
// /*20*/	uint32	icon;		// Icon to display for tradeskill containers
// /*24*/	uint32	unknown24;	//
// /*28*/	char	object_name[64]; // Object name to display
// /*92*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ClickObjectAction packet structure for EverQuest network communication.
	/// </summary>
	public struct ClickObjectAction : IEQStruct {
		/// <summary>
		/// Gets or sets the playerid value.
		/// </summary>
		public uint PlayerId { get; set; }

		/// <summary>
		/// Gets or sets the dropid value.
		/// </summary>
		public uint DropId { get; set; }

		/// <summary>
		/// Gets or sets the open value.
		/// </summary>
		public uint Open { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Gets or sets the unknown16 value.
		/// </summary>
		public uint Unknown16 { get; set; }

		/// <summary>
		/// Gets or sets the icon value.
		/// </summary>
		public uint Icon { get; set; }

		/// <summary>
		/// Gets or sets the unknown24 value.
		/// </summary>
		public uint Unknown24 { get; set; }

		/// <summary>
		/// Gets or sets the objectname value.
		/// </summary>
		public byte[] ObjectName { get; set; }

		/// <summary>
		/// Initializes a new instance of the ClickObjectAction struct with specified field values.
		/// </summary>
		/// <param name="player_id">The playerid value.</param>
		/// <param name="drop_id">The dropid value.</param>
		/// <param name="open">The open value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="unknown16">The unknown16 value.</param>
		/// <param name="icon">The icon value.</param>
		/// <param name="unknown24">The unknown24 value.</param>
		/// <param name="object_name">The objectname value.</param>
		public ClickObjectAction(uint player_id, uint drop_id, uint open, uint type, uint unknown16, uint icon, uint unknown24, byte[] object_name) : this() {
			PlayerId = player_id;
			DropId = drop_id;
			Open = open;
			Type = type;
			Unknown16 = unknown16;
			Icon = icon;
			Unknown24 = unknown24;
			ObjectName = object_name;
		}

		/// <summary>
		/// Initializes a new instance of the ClickObjectAction struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ClickObjectAction(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ClickObjectAction struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ClickObjectAction(BinaryReader br) : this() {
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
			PlayerId = br.ReadUInt32();
			DropId = br.ReadUInt32();
			Open = br.ReadUInt32();
			Type = br.ReadUInt32();
			Unknown16 = br.ReadUInt32();
			Icon = br.ReadUInt32();
			Unknown24 = br.ReadUInt32();
			// TODO: Array reading for ObjectName - implement based on actual array size
			// ObjectName = new byte[size];
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
			bw.Write(PlayerId);
			bw.Write(DropId);
			bw.Write(Open);
			bw.Write(Type);
			bw.Write(Unknown16);
			bw.Write(Icon);
			bw.Write(Unknown24);
			// TODO: Array writing for ObjectName - implement based on actual array size
			// foreach(var item in ObjectName) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ClickObjectAction {\n";
			ret += "	PlayerId = ";
			try {
				ret += $"{ Indentify(PlayerId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DropId = ";
			try {
				ret += $"{ Indentify(DropId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Open = ";
			try {
				ret += $"{ Indentify(Open) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown16 = ";
			try {
				ret += $"{ Indentify(Unknown16) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Icon = ";
			try {
				ret += $"{ Indentify(Icon) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown24 = ";
			try {
				ret += $"{ Indentify(Unknown24) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ObjectName = ";
			try {
				ret += $"{ Indentify(ObjectName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}