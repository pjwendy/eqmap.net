using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct PVPLeaderBoard_Struct
// {
// /*0000*/ uint32 Unknown0000;
// /*0004*/ uint32 MyKills;
// /*0008*/ uint32 MyTotalPoints;
// /*0012*/ uint32 MyRank;
// /*0016*/ uint32 MyDeaths;
// /*0020*/ uint32 MyInfamy;
// /*0024*/ PVPLeaderBoardEntry_Struct Entries[100];
// /*8024*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the QueryUCSServerStatus packet structure for EverQuest network communication.
	/// </summary>
	public struct QueryUCSServerStatus : IEQStruct {
		/// <summary>
		/// Gets or sets the mykills value.
		/// </summary>
		public uint Mykills { get; set; }

		/// <summary>
		/// Gets or sets the mytotalpoints value.
		/// </summary>
		public uint Mytotalpoints { get; set; }

		/// <summary>
		/// Gets or sets the myrank value.
		/// </summary>
		public uint Myrank { get; set; }

		/// <summary>
		/// Gets or sets the mydeaths value.
		/// </summary>
		public uint Mydeaths { get; set; }

		/// <summary>
		/// Gets or sets the myinfamy value.
		/// </summary>
		public uint Myinfamy { get; set; }

		/// <summary>
		/// Gets or sets the entries value.
		/// </summary>
		public uint[] Entries { get; set; }

		/// <summary>
		/// Initializes a new instance of the QueryUCSServerStatus struct with specified field values.
		/// </summary>
		/// <param name="MyKills">The mykills value.</param>
		/// <param name="MyTotalPoints">The mytotalpoints value.</param>
		/// <param name="MyRank">The myrank value.</param>
		/// <param name="MyDeaths">The mydeaths value.</param>
		/// <param name="MyInfamy">The myinfamy value.</param>
		/// <param name="Entries">The entries value.</param>
		public QueryUCSServerStatus(uint MyKills, uint MyTotalPoints, uint MyRank, uint MyDeaths, uint MyInfamy, uint[] Entries) : this() {
			Mykills = MyKills;
			Mytotalpoints = MyTotalPoints;
			Myrank = MyRank;
			Mydeaths = MyDeaths;
			Myinfamy = MyInfamy;
			Entries = Entries;
		}

		/// <summary>
		/// Initializes a new instance of the QueryUCSServerStatus struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public QueryUCSServerStatus(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the QueryUCSServerStatus struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public QueryUCSServerStatus(BinaryReader br) : this() {
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
			Mykills = br.ReadUInt32();
			Mytotalpoints = br.ReadUInt32();
			Myrank = br.ReadUInt32();
			Mydeaths = br.ReadUInt32();
			Myinfamy = br.ReadUInt32();
			// TODO: Array reading for Entries - implement based on actual array size
			// Entries = new uint[size];
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
			bw.Write(Mykills);
			bw.Write(Mytotalpoints);
			bw.Write(Myrank);
			bw.Write(Mydeaths);
			bw.Write(Myinfamy);
			// TODO: Array writing for Entries - implement based on actual array size
			// foreach(var item in Entries) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct QueryUCSServerStatus {\n";
			ret += "	Mykills = ";
			try {
				ret += $"{ Indentify(Mykills) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mytotalpoints = ";
			try {
				ret += $"{ Indentify(Mytotalpoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Myrank = ";
			try {
				ret += $"{ Indentify(Myrank) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mydeaths = ";
			try {
				ret += $"{ Indentify(Mydeaths) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Myinfamy = ";
			try {
				ret += $"{ Indentify(Myinfamy) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Entries = ";
			try {
				ret += $"{ Indentify(Entries) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}