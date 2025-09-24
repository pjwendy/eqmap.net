using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ActionAlt_Struct
// {
// /*00*/	uint16 target;			// id of target
// /*02*/	uint16 source;			// id of caster
// /*04*/	uint16 level;			// level of caster for spells, OSX dump says attack rating, guess spells use it for level
// /*06*/  uint32 unknown06;		// OSX dump says base_damage, was used for bard mod too, this is 0'd :(
// /*10*/	float instrument_mod;
// /*14*/  float force;
// /*18*/  float hit_heading;
// /*22*/  float hit_pitch;
// /*26*/  uint8 type;				// 231 (0xE7) for spells, skill
// /*27*/  uint32 damage;			// OSX says min_damage
// /*31*/  uint16 unknown31;		// OSX says tohit
// /*33*/	uint16 spell;			// spell id being cast
// /*35*/	uint8 spell_level;		// level of caster again? Or maybe the castee
// /*36*/	uint8 effect_flag;		// if this is 4, the effect is valid: or if two are sent at the same time?
// /*37*/	uint8 spell_gem;
// /*38*/	uint8 padding38[2];
// /*40*/	uint32 slot[5];
// /*60*/	uint32 item_cast_type;	// ItemSpellTypes enum from MQ2
// /*64*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_Action)
// {
// ENCODE_LENGTH_EXACT(Action_Struct);
// SETUP_DIRECT_ENCODE(Action_Struct, structs::ActionAlt_Struct);
// 
// OUT(target);
// OUT(source);
// OUT(level);
// eq->instrument_mod = 1.0f + (emu->instrument_mod - 10) / 10.0f;
// OUT(force);
// OUT(hit_heading);
// OUT(hit_pitch);
// OUT(type);
// OUT(spell);
// OUT(spell_level);
// OUT(effect_flag);
// eq->spell_gem = 0;
// eq->slot[0] = -1; // type
// eq->slot[1] = -1; // slot
// eq->slot[2] = -1; // sub index
// eq->slot[3] = -1; // aug index
// eq->slot[4] = -1; // unknown
// eq->item_cast_type = 0;
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Action packet structure for EverQuest network communication.
	/// </summary>
	public struct Action : IEQStruct {
		/// <summary>
		/// Gets or sets the target value.
		/// </summary>
		public ushort Target { get; set; }

		/// <summary>
		/// Gets or sets the source value.
		/// </summary>
		public ushort Source { get; set; }

		/// <summary>
		/// Gets or sets the level value.
		/// </summary>
		public ushort Level { get; set; }

		/// <summary>
		/// Gets or sets the unknown06 value.
		/// </summary>
		public uint Unknown06 { get; set; }

		/// <summary>
		/// Gets or sets the instrumentmod value.
		/// </summary>
		public float InstrumentMod { get; set; }

		/// <summary>
		/// Gets or sets the force value.
		/// </summary>
		public float Force { get; set; }

		/// <summary>
		/// Gets or sets the hitheading value.
		/// </summary>
		public float HitHeading { get; set; }

		/// <summary>
		/// Gets or sets the hitpitch value.
		/// </summary>
		public float HitPitch { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public byte Type { get; set; }

		/// <summary>
		/// Gets or sets the damage value.
		/// </summary>
		public uint Damage { get; set; }

		/// <summary>
		/// Gets or sets the unknown31 value.
		/// </summary>
		public ushort Unknown31 { get; set; }

		/// <summary>
		/// Gets or sets the spell value.
		/// </summary>
		public ushort Spell { get; set; }

		/// <summary>
		/// Gets or sets the spelllevel value.
		/// </summary>
		public byte SpellLevel { get; set; }

		/// <summary>
		/// Gets or sets the effectflag value.
		/// </summary>
		public byte EffectFlag { get; set; }

		/// <summary>
		/// Gets or sets the spellgem value.
		/// </summary>
		public byte SpellGem { get; set; }

		/// <summary>
		/// Gets or sets the padding38 value.
		/// </summary>
		public byte[] Padding38 { get; set; }

		/// <summary>
		/// Gets or sets the slot value.
		/// </summary>
		public uint[] Slot { get; set; }

		/// <summary>
		/// Gets or sets the itemcasttype value.
		/// </summary>
		public uint ItemCastType { get; set; }

		/// <summary>
		/// Initializes a new instance of the Action struct with specified field values.
		/// </summary>
		/// <param name="target">The target value.</param>
		/// <param name="source">The source value.</param>
		/// <param name="level">The level value.</param>
		/// <param name="unknown06">The unknown06 value.</param>
		/// <param name="instrument_mod">The instrumentmod value.</param>
		/// <param name="force">The force value.</param>
		/// <param name="hit_heading">The hitheading value.</param>
		/// <param name="hit_pitch">The hitpitch value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="damage">The damage value.</param>
		/// <param name="unknown31">The unknown31 value.</param>
		/// <param name="spell">The spell value.</param>
		/// <param name="spell_level">The spelllevel value.</param>
		/// <param name="effect_flag">The effectflag value.</param>
		/// <param name="spell_gem">The spellgem value.</param>
		/// <param name="padding38">The padding38 value.</param>
		/// <param name="slot">The slot value.</param>
		/// <param name="item_cast_type">The itemcasttype value.</param>
		public Action(ushort target, ushort source, ushort level, uint unknown06, float instrument_mod, float force, float hit_heading, float hit_pitch, byte type, uint damage, ushort unknown31, ushort spell, byte spell_level, byte effect_flag, byte spell_gem, byte[] padding38, uint[] slot, uint item_cast_type) : this() {
			Target = target;
			Source = source;
			Level = level;
			Unknown06 = unknown06;
			InstrumentMod = instrument_mod;
			Force = force;
			HitHeading = hit_heading;
			HitPitch = hit_pitch;
			Type = type;
			Damage = damage;
			Unknown31 = unknown31;
			Spell = spell;
			SpellLevel = spell_level;
			EffectFlag = effect_flag;
			SpellGem = spell_gem;
			Padding38 = padding38;
			Slot = slot;
			ItemCastType = item_cast_type;
		}

		/// <summary>
		/// Initializes a new instance of the Action struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Action(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Action struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Action(BinaryReader br) : this() {
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
			Target = br.ReadUInt16();
			Source = br.ReadUInt16();
			Level = br.ReadUInt16();
			Unknown06 = br.ReadUInt32();
			InstrumentMod = br.ReadSingle();
			Force = br.ReadSingle();
			HitHeading = br.ReadSingle();
			HitPitch = br.ReadSingle();
			Type = br.ReadByte();
			Damage = br.ReadUInt32();
			Unknown31 = br.ReadUInt16();
			Spell = br.ReadUInt16();
			SpellLevel = br.ReadByte();
			EffectFlag = br.ReadByte();
			SpellGem = br.ReadByte();
			// TODO: Array reading for Padding38 - implement based on actual array size
			// Padding38 = new byte[size];
			// TODO: Array reading for Slot - implement based on actual array size
			// Slot = new uint[size];
			ItemCastType = br.ReadUInt32();
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
			bw.Write(Target);
			bw.Write(Source);
			bw.Write(Level);
			bw.Write(Unknown06);
			bw.Write(InstrumentMod);
			bw.Write(Force);
			bw.Write(HitHeading);
			bw.Write(HitPitch);
			bw.Write(Type);
			bw.Write(Damage);
			bw.Write(Unknown31);
			bw.Write(Spell);
			bw.Write(SpellLevel);
			bw.Write(EffectFlag);
			bw.Write(SpellGem);
			// TODO: Array writing for Padding38 - implement based on actual array size
			// foreach(var item in Padding38) bw.Write(item);
			// TODO: Array writing for Slot - implement based on actual array size
			// foreach(var item in Slot) bw.Write(item);
			bw.Write(ItemCastType);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Action {\n";
			ret += "	Target = ";
			try {
				ret += $"{ Indentify(Target) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Source = ";
			try {
				ret += $"{ Indentify(Source) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Level = ";
			try {
				ret += $"{ Indentify(Level) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown06 = ";
			try {
				ret += $"{ Indentify(Unknown06) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	InstrumentMod = ";
			try {
				ret += $"{ Indentify(InstrumentMod) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Force = ";
			try {
				ret += $"{ Indentify(Force) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	HitHeading = ";
			try {
				ret += $"{ Indentify(HitHeading) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	HitPitch = ";
			try {
				ret += $"{ Indentify(HitPitch) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Damage = ";
			try {
				ret += $"{ Indentify(Damage) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown31 = ";
			try {
				ret += $"{ Indentify(Unknown31) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Spell = ";
			try {
				ret += $"{ Indentify(Spell) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpellLevel = ";
			try {
				ret += $"{ Indentify(SpellLevel) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	EffectFlag = ";
			try {
				ret += $"{ Indentify(EffectFlag) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpellGem = ";
			try {
				ret += $"{ Indentify(SpellGem) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Padding38 = ";
			try {
				ret += $"{ Indentify(Padding38) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ItemCastType = ";
			try {
				ret += $"{ Indentify(ItemCastType) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}