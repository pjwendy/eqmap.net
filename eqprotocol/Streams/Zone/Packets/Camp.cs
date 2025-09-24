using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BugReport_Struct {
// /*0000*/	uint32	category_id;
// /*0004*/	char	category_name[64];
// /*0068*/	char	reporter_name[64];
// /*0132*/	char	unused_0132[32];
// /*0164*/	char	ui_path[128];
// /*0292*/	float	pos_x;
// /*0296*/	float	pos_y;
// /*0300*/	float	pos_z;
// /*0304*/	uint32	heading;
// /*0308*/	uint32	unused_0308;
// /*0312*/	uint32	time_played;
// /*0316*/	char	padding_0316[8];
// /*0324*/	uint32	target_id;
// /*0328*/	char	padding_0328[140];
// /*0468*/	uint32	unknown_0468;	// seems to always be '0'
// /*0472*/	char	target_name[64];
// /*0536*/	uint32	optional_info_mask;
// 
// // this looks like a butchered 8k buffer with 2 trailing dword fields
// /*0540*/	char	unused_0540[2052];
// /*2592*/	char	bug_report[2050];
// /*4642*/	char	system_info[4098];
// /*8740*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Camp packet structure for EverQuest network communication.
	/// </summary>
	public struct Camp : IEQStruct {
		/// <summary>
		/// Gets or sets the categoryid value.
		/// </summary>
		public uint CategoryId { get; set; }

		/// <summary>
		/// Gets or sets the categoryname value.
		/// </summary>
		public byte[] CategoryName { get; set; }

		/// <summary>
		/// Gets or sets the reportername value.
		/// </summary>
		public byte[] ReporterName { get; set; }

		/// <summary>
		/// Gets or sets the unused0132 value.
		/// </summary>
		public byte[] Unused0132 { get; set; }

		/// <summary>
		/// Gets or sets the uipath value.
		/// </summary>
		public byte[] UiPath { get; set; }

		/// <summary>
		/// Gets or sets the posx value.
		/// </summary>
		public float PosX { get; set; }

		/// <summary>
		/// Gets or sets the posy value.
		/// </summary>
		public float PosY { get; set; }

		/// <summary>
		/// Gets or sets the posz value.
		/// </summary>
		public float PosZ { get; set; }

		/// <summary>
		/// Gets or sets the heading value.
		/// </summary>
		public uint Heading { get; set; }

		/// <summary>
		/// Gets or sets the unused0308 value.
		/// </summary>
		public uint Unused0308 { get; set; }

		/// <summary>
		/// Gets or sets the timeplayed value.
		/// </summary>
		public uint TimePlayed { get; set; }

		/// <summary>
		/// Gets or sets the targetid value.
		/// </summary>
		public uint TargetId { get; set; }

		/// <summary>
		/// Gets or sets the targetname value.
		/// </summary>
		public byte[] TargetName { get; set; }

		/// <summary>
		/// Gets or sets the optionalinfomask value.
		/// </summary>
		public uint OptionalInfoMask { get; set; }

		/// <summary>
		/// Gets or sets the unused0540 value.
		/// </summary>
		public byte[] Unused0540 { get; set; }

		/// <summary>
		/// Gets or sets the bugreport value.
		/// </summary>
		public byte[] BugReport { get; set; }

		/// <summary>
		/// Gets or sets the systeminfo value.
		/// </summary>
		public byte[] SystemInfo { get; set; }

		/// <summary>
		/// Initializes a new instance of the Camp struct with specified field values.
		/// </summary>
		/// <param name="category_id">The categoryid value.</param>
		/// <param name="category_name">The categoryname value.</param>
		/// <param name="reporter_name">The reportername value.</param>
		/// <param name="unused_0132">The unused0132 value.</param>
		/// <param name="ui_path">The uipath value.</param>
		/// <param name="pos_x">The posx value.</param>
		/// <param name="pos_y">The posy value.</param>
		/// <param name="pos_z">The posz value.</param>
		/// <param name="heading">The heading value.</param>
		/// <param name="unused_0308">The unused0308 value.</param>
		/// <param name="time_played">The timeplayed value.</param>
		/// <param name="target_id">The targetid value.</param>
		/// <param name="target_name">The targetname value.</param>
		/// <param name="optional_info_mask">The optionalinfomask value.</param>
		/// <param name="unused_0540">The unused0540 value.</param>
		/// <param name="bug_report">The bugreport value.</param>
		/// <param name="system_info">The systeminfo value.</param>
		public Camp(uint category_id, byte[] category_name, byte[] reporter_name, byte[] unused_0132, byte[] ui_path, float pos_x, float pos_y, float pos_z, uint heading, uint unused_0308, uint time_played, uint target_id, byte[] target_name, uint optional_info_mask, byte[] unused_0540, byte[] bug_report, byte[] system_info) : this() {
			CategoryId = category_id;
			CategoryName = category_name;
			ReporterName = reporter_name;
			Unused0132 = unused_0132;
			UiPath = ui_path;
			PosX = pos_x;
			PosY = pos_y;
			PosZ = pos_z;
			Heading = heading;
			Unused0308 = unused_0308;
			TimePlayed = time_played;
			TargetId = target_id;
			TargetName = target_name;
			OptionalInfoMask = optional_info_mask;
			Unused0540 = unused_0540;
			BugReport = bug_report;
			SystemInfo = system_info;
		}

		/// <summary>
		/// Initializes a new instance of the Camp struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Camp(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Camp struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Camp(BinaryReader br) : this() {
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
			CategoryId = br.ReadUInt32();
			// TODO: Array reading for CategoryName - implement based on actual array size
			// CategoryName = new byte[size];
			// TODO: Array reading for ReporterName - implement based on actual array size
			// ReporterName = new byte[size];
			// TODO: Array reading for Unused0132 - implement based on actual array size
			// Unused0132 = new byte[size];
			// TODO: Array reading for UiPath - implement based on actual array size
			// UiPath = new byte[size];
			PosX = br.ReadSingle();
			PosY = br.ReadSingle();
			PosZ = br.ReadSingle();
			Heading = br.ReadUInt32();
			Unused0308 = br.ReadUInt32();
			TimePlayed = br.ReadUInt32();
			TargetId = br.ReadUInt32();
			// TODO: Array reading for TargetName - implement based on actual array size
			// TargetName = new byte[size];
			OptionalInfoMask = br.ReadUInt32();
			// TODO: Array reading for Unused0540 - implement based on actual array size
			// Unused0540 = new byte[size];
			// TODO: Array reading for BugReport - implement based on actual array size
			// BugReport = new byte[size];
			// TODO: Array reading for SystemInfo - implement based on actual array size
			// SystemInfo = new byte[size];
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
			bw.Write(CategoryId);
			// TODO: Array writing for CategoryName - implement based on actual array size
			// foreach(var item in CategoryName) bw.Write(item);
			// TODO: Array writing for ReporterName - implement based on actual array size
			// foreach(var item in ReporterName) bw.Write(item);
			// TODO: Array writing for Unused0132 - implement based on actual array size
			// foreach(var item in Unused0132) bw.Write(item);
			// TODO: Array writing for UiPath - implement based on actual array size
			// foreach(var item in UiPath) bw.Write(item);
			bw.Write(PosX);
			bw.Write(PosY);
			bw.Write(PosZ);
			bw.Write(Heading);
			bw.Write(Unused0308);
			bw.Write(TimePlayed);
			bw.Write(TargetId);
			// TODO: Array writing for TargetName - implement based on actual array size
			// foreach(var item in TargetName) bw.Write(item);
			bw.Write(OptionalInfoMask);
			// TODO: Array writing for Unused0540 - implement based on actual array size
			// foreach(var item in Unused0540) bw.Write(item);
			// TODO: Array writing for BugReport - implement based on actual array size
			// foreach(var item in BugReport) bw.Write(item);
			// TODO: Array writing for SystemInfo - implement based on actual array size
			// foreach(var item in SystemInfo) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Camp {\n";
			ret += "	CategoryId = ";
			try {
				ret += $"{ Indentify(CategoryId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CategoryName = ";
			try {
				ret += $"{ Indentify(CategoryName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ReporterName = ";
			try {
				ret += $"{ Indentify(ReporterName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unused0132 = ";
			try {
				ret += $"{ Indentify(Unused0132) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	UiPath = ";
			try {
				ret += $"{ Indentify(UiPath) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PosX = ";
			try {
				ret += $"{ Indentify(PosX) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PosY = ";
			try {
				ret += $"{ Indentify(PosY) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PosZ = ";
			try {
				ret += $"{ Indentify(PosZ) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Heading = ";
			try {
				ret += $"{ Indentify(Heading) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unused0308 = ";
			try {
				ret += $"{ Indentify(Unused0308) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TimePlayed = ";
			try {
				ret += $"{ Indentify(TimePlayed) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TargetId = ";
			try {
				ret += $"{ Indentify(TargetId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TargetName = ";
			try {
				ret += $"{ Indentify(TargetName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	OptionalInfoMask = ";
			try {
				ret += $"{ Indentify(OptionalInfoMask) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unused0540 = ";
			try {
				ret += $"{ Indentify(Unused0540) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	BugReport = ";
			try {
				ret += $"{ Indentify(BugReport) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SystemInfo = ";
			try {
				ret += $"{ Indentify(SystemInfo) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}