using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Arrow_Struct
// {
// /*000*/	float	src_y;
// /*004*/	float	src_x;
// /*008*/	float	src_z;
// /*012*/	uint8	unknown012[12];
// /*024*/	float	velocity;		//4 is normal, 20 is quite fast
// /*028*/	float	launch_angle;	//0-450ish, not sure the units, 140ish is straight
// /*032*/	float	tilt;		//on the order of 125
// /*036*/	uint8	unknown036[8];
// /*044*/	float	arc;
// /*048*/	uint32	source_id;
// /*052*/	uint32	target_id;	//entity ID
// /*056*/	uint32	item_id;
// /*060*/	uint8	unknown060[10];
// /*070*/	uint8	unknown070;
// /*071*/	uint8	item_type;
// /*072*/	uint8	skill;
// /*073*/	uint8	unknown073[13];
// /*086*/	char	model_name[30];
// /*116*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_SomeItemPacketMaybe)
// {
// // This Opcode is not named very well. It is used for the animation of arrows leaving the player's bow
// // and flying to the target.
// //
// 
// ENCODE_LENGTH_EXACT(Arrow_Struct);
// SETUP_DIRECT_ENCODE(Arrow_Struct, structs::Arrow_Struct);
// 
// OUT(src_y);
// OUT(src_x);
// OUT(src_z);
// OUT(velocity);
// OUT(launch_angle);
// OUT(tilt);
// OUT(arc);
// OUT(source_id);
// OUT(target_id);
// OUT(item_id);
// 
// eq->unknown070 = 135; // This needs to be set to something, else we get a 1HS animation instead of ranged.
// 
// OUT(item_type);
// OUT(skill);
// 
// strcpy(eq->model_name, emu->model_name);
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SomeItemPacketMaybe packet structure for EverQuest network communication.
	/// </summary>
	public struct SomeItemPacketMaybe : IEQStruct {
		/// <summary>
		/// Gets or sets the srcy value.
		/// </summary>
		public float SrcY { get; set; }

		/// <summary>
		/// Gets or sets the srcx value.
		/// </summary>
		public float SrcX { get; set; }

		/// <summary>
		/// Gets or sets the srcz value.
		/// </summary>
		public float SrcZ { get; set; }

		/// <summary>
		/// Gets or sets the unknown012 value.
		/// </summary>
		public byte[] Unknown012 { get; set; }

		/// <summary>
		/// Gets or sets the velocity value.
		/// </summary>
		public float Velocity { get; set; }

		/// <summary>
		/// Gets or sets the launchangle value.
		/// </summary>
		public float LaunchAngle { get; set; }

		/// <summary>
		/// Gets or sets the tilt value.
		/// </summary>
		public float Tilt { get; set; }

		/// <summary>
		/// Gets or sets the unknown036 value.
		/// </summary>
		public byte[] Unknown036 { get; set; }

		/// <summary>
		/// Gets or sets the arc value.
		/// </summary>
		public float Arc { get; set; }

		/// <summary>
		/// Gets or sets the sourceid value.
		/// </summary>
		public uint SourceId { get; set; }

		/// <summary>
		/// Gets or sets the targetid value.
		/// </summary>
		public uint TargetId { get; set; }

		/// <summary>
		/// Gets or sets the itemid value.
		/// </summary>
		public uint ItemId { get; set; }

		/// <summary>
		/// Gets or sets the unknown060 value.
		/// </summary>
		public byte[] Unknown060 { get; set; }

		/// <summary>
		/// Gets or sets the unknown070 value.
		/// </summary>
		public byte Unknown070 { get; set; }

		/// <summary>
		/// Gets or sets the itemtype value.
		/// </summary>
		public byte ItemType { get; set; }

		/// <summary>
		/// Gets or sets the skill value.
		/// </summary>
		public byte Skill { get; set; }

		/// <summary>
		/// Gets or sets the unknown073 value.
		/// </summary>
		public byte[] Unknown073 { get; set; }

		/// <summary>
		/// Gets or sets the modelname value.
		/// </summary>
		public byte[] ModelName { get; set; }

		/// <summary>
		/// Initializes a new instance of the SomeItemPacketMaybe struct with specified field values.
		/// </summary>
		/// <param name="src_y">The srcy value.</param>
		/// <param name="src_x">The srcx value.</param>
		/// <param name="src_z">The srcz value.</param>
		/// <param name="unknown012">The unknown012 value.</param>
		/// <param name="velocity">The velocity value.</param>
		/// <param name="launch_angle">The launchangle value.</param>
		/// <param name="tilt">The tilt value.</param>
		/// <param name="unknown036">The unknown036 value.</param>
		/// <param name="arc">The arc value.</param>
		/// <param name="source_id">The sourceid value.</param>
		/// <param name="target_id">The targetid value.</param>
		/// <param name="item_id">The itemid value.</param>
		/// <param name="unknown060">The unknown060 value.</param>
		/// <param name="unknown070">The unknown070 value.</param>
		/// <param name="item_type">The itemtype value.</param>
		/// <param name="skill">The skill value.</param>
		/// <param name="unknown073">The unknown073 value.</param>
		/// <param name="model_name">The modelname value.</param>
		public SomeItemPacketMaybe(float src_y, float src_x, float src_z, byte[] unknown012, float velocity, float launch_angle, float tilt, byte[] unknown036, float arc, uint source_id, uint target_id, uint item_id, byte[] unknown060, byte unknown070, byte item_type, byte skill, byte[] unknown073, byte[] model_name) : this() {
			SrcY = src_y;
			SrcX = src_x;
			SrcZ = src_z;
			Unknown012 = unknown012;
			Velocity = velocity;
			LaunchAngle = launch_angle;
			Tilt = tilt;
			Unknown036 = unknown036;
			Arc = arc;
			SourceId = source_id;
			TargetId = target_id;
			ItemId = item_id;
			Unknown060 = unknown060;
			Unknown070 = unknown070;
			ItemType = item_type;
			Skill = skill;
			Unknown073 = unknown073;
			ModelName = model_name;
		}

		/// <summary>
		/// Initializes a new instance of the SomeItemPacketMaybe struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SomeItemPacketMaybe(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SomeItemPacketMaybe struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SomeItemPacketMaybe(BinaryReader br) : this() {
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
			SrcY = br.ReadSingle();
			SrcX = br.ReadSingle();
			SrcZ = br.ReadSingle();
			// TODO: Array reading for Unknown012 - implement based on actual array size
			// Unknown012 = new byte[size];
			Velocity = br.ReadSingle();
			LaunchAngle = br.ReadSingle();
			Tilt = br.ReadSingle();
			// TODO: Array reading for Unknown036 - implement based on actual array size
			// Unknown036 = new byte[size];
			Arc = br.ReadSingle();
			SourceId = br.ReadUInt32();
			TargetId = br.ReadUInt32();
			ItemId = br.ReadUInt32();
			// TODO: Array reading for Unknown060 - implement based on actual array size
			// Unknown060 = new byte[size];
			Unknown070 = br.ReadByte();
			ItemType = br.ReadByte();
			Skill = br.ReadByte();
			// TODO: Array reading for Unknown073 - implement based on actual array size
			// Unknown073 = new byte[size];
			// TODO: Array reading for ModelName - implement based on actual array size
			// ModelName = new byte[size];
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
			bw.Write(SrcY);
			bw.Write(SrcX);
			bw.Write(SrcZ);
			// TODO: Array writing for Unknown012 - implement based on actual array size
			// foreach(var item in Unknown012) bw.Write(item);
			bw.Write(Velocity);
			bw.Write(LaunchAngle);
			bw.Write(Tilt);
			// TODO: Array writing for Unknown036 - implement based on actual array size
			// foreach(var item in Unknown036) bw.Write(item);
			bw.Write(Arc);
			bw.Write(SourceId);
			bw.Write(TargetId);
			bw.Write(ItemId);
			// TODO: Array writing for Unknown060 - implement based on actual array size
			// foreach(var item in Unknown060) bw.Write(item);
			bw.Write(Unknown070);
			bw.Write(ItemType);
			bw.Write(Skill);
			// TODO: Array writing for Unknown073 - implement based on actual array size
			// foreach(var item in Unknown073) bw.Write(item);
			// TODO: Array writing for ModelName - implement based on actual array size
			// foreach(var item in ModelName) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SomeItemPacketMaybe {\n";
			ret += "	SrcY = ";
			try {
				ret += $"{ Indentify(SrcY) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SrcX = ";
			try {
				ret += $"{ Indentify(SrcX) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SrcZ = ";
			try {
				ret += $"{ Indentify(SrcZ) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown012 = ";
			try {
				ret += $"{ Indentify(Unknown012) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Velocity = ";
			try {
				ret += $"{ Indentify(Velocity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LaunchAngle = ";
			try {
				ret += $"{ Indentify(LaunchAngle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Tilt = ";
			try {
				ret += $"{ Indentify(Tilt) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown036 = ";
			try {
				ret += $"{ Indentify(Unknown036) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Arc = ";
			try {
				ret += $"{ Indentify(Arc) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SourceId = ";
			try {
				ret += $"{ Indentify(SourceId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TargetId = ";
			try {
				ret += $"{ Indentify(TargetId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ItemId = ";
			try {
				ret += $"{ Indentify(ItemId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown060 = ";
			try {
				ret += $"{ Indentify(Unknown060) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown070 = ";
			try {
				ret += $"{ Indentify(Unknown070) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ItemType = ";
			try {
				ret += $"{ Indentify(ItemType) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Skill = ";
			try {
				ret += $"{ Indentify(Skill) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown073 = ";
			try {
				ret += $"{ Indentify(Unknown073) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ModelName = ";
			try {
				ret += $"{ Indentify(ModelName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}