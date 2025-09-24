using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GMTrainSkillConfirm_Struct {	// SoF only
// /*000*/	uint32	SkillID;
// /*004*/	uint32	Cost;
// /*008*/	uint8	NewSkill;	// Set to 1 for 'You have learned the basics' message.
// /*009*/	char	TrainerName[64];
// /*073*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GMTrainSkill packet structure for EverQuest network communication.
	/// </summary>
	public struct GMTrainSkill : IEQStruct {
		/// <summary>
		/// Gets or sets the skillid value.
		/// </summary>
		public uint Skillid { get; set; }

		/// <summary>
		/// Gets or sets the cost value.
		/// </summary>
		public uint Cost { get; set; }

		/// <summary>
		/// Gets or sets the newskill value.
		/// </summary>
		public byte Newskill { get; set; }

		/// <summary>
		/// Gets or sets the trainername value.
		/// </summary>
		public byte[] Trainername { get; set; }

		/// <summary>
		/// Initializes a new instance of the GMTrainSkill struct with specified field values.
		/// </summary>
		/// <param name="SkillID">The skillid value.</param>
		/// <param name="Cost">The cost value.</param>
		/// <param name="NewSkill">The newskill value.</param>
		/// <param name="TrainerName">The trainername value.</param>
		public GMTrainSkill(uint SkillID, uint Cost, byte NewSkill, byte[] TrainerName) : this() {
			Skillid = SkillID;
			Cost = Cost;
			Newskill = NewSkill;
			Trainername = TrainerName;
		}

		/// <summary>
		/// Initializes a new instance of the GMTrainSkill struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GMTrainSkill(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GMTrainSkill struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GMTrainSkill(BinaryReader br) : this() {
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
			Skillid = br.ReadUInt32();
			Cost = br.ReadUInt32();
			Newskill = br.ReadByte();
			// TODO: Array reading for Trainername - implement based on actual array size
			// Trainername = new byte[size];
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
			bw.Write(Skillid);
			bw.Write(Cost);
			bw.Write(Newskill);
			// TODO: Array writing for Trainername - implement based on actual array size
			// foreach(var item in Trainername) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GMTrainSkill {\n";
			ret += "	Skillid = ";
			try {
				ret += $"{ Indentify(Skillid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Cost = ";
			try {
				ret += $"{ Indentify(Cost) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Newskill = ";
			try {
				ret += $"{ Indentify(Newskill) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Trainername = ";
			try {
				ret += $"{ Indentify(Trainername) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}