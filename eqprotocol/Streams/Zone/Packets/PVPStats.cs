using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct PVPStats_Struct
// {
// /*0000*/ uint32 Kills;
// /*0004*/ uint32 Deaths;
// /*0008*/ uint32 PVPPointsAvailable;
// /*0012*/ uint32 TotalPVPPoints;
// /*0016*/ uint32 BestKillStreak;
// /*0020*/ uint32 WorstDeathStreak;
// /*0024*/ uint32 CurrentKillStreak;
// /*0028*/ uint32 Infamy;
// /*0032*/ uint32 Vitality;
// /*0036*/ PVPStatsEntry_Struct LastDeath;
// /*0124*/ PVPStatsEntry_Struct LastKill;
// /*0212*/ PVPStatsEntry_Struct KillsLast24Hours[50];
// /*4612*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the PVPStats packet structure for EverQuest network communication.
	/// </summary>
	public struct PVPStats : IEQStruct {
		/// <summary>
		/// Gets or sets the kills value.
		/// </summary>
		public uint Kills { get; set; }

		/// <summary>
		/// Gets or sets the deaths value.
		/// </summary>
		public uint Deaths { get; set; }

		/// <summary>
		/// Gets or sets the pvppointsavailable value.
		/// </summary>
		public uint Pvppointsavailable { get; set; }

		/// <summary>
		/// Gets or sets the totalpvppoints value.
		/// </summary>
		public uint Totalpvppoints { get; set; }

		/// <summary>
		/// Gets or sets the bestkillstreak value.
		/// </summary>
		public uint Bestkillstreak { get; set; }

		/// <summary>
		/// Gets or sets the worstdeathstreak value.
		/// </summary>
		public uint Worstdeathstreak { get; set; }

		/// <summary>
		/// Gets or sets the currentkillstreak value.
		/// </summary>
		public uint Currentkillstreak { get; set; }

		/// <summary>
		/// Gets or sets the infamy value.
		/// </summary>
		public uint Infamy { get; set; }

		/// <summary>
		/// Gets or sets the vitality value.
		/// </summary>
		public uint Vitality { get; set; }

		/// <summary>
		/// Gets or sets the lastdeath value.
		/// </summary>
		public uint Lastdeath { get; set; }

		/// <summary>
		/// Gets or sets the lastkill value.
		/// </summary>
		public uint Lastkill { get; set; }

		/// <summary>
		/// Gets or sets the killslast24hours value.
		/// </summary>
		public uint[] Killslast24hours { get; set; }

		/// <summary>
		/// Initializes a new instance of the PVPStats struct with specified field values.
		/// </summary>
		/// <param name="Kills">The kills value.</param>
		/// <param name="Deaths">The deaths value.</param>
		/// <param name="PVPPointsAvailable">The pvppointsavailable value.</param>
		/// <param name="TotalPVPPoints">The totalpvppoints value.</param>
		/// <param name="BestKillStreak">The bestkillstreak value.</param>
		/// <param name="WorstDeathStreak">The worstdeathstreak value.</param>
		/// <param name="CurrentKillStreak">The currentkillstreak value.</param>
		/// <param name="Infamy">The infamy value.</param>
		/// <param name="Vitality">The vitality value.</param>
		/// <param name="LastDeath">The lastdeath value.</param>
		/// <param name="LastKill">The lastkill value.</param>
		/// <param name="KillsLast24Hours">The killslast24hours value.</param>
		public PVPStats(uint Kills, uint Deaths, uint PVPPointsAvailable, uint TotalPVPPoints, uint BestKillStreak, uint WorstDeathStreak, uint CurrentKillStreak, uint Infamy, uint Vitality, uint LastDeath, uint LastKill, uint[] KillsLast24Hours) : this() {
			Kills = Kills;
			Deaths = Deaths;
			Pvppointsavailable = PVPPointsAvailable;
			Totalpvppoints = TotalPVPPoints;
			Bestkillstreak = BestKillStreak;
			Worstdeathstreak = WorstDeathStreak;
			Currentkillstreak = CurrentKillStreak;
			Infamy = Infamy;
			Vitality = Vitality;
			Lastdeath = LastDeath;
			Lastkill = LastKill;
			Killslast24hours = KillsLast24Hours;
		}

		/// <summary>
		/// Initializes a new instance of the PVPStats struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public PVPStats(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the PVPStats struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public PVPStats(BinaryReader br) : this() {
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
			Kills = br.ReadUInt32();
			Deaths = br.ReadUInt32();
			Pvppointsavailable = br.ReadUInt32();
			Totalpvppoints = br.ReadUInt32();
			Bestkillstreak = br.ReadUInt32();
			Worstdeathstreak = br.ReadUInt32();
			Currentkillstreak = br.ReadUInt32();
			Infamy = br.ReadUInt32();
			Vitality = br.ReadUInt32();
			Lastdeath = br.ReadUInt32();
			Lastkill = br.ReadUInt32();
			// TODO: Array reading for Killslast24hours - implement based on actual array size
			// Killslast24hours = new uint[size];
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
			bw.Write(Kills);
			bw.Write(Deaths);
			bw.Write(Pvppointsavailable);
			bw.Write(Totalpvppoints);
			bw.Write(Bestkillstreak);
			bw.Write(Worstdeathstreak);
			bw.Write(Currentkillstreak);
			bw.Write(Infamy);
			bw.Write(Vitality);
			bw.Write(Lastdeath);
			bw.Write(Lastkill);
			// TODO: Array writing for Killslast24hours - implement based on actual array size
			// foreach(var item in Killslast24hours) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct PVPStats {\n";
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
			ret += "	Pvppointsavailable = ";
			try {
				ret += $"{ Indentify(Pvppointsavailable) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Totalpvppoints = ";
			try {
				ret += $"{ Indentify(Totalpvppoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Bestkillstreak = ";
			try {
				ret += $"{ Indentify(Bestkillstreak) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Worstdeathstreak = ";
			try {
				ret += $"{ Indentify(Worstdeathstreak) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Currentkillstreak = ";
			try {
				ret += $"{ Indentify(Currentkillstreak) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Infamy = ";
			try {
				ret += $"{ Indentify(Infamy) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Vitality = ";
			try {
				ret += $"{ Indentify(Vitality) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Lastdeath = ";
			try {
				ret += $"{ Indentify(Lastdeath) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Lastkill = ";
			try {
				ret += $"{ Indentify(Lastkill) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Killslast24hours = ";
			try {
				ret += $"{ Indentify(Killslast24hours) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}