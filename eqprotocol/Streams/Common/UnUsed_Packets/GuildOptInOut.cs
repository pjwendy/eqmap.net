using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildTributeOptInOutReply_Struct {
// /*000*/uint32 guild_id;
// /*004*/char   player_name[64];
// /*068*/uint32 tribute_toggle;//			0 off 1 on
// /*072*/uint32 tribute_trophy_toggle;// 	0 off 1 on		not yet implemented
// /*076*/uint32 no_donations;
// /*080*/uint32 time;
// /*084*/uint32 command;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace EQProtocol.Streams.Common.UnUsed_Packets {
	/// <summary>
	/// Represents the GuildOptInOut packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildOptInOut : IEQStruct {
		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint GuildId { get; set; }

		/// <summary>
		/// Gets or sets the playername value.
		/// </summary>
		public byte[] PlayerName { get; set; }

		/// <summary>
		/// Gets or sets the tributetoggle value.
		/// </summary>
		public uint TributeToggle { get; set; }

		/// <summary>
		/// Gets or sets the tributetrophytoggle value.
		/// </summary>
		public uint TributeTrophyToggle { get; set; }

		/// <summary>
		/// Gets or sets the nodonations value.
		/// </summary>
		public uint NoDonations { get; set; }

		/// <summary>
		/// Gets or sets the time value.
		/// </summary>
		public uint Time { get; set; }

		/// <summary>
		/// Gets or sets the command value.
		/// </summary>
		public uint Command { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildOptInOut struct with specified field values.
		/// </summary>
		/// <param name="guild_id">The guildid value.</param>
		/// <param name="player_name">The playername value.</param>
		/// <param name="tribute_toggle">The tributetoggle value.</param>
		/// <param name="tribute_trophy_toggle">The tributetrophytoggle value.</param>
		/// <param name="no_donations">The nodonations value.</param>
		/// <param name="time">The time value.</param>
		/// <param name="command">The command value.</param>
		public GuildOptInOut(uint guild_id, byte[] player_name, uint tribute_toggle, uint tribute_trophy_toggle, uint no_donations, uint time, uint command) : this() {
			GuildId = guild_id;
			PlayerName = player_name;
			TributeToggle = tribute_toggle;
			TributeTrophyToggle = tribute_trophy_toggle;
			NoDonations = no_donations;
			Time = time;
			Command = command;
		}

		/// <summary>
		/// Initializes a new instance of the GuildOptInOut struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildOptInOut(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildOptInOut struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildOptInOut(BinaryReader br) : this() {
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
			GuildId = br.ReadUInt32();
			// TODO: Array reading for PlayerName - implement based on actual array size
			// PlayerName = new byte[size];
			TributeToggle = br.ReadUInt32();
			TributeTrophyToggle = br.ReadUInt32();
			NoDonations = br.ReadUInt32();
			Time = br.ReadUInt32();
			Command = br.ReadUInt32();
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
			bw.Write(GuildId);
			// TODO: Array writing for PlayerName - implement based on actual array size
			// foreach(var item in PlayerName) bw.Write(item);
			bw.Write(TributeToggle);
			bw.Write(TributeTrophyToggle);
			bw.Write(NoDonations);
			bw.Write(Time);
			bw.Write(Command);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildOptInOut {\n";
			ret += "	GuildId = ";
			try {
				ret += $"{ Indentify(GuildId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PlayerName = ";
			try {
				ret += $"{ Indentify(PlayerName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeToggle = ";
			try {
				ret += $"{ Indentify(TributeToggle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TributeTrophyToggle = ";
			try {
				ret += $"{ Indentify(TributeTrophyToggle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NoDonations = ";
			try {
				ret += $"{ Indentify(NoDonations) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Time = ";
			try {
				ret += $"{ Indentify(Time) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Command = ";
			try {
				ret += $"{ Indentify(Command) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}