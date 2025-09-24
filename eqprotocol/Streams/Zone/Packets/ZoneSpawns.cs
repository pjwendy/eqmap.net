using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Spawn_Struct
// {
// // Note this struct is not used as such, it is here for reference. As the struct is variable sized, the packets
// // are constructed in Underfoot.cpp
// //
// /*0000*/ char     name[1];	//name[64];
// /*0000*/ //uint8     nullterm1; // hack to null terminate name
// /*0064*/ uint32 spawnId;
// /*0068*/ uint8  level;
// /*0069*/ float  bounding_radius; // used in melee, overrides calc
// /*0073*/ uint8  NPC;           // 0=player,1=npc,2=pc corpse,3=npc corpse
// Spawn_Struct_Bitfields	Bitfields;
// /*0000*/ uint8  otherData; // & 4 - has title, & 8 - has suffix, & 1 - it's a chest or untargetable
// /*0000*/ float unknown3;	// seen -1
// /*0000*/ float unknown4;
// /*0000*/ float size;
// /*0000*/ uint8  face;
// /*0000*/ float    walkspeed;
// /*0000*/ float    runspeed;
// /*0000*/ uint32 race;
// /*0000*/ uint8  showname; // for body types - was charProperties
// /*0000*/ uint32 bodytype;
// /*0000*/ //uint32 bodytype2;      // this is only present if charProperties==2
// // are there more than two possible properties?
// /*0000*/ uint8  curHp;
// /*0000*/ uint8  haircolor;
// /*0000*/ uint8  beardcolor;
// /*0000*/ uint8  eyecolor1;
// /*0000*/ uint8  eyecolor2;
// /*0000*/ uint8  hairstyle;
// /*0000*/ uint8  beard;
// /*0000*/ uint32 drakkin_heritage;
// /*0000*/ uint32 drakkin_tattoo;
// /*0000*/ uint32 drakkin_details;
// /*0000*/ uint8  statue;				// was holding
// /*0000*/ uint32 deity;
// /*0000*/ uint32 guildID;
// /*0000*/ uint32 guildrank;			// 0=member, 1=officer, 2=leader, -1=not guilded
// /*0000*/ uint8  class_;
// /*0000*/ uint8  pvp;					// 0 = normal name color, 2 = PVP name color
// /*0000*/ uint8  StandState;			// stand state - 0x64 for normal animation
// /*0000*/ uint8  light;
// /*0000*/ uint8  flymode;
// /*0000*/ uint8  equip_chest2;
// /*0000*/ uint8  unknown9;
// /*0000*/ uint8  unknown10;
// /*0000*/ uint8  helm;
// /*0000*/ char     lastName[1];
// /*0000*/ //uint8     lastNameNull; //hack!
// /*0000*/ uint32 aatitle;		// 0=none, 1=general, 2=archtype, 3=class was AARank
// /*0000*/ uint8  unknown12;
// /*0000*/ uint32 petOwnerId;
// /*0000*/ uint8  unknown13;
// /*0000*/ uint32 PlayerState;		// Stance 64 = normal 4 = aggressive 40 = stun/mezzed
// /*0000*/ uint32 unknown15;
// /*0000*/ uint32 unknown16;
// /*0000*/ uint32 unknown17;
// /*0000*/ uint32 unknown18;
// /*0000*/ uint32 unknown19;
// Spawn_Struct_Position Position;
// /*0000*/ TintProfile equipment_tint;
// 
// // skip these bytes if not a valid player race
// /*0000*/ TextureProfile equipment;
// 
// /*0000*/ //char title[0];  // only read if(hasTitleOrSuffix & 4)
// /*0000*/ //char suffix[0]; // only read if(hasTitleOrSuffix & 8)
// char unknown20[8];
// uint8 IsMercenary;	// If NPC == 1 and this == 1, then the NPC name is Orange.
// /*0000*/ char unknown21[28];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_ZoneSpawns)
// {
// //consume the packet
// EQApplicationPacket *in = *p;
// *p = nullptr;
// 
// //store away the emu struct
// unsigned char *__emu_buffer = in->pBuffer;
// Spawn_Struct *emu = (Spawn_Struct *)__emu_buffer;
// 
// //determine and verify length
// int entrycount = in->size / sizeof(Spawn_Struct);
// if (entrycount == 0 || (in->size % sizeof(Spawn_Struct)) != 0) {
// LogNetcode("[STRUCTS] Wrong size on outbound [{}]: Got [{}], expected multiple of [{}]", opcodes->EmuToName(in->GetOpcode()), in->size, sizeof(Spawn_Struct));
// delete in;
// return;
// }
// 
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[STRUCTS] Spawn name is [%s]", emu->name);
// 
// emu = (Spawn_Struct *)__emu_buffer;
// 
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[STRUCTS] Spawn packet size is %i, entries = %i", in->size, entrycount);
// 
// char *Buffer = (char *)in->pBuffer;
// 
// int r;
// int k;
// for (r = 0; r < entrycount; r++, emu++) {
// 
// int PacketSize = sizeof(structs::Spawn_Struct);
// 
// PacketSize += strlen(emu->name);
// PacketSize += strlen(emu->lastName);
// 
// if (strlen(emu->title))
// PacketSize += strlen(emu->title) + 1;
// 
// if (strlen(emu->suffix))
// PacketSize += strlen(emu->suffix) + 1;
// 
// if (emu->DestructibleObject || emu->class_ == Class::LDoNTreasure)
// {
// if (emu->DestructibleObject)
// PacketSize = PacketSize - 4;	// No bodytype
// 
// PacketSize += 53;	// Fixed portion
// PacketSize += strlen(emu->DestructibleModel) + 1;
// PacketSize += strlen(emu->DestructibleName2) + 1;
// PacketSize += strlen(emu->DestructibleString) + 1;
// }
// 
// bool ShowName = emu->show_name;
// if (emu->bodytype >= 66)
// {
// emu->race = 127;
// emu->bodytype = 11;
// emu->gender = 0;
// ShowName = 0;
// }
// 
// float SpawnSize = emu->size;
// if (!((emu->NPC == 0) || (emu->race <= Race::Gnome) || (emu->race == Race::Iksar) ||
// (emu->race == Race::VahShir) || (emu->race == Race::Froglok2) || (emu->race == Race::Drakkin))
// )
// {
// PacketSize -= (sizeof(structs::Texture_Struct) * EQ::textures::materialCount);
// 
// if (emu->size == 0)
// {
// emu->size = 6;
// SpawnSize = 6;
// }
// }
// 
// if (SpawnSize == 0)
// {
// SpawnSize = 3;
// }
// 
// auto outapp = new EQApplicationPacket(OP_ZoneEntry, PacketSize);
// Buffer = (char *)outapp->pBuffer;
// 
// VARSTRUCT_ENCODE_STRING(Buffer, emu->name);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->spawnId);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->level);
// 
// if (emu->DestructibleObject)
// {
// VARSTRUCT_ENCODE_TYPE(float, Buffer, 10);	// was int and 0x41200000
// }
// else
// {
// VARSTRUCT_ENCODE_TYPE(float, Buffer, SpawnSize - 0.7);	// Eye Height?
// }
// 
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->NPC);
// 
// structs::Spawn_Struct_Bitfields *Bitfields = (structs::Spawn_Struct_Bitfields*)Buffer;
// 
// Bitfields->afk = 0;
// Bitfields->linkdead = 0;
// Bitfields->gender = emu->gender;
// 
// Bitfields->invis = emu->invis;
// Bitfields->sneak = 0;
// Bitfields->lfg = emu->lfg;
// Bitfields->gm = emu->gm;
// Bitfields->anon = emu->anon;
// Bitfields->showhelm = emu->showhelm;
// Bitfields->targetable = 1;
// Bitfields->targetable_with_hotkey = emu->targetable_with_hotkey ? 1 : 0;
// Bitfields->statue = 0;
// Bitfields->trader = emu->trader ? 1 : 0;
// Bitfields->buyer = 0;
// 
// Bitfields->showname = ShowName;
// 
// if (emu->DestructibleObject)
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0x1d600000);
// Buffer = Buffer - 4;
// }
// 
// Bitfields->ispet = emu->is_pet;
// 
// Buffer += sizeof(structs::Spawn_Struct_Bitfields);
// 
// uint8 OtherData = 0;
// 
// if (emu->class_ == Class::LDoNTreasure) //Ldon chest
// {
// OtherData = OtherData | 0x01;
// }
// 
// if (strlen(emu->title)) {
// OtherData = OtherData | 0x04;
// }
// if (strlen(emu->suffix)) {
// OtherData = OtherData | 0x08;
// }
// if (emu->DestructibleObject) {
// OtherData = OtherData | 0xd1;	// Live has 0xe1 for OtherData
// }
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, OtherData);
// 
// if (emu->DestructibleObject)
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0x00000000);
// }
// else
// {
// VARSTRUCT_ENCODE_TYPE(float, Buffer, -1);	// unknown3
// }
// VARSTRUCT_ENCODE_TYPE(float, Buffer, 0);	// unknown4
// 
// if (emu->DestructibleObject || emu->class_ == Class::LDoNTreasure)
// {
// VARSTRUCT_ENCODE_STRING(Buffer, emu->DestructibleModel);
// VARSTRUCT_ENCODE_STRING(Buffer, emu->DestructibleName2);
// VARSTRUCT_ENCODE_STRING(Buffer, emu->DestructibleString);
// 
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleAppearance);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleUnk1);
// 
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleID1);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleID2);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleID3);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleID4);
// 
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleUnk2);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleUnk3);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleUnk4);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleUnk5);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleUnk6);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleUnk7);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->DestructibleUnk8);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->DestructibleUnk9);
// }
// 
// VARSTRUCT_ENCODE_TYPE(float, Buffer, emu->size);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->face);
// VARSTRUCT_ENCODE_TYPE(float, Buffer, emu->walkspeed);
// VARSTRUCT_ENCODE_TYPE(float, Buffer, emu->runspeed);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->race);
// /*
// if(emu->bodytype >=66)
// {
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);	// showname
// }
// else
// {
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 1);	// showname
// }*/
// 
// 
// if (!emu->DestructibleObject)
// {
// // Setting this next field to zero will cause a crash. Looking at ShowEQ, if it is zero, the bodytype field is not
// // present. Will sort that out later.
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 1);	// This is a properties count field
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->bodytype);
// }
// else
// {
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);
// }
// 
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->curHp);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->haircolor);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->beardcolor);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->eyecolor1);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->eyecolor2);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->hairstyle);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->beard);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->drakkin_heritage);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->drakkin_tattoo);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->drakkin_details);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);	// ShowEQ calls this 'Holding'
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->deity);
// if (emu->NPC)
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0xFFFFFFFF);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0x00000000);
// }
// else
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->guildID);
// //Translate older ranks to new values* /
// switch (emu->guildrank) {
// case GUILD_SENIOR_MEMBER:
// case GUILD_MEMBER:
// case GUILD_JUNIOR_MEMBER:
// case GUILD_INITIATE:
// case GUILD_RECRUIT: {
// emu->guildrank = GUILD_MEMBER_TI;
// break;
// }
// case GUILD_OFFICER:
// case GUILD_SENIOR_OFFICER: {
// emu->guildrank = GUILD_OFFICER_TI;
// break;
// }
// case GUILD_LEADER: {
// emu->guildrank = GUILD_LEADER_TI;
// break;
// }
// default: {
// emu->guildrank = GUILD_RANK_NONE_TI;
// break;
// }
// }
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->guildrank);
// }
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->class_);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);	// pvp
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->StandState);	// standstate
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->light);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->flymode);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->equip_chest2); // unknown8
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0); // unknown9
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0); // unknown10
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->helm); // unknown11
// VARSTRUCT_ENCODE_STRING(Buffer, emu->lastName);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);	// aatitle
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0); // unknown12
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->petOwnerId);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0); // unknown13
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->PlayerState);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0); // unknown15
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0); // unknown16
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0); // unknown17
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0xffffffff); // unknown18
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0xffffffff); // unknown19
// 
// structs::Spawn_Struct_Position *Position = (structs::Spawn_Struct_Position*)Buffer;
// 
// Position->deltaX = emu->deltaX;
// Position->deltaHeading = emu->deltaHeading;
// Position->deltaY = emu->deltaY;
// Position->y = emu->y;
// Position->animation = emu->animation;
// Position->heading = emu->heading;
// Position->x = emu->x;
// Position->z = emu->z;
// Position->deltaZ = emu->deltaZ;
// 
// Buffer += sizeof(structs::Spawn_Struct_Position);
// 
// if ((emu->NPC == 0) || (emu->race <= Race::Gnome) || (emu->race == Race::Iksar) ||
// (emu->race == Race::VahShir) || (emu->race == Race::Froglok2) || (emu->race == Race::Drakkin)
// )
// {
// for (k = EQ::textures::textureBegin; k < EQ::textures::materialCount; ++k)
// {
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->equipment_tint.Slot[k].Color);
// }
// }
// }
// else
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// 
// if (emu->equipment.Primary.Material > 99999) {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 63);
// } else {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->equipment.Primary.Material);
// }
// 
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// 
// if (emu->equipment.Secondary.Material > 99999) {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 63);
// } else {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->equipment.Secondary.Material);
// }
// 
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// }
// 
// if ((emu->NPC == 0) || (emu->race <= Race::Gnome) || (emu->race == Race::Iksar) ||
// (emu->race == Race::VahShir) || (emu->race == Race::Froglok2) || (emu->race == Race::Drakkin)
// )
// {
// structs::Texture_Struct *Equipment = (structs::Texture_Struct *)Buffer;
// 
// for (k = EQ::textures::textureBegin; k < EQ::textures::materialCount; k++) {
// if (emu->equipment.Slot[k].Material > 99999) {
// Equipment[k].Material = 63;
// } else {
// Equipment[k].Material = emu->equipment.Slot[k].Material;
// }
// Equipment[k].Unknown1 = emu->equipment.Slot[k].Unknown1;
// Equipment[k].EliteMaterial = emu->equipment.Slot[k].EliteModel;
// }
// 
// Buffer += (sizeof(structs::Texture_Struct) * EQ::textures::materialCount);
// }
// if (strlen(emu->title))
// {
// VARSTRUCT_ENCODE_STRING(Buffer, emu->title);
// }
// 
// if (strlen(emu->suffix))
// {
// VARSTRUCT_ENCODE_STRING(Buffer, emu->suffix);
// }
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0); // Unknown;
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0); // Unknown;
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->IsMercenary); //IsMercenary
// Buffer += 28; // Unknown;
// 
// dest->FastQueuePacket(&outapp, ack_req);
// }
// 
// delete in;
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ZoneSpawns packet structure for EverQuest network communication.
	/// </summary>
	public struct ZoneSpawns : IEQStruct {
		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte Name { get; set; }

		/// <summary>
		/// Gets or sets the spawnid value.
		/// </summary>
		public uint Spawnid { get; set; }

		/// <summary>
		/// Gets or sets the level value.
		/// </summary>
		public byte Level { get; set; }

		/// <summary>
		/// Gets or sets the boundingradius value.
		/// </summary>
		public float BoundingRadius { get; set; }

		/// <summary>
		/// Gets or sets the npc value.
		/// </summary>
		public byte NPC { get; set; }

		/// <summary>
		/// Gets or sets the bitfields value.
		/// </summary>
		public uint Bitfields { get; set; }

		/// <summary>
		/// Gets or sets the otherdata value.
		/// </summary>
		public byte Otherdata { get; set; }

		/// <summary>
		/// Gets or sets the unknown3 value.
		/// </summary>
		public float Unknown3 { get; set; }

		/// <summary>
		/// Gets or sets the unknown4 value.
		/// </summary>
		public float Unknown4 { get; set; }

		/// <summary>
		/// Gets or sets the size value.
		/// </summary>
		public float Size { get; set; }

		/// <summary>
		/// Gets or sets the face value.
		/// </summary>
		public byte Face { get; set; }

		/// <summary>
		/// Gets or sets the walkspeed value.
		/// </summary>
		public float Walkspeed { get; set; }

		/// <summary>
		/// Gets or sets the runspeed value.
		/// </summary>
		public float Runspeed { get; set; }

		/// <summary>
		/// Gets or sets the race value.
		/// </summary>
		public uint Race { get; set; }

		/// <summary>
		/// Gets or sets the showname value.
		/// </summary>
		public byte Showname { get; set; }

		/// <summary>
		/// Gets or sets the bodytype value.
		/// </summary>
		public uint Bodytype { get; set; }

		/// <summary>
		/// Gets or sets the curhp value.
		/// </summary>
		public byte Curhp { get; set; }

		/// <summary>
		/// Gets or sets the haircolor value.
		/// </summary>
		public byte Haircolor { get; set; }

		/// <summary>
		/// Gets or sets the beardcolor value.
		/// </summary>
		public byte Beardcolor { get; set; }

		/// <summary>
		/// Gets or sets the eyecolor1 value.
		/// </summary>
		public byte Eyecolor1 { get; set; }

		/// <summary>
		/// Gets or sets the eyecolor2 value.
		/// </summary>
		public byte Eyecolor2 { get; set; }

		/// <summary>
		/// Gets or sets the hairstyle value.
		/// </summary>
		public byte Hairstyle { get; set; }

		/// <summary>
		/// Gets or sets the beard value.
		/// </summary>
		public byte Beard { get; set; }

		/// <summary>
		/// Gets or sets the drakkinheritage value.
		/// </summary>
		public uint DrakkinHeritage { get; set; }

		/// <summary>
		/// Gets or sets the drakkintattoo value.
		/// </summary>
		public uint DrakkinTattoo { get; set; }

		/// <summary>
		/// Gets or sets the drakkindetails value.
		/// </summary>
		public uint DrakkinDetails { get; set; }

		/// <summary>
		/// Gets or sets the statue value.
		/// </summary>
		public byte Statue { get; set; }

		/// <summary>
		/// Gets or sets the deity value.
		/// </summary>
		public uint Deity { get; set; }

		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint Guildid { get; set; }

		/// <summary>
		/// Gets or sets the guildrank value.
		/// </summary>
		public uint Guildrank { get; set; }

		/// <summary>
		/// Gets or sets the class value.
		/// </summary>
		public byte Class { get; set; }

		/// <summary>
		/// Gets or sets the pvp value.
		/// </summary>
		public byte Pvp { get; set; }

		/// <summary>
		/// Gets or sets the standstate value.
		/// </summary>
		public byte Standstate { get; set; }

		/// <summary>
		/// Gets or sets the light value.
		/// </summary>
		public byte Light { get; set; }

		/// <summary>
		/// Gets or sets the flymode value.
		/// </summary>
		public byte Flymode { get; set; }

		/// <summary>
		/// Gets or sets the equipchest2 value.
		/// </summary>
		public byte EquipChest2 { get; set; }

		/// <summary>
		/// Gets or sets the unknown9 value.
		/// </summary>
		public byte Unknown9 { get; set; }

		/// <summary>
		/// Gets or sets the unknown10 value.
		/// </summary>
		public byte Unknown10 { get; set; }

		/// <summary>
		/// Gets or sets the helm value.
		/// </summary>
		public byte Helm { get; set; }

		/// <summary>
		/// Gets or sets the lastname value.
		/// </summary>
		public byte Lastname { get; set; }

		/// <summary>
		/// Gets or sets the aatitle value.
		/// </summary>
		public uint Aatitle { get; set; }

		/// <summary>
		/// Gets or sets the unknown12 value.
		/// </summary>
		public byte Unknown12 { get; set; }

		/// <summary>
		/// Gets or sets the petownerid value.
		/// </summary>
		public uint Petownerid { get; set; }

		/// <summary>
		/// Gets or sets the unknown13 value.
		/// </summary>
		public byte Unknown13 { get; set; }

		/// <summary>
		/// Gets or sets the playerstate value.
		/// </summary>
		public uint Playerstate { get; set; }

		/// <summary>
		/// Gets or sets the unknown15 value.
		/// </summary>
		public uint Unknown15 { get; set; }

		/// <summary>
		/// Gets or sets the unknown16 value.
		/// </summary>
		public uint Unknown16 { get; set; }

		/// <summary>
		/// Gets or sets the unknown17 value.
		/// </summary>
		public uint Unknown17 { get; set; }

		/// <summary>
		/// Gets or sets the unknown18 value.
		/// </summary>
		public uint Unknown18 { get; set; }

		/// <summary>
		/// Gets or sets the unknown19 value.
		/// </summary>
		public uint Unknown19 { get; set; }

		/// <summary>
		/// Gets or sets the position value.
		/// </summary>
		public uint Position { get; set; }

		/// <summary>
		/// Gets or sets the equipmenttint value.
		/// </summary>
		public uint EquipmentTint { get; set; }

		/// <summary>
		/// Gets or sets the equipment value.
		/// </summary>
		public uint Equipment { get; set; }

		/// <summary>
		/// Gets or sets the unknown20 value.
		/// </summary>
		public byte[] Unknown20 { get; set; }

		/// <summary>
		/// Gets or sets the ismercenary value.
		/// </summary>
		public byte Ismercenary { get; set; }

		/// <summary>
		/// Gets or sets the unknown21 value.
		/// </summary>
		public byte[] Unknown21 { get; set; }

		/// <summary>
		/// Initializes a new instance of the ZoneSpawns struct with specified field values.
		/// </summary>
		/// <param name="name">The name value.</param>
		/// <param name="spawnId">The spawnid value.</param>
		/// <param name="level">The level value.</param>
		/// <param name="bounding_radius">The boundingradius value.</param>
		/// <param name="NPC">The npc value.</param>
		/// <param name="Bitfields">The bitfields value.</param>
		/// <param name="otherData">The otherdata value.</param>
		/// <param name="unknown3">The unknown3 value.</param>
		/// <param name="unknown4">The unknown4 value.</param>
		/// <param name="size">The size value.</param>
		/// <param name="face">The face value.</param>
		/// <param name="walkspeed">The walkspeed value.</param>
		/// <param name="runspeed">The runspeed value.</param>
		/// <param name="race">The race value.</param>
		/// <param name="showname">The showname value.</param>
		/// <param name="bodytype">The bodytype value.</param>
		/// <param name="curHp">The curhp value.</param>
		/// <param name="haircolor">The haircolor value.</param>
		/// <param name="beardcolor">The beardcolor value.</param>
		/// <param name="eyecolor1">The eyecolor1 value.</param>
		/// <param name="eyecolor2">The eyecolor2 value.</param>
		/// <param name="hairstyle">The hairstyle value.</param>
		/// <param name="beard">The beard value.</param>
		/// <param name="drakkin_heritage">The drakkinheritage value.</param>
		/// <param name="drakkin_tattoo">The drakkintattoo value.</param>
		/// <param name="drakkin_details">The drakkindetails value.</param>
		/// <param name="statue">The statue value.</param>
		/// <param name="deity">The deity value.</param>
		/// <param name="guildID">The guildid value.</param>
		/// <param name="guildrank">The guildrank value.</param>
		/// <param name="class_">The class value.</param>
		/// <param name="pvp">The pvp value.</param>
		/// <param name="StandState">The standstate value.</param>
		/// <param name="light">The light value.</param>
		/// <param name="flymode">The flymode value.</param>
		/// <param name="equip_chest2">The equipchest2 value.</param>
		/// <param name="unknown9">The unknown9 value.</param>
		/// <param name="unknown10">The unknown10 value.</param>
		/// <param name="helm">The helm value.</param>
		/// <param name="lastName">The lastname value.</param>
		/// <param name="aatitle">The aatitle value.</param>
		/// <param name="unknown12">The unknown12 value.</param>
		/// <param name="petOwnerId">The petownerid value.</param>
		/// <param name="unknown13">The unknown13 value.</param>
		/// <param name="PlayerState">The playerstate value.</param>
		/// <param name="unknown15">The unknown15 value.</param>
		/// <param name="unknown16">The unknown16 value.</param>
		/// <param name="unknown17">The unknown17 value.</param>
		/// <param name="unknown18">The unknown18 value.</param>
		/// <param name="unknown19">The unknown19 value.</param>
		/// <param name="Position">The position value.</param>
		/// <param name="equipment_tint">The equipmenttint value.</param>
		/// <param name="equipment">The equipment value.</param>
		/// <param name="unknown20">The unknown20 value.</param>
		/// <param name="IsMercenary">The ismercenary value.</param>
		/// <param name="unknown21">The unknown21 value.</param>
		public ZoneSpawns(byte name, uint spawnId, byte level, float bounding_radius, byte NPC, uint Bitfields, byte otherData, float unknown3, float unknown4, float size, byte face, float walkspeed, float runspeed, uint race, byte showname, uint bodytype, byte curHp, byte haircolor, byte beardcolor, byte eyecolor1, byte eyecolor2, byte hairstyle, byte beard, uint drakkin_heritage, uint drakkin_tattoo, uint drakkin_details, byte statue, uint deity, uint guildID, uint guildrank, byte class_, byte pvp, byte StandState, byte light, byte flymode, byte equip_chest2, byte unknown9, byte unknown10, byte helm, byte lastName, uint aatitle, byte unknown12, uint petOwnerId, byte unknown13, uint PlayerState, uint unknown15, uint unknown16, uint unknown17, uint unknown18, uint unknown19, uint Position, uint equipment_tint, uint equipment, byte[] unknown20, byte IsMercenary, byte[] unknown21) : this() {
			Name = name;
			Spawnid = spawnId;
			Level = level;
			BoundingRadius = bounding_radius;
			NPC = NPC;
			Bitfields = Bitfields;
			Otherdata = otherData;
			Unknown3 = unknown3;
			Unknown4 = unknown4;
			Size = size;
			Face = face;
			Walkspeed = walkspeed;
			Runspeed = runspeed;
			Race = race;
			Showname = showname;
			Bodytype = bodytype;
			Curhp = curHp;
			Haircolor = haircolor;
			Beardcolor = beardcolor;
			Eyecolor1 = eyecolor1;
			Eyecolor2 = eyecolor2;
			Hairstyle = hairstyle;
			Beard = beard;
			DrakkinHeritage = drakkin_heritage;
			DrakkinTattoo = drakkin_tattoo;
			DrakkinDetails = drakkin_details;
			Statue = statue;
			Deity = deity;
			Guildid = guildID;
			Guildrank = guildrank;
			Class = class_;
			Pvp = pvp;
			Standstate = StandState;
			Light = light;
			Flymode = flymode;
			EquipChest2 = equip_chest2;
			Unknown9 = unknown9;
			Unknown10 = unknown10;
			Helm = helm;
			Lastname = lastName;
			Aatitle = aatitle;
			Unknown12 = unknown12;
			Petownerid = petOwnerId;
			Unknown13 = unknown13;
			Playerstate = PlayerState;
			Unknown15 = unknown15;
			Unknown16 = unknown16;
			Unknown17 = unknown17;
			Unknown18 = unknown18;
			Unknown19 = unknown19;
			Position = Position;
			EquipmentTint = equipment_tint;
			Equipment = equipment;
			Unknown20 = unknown20;
			Ismercenary = IsMercenary;
			Unknown21 = unknown21;
		}

		/// <summary>
		/// Initializes a new instance of the ZoneSpawns struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ZoneSpawns(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ZoneSpawns struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ZoneSpawns(BinaryReader br) : this() {
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
			Name = br.ReadByte();
			Spawnid = br.ReadUInt32();
			Level = br.ReadByte();
			BoundingRadius = br.ReadSingle();
			NPC = br.ReadByte();
			Bitfields = br.ReadUInt32();
			Otherdata = br.ReadByte();
			Unknown3 = br.ReadSingle();
			Unknown4 = br.ReadSingle();
			Size = br.ReadSingle();
			Face = br.ReadByte();
			Walkspeed = br.ReadSingle();
			Runspeed = br.ReadSingle();
			Race = br.ReadUInt32();
			Showname = br.ReadByte();
			Bodytype = br.ReadUInt32();
			Curhp = br.ReadByte();
			Haircolor = br.ReadByte();
			Beardcolor = br.ReadByte();
			Eyecolor1 = br.ReadByte();
			Eyecolor2 = br.ReadByte();
			Hairstyle = br.ReadByte();
			Beard = br.ReadByte();
			DrakkinHeritage = br.ReadUInt32();
			DrakkinTattoo = br.ReadUInt32();
			DrakkinDetails = br.ReadUInt32();
			Statue = br.ReadByte();
			Deity = br.ReadUInt32();
			Guildid = br.ReadUInt32();
			Guildrank = br.ReadUInt32();
			Class = br.ReadByte();
			Pvp = br.ReadByte();
			Standstate = br.ReadByte();
			Light = br.ReadByte();
			Flymode = br.ReadByte();
			EquipChest2 = br.ReadByte();
			Unknown9 = br.ReadByte();
			Unknown10 = br.ReadByte();
			Helm = br.ReadByte();
			Lastname = br.ReadByte();
			Aatitle = br.ReadUInt32();
			Unknown12 = br.ReadByte();
			Petownerid = br.ReadUInt32();
			Unknown13 = br.ReadByte();
			Playerstate = br.ReadUInt32();
			Unknown15 = br.ReadUInt32();
			Unknown16 = br.ReadUInt32();
			Unknown17 = br.ReadUInt32();
			Unknown18 = br.ReadUInt32();
			Unknown19 = br.ReadUInt32();
			Position = br.ReadUInt32();
			EquipmentTint = br.ReadUInt32();
			Equipment = br.ReadUInt32();
			// TODO: Array reading for Unknown20 - implement based on actual array size
			// Unknown20 = new byte[size];
			Ismercenary = br.ReadByte();
			// TODO: Array reading for Unknown21 - implement based on actual array size
			// Unknown21 = new byte[size];
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
			bw.Write(Name);
			bw.Write(Spawnid);
			bw.Write(Level);
			bw.Write(BoundingRadius);
			bw.Write(NPC);
			bw.Write(Bitfields);
			bw.Write(Otherdata);
			bw.Write(Unknown3);
			bw.Write(Unknown4);
			bw.Write(Size);
			bw.Write(Face);
			bw.Write(Walkspeed);
			bw.Write(Runspeed);
			bw.Write(Race);
			bw.Write(Showname);
			bw.Write(Bodytype);
			bw.Write(Curhp);
			bw.Write(Haircolor);
			bw.Write(Beardcolor);
			bw.Write(Eyecolor1);
			bw.Write(Eyecolor2);
			bw.Write(Hairstyle);
			bw.Write(Beard);
			bw.Write(DrakkinHeritage);
			bw.Write(DrakkinTattoo);
			bw.Write(DrakkinDetails);
			bw.Write(Statue);
			bw.Write(Deity);
			bw.Write(Guildid);
			bw.Write(Guildrank);
			bw.Write(Class);
			bw.Write(Pvp);
			bw.Write(Standstate);
			bw.Write(Light);
			bw.Write(Flymode);
			bw.Write(EquipChest2);
			bw.Write(Unknown9);
			bw.Write(Unknown10);
			bw.Write(Helm);
			bw.Write(Lastname);
			bw.Write(Aatitle);
			bw.Write(Unknown12);
			bw.Write(Petownerid);
			bw.Write(Unknown13);
			bw.Write(Playerstate);
			bw.Write(Unknown15);
			bw.Write(Unknown16);
			bw.Write(Unknown17);
			bw.Write(Unknown18);
			bw.Write(Unknown19);
			bw.Write(Position);
			bw.Write(EquipmentTint);
			bw.Write(Equipment);
			// TODO: Array writing for Unknown20 - implement based on actual array size
			// foreach(var item in Unknown20) bw.Write(item);
			bw.Write(Ismercenary);
			// TODO: Array writing for Unknown21 - implement based on actual array size
			// foreach(var item in Unknown21) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ZoneSpawns {\n";
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Spawnid = ";
			try {
				ret += $"{ Indentify(Spawnid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Level = ";
			try {
				ret += $"{ Indentify(Level) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	BoundingRadius = ";
			try {
				ret += $"{ Indentify(BoundingRadius) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NPC = ";
			try {
				ret += $"{ Indentify(NPC) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Bitfields = ";
			try {
				ret += $"{ Indentify(Bitfields) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Otherdata = ";
			try {
				ret += $"{ Indentify(Otherdata) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown3 = ";
			try {
				ret += $"{ Indentify(Unknown3) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown4 = ";
			try {
				ret += $"{ Indentify(Unknown4) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Size = ";
			try {
				ret += $"{ Indentify(Size) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Face = ";
			try {
				ret += $"{ Indentify(Face) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Walkspeed = ";
			try {
				ret += $"{ Indentify(Walkspeed) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Runspeed = ";
			try {
				ret += $"{ Indentify(Runspeed) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Race = ";
			try {
				ret += $"{ Indentify(Race) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Showname = ";
			try {
				ret += $"{ Indentify(Showname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Bodytype = ";
			try {
				ret += $"{ Indentify(Bodytype) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Curhp = ";
			try {
				ret += $"{ Indentify(Curhp) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Haircolor = ";
			try {
				ret += $"{ Indentify(Haircolor) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Beardcolor = ";
			try {
				ret += $"{ Indentify(Beardcolor) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Eyecolor1 = ";
			try {
				ret += $"{ Indentify(Eyecolor1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Eyecolor2 = ";
			try {
				ret += $"{ Indentify(Eyecolor2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Hairstyle = ";
			try {
				ret += $"{ Indentify(Hairstyle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Beard = ";
			try {
				ret += $"{ Indentify(Beard) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DrakkinHeritage = ";
			try {
				ret += $"{ Indentify(DrakkinHeritage) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DrakkinTattoo = ";
			try {
				ret += $"{ Indentify(DrakkinTattoo) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DrakkinDetails = ";
			try {
				ret += $"{ Indentify(DrakkinDetails) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Statue = ";
			try {
				ret += $"{ Indentify(Statue) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Deity = ";
			try {
				ret += $"{ Indentify(Deity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Guildid = ";
			try {
				ret += $"{ Indentify(Guildid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Guildrank = ";
			try {
				ret += $"{ Indentify(Guildrank) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Class = ";
			try {
				ret += $"{ Indentify(Class) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Pvp = ";
			try {
				ret += $"{ Indentify(Pvp) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Standstate = ";
			try {
				ret += $"{ Indentify(Standstate) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Light = ";
			try {
				ret += $"{ Indentify(Light) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Flymode = ";
			try {
				ret += $"{ Indentify(Flymode) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	EquipChest2 = ";
			try {
				ret += $"{ Indentify(EquipChest2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown9 = ";
			try {
				ret += $"{ Indentify(Unknown9) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown10 = ";
			try {
				ret += $"{ Indentify(Unknown10) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Helm = ";
			try {
				ret += $"{ Indentify(Helm) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Lastname = ";
			try {
				ret += $"{ Indentify(Lastname) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Aatitle = ";
			try {
				ret += $"{ Indentify(Aatitle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown12 = ";
			try {
				ret += $"{ Indentify(Unknown12) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Petownerid = ";
			try {
				ret += $"{ Indentify(Petownerid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown13 = ";
			try {
				ret += $"{ Indentify(Unknown13) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Playerstate = ";
			try {
				ret += $"{ Indentify(Playerstate) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown15 = ";
			try {
				ret += $"{ Indentify(Unknown15) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown16 = ";
			try {
				ret += $"{ Indentify(Unknown16) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown17 = ";
			try {
				ret += $"{ Indentify(Unknown17) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown18 = ";
			try {
				ret += $"{ Indentify(Unknown18) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown19 = ";
			try {
				ret += $"{ Indentify(Unknown19) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Position = ";
			try {
				ret += $"{ Indentify(Position) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	EquipmentTint = ";
			try {
				ret += $"{ Indentify(EquipmentTint) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Equipment = ";
			try {
				ret += $"{ Indentify(Equipment) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown20 = ";
			try {
				ret += $"{ Indentify(Unknown20) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Ismercenary = ";
			try {
				ret += $"{ Indentify(Ismercenary) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown21 = ";
			try {
				ret += $"{ Indentify(Unknown21) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}