using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct LeadershipExpUpdate_Struct {
// /*00*/	double	group_leadership_exp;
// /*08*/	uint32	group_leadership_points;
// /*12*/	uint32	Unknown12;
// /*16*/	double	raid_leadership_exp;
// /*24*/	uint32	raid_leadership_points;
// };

// ENCODE/DECODE Section:
// ENCODE(OP_LeadershipExpUpdate)
// {
// SETUP_DIRECT_ENCODE(LeadershipExpUpdate_Struct, structs::LeadershipExpUpdate_Struct);
// 
// OUT(group_leadership_exp);
// OUT(group_leadership_points);
// OUT(raid_leadership_exp);
// OUT(raid_leadership_points);
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the LeadershipExpUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct LeadershipExpUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the groupleadershipexp value.
		/// </summary>
		public double GroupLeadershipExp { get; set; }

		/// <summary>
		/// Gets or sets the groupleadershippoints value.
		/// </summary>
		public uint GroupLeadershipPoints { get; set; }

		/// <summary>
		/// Gets or sets the unknown12 value.
		/// </summary>
		public uint Unknown12 { get; set; }

		/// <summary>
		/// Gets or sets the raidleadershipexp value.
		/// </summary>
		public double RaidLeadershipExp { get; set; }

		/// <summary>
		/// Gets or sets the raidleadershippoints value.
		/// </summary>
		public uint RaidLeadershipPoints { get; set; }

		/// <summary>
		/// Initializes a new instance of the LeadershipExpUpdate struct with specified field values.
		/// </summary>
		/// <param name="group_leadership_exp">The groupleadershipexp value.</param>
		/// <param name="group_leadership_points">The groupleadershippoints value.</param>
		/// <param name="Unknown12">The unknown12 value.</param>
		/// <param name="raid_leadership_exp">The raidleadershipexp value.</param>
		/// <param name="raid_leadership_points">The raidleadershippoints value.</param>
		public LeadershipExpUpdate(double group_leadership_exp, uint group_leadership_points, uint Unknown12, double raid_leadership_exp, uint raid_leadership_points) : this() {
			GroupLeadershipExp = group_leadership_exp;
			GroupLeadershipPoints = group_leadership_points;
			Unknown12 = Unknown12;
			RaidLeadershipExp = raid_leadership_exp;
			RaidLeadershipPoints = raid_leadership_points;
		}

		/// <summary>
		/// Initializes a new instance of the LeadershipExpUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public LeadershipExpUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the LeadershipExpUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public LeadershipExpUpdate(BinaryReader br) : this() {
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
			GroupLeadershipExp = br.ReadDouble();
			GroupLeadershipPoints = br.ReadUInt32();
			Unknown12 = br.ReadUInt32();
			RaidLeadershipExp = br.ReadDouble();
			RaidLeadershipPoints = br.ReadUInt32();
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
			bw.Write(GroupLeadershipExp);
			bw.Write(GroupLeadershipPoints);
			bw.Write(Unknown12);
			bw.Write(RaidLeadershipExp);
			bw.Write(RaidLeadershipPoints);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct LeadershipExpUpdate {\n";
			ret += "	GroupLeadershipExp = ";
			try {
				ret += $"{ Indentify(GroupLeadershipExp) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	GroupLeadershipPoints = ";
			try {
				ret += $"{ Indentify(GroupLeadershipPoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown12 = ";
			try {
				ret += $"{ Indentify(Unknown12) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	RaidLeadershipExp = ";
			try {
				ret += $"{ Indentify(RaidLeadershipExp) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	RaidLeadershipPoints = ";
			try {
				ret += $"{ Indentify(RaidLeadershipPoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}