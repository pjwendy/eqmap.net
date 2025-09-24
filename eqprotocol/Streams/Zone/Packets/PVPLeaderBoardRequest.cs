using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct PVPLeaderBoardDetailsReply_Struct
// {
// /*000*/ char Name[64];
// /*064*/ uint8 Unknown064[64];
// /*128*/ uint32 Level;
// /*132*/ uint32 Race;
// /*136*/ uint32 Class;
// /*140*/ uint32 GuildID;
// /*144*/ uint32 TotalAA;
// /*148*/ uint32 Unknown148;
// /*152*/ uint32 Kills;
// /*156*/ uint32 Deaths;
// /*160*/ uint32 Infamy;
// /*164*/ uint32 Points;
// /*168*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the PVPLeaderBoardRequest packet structure for EverQuest network communication.
	/// </summary>
	public struct PVPLeaderBoardRequest : IEQStruct {
		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte[] Name { get; set; }

		/// <summary>
		/// Gets or sets the unknown064 value.
		/// </summary>
		public byte[] Unknown064 { get; set; }

		/// <summary>
		/// Gets or sets the level value.
		/// </summary>
		public uint Level { get; set; }

		/// <summary>
		/// Gets or sets the race value.
		/// </summary>
		public uint Race { get; set; }

		/// <summary>
		/// Gets or sets the class value.
		/// </summary>
		public uint Class { get; set; }

		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint Guildid { get; set; }

		/// <summary>
		/// Gets or sets the totalaa value.
		/// </summary>
		public uint Totalaa { get; set; }

		/// <summary>
		/// Gets or sets the unknown148 value.
		/// </summary>
		public uint Unknown148 { get; set; }

		/// <summary>
		/// Gets or sets the kills value.
		/// </summary>
		public uint Kills { get; set; }

		/// <summary>
		/// Gets or sets the deaths value.
		/// </summary>
		public uint Deaths { get; set; }

		/// <summary>
		/// Gets or sets the infamy value.
		/// </summary>
		public uint Infamy { get; set; }

		/// <summary>
		/// Gets or sets the points value.
		/// </summary>
		public uint Points { get; set; }

		/// <summary>
		/// Initializes a new instance of the PVPLeaderBoardRequest struct with specified field values.
		/// </summary>
		/// <param name="Name">The name value.</param>
		/// <param name="Unknown064">The unknown064 value.</param>
		/// <param name="Level">The level value.</param>
		/// <param name="Race">The race value.</param>
		/// <param name="Class">The class value.</param>
		/// <param name="GuildID">The guildid value.</param>
		/// <param name="TotalAA">The totalaa value.</param>
		/// <param name="Unknown148">The unknown148 value.</param>
		/// <param name="Kills">The kills value.</param>
		/// <param name="Deaths">The deaths value.</param>
		/// <param name="Infamy">The infamy value.</param>
		/// <param name="Points">The points value.</param>
		public PVPLeaderBoardRequest(byte[] Name, byte[] Unknown064, uint Level, uint Race, uint Class, uint GuildID, uint TotalAA, uint Unknown148, uint Kills, uint Deaths, uint Infamy, uint Points) : this() {
			Name = Name;
			Unknown064 = Unknown064;
			Level = Level;
			Race = Race;
			Class = Class;
			Guildid = GuildID;
			Totalaa = TotalAA;
			Unknown148 = Unknown148;
			Kills = Kills;
			Deaths = Deaths;
			Infamy = Infamy;
			Points = Points;
		}

		/// <summary>
		/// Initializes a new instance of the PVPLeaderBoardRequest struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public PVPLeaderBoardRequest(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the PVPLeaderBoardRequest struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public PVPLeaderBoardRequest(BinaryReader br) : this() {
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
			// TODO: Array reading for Name - implement based on actual array size
			// Name = new byte[size];
			// TODO: Array reading for Unknown064 - implement based on actual array size
			// Unknown064 = new byte[size];
			Level = br.ReadUInt32();
			Race = br.ReadUInt32();
			Class = br.ReadUInt32();
			Guildid = br.ReadUInt32();
			Totalaa = br.ReadUInt32();
			Unknown148 = br.ReadUInt32();
			Kills = br.ReadUInt32();
			Deaths = br.ReadUInt32();
			Infamy = br.ReadUInt32();
			Points = br.ReadUInt32();
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
			// TODO: Array writing for Name - implement based on actual array size
			// foreach(var item in Name) bw.Write(item);
			// TODO: Array writing for Unknown064 - implement based on actual array size
			// foreach(var item in Unknown064) bw.Write(item);
			bw.Write(Level);
			bw.Write(Race);
			bw.Write(Class);
			bw.Write(Guildid);
			bw.Write(Totalaa);
			bw.Write(Unknown148);
			bw.Write(Kills);
			bw.Write(Deaths);
			bw.Write(Infamy);
			bw.Write(Points);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct PVPLeaderBoardRequest {\n";
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown064 = ";
			try {
				ret += $"{ Indentify(Unknown064) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Level = ";
			try {
				ret += $"{ Indentify(Level) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Race = ";
			try {
				ret += $"{ Indentify(Race) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Class = ";
			try {
				ret += $"{ Indentify(Class) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Guildid = ";
			try {
				ret += $"{ Indentify(Guildid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Totalaa = ";
			try {
				ret += $"{ Indentify(Totalaa) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown148 = ";
			try {
				ret += $"{ Indentify(Unknown148) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Kills = ";
			try {
				ret += $"{ Indentify(Kills) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Deaths = ";
			try {
				ret += $"{ Indentify(Deaths) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Infamy = ";
			try {
				ret += $"{ Indentify(Infamy) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Points = ";
			try {
				ret += $"{ Indentify(Points) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}