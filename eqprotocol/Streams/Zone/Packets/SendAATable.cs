using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SendAA_Struct {
// /*0000*/	uint32 id;
// /*0004*/	uint8 unknown004;		// uint32 unknown004; set to 1.
// /*0005*/	uint32 hotkey_sid;
// /*0009*/	uint32 hotkey_sid2;
// /*0013*/	uint32 title_sid;
// /*0017*/	uint32 desc_sid;
// /*0021*/	uint32 class_type;
// /*0025*/	uint32 cost;
// /*0029*/	uint32 seq;
// /*0033*/	uint32 current_level; //1s, MQ2 calls this AARankRequired
// /*0037*/	uint32 prereq_skill;		//is < 0, abs() is category #
// /*0041*/	uint32 prereq_minpoints; //min points in the prereq
// /*0045*/	uint32 type;
// /*0049*/	uint32 spellid;
// /*0053*/	uint32 spell_type;
// /*0057*/	uint32 spell_refresh;
// /*0061*/	uint32 classes;
// /*0065*/	uint32 max_level;
// /*0069*/	uint32 last_id;
// /*0073*/	uint32 next_id;
// /*0077*/	uint32 cost2;
// /*0081*/	uint8 unknown81;
// /*0082*/	uint8 grant_only; // VetAAs, progression, etc
// /*0083*/	uint8 unknown83; // 1 for skill cap increase AAs, Mystical Attuning, and RNG attack inc, doesn't seem to matter though
// /*0084*/	uint32 expendable_charges; // max charges of the AA
// /*0088*/	uint32 aa_expansion;
// /*0092*/	uint32 special_category;
// /*0096*/	uint8 shroud;
// /*0097*/	uint8 unknown97;
// /*0098*/	uint8 reset_on_death; // timer is reset on death
// /*0099*/	uint8 unknown99;
// /*0100*/	uint32 total_abilities;
// /*0104*/	AA_Ability abilities[0];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_SendAATable)
// {
// EQApplicationPacket *inapp = *p;
// *p = nullptr;
// AARankInfo_Struct *emu = (AARankInfo_Struct*)inapp->pBuffer;
// 
// auto outapp = new EQApplicationPacket(
// OP_SendAATable, sizeof(structs::SendAA_Struct) + emu->total_effects * sizeof(structs::AA_Ability));
// structs::SendAA_Struct *eq = (structs::SendAA_Struct*)outapp->pBuffer;
// 
// inapp->SetReadPosition(sizeof(AARankInfo_Struct));
// outapp->SetWritePosition(sizeof(structs::SendAA_Struct));
// 
// eq->id = emu->id;
// eq->unknown004 = 1;
// eq->id = emu->id;
// eq->hotkey_sid = emu->upper_hotkey_sid;
// eq->hotkey_sid2 = emu->lower_hotkey_sid;
// eq->desc_sid = emu->desc_sid;
// eq->title_sid = emu->title_sid;
// eq->class_type = emu->level_req;
// eq->cost = emu->cost;
// eq->seq = emu->seq;
// eq->current_level = emu->current_level;
// eq->type = emu->type;
// eq->spellid = emu->spell;
// eq->spell_type = emu->spell_type;
// eq->spell_refresh = emu->spell_refresh;
// eq->classes = emu->classes;
// eq->max_level = emu->max_level;
// eq->last_id = emu->prev_id;
// eq->next_id = emu->next_id;
// eq->cost2 = emu->total_cost;
// eq->grant_only = emu->grant_only;
// eq->expendable_charges = emu->charges;
// eq->aa_expansion = emu->expansion;
// eq->special_category = emu->category;
// eq->total_abilities = emu->total_effects;
// 
// for(auto i = 0; i < eq->total_abilities; ++i) {
// eq->abilities[i].skill_id = inapp->ReadUInt32();
// eq->abilities[i].base_value = inapp->ReadUInt32();
// eq->abilities[i].limit_value = inapp->ReadUInt32();
// eq->abilities[i].slot = inapp->ReadUInt32();
// }
// 
// if(emu->total_prereqs > 0) {
// eq->prereq_skill = inapp->ReadUInt32();
// eq->prereq_minpoints = inapp->ReadUInt32();
// }
// 
// dest->FastQueuePacket(&outapp);
// delete inapp;
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SendAATable packet structure for EverQuest network communication.
	/// </summary>
	public struct SendAATable : IEQStruct {
		/// <summary>
		/// Gets or sets the id value.
		/// </summary>
		public uint Id { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public byte Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the hotkeysid value.
		/// </summary>
		public uint HotkeySid { get; set; }

		/// <summary>
		/// Gets or sets the hotkeysid2 value.
		/// </summary>
		public uint HotkeySid2 { get; set; }

		/// <summary>
		/// Gets or sets the titlesid value.
		/// </summary>
		public uint TitleSid { get; set; }

		/// <summary>
		/// Gets or sets the descsid value.
		/// </summary>
		public uint DescSid { get; set; }

		/// <summary>
		/// Gets or sets the classtype value.
		/// </summary>
		public uint ClassType { get; set; }

		/// <summary>
		/// Gets or sets the cost value.
		/// </summary>
		public uint Cost { get; set; }

		/// <summary>
		/// Gets or sets the seq value.
		/// </summary>
		public uint Seq { get; set; }

		/// <summary>
		/// Gets or sets the currentlevel value.
		/// </summary>
		public uint CurrentLevel { get; set; }

		/// <summary>
		/// Gets or sets the prereqskill value.
		/// </summary>
		public uint PrereqSkill { get; set; }

		/// <summary>
		/// Gets or sets the prereqminpoints value.
		/// </summary>
		public uint PrereqMinpoints { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Gets or sets the spellid value.
		/// </summary>
		public uint Spellid { get; set; }

		/// <summary>
		/// Gets or sets the spelltype value.
		/// </summary>
		public uint SpellType { get; set; }

		/// <summary>
		/// Gets or sets the spellrefresh value.
		/// </summary>
		public uint SpellRefresh { get; set; }

		/// <summary>
		/// Gets or sets the classes value.
		/// </summary>
		public uint Classes { get; set; }

		/// <summary>
		/// Gets or sets the maxlevel value.
		/// </summary>
		public uint MaxLevel { get; set; }

		/// <summary>
		/// Gets or sets the lastid value.
		/// </summary>
		public uint LastId { get; set; }

		/// <summary>
		/// Gets or sets the nextid value.
		/// </summary>
		public uint NextId { get; set; }

		/// <summary>
		/// Gets or sets the cost2 value.
		/// </summary>
		public uint Cost2 { get; set; }

		/// <summary>
		/// Gets or sets the unknown81 value.
		/// </summary>
		public byte Unknown81 { get; set; }

		/// <summary>
		/// Gets or sets the grantonly value.
		/// </summary>
		public byte GrantOnly { get; set; }

		/// <summary>
		/// Gets or sets the unknown83 value.
		/// </summary>
		public byte Unknown83 { get; set; }

		/// <summary>
		/// Gets or sets the expendablecharges value.
		/// </summary>
		public uint ExpendableCharges { get; set; }

		/// <summary>
		/// Gets or sets the aaexpansion value.
		/// </summary>
		public uint AaExpansion { get; set; }

		/// <summary>
		/// Gets or sets the specialcategory value.
		/// </summary>
		public uint SpecialCategory { get; set; }

		/// <summary>
		/// Gets or sets the shroud value.
		/// </summary>
		public byte Shroud { get; set; }

		/// <summary>
		/// Gets or sets the unknown97 value.
		/// </summary>
		public byte Unknown97 { get; set; }

		/// <summary>
		/// Gets or sets the resetondeath value.
		/// </summary>
		public byte ResetOnDeath { get; set; }

		/// <summary>
		/// Gets or sets the unknown99 value.
		/// </summary>
		public byte Unknown99 { get; set; }

		/// <summary>
		/// Gets or sets the totalabilities value.
		/// </summary>
		public uint TotalAbilities { get; set; }

		/// <summary>
		/// Gets or sets the abilities value.
		/// </summary>
		public uint Abilities { get; set; }

		/// <summary>
		/// Initializes a new instance of the SendAATable struct with specified field values.
		/// </summary>
		/// <param name="id">The id value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="hotkey_sid">The hotkeysid value.</param>
		/// <param name="hotkey_sid2">The hotkeysid2 value.</param>
		/// <param name="title_sid">The titlesid value.</param>
		/// <param name="desc_sid">The descsid value.</param>
		/// <param name="class_type">The classtype value.</param>
		/// <param name="cost">The cost value.</param>
		/// <param name="seq">The seq value.</param>
		/// <param name="current_level">The currentlevel value.</param>
		/// <param name="prereq_skill">The prereqskill value.</param>
		/// <param name="prereq_minpoints">The prereqminpoints value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="spellid">The spellid value.</param>
		/// <param name="spell_type">The spelltype value.</param>
		/// <param name="spell_refresh">The spellrefresh value.</param>
		/// <param name="classes">The classes value.</param>
		/// <param name="max_level">The maxlevel value.</param>
		/// <param name="last_id">The lastid value.</param>
		/// <param name="next_id">The nextid value.</param>
		/// <param name="cost2">The cost2 value.</param>
		/// <param name="unknown81">The unknown81 value.</param>
		/// <param name="grant_only">The grantonly value.</param>
		/// <param name="unknown83">The unknown83 value.</param>
		/// <param name="expendable_charges">The expendablecharges value.</param>
		/// <param name="aa_expansion">The aaexpansion value.</param>
		/// <param name="special_category">The specialcategory value.</param>
		/// <param name="shroud">The shroud value.</param>
		/// <param name="unknown97">The unknown97 value.</param>
		/// <param name="reset_on_death">The resetondeath value.</param>
		/// <param name="unknown99">The unknown99 value.</param>
		/// <param name="total_abilities">The totalabilities value.</param>
		/// <param name="abilities">The abilities value.</param>
		public SendAATable(uint id, byte unknown004, uint hotkey_sid, uint hotkey_sid2, uint title_sid, uint desc_sid, uint class_type, uint cost, uint seq, uint current_level, uint prereq_skill, uint prereq_minpoints, uint type, uint spellid, uint spell_type, uint spell_refresh, uint classes, uint max_level, uint last_id, uint next_id, uint cost2, byte unknown81, byte grant_only, byte unknown83, uint expendable_charges, uint aa_expansion, uint special_category, byte shroud, byte unknown97, byte reset_on_death, byte unknown99, uint total_abilities, uint abilities) : this() {
			Id = id;
			Unknown004 = unknown004;
			HotkeySid = hotkey_sid;
			HotkeySid2 = hotkey_sid2;
			TitleSid = title_sid;
			DescSid = desc_sid;
			ClassType = class_type;
			Cost = cost;
			Seq = seq;
			CurrentLevel = current_level;
			PrereqSkill = prereq_skill;
			PrereqMinpoints = prereq_minpoints;
			Type = type;
			Spellid = spellid;
			SpellType = spell_type;
			SpellRefresh = spell_refresh;
			Classes = classes;
			MaxLevel = max_level;
			LastId = last_id;
			NextId = next_id;
			Cost2 = cost2;
			Unknown81 = unknown81;
			GrantOnly = grant_only;
			Unknown83 = unknown83;
			ExpendableCharges = expendable_charges;
			AaExpansion = aa_expansion;
			SpecialCategory = special_category;
			Shroud = shroud;
			Unknown97 = unknown97;
			ResetOnDeath = reset_on_death;
			Unknown99 = unknown99;
			TotalAbilities = total_abilities;
			Abilities = abilities;
		}

		/// <summary>
		/// Initializes a new instance of the SendAATable struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SendAATable(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SendAATable struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SendAATable(BinaryReader br) : this() {
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
			Id = br.ReadUInt32();
			Unknown004 = br.ReadByte();
			HotkeySid = br.ReadUInt32();
			HotkeySid2 = br.ReadUInt32();
			TitleSid = br.ReadUInt32();
			DescSid = br.ReadUInt32();
			ClassType = br.ReadUInt32();
			Cost = br.ReadUInt32();
			Seq = br.ReadUInt32();
			CurrentLevel = br.ReadUInt32();
			PrereqSkill = br.ReadUInt32();
			PrereqMinpoints = br.ReadUInt32();
			Type = br.ReadUInt32();
			Spellid = br.ReadUInt32();
			SpellType = br.ReadUInt32();
			SpellRefresh = br.ReadUInt32();
			Classes = br.ReadUInt32();
			MaxLevel = br.ReadUInt32();
			LastId = br.ReadUInt32();
			NextId = br.ReadUInt32();
			Cost2 = br.ReadUInt32();
			Unknown81 = br.ReadByte();
			GrantOnly = br.ReadByte();
			Unknown83 = br.ReadByte();
			ExpendableCharges = br.ReadUInt32();
			AaExpansion = br.ReadUInt32();
			SpecialCategory = br.ReadUInt32();
			Shroud = br.ReadByte();
			Unknown97 = br.ReadByte();
			ResetOnDeath = br.ReadByte();
			Unknown99 = br.ReadByte();
			TotalAbilities = br.ReadUInt32();
			Abilities = br.ReadUInt32();
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
			bw.Write(Id);
			bw.Write(Unknown004);
			bw.Write(HotkeySid);
			bw.Write(HotkeySid2);
			bw.Write(TitleSid);
			bw.Write(DescSid);
			bw.Write(ClassType);
			bw.Write(Cost);
			bw.Write(Seq);
			bw.Write(CurrentLevel);
			bw.Write(PrereqSkill);
			bw.Write(PrereqMinpoints);
			bw.Write(Type);
			bw.Write(Spellid);
			bw.Write(SpellType);
			bw.Write(SpellRefresh);
			bw.Write(Classes);
			bw.Write(MaxLevel);
			bw.Write(LastId);
			bw.Write(NextId);
			bw.Write(Cost2);
			bw.Write(Unknown81);
			bw.Write(GrantOnly);
			bw.Write(Unknown83);
			bw.Write(ExpendableCharges);
			bw.Write(AaExpansion);
			bw.Write(SpecialCategory);
			bw.Write(Shroud);
			bw.Write(Unknown97);
			bw.Write(ResetOnDeath);
			bw.Write(Unknown99);
			bw.Write(TotalAbilities);
			bw.Write(Abilities);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SendAATable {\n";
			ret += "	Id = ";
			try {
				ret += $"{ Indentify(Id) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	HotkeySid = ";
			try {
				ret += $"{ Indentify(HotkeySid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	HotkeySid2 = ";
			try {
				ret += $"{ Indentify(HotkeySid2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TitleSid = ";
			try {
				ret += $"{ Indentify(TitleSid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DescSid = ";
			try {
				ret += $"{ Indentify(DescSid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ClassType = ";
			try {
				ret += $"{ Indentify(ClassType) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Cost = ";
			try {
				ret += $"{ Indentify(Cost) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Seq = ";
			try {
				ret += $"{ Indentify(Seq) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CurrentLevel = ";
			try {
				ret += $"{ Indentify(CurrentLevel) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PrereqSkill = ";
			try {
				ret += $"{ Indentify(PrereqSkill) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	PrereqMinpoints = ";
			try {
				ret += $"{ Indentify(PrereqMinpoints) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Spellid = ";
			try {
				ret += $"{ Indentify(Spellid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpellType = ";
			try {
				ret += $"{ Indentify(SpellType) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpellRefresh = ";
			try {
				ret += $"{ Indentify(SpellRefresh) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Classes = ";
			try {
				ret += $"{ Indentify(Classes) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MaxLevel = ";
			try {
				ret += $"{ Indentify(MaxLevel) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LastId = ";
			try {
				ret += $"{ Indentify(LastId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NextId = ";
			try {
				ret += $"{ Indentify(NextId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Cost2 = ";
			try {
				ret += $"{ Indentify(Cost2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown81 = ";
			try {
				ret += $"{ Indentify(Unknown81) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	GrantOnly = ";
			try {
				ret += $"{ Indentify(GrantOnly) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown83 = ";
			try {
				ret += $"{ Indentify(Unknown83) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ExpendableCharges = ";
			try {
				ret += $"{ Indentify(ExpendableCharges) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AaExpansion = ";
			try {
				ret += $"{ Indentify(AaExpansion) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpecialCategory = ";
			try {
				ret += $"{ Indentify(SpecialCategory) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Shroud = ";
			try {
				ret += $"{ Indentify(Shroud) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown97 = ";
			try {
				ret += $"{ Indentify(Unknown97) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ResetOnDeath = ";
			try {
				ret += $"{ Indentify(ResetOnDeath) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown99 = ";
			try {
				ret += $"{ Indentify(Unknown99) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TotalAbilities = ";
			try {
				ret += $"{ Indentify(TotalAbilities) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Abilities = ";
			try {
				ret += $"{ Indentify(Abilities) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}