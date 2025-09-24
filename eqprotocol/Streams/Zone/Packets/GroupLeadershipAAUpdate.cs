using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GroupLeadershipAAUpdate_Struct
// {
// /*000*/	uint32	Unknown000;	// GroupID or Leader EntityID ?
// /*004*/	GroupLeadershipAA_Struct LeaderAAs;
// /*068*/	uint32	Unknown068[49];	// Was 63
// /*264*/	uint32	NPCMarkerID;
// /*268*/	uint32	Unknown268[13];
// /*320*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GroupLeadershipAAUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct GroupLeadershipAAUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the leaderaas value.
		/// </summary>
		public uint Leaderaas { get; set; }

		/// <summary>
		/// Gets or sets the unknown068 value.
		/// </summary>
		public uint[] Unknown068 { get; set; }

		/// <summary>
		/// Gets or sets the npcmarkerid value.
		/// </summary>
		public uint Npcmarkerid { get; set; }

		/// <summary>
		/// Gets or sets the unknown268 value.
		/// </summary>
		public uint[] Unknown268 { get; set; }

		/// <summary>
		/// Initializes a new instance of the GroupLeadershipAAUpdate struct with specified field values.
		/// </summary>
		/// <param name="Unknown000">The unknown000 value.</param>
		/// <param name="LeaderAAs">The leaderaas value.</param>
		/// <param name="Unknown068">The unknown068 value.</param>
		/// <param name="NPCMarkerID">The npcmarkerid value.</param>
		/// <param name="Unknown268">The unknown268 value.</param>
		public GroupLeadershipAAUpdate(uint Unknown000, uint LeaderAAs, uint[] Unknown068, uint NPCMarkerID, uint[] Unknown268) : this() {
			Unknown000 = Unknown000;
			Leaderaas = LeaderAAs;
			Unknown068 = Unknown068;
			Npcmarkerid = NPCMarkerID;
			Unknown268 = Unknown268;
		}

		/// <summary>
		/// Initializes a new instance of the GroupLeadershipAAUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GroupLeadershipAAUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GroupLeadershipAAUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GroupLeadershipAAUpdate(BinaryReader br) : this() {
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
			Unknown000 = br.ReadUInt32();
			Leaderaas = br.ReadUInt32();
			// TODO: Array reading for Unknown068 - implement based on actual array size
			// Unknown068 = new uint[size];
			Npcmarkerid = br.ReadUInt32();
			// TODO: Array reading for Unknown268 - implement based on actual array size
			// Unknown268 = new uint[size];
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
			bw.Write(Unknown000);
			bw.Write(Leaderaas);
			// TODO: Array writing for Unknown068 - implement based on actual array size
			// foreach(var item in Unknown068) bw.Write(item);
			bw.Write(Npcmarkerid);
			// TODO: Array writing for Unknown268 - implement based on actual array size
			// foreach(var item in Unknown268) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GroupLeadershipAAUpdate {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Leaderaas = ";
			try {
				ret += $"{ Indentify(Leaderaas) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown068 = ";
			try {
				ret += $"{ Indentify(Unknown068) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Npcmarkerid = ";
			try {
				ret += $"{ Indentify(Npcmarkerid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown268 = ";
			try {
				ret += $"{ Indentify(Unknown268) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}