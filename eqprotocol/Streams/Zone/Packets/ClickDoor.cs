using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ClickDoor_Struct {
// /*000*/	uint8	doorid;
// /*001*/	uint8	unknown001;		// This may be some type of action setting
// /*002*/	uint8	unknown002;		// This is sometimes set after a lever is closed
// /*003*/	uint8	unknown003;		// Seen 0
// /*004*/	uint8	picklockskill;
// /*005*/	uint8	unknown005[3];
// /*008*/ uint32	item_id;
// /*012*/ uint16	player_id;
// /*014*/ uint8	unknown014[2];
// /*016*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ClickDoor packet structure for EverQuest network communication.
	/// </summary>
	public struct ClickDoor : IEQStruct {
		/// <summary>
		/// Gets or sets the doorid value.
		/// </summary>
		public byte Doorid { get; set; }

		/// <summary>
		/// Gets or sets the unknown001 value.
		/// </summary>
		public byte Unknown001 { get; set; }

		/// <summary>
		/// Gets or sets the unknown002 value.
		/// </summary>
		public byte Unknown002 { get; set; }

		/// <summary>
		/// Gets or sets the unknown003 value.
		/// </summary>
		public byte Unknown003 { get; set; }

		/// <summary>
		/// Gets or sets the picklockskill value.
		/// </summary>
		public byte Picklockskill { get; set; }

		/// <summary>
		/// Gets or sets the unknown005 value.
		/// </summary>
		public byte[] Unknown005 { get; set; }

		/// <summary>
		/// Gets or sets the itemid value.
		/// </summary>
		public uint ItemId { get; set; }

		/// <summary>
		/// Gets or sets the playerid value.
		/// </summary>
		public ushort PlayerId { get; set; }

		/// <summary>
		/// Gets or sets the unknown014 value.
		/// </summary>
		public byte[] Unknown014 { get; set; }

		/// <summary>
		/// Initializes a new instance of the ClickDoor struct with specified field values.
		/// </summary>
		/// <param name="doorid">The doorid value.</param>
		/// <param name="unknown001">The unknown001 value.</param>
		/// <param name="unknown002">The unknown002 value.</param>
		/// <param name="unknown003">The unknown003 value.</param>
		/// <param name="picklockskill">The picklockskill value.</param>
		/// <param name="unknown005">The unknown005 value.</param>
		/// <param name="item_id">The itemid value.</param>
		/// <param name="player_id">The playerid value.</param>
		/// <param name="unknown014">The unknown014 value.</param>
		public ClickDoor(byte doorid, byte unknown001, byte unknown002, byte unknown003, byte picklockskill, byte[] unknown005, uint item_id, ushort player_id, byte[] unknown014) : this() {
			Doorid = doorid;
			Unknown001 = unknown001;
			Unknown002 = unknown002;
			Unknown003 = unknown003;
			Picklockskill = picklockskill;
			Unknown005 = unknown005;
			ItemId = item_id;
			PlayerId = player_id;
			Unknown014 = unknown014;
		}

		/// <summary>
		/// Initializes a new instance of the ClickDoor struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ClickDoor(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ClickDoor struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ClickDoor(BinaryReader br) : this() {
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
			Doorid = br.ReadByte();
			Unknown001 = br.ReadByte();
			Unknown002 = br.ReadByte();
			Unknown003 = br.ReadByte();
			Picklockskill = br.ReadByte();
			// TODO: Array reading for Unknown005 - implement based on actual array size
			// Unknown005 = new byte[size];
			ItemId = br.ReadUInt32();
			PlayerId = br.ReadUInt16();
			// TODO: Array reading for Unknown014 - implement based on actual array size
			// Unknown014 = new byte[size];
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
			bw.Write(Doorid);
			bw.Write(Unknown001);
			bw.Write(Unknown002);
			bw.Write(Unknown003);
			bw.Write(Picklockskill);
			// TODO: Array writing for Unknown005 - implement based on actual array size
			// foreach(var item in Unknown005) bw.Write(item);
			bw.Write(ItemId);
			bw.Write(PlayerId);
			// TODO: Array writing for Unknown014 - implement based on actual array size
			// foreach(var item in Unknown014) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ClickDoor {\n";
			ret += "	Doorid = ";
			try {
				ret += $"{ Indentify(Doorid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown001 = ";
			try {
				ret += $"{ Indentify(Unknown001) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown002 = ";
			try {
				ret += $"{ Indentify(Unknown002) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown003 = ";
			try {
				ret += $"{ Indentify(Unknown003) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Picklockskill = ";
			try {
				ret += $"{ Indentify(Picklockskill) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown005 = ";
			try {
				ret += $"{ Indentify(Unknown005) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ItemId = ";
			try {
				ret += $"{ Indentify(ItemId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PlayerId = ";
			try {
				ret += $"{ Indentify(PlayerId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown014 = ";
			try {
				ret += $"{ Indentify(Unknown014) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}