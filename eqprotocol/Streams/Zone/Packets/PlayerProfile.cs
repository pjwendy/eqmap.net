using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;
using static EQProtocol.Streams.Zone.Constants;
using EQProtocol.Streams.Common;

// C++ Structure Definition:
// struct PlayerProfile_Struct
// {
// /*00000*/ uint32  checksum;				//
// //BEGIN SUB-STRUCT used for shrouding stuff...
// /*00004*/ uint32  gender;				// Player Gender - 0 Male, 1 Female
// /*00008*/ uint32  race;					// Player race
// /*00012*/ uint32  class_;				// Player class
// /*00016*/ uint8  unknown00016[40];		// #### uint32  unknown00016;   in Titanium ####uint8[40]
// /*00056*/ uint8   level;				// Level of player
// /*00057*/ uint8   level1;				// Level of player (again?)
// /*00058*/ uint8   unknown00058[2];		// ***Placeholder
// /*00060*/ BindStruct binds[5];			// Bind points (primary is first)
// /*00160*/ uint32  deity;				// deity
// /*00164*/ uint32  intoxication;			// Alcohol level (in ticks till sober?)
// /*00168*/ uint32  spellSlotRefresh[spells::SPELL_GEM_COUNT]; // Refresh time (millis) - 4 Octets Each
// /*00208*/ uint8   unknown00208[6];		// Seen 00 00 00 00 00 00 00 00 00 00 00 00 02 01
// /*00222*/ uint32  abilitySlotRefresh;
// /*00226*/ uint8   haircolor;			// Player hair color
// /*00227*/ uint8   beardcolor;			// Player beard color
// /*00228*/ uint8   eyecolor1;			// Player left eye color
// /*00229*/ uint8   eyecolor2;			// Player right eye color
// /*00230*/ uint8   hairstyle;			// Player hair style
// /*00231*/ uint8   beard;				// Player beard type
// /*00232*/ uint8	  unknown00232[4];		// was 14
// /*00236*/ TextureProfile equipment;
// /*00344*/ uint8 unknown00344[168];		// Underfoot Shows [160]
// /*00512*/ TintProfile item_tint;		// RR GG BB 00
// /*00548*/ AA_Array  aa_array[MAX_PP_AA_ARRAY];	// [3600] AAs 12 bytes each
// /*04148*/ uint32  points;				// Unspent Practice points - RELOCATED???
// /*04152*/ uint32  mana;					// Current mana
// /*04156*/ uint32  cur_hp;				// Current HP without +HP equipment
// /*04160*/ uint32  STR;					// Strength - 6e 00 00 00 - 110
// /*04164*/ uint32  STA;					// Stamina - 73 00 00 00 - 115
// /*04168*/ uint32  CHA;					// Charisma - 37 00 00 00 - 55
// /*04172*/ uint32  DEX;					// Dexterity - 50 00 00 00 - 80
// /*04176*/ uint32  INT;					// Intelligence - 3c 00 00 00 - 60
// /*04180*/ uint32  AGI;					// Agility - 5f 00 00 00 - 95
// /*04184*/ uint32  WIS;					// Wisdom - 46 00 00 00 - 70
// /*04188*/ uint8   unknown04188[28];		//
// /*04216*/ uint8   face;					// Player face - Actually uint32?
// /*04217*/ uint8   unknown04217[147];		// was [175]
// /*04364*/ uint32   spell_book[spells::SPELLBOOK_SIZE];	// List of the Spells in spellbook 720 = 90 pages [2880] was [1920]
// /*07244*/ uint32   mem_spells[spells::SPELL_GEM_COUNT]; // List of spells memorized
// /*07284*/ uint8   unknown07284[20];		//#### uint8 unknown04396[32]; in Titanium ####[28]
// /*07312*/ uint32  platinum;				// Platinum Pieces on player
// /*07316*/ uint32  gold;					// Gold Pieces on player
// /*07320*/ uint32  silver;				// Silver Pieces on player
// /*07324*/ uint32  copper;				// Copper Pieces on player
// /*07328*/ uint32  platinum_cursor;		// Platinum Pieces on cursor
// /*07332*/ uint32  gold_cursor;			// Gold Pieces on cursor
// /*07336*/ uint32  silver_cursor;		// Silver Pieces on cursor
// /*07340*/ uint32  copper_cursor;		// Copper Pieces on cursor
// /*07344*/ uint32  skills[MAX_PP_SKILL];	// [400] List of skills	// 100 dword buffer
// /*07744*/ uint32  InnateSkills[MAX_PP_INNATE_SKILL];
// /*07844*/ uint8   unknown07644[36];
// /*07880*/ uint32  toxicity;				// Potion Toxicity (15=too toxic, each potion adds 3)
// /*07884*/ uint32  thirst_level;			// Drink (ticks till next drink)
// /*07888*/ uint32  hunger_level;			// Food (ticks till next eat)
// /*07892*/ SpellBuff_Struct buffs[BUFF_COUNT];	// [2280] Buffs currently on the player (30 Max) - (Each Size 76)
// /*10172*/ Disciplines_Struct  disciplines;	// [400] Known disciplines
// /*10972*/ uint32  recastTimers[MAX_RECAST_TYPES]; // Timers (UNIX Time of last use)
// /*11052*/ uint8   unknown11052[160];		// Some type of Timers
// /*11212*/ uint32  endurance;			// Current endurance
// /*11216*/ uint8   unknown11216[20];		// ?
// /*11236*/ uint32  aapoints_spent;		// Number of spent AA points
// /*11240*/ uint32  aapoints;				// Unspent AA points
// /*11244*/ uint8 unknown11244[4];
// /*11248*/ Bandolier_Struct bandoliers[profile::BANDOLIERS_SIZE]; // [6400] bandolier contents
// /*17648*/ PotionBelt_Struct  potionbelt;	// [360] potion belt 72 extra octets by adding 1 more belt slot
// /*18008*/ uint8 unknown18008[8];
// /*18016*/ uint32 available_slots;
// /*18020*/ uint8 unknown18020[80];		//
// //END SUB-STRUCT used for shrouding.
// /*18100*/ char    name[64];				// Name of player
// /*18164*/ char    last_name[32];		// Last name of player
// /*18196*/ uint8   unknown18196[8];  //#### Not In Titanium #### new to SoF[12]
// /*18204*/ uint32   guild_id;            // guildid
// /*18208*/ uint32  birthday;       // character birthday
// /*18212*/ uint32  lastlogin;       // character last save time
// /*18216*/ uint32  account_startdate;       // Date the Account was started - New Field for Underfoot***
// /*18220*/ uint32  timePlayedMin;      // time character played
// /*18224*/ uint8   pvp;                // 1=pvp, 0=not pvp
// /*18225*/ uint8   anon;               // 2=roleplay, 1=anon, 0=not anon
// /*18226*/ uint8   gm;                 // 0=no, 1=yes (guessing!)
// /*18227*/ uint8    guildrank;        // 0=member, 1=officer, 2=guildleader -1=no guild
// /*18228*/ uint32  guildbanker;
// /*18232*/ uint8 unknown18232[4];  //was [8]
// /*18236*/ uint32  exp;                // Current Experience
// /*18240*/ uint8 unknown18240[8];
// /*18248*/ uint32  timeentitledonaccount;
// /*18252*/ uint8   languages[MAX_PP_LANGUAGE]; // List of languages
// /*18277*/ uint8   unknown18277[7];    //#### uint8   unknown13109[4]; in Titanium ####[7]
// /*18284*/ float     y;                  // Players y position (NOT positive about this switch)
// /*18288*/ float     x;                  // Players x position
// /*18292*/ float     z;                  // Players z position
// /*18296*/ float     heading;            // Players heading
// /*18300*/ uint8   unknown18300[4];    // ***Placeholder
// /*18304*/ uint32  platinum_bank;      // Platinum Pieces in Bank
// /*18308*/ uint32  gold_bank;          // Gold Pieces in Bank
// /*18312*/ uint32  silver_bank;        // Silver Pieces in Bank
// /*18316*/ uint32  copper_bank;        // Copper Pieces in Bank
// /*18320*/ uint32  platinum_shared;    // Shared platinum pieces
// /*18324*/ uint8 unknown18324[1036];    // was [716]
// /*19360*/ uint32  expansions;         // Bitmask for expansions ff 7f 00 00 - SoD
// /*19364*/ uint8 unknown19364[12];
// /*19376*/ uint32  autosplit;          // 0 = off, 1 = on
// /*19380*/ uint8 unknown19380[16];
// /*19396*/ uint16  zone_id;             // see zones.h
// /*19398*/ uint16  zoneInstance;       // Instance id
// /*19400*/ char      groupMembers[MAX_GROUP_MEMBERS][64];// 384 all the members in group, including self
// /*19784*/ char      groupLeader[64];    // Leader of the group ?
// /*19848*/ uint8 unknown19848[540];  // was [348]
// /*20388*/ uint32  entityid;
// /*20392*/ uint32  leadAAActive;       // 0 = leader AA off, 1 = leader AA on
// /*20396*/ uint8 unknown20396[4];
// /*20400*/ int32  ldon_points_guk;    // Earned GUK points
// /*20404*/ int32  ldon_points_mir;    // Earned MIR points
// /*20408*/ int32  ldon_points_mmc;    // Earned MMC points
// /*20412*/ int32  ldon_points_ruj;    // Earned RUJ points
// /*20416*/ int32  ldon_points_tak;    // Earned TAK points
// /*20420*/ int32  ldon_points_available;  // Available LDON points
// /*20424*/ uint32  unknown20424[7];
// /*20452*/ uint32  unknown20452;
// /*20456*/ uint32  unknown20456;
// /*20460*/ uint8 unknown20460[4];
// /*20464*/ uint32  unknown20464[6];
// /*20488*/ uint8 unknown20488[72]; // was [136]
// /*20560*/ float  tribute_time_remaining;        // Time remaining on tribute (millisecs)
// /*20564*/ uint32  career_tribute_points;      // Total favor points for this char
// /*20568*/ uint32  unknown20546;        // *** Placeholder
// /*20572*/ uint32  tribute_points;     // Current tribute points
// /*20576*/ uint32  unknown20572;        // *** Placeholder
// /*20580*/ uint32  tribute_active;      // 0 = off, 1=on
// /*20584*/ Tribute_Struct tributes[MAX_PLAYER_TRIBUTES]; // [40] Current tribute loadout
// /*20624*/ uint8 unknown20620[4];
// /*20628*/ double group_leadership_exp;     // Current group lead exp points
// /*20636*/ double raid_leadership_exp;      // Current raid lead AA exp points
// /*20644*/ uint32  group_leadership_points; // Unspent group lead AA points
// /*20648*/ uint32  raid_leadership_points;  // Unspent raid lead AA points
// /*20652*/ LeadershipAA_Struct leader_abilities; // [128]Leader AA ranks 19332
// /*20780*/ uint8 unknown20776[128];		// was [128]
// /*20908*/ uint32  air_remaining;       // Air supply (seconds)
// /*20912*/ uint32  PVPKills;
// /*20916*/ uint32  PVPDeaths;
// /*20920*/ uint32  PVPCurrentPoints;
// /*20924*/ uint32  PVPCareerPoints;
// /*20928*/ uint32  PVPBestKillStreak;
// /*20932*/ uint32  PVPWorstDeathStreak;
// /*20936*/ uint32  PVPCurrentKillStreak;
// /*20940*/ PVPStatsEntry_Struct PVPLastKill;		// size 88
// /*21028*/ PVPStatsEntry_Struct PVPLastDeath;	// size 88
// /*21116*/ uint32  PVPNumberOfKillsInLast24Hours;
// /*21120*/ PVPStatsEntry_Struct PVPRecentKills[50];	// size 4400 - 88 each
// /*25520*/ uint32 expAA;               // Exp earned in current AA point
// /*25524*/ uint8 unknown25524[40];
// /*25564*/ uint32 currentRadCrystals;  // Current count of radiant crystals
// /*25568*/ uint32 careerRadCrystals;   // Total count of radiant crystals ever
// /*25572*/ uint32 currentEbonCrystals; // Current count of ebon crystals
// /*25576*/ uint32 careerEbonCrystals;  // Total count of ebon crystals ever
// /*25580*/ uint8  groupAutoconsent;    // 0=off, 1=on
// /*25581*/ uint8  raidAutoconsent;     // 0=off, 1=on
// /*25582*/ uint8  guildAutoconsent;    // 0=off, 1=on
// /*25583*/ uint8  unknown25583;     // ***Placeholder (6/29/2005)
// /*25584*/ uint32 level3;		// SoF looks at the level here to determine how many leadership AA you can bank.
// /*25588*/ uint32 showhelm;            // 0=no, 1=yes
// /*25592*/ uint32 RestTimer;
// /*25596*/ uint8   unknown25596[1036]; // ***Placeholder (2/13/2007) was[1028]or[940]or[1380] - END of Struct
// /*26632*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_PlayerProfile)
// {
// SETUP_DIRECT_ENCODE(PlayerProfile_Struct, structs::PlayerProfile_Struct);
// 
// uint32 r;
// 
// eq->available_slots = 0xffffffff;
// memset(eq->unknown07284, 0xff, sizeof(eq->unknown07284));
// 
// //	OUT(checksum);
// OUT(gender);
// OUT(race);
// OUT(class_);
// //	OUT(unknown00016);
// OUT(level);
// eq->level1 = emu->level;
// //	OUT(unknown00022[2]);
// for (r = 0; r < 5; r++) {
// OUT(binds[r].zone_id);
// OUT(binds[r].x);
// OUT(binds[r].y);
// OUT(binds[r].z);
// OUT(binds[r].heading);
// }
// OUT(deity);
// OUT(intoxication);
// OUT_array(spellSlotRefresh, spells::SPELL_GEM_COUNT);
// OUT(abilitySlotRefresh);
// OUT(points); // Relocation Test
// //	OUT(unknown0166[4]);
// OUT(haircolor);
// OUT(beardcolor);
// OUT(eyecolor1);
// OUT(eyecolor2);
// OUT(hairstyle);
// OUT(beard);
// //	OUT(unknown00178[10]);
// for (r = EQ::textures::textureBegin; r < EQ::textures::materialCount; r++) {
// eq->equipment.Slot[r].Material = emu->item_material.Slot[r].Material;
// eq->equipment.Slot[r].Unknown1 = 0;
// eq->equipment.Slot[r].EliteMaterial = 0;
// //eq->colors[r].color = emu->colors[r].color;
// }
// for (r = 0; r < 7; r++) {
// OUT(item_tint.Slot[r].Color);
// }
// //	OUT(unknown00224[48]);
// //NOTE: new client supports 300 AAs, our internal rep/PP
// //only supports 240..
// for (r = 0; r < MAX_PP_AA_ARRAY; r++) {
// eq->aa_array[r].AA = emu->aa_array[r].AA;
// eq->aa_array[r].value = emu->aa_array[r].value;
// eq->aa_array[r].charges = emu->aa_array[r].charges;
// }
// 
// //	OUT(unknown02220[4]);
// 
// OUT(mana);
// OUT(cur_hp);
// OUT(STR);
// OUT(STA);
// OUT(CHA);
// OUT(AGI);
// OUT(INT);
// OUT(DEX);
// OUT(WIS);
// OUT(face);
// //	OUT(unknown02264[47]);
// 
// if (spells::SPELLBOOK_SIZE <= EQ::spells::SPELLBOOK_SIZE) {
// for (uint32 r = 0; r < spells::SPELLBOOK_SIZE; r++) {
// if (emu->spell_book[r] <= spells::SPELL_ID_MAX)
// eq->spell_book[r] = emu->spell_book[r];
// else
// eq->spell_book[r] = 0xFFFFFFFFU;
// }
// }
// else {
// for (uint32 r = 0; r < EQ::spells::SPELLBOOK_SIZE; r++) {
// if (emu->spell_book[r] <= spells::SPELL_ID_MAX)
// eq->spell_book[r] = emu->spell_book[r];
// else
// eq->spell_book[r] = 0xFFFFFFFFU;
// }
// // invalidate the rest of the spellbook slots
// memset(&eq->spell_book[EQ::spells::SPELLBOOK_SIZE], 0xFF, (sizeof(uint32) * (spells::SPELLBOOK_SIZE - EQ::spells::SPELLBOOK_SIZE)));
// }
// 
// //	OUT(unknown4184[128]);
// OUT_array(mem_spells, spells::SPELL_GEM_COUNT);
// //	OUT(unknown04396[32]);
// OUT(platinum);
// OUT(gold);
// OUT(silver);
// OUT(copper);
// OUT(platinum_cursor);
// OUT(gold_cursor);
// OUT(silver_cursor);
// OUT(copper_cursor);
// 
// OUT_array(skills, structs::MAX_PP_SKILL);	// 1:1 direct copy (100 dword)
// OUT_array(InnateSkills, structs::MAX_PP_INNATE_SKILL);  // 1:1 direct copy (25 dword)
// 
// //	OUT(unknown04760[236]);
// OUT(toxicity);
// OUT(thirst_level);
// OUT(hunger_level);
// //PS this needs to be figured out more; but it was 'good enough'
// for (r = 0; r < BUFF_COUNT; r++)
// {
// if (emu->buffs[r].spellid != 0xFFFF && emu->buffs[r].spellid != 0)
// {
// eq->buffs[r].bard_modifier = 1.0f + (emu->buffs[r].bard_modifier - 10) / 10.0f;
// eq->buffs[r].effect_type= 2;
// eq->buffs[r].player_id = 0x000717fd;
// }
// else
// {
// eq->buffs[r].effect_type = 0;
// eq->buffs[r].bard_modifier = 1.0f;
// }
// OUT(buffs[r].effect_type);
// OUT(buffs[r].level);
// OUT(buffs[r].unknown003);
// OUT(buffs[r].spellid);
// OUT(buffs[r].duration);
// OUT(buffs[r].num_hits);
// OUT(buffs[r].player_id);
// }
// for (r = 0; r < MAX_PP_DISCIPLINES; r++) {
// OUT(disciplines.values[r]);
// }
// OUT_array(recastTimers, structs::MAX_RECAST_TYPES);
// //	OUT(unknown08124[360]);
// OUT(endurance);
// OUT(aapoints_spent);
// OUT(aapoints);
// 
// //	OUT(unknown06160[4]);
// 
// // Copy bandoliers where server and client indices converge
// for (r = 0; r < EQ::profile::BANDOLIERS_SIZE && r < profile::BANDOLIERS_SIZE; ++r) {
// OUT_str(bandoliers[r].Name);
// for (uint32 k = 0; k < profile::BANDOLIER_ITEM_COUNT; ++k) { // Will need adjusting if 'server != client' is ever true
// OUT(bandoliers[r].Items[k].ID);
// OUT(bandoliers[r].Items[k].Icon);
// OUT_str(bandoliers[r].Items[k].Name);
// }
// }
// // Nullify bandoliers where server and client indices diverge, with a client bias
// for (r = EQ::profile::BANDOLIERS_SIZE; r < profile::BANDOLIERS_SIZE; ++r) {
// eq->bandoliers[r].Name[0] = '\0';
// for (uint32 k = 0; k < profile::BANDOLIER_ITEM_COUNT; ++k) { // Will need adjusting if 'server != client' is ever true
// eq->bandoliers[r].Items[k].ID = 0;
// eq->bandoliers[r].Items[k].Icon = 0;
// eq->bandoliers[r].Items[k].Name[0] = '\0';
// }
// }
// 
// //	OUT(unknown07444[5120]);
// 
// // Copy potion belt where server and client indices converge
// for (r = 0; r < EQ::profile::POTION_BELT_SIZE && r < profile::POTION_BELT_SIZE; ++r) {
// OUT(potionbelt.Items[r].ID);
// OUT(potionbelt.Items[r].Icon);
// OUT_str(potionbelt.Items[r].Name);
// }
// // Nullify potion belt where server and client indices diverge, with a client bias
// for (r = EQ::profile::POTION_BELT_SIZE; r < profile::POTION_BELT_SIZE; ++r) {
// eq->potionbelt.Items[r].ID = 0;
// eq->potionbelt.Items[r].Icon = 0;
// eq->potionbelt.Items[r].Name[0] = '\0';
// }
// 
// //	OUT(unknown12852[8]);
// //	OUT(unknown12864[76]);
// 
// OUT_str(name);
// OUT_str(last_name);
// OUT(guild_id);
// OUT(birthday);
// OUT(lastlogin);
// OUT(timePlayedMin);
// OUT(pvp);
// OUT(anon);
// OUT(gm);
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
// OUT(guildrank);
// OUT(guildbanker);
// //	OUT(unknown13054[12]);
// OUT(exp);
// //	OUT(unknown13072[8]);
// OUT(timeentitledonaccount);
// OUT_array(languages, structs::MAX_PP_LANGUAGE);
// //	OUT(unknown13109[7]);
// OUT(y); //reversed x and y
// OUT(x);
// OUT(z);
// OUT(heading);
// //	OUT(unknown13132[4]);
// OUT(platinum_bank);
// OUT(gold_bank);
// OUT(silver_bank);
// OUT(copper_bank);
// OUT(platinum_shared);
// //	OUT(unknown13156[84]);
// OUT(expansions);
// //eq->expansions = 0x1ffff;
// //	OUT(unknown13244[12]);
// OUT(autosplit);
// //	OUT(unknown13260[16]);
// OUT(zone_id);
// OUT(zoneInstance);
// for (r = 0; r < structs::MAX_GROUP_MEMBERS; r++) {
// OUT_str(groupMembers[r]);
// }
// strcpy(eq->groupLeader, emu->groupMembers[0]);
// //	OUT_str(groupLeader);
// //	OUT(unknown13728[660]);
// OUT(entityid);
// OUT(leadAAActive);
// //	OUT(unknown14392[4]);
// OUT(ldon_points_guk);
// OUT(ldon_points_mir);
// OUT(ldon_points_mmc);
// OUT(ldon_points_ruj);
// OUT(ldon_points_tak);
// OUT(ldon_points_available);
// //	OUT(unknown14420[132]);
// OUT(tribute_time_remaining);
// OUT(career_tribute_points);
// //	OUT(unknown7208);
// OUT(tribute_points);
// //	OUT(unknown7216);
// OUT(tribute_active);
// for (r = 0; r < structs::MAX_PLAYER_TRIBUTES; r++) {
// OUT(tributes[r].tribute);
// OUT(tributes[r].tier);
// }
// //	OUT(unknown14616[8]);
// OUT(group_leadership_exp);
// //	OUT(unknown14628);
// OUT(raid_leadership_exp);
// OUT(group_leadership_points);
// OUT(raid_leadership_points);
// OUT_array(leader_abilities.ranks, structs::MAX_LEADERSHIP_AA_ARRAY);
// //	OUT(unknown14772[128]);
// OUT(air_remaining);
// OUT(PVPKills);
// OUT(PVPDeaths);
// OUT(PVPCurrentPoints);
// OUT(PVPCareerPoints);
// OUT(PVPBestKillStreak);
// OUT(PVPWorstDeathStreak);
// OUT(PVPCurrentKillStreak);
// //	OUT(unknown17892[4580]);
// OUT(expAA);
// //	OUT(unknown19516[40]);
// OUT(currentRadCrystals);
// OUT(careerRadCrystals);
// OUT(currentEbonCrystals);
// OUT(careerEbonCrystals);
// OUT(groupAutoconsent);
// OUT(raidAutoconsent);
// OUT(guildAutoconsent);
// //	OUT(unknown19575[5]);
// eq->level3 = emu->level;
// eq->showhelm = emu->showhelm;
// OUT(RestTimer);
// //	OUT(unknown19584[4]);
// //	OUT(unknown19588);
// 
// const uint8 bytes[] = {
// 0xa3, 0x02, 0x00, 0x00, 0x95, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x19, 0x00, 0x00, 0x00,
// 0x19, 0x00, 0x00, 0x00, 0x19, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00,
// 0x0F, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x1F, 0x85, 0xEB, 0x3E, 0x33, 0x33, 0x33, 0x3F,
// 0x04, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00,
// 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
// };
// 
// memcpy(eq->unknown18020, bytes, sizeof(bytes));
// 
// //set the checksum...
// CRC32::SetEQChecksum(__packet->pBuffer, sizeof(structs::PlayerProfile_Struct) - 4);
// 
// FINISH_ENCODE();
// }

//public struct PlayerProfile : IEQStruct
//{
//    uint checksum;
//    public Gender Gender;
//    public uint Race;
//    public uint Class;
//    public byte Level;
//    byte unkLevel;
//    public Bind[] Binds;
//    public uint Deity;
//    public uint Intoxication;
//    public uint[] SpellSlotRefresh;
//    public uint AbilitySlotRefresh;
//    public byte HairColor;
//    public byte BeardColor;
//    public byte EyeColor1;
//    public byte EyeColor2;
//    public byte HairStyle;
//    public byte Beard;
//    public TextureProfile Equipment;
//    public TintProfile ItemTint;
//    public AAArray[] AAArray;
//    public uint Points;
//    public uint Mana;
//    public uint CurHP;
//    public uint STR;
//    public uint STA;
//    public uint CHA;
//    public uint DEX;
//    public uint INT;
//    public uint AGI;
//    public uint WIS;
//    public byte Face;
//    public uint[] SpellBook;
//    public uint[] MemSpells;
//    public Money PlayerMoney;
//    public Money CursorMoney;
//    public uint[] Skills;
//    public uint Toxicity;
//    public uint Thirst;
//    public uint Hunger;
//    public SpellBuff[] Buffs;
//    public uint[] Disciplines;
//    public uint[] RecastTimers;
//    public uint Endurance;
//    public uint AAPointsSpent;
//    public uint AAPoints;
//    public Bandolier[] Bandoliers;
//    public PotionBandolierItem[] PotionBelt;
//    public uint AvailableSlots;
//    public string Name;
//    public string LastName;
//    public uint GuildID;
//    public uint Birthday;
//    public uint LastLogin;
//    public uint AccountStartdate;
//    public uint TimePlayed;
//    public bool PVP;
//    public Roleplay Roleplay;
//    public bool GM;
//    public GuildRank GuildRank;
//    public uint GuildBanker;
//    public uint Experience;
//    uint timeEntitledOnAccount;
//    public byte[] Languages;
//    public float X;
//    public float Y;
//    public float Z;
//    public float Heading;
//    public Money BankMoney;
//    public uint PlatinumShared;
//    uint expansions;
//    public bool Autosplit;
//    public ushort ZoneID;
//    public ushort ZoneInstance;
//    public GroupMember[] GroupMembers;
//    public string GroupLeader;
//    public uint EntityID;
//    public bool LeadAAActive;
//    public LdonPoints LdonPoints;
//    public float TributeTimeRemaining;
//    public uint CareerTributePoints;
//    public uint TributePoints;
//    public bool TributeActive;
//    public Tribute[] Tributes;
//    public double GroupLeadershipExp;
//    public double RaidLeadershipExp;
//    public uint GroupLeadershipPoints;
//    public uint RaidLeadershipPoints;
//    public LeadershipAA LeaderAbilities;
//    public uint AirRemaining;
//    public PVPStats PVPStats;
//    public uint AAExperience;
//    public uint CurrentRadCrystals;
//    public uint CareerRadCrystals;
//    public uint CurrentEbonCrystals;
//    public uint CareerEbonCrystals;
//    public Autoconsent Autoconsent;
//    public uint Level3;
//    public bool ShowHelm;
//    public uint RestTimer;

//    public PlayerProfile(Gender Gender, uint Race, uint Class, byte Level, Bind[] Binds, uint Deity, uint Intoxication, uint[] SpellSlotRefresh, uint AbilitySlotRefresh, byte HairColor, byte BeardColor, byte EyeColor1, byte EyeColor2, byte HairStyle, byte Beard, TextureProfile Equipment, TintProfile ItemTint, AAArray[] AAArray, uint Points, uint Mana, uint CurHP, uint STR, uint STA, uint CHA, uint DEX, uint INT, uint AGI, uint WIS, byte Face, uint[] SpellBook, uint[] MemSpells, Money PlayerMoney, Money CursorMoney, uint[] Skills, uint Toxicity, uint Thirst, uint Hunger, SpellBuff[] Buffs, uint[] Disciplines, uint[] RecastTimers, uint Endurance, uint AAPointsSpent, uint AAPoints, Bandolier[] Bandoliers, PotionBandolierItem[] PotionBelt, uint AvailableSlots, string Name, string LastName, uint GuildID, uint Birthday, uint LastLogin, uint AccountStartdate, uint TimePlayed, bool PVP, Roleplay Roleplay, bool GM, GuildRank GuildRank, uint GuildBanker, uint Experience, byte[] Languages, float X, float Y, float Z, float Heading, Money BankMoney, uint PlatinumShared, bool Autosplit, ushort ZoneID, ushort ZoneInstance, GroupMember[] GroupMembers, string GroupLeader, uint EntityID, bool LeadAAActive, LdonPoints LdonPoints, float TributeTimeRemaining, uint CareerTributePoints, uint TributePoints, bool TributeActive, Tribute[] Tributes, double GroupLeadershipExp, double RaidLeadershipExp, uint GroupLeadershipPoints, uint RaidLeadershipPoints, LeadershipAA LeaderAbilities, uint AirRemaining, PVPStats PVPStats, uint AAExperience, uint CurrentRadCrystals, uint CareerRadCrystals, uint CurrentEbonCrystals, uint CareerEbonCrystals, Autoconsent Autoconsent, uint Level3, bool ShowHelm, uint RestTimer) : this()
//    {
//        this.Gender = Gender;
//        this.Race = Race;
//        this.Class = Class;
//        this.Level = Level;
//        this.Binds = Binds;
//        this.Deity = Deity;
//        this.Intoxication = Intoxication;
//        this.SpellSlotRefresh = SpellSlotRefresh;
//        this.AbilitySlotRefresh = AbilitySlotRefresh;
//        this.HairColor = HairColor;
//        this.BeardColor = BeardColor;
//        this.EyeColor1 = EyeColor1;
//        this.EyeColor2 = EyeColor2;
//        this.HairStyle = HairStyle;
//        this.Beard = Beard;
//        this.Equipment = Equipment;
//        this.ItemTint = ItemTint;
//        this.AAArray = AAArray;
//        this.Points = Points;
//        this.Mana = Mana;
//        this.CurHP = CurHP;
//        this.STR = STR;
//        this.STA = STA;
//        this.CHA = CHA;
//        this.DEX = DEX;
//        this.INT = INT;
//        this.AGI = AGI;
//        this.WIS = WIS;
//        this.Face = Face;
//        this.SpellBook = SpellBook;
//        this.MemSpells = MemSpells;
//        this.PlayerMoney = PlayerMoney;
//        this.CursorMoney = CursorMoney;
//        this.Skills = Skills;
//        this.Toxicity = Toxicity;
//        this.Thirst = Thirst;
//        this.Hunger = Hunger;
//        this.Buffs = Buffs;
//        this.Disciplines = Disciplines;
//        this.RecastTimers = RecastTimers;
//        this.Endurance = Endurance;
//        this.AAPointsSpent = AAPointsSpent;
//        this.AAPoints = AAPoints;
//        this.Bandoliers = Bandoliers;
//        this.PotionBelt = PotionBelt;
//        this.AvailableSlots = AvailableSlots;
//        this.Name = Name;
//        this.LastName = LastName;
//        this.GuildID = GuildID;
//        this.Birthday = Birthday;
//        this.LastLogin = LastLogin;
//        this.AccountStartdate = AccountStartdate;
//        this.TimePlayed = TimePlayed;
//        this.PVP = PVP;
//        this.Roleplay = Roleplay;
//        this.GM = GM;
//        this.GuildRank = GuildRank;
//        this.GuildBanker = GuildBanker;
//        this.Experience = Experience;
//        this.Languages = Languages;
//        this.X = X;
//        this.Y = Y;
//        this.Z = Z;
//        this.Heading = Heading;
//        this.BankMoney = BankMoney;
//        this.PlatinumShared = PlatinumShared;
//        this.Autosplit = Autosplit;
//        this.ZoneID = ZoneID;
//        this.ZoneInstance = ZoneInstance;
//        this.GroupMembers = GroupMembers;
//        this.GroupLeader = GroupLeader;
//        this.EntityID = EntityID;
//        this.LeadAAActive = LeadAAActive;
//        this.LdonPoints = LdonPoints;
//        this.TributeTimeRemaining = TributeTimeRemaining;
//        this.CareerTributePoints = CareerTributePoints;
//        this.TributePoints = TributePoints;
//        this.TributeActive = TributeActive;
//        this.Tributes = Tributes;
//        this.GroupLeadershipExp = GroupLeadershipExp;
//        this.RaidLeadershipExp = RaidLeadershipExp;
//        this.GroupLeadershipPoints = GroupLeadershipPoints;
//        this.RaidLeadershipPoints = RaidLeadershipPoints;
//        this.LeaderAbilities = LeaderAbilities;
//        this.AirRemaining = AirRemaining;
//        this.PVPStats = PVPStats;
//        this.AAExperience = AAExperience;
//        this.CurrentRadCrystals = CurrentRadCrystals;
//        this.CareerRadCrystals = CareerRadCrystals;
//        this.CurrentEbonCrystals = CurrentEbonCrystals;
//        this.CareerEbonCrystals = CareerEbonCrystals;
//        this.Autoconsent = Autoconsent;
//        this.Level3 = Level3;
//        this.ShowHelm = ShowHelm;
//        this.RestTimer = RestTimer;
//    }

//    public PlayerProfile(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public PlayerProfile(BinaryReader br) : this()
//    {
//        Unpack(br);
//    }
//    public void Unpack(byte[] data, int offset = 0)
//    {
//        using (var ms = new MemoryStream(data, offset, data.Length - offset))
//        {
//            using (var br = new BinaryReader(ms))
//            {
//                Unpack(br);
//            }
//        }
//    }
//    public void Unpack(BinaryReader br)
//    {
//        checksum = br.ReadUInt32();
//        Gender = ((Gender)0).Unpack(br);
//        Race = br.ReadUInt32();
//        Class = br.ReadUInt32();
//        br.ReadBytes(40);
//        Level = br.ReadByte();
//        unkLevel = br.ReadByte();
//        br.ReadBytes(2);
//        Binds = new Bind[5];
//        for (var i = 0; i < 5; ++i)
//        {
//            Binds[i] = new Bind(br);
//        }
//        Deity = br.ReadUInt32();
//        Intoxication = br.ReadUInt32();
//        SpellSlotRefresh = new uint[MAX_PP_MEMSPELL];
//        for (var i = 0; i < MAX_PP_MEMSPELL; ++i)
//        {
//            SpellSlotRefresh[i] = br.ReadUInt32();
//        }
//        br.ReadBytes(6);
//        AbilitySlotRefresh = br.ReadUInt32();
//        HairColor = br.ReadByte();
//        BeardColor = br.ReadByte();
//        EyeColor1 = br.ReadByte();
//        EyeColor2 = br.ReadByte();
//        HairStyle = br.ReadByte();
//        Beard = br.ReadByte();
//        br.ReadBytes(4);
//        Equipment = new TextureProfile(br);
//        br.ReadBytes(168);
//        ItemTint = new TintProfile(br);
//        AAArray = new AAArray[MAX_PP_AA_ARRAY];
//        for (var i = 0; i < MAX_PP_AA_ARRAY; ++i)
//        {
//            AAArray[i] = new AAArray(br);
//        }
//        Points = br.ReadUInt32();
//        Mana = br.ReadUInt32();
//        CurHP = br.ReadUInt32();
//        STR = br.ReadUInt32();
//        STA = br.ReadUInt32();
//        CHA = br.ReadUInt32();
//        DEX = br.ReadUInt32();
//        INT = br.ReadUInt32();
//        AGI = br.ReadUInt32();
//        WIS = br.ReadUInt32();
//        br.ReadBytes(28);
//        Face = br.ReadByte();
//        br.ReadBytes(147);
//        SpellBook = new uint[MAX_PP_SPELLBOOK];
//        for (var i = 0; i < MAX_PP_SPELLBOOK; ++i)
//        {
//            SpellBook[i] = br.ReadUInt32();
//        }
//        MemSpells = new uint[MAX_PP_MEMSPELL];
//        for (var i = 0; i < MAX_PP_MEMSPELL; ++i)
//        {
//            MemSpells[i] = br.ReadUInt32();
//        }
//        br.ReadBytes(20);
//        PlayerMoney = new Money(br);
//        CursorMoney = new Money(br);
//        Skills = new uint[MAX_PP_SKILL];
//        for (var i = 0; i < MAX_PP_SKILL; ++i)
//        {
//            Skills[i] = br.ReadUInt32();
//        }
//        br.ReadBytes(136);
//        Toxicity = br.ReadUInt32();
//        Thirst = br.ReadUInt32();
//        Hunger = br.ReadUInt32();
//        Buffs = new SpellBuff[BUFF_COUNT];
//        for (var i = 0; i < BUFF_COUNT; ++i)
//        {
//            Buffs[i] = new SpellBuff(br);
//        }
//        Disciplines = new uint[MAX_PP_DISCIPLINES];
//        for (var i = 0; i < MAX_PP_DISCIPLINES; ++i)
//        {
//            Disciplines[i] = br.ReadUInt32();
//        }
//        RecastTimers = new uint[MAX_RECAST_TYPES];
//        for (var i = 0; i < MAX_RECAST_TYPES; ++i)
//        {
//            RecastTimers[i] = br.ReadUInt32();
//        }
//        br.ReadBytes(160);
//        Endurance = br.ReadUInt32();
//        br.ReadBytes(20);
//        AAPointsSpent = br.ReadUInt32();
//        AAPoints = br.ReadUInt32();
//        br.ReadBytes(4);
//        Bandoliers = new Bandolier[BANDOLIERS_SIZE];
//        for (var i = 0; i < BANDOLIERS_SIZE; ++i)
//        {
//            Bandoliers[i] = new Bandolier(br);
//        }
//        PotionBelt = new PotionBandolierItem[POTIONBELT_SIZE];
//        for (var i = 0; i < POTIONBELT_SIZE; ++i)
//        {
//            PotionBelt[i] = new PotionBandolierItem(br);
//        }
//        br.ReadBytes(8);
//        AvailableSlots = br.ReadUInt32();
//        br.ReadBytes(80);
//        Name = br.ReadString(64);
//        LastName = br.ReadString(32);
//        br.ReadBytes(8);
//        GuildID = br.ReadUInt32();
//        Birthday = br.ReadUInt32();
//        LastLogin = br.ReadUInt32();
//        AccountStartdate = br.ReadUInt32();
//        TimePlayed = br.ReadUInt32();
//        PVP = br.ReadByte() != 0;
//        Roleplay = ((Roleplay)0).Unpack(br);
//        GM = br.ReadByte() != 0;
//        GuildRank = ((GuildRank)0).Unpack(br);
//        GuildBanker = br.ReadUInt32();
//        br.ReadBytes(4);
//        Experience = br.ReadUInt32();
//        br.ReadBytes(8);
//        timeEntitledOnAccount = br.ReadUInt32();
//        Languages = new byte[MAX_PP_LANGUAGE];
//        for (var i = 0; i < MAX_PP_LANGUAGE; ++i)
//        {
//            Languages[i] = br.ReadByte();
//        }
//        br.ReadBytes(7);
//        X = br.ReadSingle();
//        Y = br.ReadSingle();
//        Z = br.ReadSingle();
//        Heading = br.ReadSingle();
//        br.ReadBytes(4);
//        BankMoney = new Money(br);
//        PlatinumShared = br.ReadUInt32();
//        br.ReadBytes(1036);
//        expansions = br.ReadUInt32();
//        br.ReadBytes(12);
//        Autosplit = br.ReadUInt32() != 0;
//        br.ReadBytes(16);
//        ZoneID = br.ReadUInt16();
//        ZoneInstance = br.ReadUInt16();
//        GroupMembers = new GroupMember[MAX_GROUP_MEMBERS];
//        for (var i = 0; i < MAX_GROUP_MEMBERS; ++i)
//        {
//            GroupMembers[i] = new GroupMember(br);
//        }
//        GroupLeader = br.ReadString(64);
//        br.ReadBytes(540);
//        EntityID = br.ReadUInt32();
//        LeadAAActive = br.ReadUInt32() != 0;
//        br.ReadBytes(4);
//        LdonPoints = new LdonPoints(br);
//        br.ReadBytes(9 * 4 + 4 + 4 * 6 + 72);
//        TributeTimeRemaining = br.ReadSingle();
//        CareerTributePoints = br.ReadUInt32();
//        br.ReadBytes(4);
//        TributePoints = br.ReadUInt32();
//        br.ReadBytes(4);
//        TributeActive = br.ReadUInt32() != 0;
//        Tributes = new Tribute[MAX_PLAYER_TRIBUTES];
//        for (var i = 0; i < MAX_PLAYER_TRIBUTES; ++i)
//        {
//            Tributes[i] = new Tribute(br);
//        }
//        br.ReadBytes(4);
//        GroupLeadershipExp = br.ReadDouble();
//        RaidLeadershipExp = br.ReadDouble();
//        GroupLeadershipPoints = br.ReadUInt32();
//        RaidLeadershipPoints = br.ReadUInt32();
//        LeaderAbilities = new LeadershipAA(br);
//        br.ReadBytes(128);
//        AirRemaining = br.ReadUInt32();
//        PVPStats = new PVPStats(br);
//        AAExperience = br.ReadUInt32();
//        br.ReadBytes(40);
//        CurrentRadCrystals = br.ReadUInt32();
//        CareerRadCrystals = br.ReadUInt32();
//        CurrentEbonCrystals = br.ReadUInt32();
//        CareerEbonCrystals = br.ReadUInt32();
//        Autoconsent = new Autoconsent(br);
//        br.ReadBytes(1);
//        Level3 = br.ReadUInt32();
//        ShowHelm = br.ReadUInt32() != 0;
//        RestTimer = br.ReadUInt32();
//        br.ReadBytes(1036);
//    }

//    public byte[] Pack()
//    {
//        using (var ms = new MemoryStream())
//        {
//            using (var bw = new BinaryWriter(ms))
//            {
//                Pack(bw);
//                return ms.ToArray();
//            }
//        }
//    }
//    public void Pack(BinaryWriter bw)
//    {
//        bw.Write(checksum);
//        bw.Write((uint)Gender);
//        bw.Write(Race);
//        bw.Write(Class);
//        bw.Write(new byte[40]);
//        bw.Write(Level);
//        bw.Write(unkLevel);
//        bw.Write(new byte[2]);
//        for (var i = 0; i < 5; ++i)
//        {
//            Binds[i].Pack(bw);
//        }
//        bw.Write(Deity);
//        bw.Write(Intoxication);
//        for (var i = 0; i < MAX_PP_MEMSPELL; ++i)
//        {
//            bw.Write(SpellSlotRefresh[i]);
//        }
//        bw.Write(new byte[6]);
//        bw.Write(AbilitySlotRefresh);
//        bw.Write(HairColor);
//        bw.Write(BeardColor);
//        bw.Write(EyeColor1);
//        bw.Write(EyeColor2);
//        bw.Write(HairStyle);
//        bw.Write(Beard);
//        bw.Write(new byte[4]);
//        Equipment.Pack(bw);
//        bw.Write(new byte[168]);
//        ItemTint.Pack(bw);
//        for (var i = 0; i < MAX_PP_AA_ARRAY; ++i)
//        {
//            AAArray[i].Pack(bw);
//        }
//        bw.Write(Points);
//        bw.Write(Mana);
//        bw.Write(CurHP);
//        bw.Write(STR);
//        bw.Write(STA);
//        bw.Write(CHA);
//        bw.Write(DEX);
//        bw.Write(INT);
//        bw.Write(AGI);
//        bw.Write(WIS);
//        bw.Write(new byte[28]);
//        bw.Write(Face);
//        bw.Write(new byte[147]);
//        for (var i = 0; i < MAX_PP_SPELLBOOK; ++i)
//        {
//            bw.Write(SpellBook[i]);
//        }
//        for (var i = 0; i < MAX_PP_MEMSPELL; ++i)
//        {
//            bw.Write(MemSpells[i]);
//        }
//        bw.Write(new byte[20]);
//        PlayerMoney.Pack(bw);
//        CursorMoney.Pack(bw);
//        for (var i = 0; i < MAX_PP_SKILL; ++i)
//        {
//            bw.Write(Skills[i]);
//        }
//        bw.Write(new byte[136]);
//        bw.Write(Toxicity);
//        bw.Write(Thirst);
//        bw.Write(Hunger);
//        for (var i = 0; i < BUFF_COUNT; ++i)
//        {
//            Buffs[i].Pack(bw);
//        }
//        for (var i = 0; i < MAX_PP_DISCIPLINES; ++i)
//        {
//            bw.Write(Disciplines[i]);
//        }
//        for (var i = 0; i < MAX_RECAST_TYPES; ++i)
//        {
//            bw.Write(RecastTimers[i]);
//        }
//        bw.Write(new byte[160]);
//        bw.Write(Endurance);
//        bw.Write(new byte[20]);
//        bw.Write(AAPointsSpent);
//        bw.Write(AAPoints);
//        bw.Write(new byte[4]);
//        for (var i = 0; i < BANDOLIERS_SIZE; ++i)
//        {
//            Bandoliers[i].Pack(bw);
//        }
//        for (var i = 0; i < POTIONBELT_SIZE; ++i)
//        {
//            PotionBelt[i].Pack(bw);
//        }
//        bw.Write(new byte[8]);
//        bw.Write(AvailableSlots);
//        bw.Write(new byte[80]);
//        bw.Write(Name.ToBytes(64));
//        bw.Write(LastName.ToBytes(32));
//        bw.Write(new byte[8]);
//        bw.Write(GuildID);
//        bw.Write(Birthday);
//        bw.Write(LastLogin);
//        bw.Write(AccountStartdate);
//        bw.Write(TimePlayed);
//        bw.Write((byte)(PVP ? 1 : 0));
//        bw.Write((byte)Roleplay);
//        bw.Write((byte)(GM ? 1 : 0));
//        bw.Write((sbyte)GuildRank);
//        bw.Write(GuildBanker);
//        bw.Write(new byte[4]);
//        bw.Write(Experience);
//        bw.Write(new byte[8]);
//        bw.Write(timeEntitledOnAccount);
//        for (var i = 0; i < MAX_PP_LANGUAGE; ++i)
//        {
//            bw.Write(Languages[i]);
//        }
//        bw.Write(new byte[7]);
//        bw.Write(X);
//        bw.Write(Y);
//        bw.Write(Z);
//        bw.Write(Heading);
//        bw.Write(new byte[4]);
//        BankMoney.Pack(bw);
//        bw.Write(PlatinumShared);
//        bw.Write(new byte[1036]);
//        bw.Write(expansions);
//        bw.Write(new byte[12]);
//        bw.Write((uint)(Autosplit ? 1 : 0));
//        bw.Write(new byte[16]);
//        bw.Write(ZoneID);
//        bw.Write(ZoneInstance);
//        for (var i = 0; i < MAX_GROUP_MEMBERS; ++i)
//        {
//            GroupMembers[i].Pack(bw);
//        }
//        bw.Write(GroupLeader.ToBytes(64));
//        bw.Write(new byte[540]);
//        bw.Write(EntityID);
//        bw.Write((uint)(LeadAAActive ? 1 : 0));
//        bw.Write(new byte[4]);
//        LdonPoints.Pack(bw);
//        bw.Write(new byte[9 * 4 + 4 + 4 * 6 + 72]);
//        bw.Write(TributeTimeRemaining);
//        bw.Write(CareerTributePoints);
//        bw.Write(new byte[4]);
//        bw.Write(TributePoints);
//        bw.Write(new byte[4]);
//        bw.Write((uint)(TributeActive ? 1 : 0));
//        for (var i = 0; i < MAX_PLAYER_TRIBUTES; ++i)
//        {
//            Tributes[i].Pack(bw);
//        }
//        bw.Write(new byte[4]);
//        bw.Write(GroupLeadershipExp);
//        bw.Write(RaidLeadershipExp);
//        bw.Write(GroupLeadershipPoints);
//        bw.Write(RaidLeadershipPoints);
//        LeaderAbilities.Pack(bw);
//        bw.Write(new byte[128]);
//        bw.Write(AirRemaining);
//        PVPStats.Pack(bw);
//        bw.Write(AAExperience);
//        bw.Write(new byte[40]);
//        bw.Write(CurrentRadCrystals);
//        bw.Write(CareerRadCrystals);
//        bw.Write(CurrentEbonCrystals);
//        bw.Write(CareerEbonCrystals);
//        Autoconsent.Pack(bw);
//        bw.Write(new byte[1]);
//        bw.Write(Level3);
//        bw.Write((uint)(ShowHelm ? 1 : 0));
//        bw.Write(RestTimer);
//        bw.Write(new byte[1036]);
//    }

//    public override string ToString()
//    {
//        var ret = "struct PlayerProfile {\n";
//        ret += "\tGender = ";
//        try
//        {
//            ret += $"{Indentify(Gender)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tRace = ";
//        try
//        {
//            ret += $"{Indentify(Race)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tClass = ";
//        try
//        {
//            ret += $"{Indentify(Class)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLevel = ";
//        try
//        {
//            ret += $"{Indentify(Level)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tBinds = ";
//        try
//        {
//            if (Binds != null)
//            {
//                ret += "{\n";
//                for (int i = 0, e = Binds.Length; i < e; ++i)
//                    ret += $"\t\t{Indentify(Binds[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
//                ret += "\t},\n";
//            }
//            else
//            {
//                ret += "!!NULL!!\n";
//            }
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tDeity = ";
//        try
//        {
//            ret += $"{Indentify(Deity)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tIntoxication = ";
//        try
//        {
//            ret += $"{Indentify(Intoxication)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAbilitySlotRefresh = ";
//        try
//        {
//            ret += $"{Indentify(AbilitySlotRefresh)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tHairColor = ";
//        try
//        {
//            ret += $"{Indentify(HairColor)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tBeardColor = ";
//        try
//        {
//            ret += $"{Indentify(BeardColor)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tEyeColor1 = ";
//        try
//        {
//            ret += $"{Indentify(EyeColor1)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tEyeColor2 = ";
//        try
//        {
//            ret += $"{Indentify(EyeColor2)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tHairStyle = ";
//        try
//        {
//            ret += $"{Indentify(HairStyle)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tBeard = ";
//        try
//        {
//            ret += $"{Indentify(Beard)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tEquipment = ";
//        try
//        {
//            ret += $"{Indentify(Equipment)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tItemTint = ";
//        try
//        {
//            ret += $"{Indentify(ItemTint)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPoints = ";
//        try
//        {
//            ret += $"{Indentify(Points)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tMana = ";
//        try
//        {
//            ret += $"{Indentify(Mana)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCurHP = ";
//        try
//        {
//            ret += $"{Indentify(CurHP)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tSTR = ";
//        try
//        {
//            ret += $"{Indentify(STR)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tSTA = ";
//        try
//        {
//            ret += $"{Indentify(STA)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCHA = ";
//        try
//        {
//            ret += $"{Indentify(CHA)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tDEX = ";
//        try
//        {
//            ret += $"{Indentify(DEX)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tINT = ";
//        try
//        {
//            ret += $"{Indentify(INT)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAGI = ";
//        try
//        {
//            ret += $"{Indentify(AGI)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tWIS = ";
//        try
//        {
//            ret += $"{Indentify(WIS)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tFace = ";
//        try
//        {
//            ret += $"{Indentify(Face)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPlayerMoney = ";
//        try
//        {
//            ret += $"{Indentify(PlayerMoney)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCursorMoney = ";
//        try
//        {
//            ret += $"{Indentify(CursorMoney)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tToxicity = ";
//        try
//        {
//            ret += $"{Indentify(Toxicity)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tThirst = ";
//        try
//        {
//            ret += $"{Indentify(Thirst)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tHunger = ";
//        try
//        {
//            ret += $"{Indentify(Hunger)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tEndurance = ";
//        try
//        {
//            ret += $"{Indentify(Endurance)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAAPointsSpent = ";
//        try
//        {
//            ret += $"{Indentify(AAPointsSpent)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAAPoints = ";
//        try
//        {
//            ret += $"{Indentify(AAPoints)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAvailableSlots = ";
//        try
//        {
//            ret += $"{Indentify(AvailableSlots)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tName = ";
//        try
//        {
//            ret += $"{Indentify(Name)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLastName = ";
//        try
//        {
//            ret += $"{Indentify(LastName)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tGuildID = ";
//        try
//        {
//            ret += $"{Indentify(GuildID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tBirthday = ";
//        try
//        {
//            ret += $"{Indentify(Birthday)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLastLogin = ";
//        try
//        {
//            ret += $"{Indentify(LastLogin)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAccountStartdate = ";
//        try
//        {
//            ret += $"{Indentify(AccountStartdate)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tTimePlayed = ";
//        try
//        {
//            ret += $"{Indentify(TimePlayed)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPVP = ";
//        try
//        {
//            ret += $"{Indentify(PVP)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tRoleplay = ";
//        try
//        {
//            ret += $"{Indentify(Roleplay)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tGM = ";
//        try
//        {
//            ret += $"{Indentify(GM)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tGuildRank = ";
//        try
//        {
//            ret += $"{Indentify(GuildRank)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tGuildBanker = ";
//        try
//        {
//            ret += $"{Indentify(GuildBanker)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tExperience = ";
//        try
//        {
//            ret += $"{Indentify(Experience)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tX = ";
//        try
//        {
//            ret += $"{Indentify(X)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tY = ";
//        try
//        {
//            ret += $"{Indentify(Y)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tZ = ";
//        try
//        {
//            ret += $"{Indentify(Z)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tHeading = ";
//        try
//        {
//            ret += $"{Indentify(Heading)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tBankMoney = ";
//        try
//        {
//            ret += $"{Indentify(BankMoney)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPlatinumShared = ";
//        try
//        {
//            ret += $"{Indentify(PlatinumShared)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAutosplit = ";
//        try
//        {
//            ret += $"{Indentify(Autosplit)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tZoneID = ";
//        try
//        {
//            ret += $"{Indentify(ZoneID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tZoneInstance = ";
//        try
//        {
//            ret += $"{Indentify(ZoneInstance)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tGroupMembers = ";
//        try
//        {
//            if (GroupMembers != null)
//            {
//                ret += "{\n";
//                for (int i = 0, e = GroupMembers.Length; i < e; ++i)
//                    ret += $"\t\t{Indentify(GroupMembers[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
//                ret += "\t},\n";
//            }
//            else
//            {
//                ret += "!!NULL!!\n";
//            }
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tGroupLeader = ";
//        try
//        {
//            ret += $"{Indentify(GroupLeader)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tEntityID = ";
//        try
//        {
//            ret += $"{Indentify(EntityID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLeadAAActive = ";
//        try
//        {
//            ret += $"{Indentify(LeadAAActive)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLdonPoints = ";
//        try
//        {
//            ret += $"{Indentify(LdonPoints)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tTributeTimeRemaining = ";
//        try
//        {
//            ret += $"{Indentify(TributeTimeRemaining)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCareerTributePoints = ";
//        try
//        {
//            ret += $"{Indentify(CareerTributePoints)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tTributePoints = ";
//        try
//        {
//            ret += $"{Indentify(TributePoints)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tTributeActive = ";
//        try
//        {
//            ret += $"{Indentify(TributeActive)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tGroupLeadershipExp = ";
//        try
//        {
//            ret += $"{Indentify(GroupLeadershipExp)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tRaidLeadershipExp = ";
//        try
//        {
//            ret += $"{Indentify(RaidLeadershipExp)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tGroupLeadershipPoints = ";
//        try
//        {
//            ret += $"{Indentify(GroupLeadershipPoints)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tRaidLeadershipPoints = ";
//        try
//        {
//            ret += $"{Indentify(RaidLeadershipPoints)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLeaderAbilities = ";
//        try
//        {
//            ret += $"{Indentify(LeaderAbilities)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAirRemaining = ";
//        try
//        {
//            ret += $"{Indentify(AirRemaining)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPVPStats = ";
//        try
//        {
//            ret += $"{Indentify(PVPStats)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAAExperience = ";
//        try
//        {
//            ret += $"{Indentify(AAExperience)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCurrentRadCrystals = ";
//        try
//        {
//            ret += $"{Indentify(CurrentRadCrystals)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCareerRadCrystals = ";
//        try
//        {
//            ret += $"{Indentify(CareerRadCrystals)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCurrentEbonCrystals = ";
//        try
//        {
//            ret += $"{Indentify(CurrentEbonCrystals)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCareerEbonCrystals = ";
//        try
//        {
//            ret += $"{Indentify(CareerEbonCrystals)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAutoconsent = ";
//        try
//        {
//            ret += $"{Indentify(Autoconsent)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLevel3 = ";
//        try
//        {
//            ret += $"{Indentify(Level3)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tShowHelm = ";
//        try
//        {
//            ret += $"{Indentify(ShowHelm)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tRestTimer = ";
//        try
//        {
//            ret += $"{Indentify(RestTimer)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        return ret + "}";
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the PlayerProfile packet structure for EverQuest network communication.
    /// </summary>
    public struct PlayerProfile : IEQStruct
    {
        /// <summary>
        /// The checksum value for the profile.
        /// </summary>
        uint checksum;

        /// <summary>
        /// The gender of the player.
        /// </summary>
        public Gender Gender;

        /// <summary>
        /// The race of the player.
        /// </summary>
        public uint Race;

        /// <summary>
        /// The class of the player.
        /// </summary>
        public uint Class;

        /// <summary>
        /// The level of the player.
        /// </summary>
        public byte Level;

        /// <summary>
        /// Unknown level byte.
        /// </summary>
        byte unkLevel;

        /// <summary>
        /// The bind points of the player.
        /// </summary>
        public Bind[] Binds;

        /// <summary>
        /// The deity of the player.
        /// </summary>
        public uint Deity;

        /// <summary>
        /// The intoxication level of the player.
        /// </summary>
        public uint Intoxication;

        /// <summary>
        /// The spell slot refresh times.
        /// </summary>
        public uint[] SpellSlotRefresh;

        /// <summary>
        /// The ability slot refresh time.
        /// </summary>
        public uint AbilitySlotRefresh;

        /// <summary>
        /// The hair color of the player.
        /// </summary>
        public byte HairColor;

        /// <summary>
        /// The beard color of the player.
        /// </summary>
        public byte BeardColor;

        /// <summary>
        /// The left eye color of the player.
        /// </summary>
        public byte EyeColor1;

        /// <summary>
        /// The right eye color of the player.
        /// </summary>
        public byte EyeColor2;

        /// <summary>
        /// The hair style of the player.
        /// </summary>
        public byte HairStyle;

        /// <summary>
        /// The beard type of the player.
        /// </summary>
        public byte Beard;

        /// <summary>
        /// The equipment texture profile.
        /// </summary>
        public TextureProfile Equipment;

        /// <summary>
        /// The item tint profile.
        /// </summary>
        public TintProfile ItemTint;

        /// <summary>
        /// The array of alternate advancement abilities.
        /// </summary>
        public AAArray[] AAArray;

        /// <summary>
        /// The unspent practice points.
        /// </summary>
        public uint Points;

        /// <summary>
        /// The current mana.
        /// </summary>
        public uint Mana;

        /// <summary>
        /// The current hit points.
        /// </summary>
        public uint CurHP;

        /// <summary>
        /// The strength stat.
        /// </summary>
        public uint STR;

        /// <summary>
        /// The stamina stat.
        /// </summary>
        public uint STA;

        /// <summary>
        /// The charisma stat.
        /// </summary>
        public uint CHA;

        /// <summary>
        /// The dexterity stat.
        /// </summary>
        public uint DEX;

        /// <summary>
        /// The intelligence stat.
        /// </summary>
        public uint INT;

        /// <summary>
        /// The agility stat.
        /// </summary>
        public uint AGI;

        /// <summary>
        /// The wisdom stat.
        /// </summary>
        public uint WIS;

        /// <summary>
        /// The face type.
        /// </summary>
        public byte Face;

        /// <summary>
        /// The spell book.
        /// </summary>
        public uint[] SpellBook;

        /// <summary>
        /// The memorized spells.
        /// </summary>
        public uint[] MemSpells;

        /// <summary>
        /// The player's money.
        /// </summary>
        public Money PlayerMoney;

        /// <summary>
        /// The money on the cursor.
        /// </summary>
        public Money CursorMoney;

        /// <summary>
        /// The array of player skills.
        /// </summary>
        public uint[] Skills;

        /// <summary>
        /// The potion toxicity.
        /// </summary>
        public uint Toxicity;

        /// <summary>
        /// The thirst level.
        /// </summary>
        public uint Thirst;

        /// <summary>
        /// The hunger level.
        /// </summary>
        public uint Hunger;

        /// <summary>
        /// The array of spell buffs.
        /// </summary>
        public SpellBuff[] Buffs;

        /// <summary>
        /// The array of disciplines.
        /// </summary>
        public uint[] Disciplines;

        /// <summary>
        /// The array of recast timers.
        /// </summary>
        public uint[] RecastTimers;

        /// <summary>
        /// The current endurance.
        /// </summary>
        public uint Endurance;

        /// <summary>
        /// The number of spent AA points.
        /// </summary>
        public uint AAPointsSpent;

        /// <summary>
        /// The number of unspent AA points.
        /// </summary>
        public uint AAPoints;

        /// <summary>
        /// The array of bandoliers.
        /// </summary>
        public Bandolier[] Bandoliers;

        /// <summary>
        /// The array of potion belt items.
        /// </summary>
        public PotionBandolierItem[] PotionBelt;

        /// <summary>
        /// The available inventory slots.
        /// </summary>
        public uint AvailableSlots;

        /// <summary>
        /// The player's name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The player's last name.
        /// </summary>
        public string LastName;

        /// <summary>
        /// The guild ID.
        /// </summary>
        public uint GuildID;

        /// <summary>
        /// The character's birthday.
        /// </summary>
        public uint Birthday;

        /// <summary>
        /// The last login time.
        /// </summary>
        public uint LastLogin;

        /// <summary>
        /// The account start date.
        /// </summary>
        public uint AccountStartdate;

        /// <summary>
        /// The total time played.
        /// </summary>
        public uint TimePlayed;

        /// <summary>
        /// Indicates if the player is PVP.
        /// </summary>
        public bool PVP;

        /// <summary>
        /// The roleplay status.
        /// </summary>
        public Roleplay Roleplay;

        /// <summary>
        /// Indicates if the player is a GM.
        /// </summary>
        public bool GM;

        /// <summary>
        /// The guild rank.
        /// </summary>
        public GuildRank GuildRank;

        /// <summary>
        /// The guild banker status.
        /// </summary>
        public uint GuildBanker;

        /// <summary>
        /// The current experience.
        /// </summary>
        public uint Experience;

        /// <summary>
        /// The time entitled on account.
        /// </summary>
        uint timeEntitledOnAccount;

        /// <summary>
        /// The array of known languages.
        /// </summary>
        public byte[] Languages;

        /// <summary>
        /// The X position.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y position.
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z position.
        /// </summary>
        public float Z;

        /// <summary>
        /// The heading.
        /// </summary>
        public float Heading;

        /// <summary>
        /// The bank money.
        /// </summary>
        public Money BankMoney;

        /// <summary>
        /// The shared platinum.
        /// </summary>
        public uint PlatinumShared;

        /// <summary>
        /// The expansions bitmask.
        /// </summary>
        uint expansions;

        /// <summary>
        /// Indicates if autosplit is enabled.
        /// </summary>
        public bool Autosplit;

        /// <summary>
        /// The zone ID.
        /// </summary>
        public ushort ZoneID;

        /// <summary>
        /// The zone instance.
        /// </summary>
        public ushort ZoneInstance;

        /// <summary>
        /// The array of group members.
        /// </summary>
        public GroupMember[] GroupMembers;

        /// <summary>
        /// The group leader name.
        /// </summary>
        public string GroupLeader;

        /// <summary>
        /// The entity ID.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// Indicates if lead AA is active.
        /// </summary>
        public bool LeadAAActive;

        /// <summary>
        /// The LDoN points.
        /// </summary>
        public LdonPoints LdonPoints;

        /// <summary>
        /// The tribute time remaining.
        /// </summary>
        public float TributeTimeRemaining;

        /// <summary>
        /// The career tribute points.
        /// </summary>
        public uint CareerTributePoints;

        /// <summary>
        /// The current tribute points.
        /// </summary>
        public uint TributePoints;

        /// <summary>
        /// Indicates if tribute is active.
        /// </summary>
        public bool TributeActive;

        /// <summary>
        /// The array of tributes.
        /// </summary>
        public Tribute[] Tributes;

        /// <summary>
        /// The group leadership experience.
        /// </summary>
        public double GroupLeadershipExp;

        /// <summary>
        /// The raid leadership experience.
        /// </summary>
        public double RaidLeadershipExp;

        /// <summary>
        /// The group leadership points.
        /// </summary>
        public uint GroupLeadershipPoints;

        /// <summary>
        /// The raid leadership points.
        /// </summary>
        public uint RaidLeadershipPoints;

        /// <summary>
        /// The leader abilities.
        /// </summary>
        public LeadershipAA LeaderAbilities;

        /// <summary>
        /// The air remaining.
        /// </summary>
        public uint AirRemaining;

        /// <summary>
        /// The PVP stats.
        /// </summary>
        public PVPStats PVPStats;

        /// <summary>
        /// The AA experience.
        /// </summary>
        public uint AAExperience;

        /// <summary>
        /// The current radiant crystals.
        /// </summary>
        public uint CurrentRadCrystals;

        /// <summary>
        /// The career radiant crystals.
        /// </summary>
        public uint CareerRadCrystals;

        /// <summary>
        /// The current ebon crystals.
        /// </summary>
        public uint CurrentEbonCrystals;

        /// <summary>
        /// The career ebon crystals.
        /// </summary>
        public uint CareerEbonCrystals;

        /// <summary>
        /// The autoconsent settings.
        /// </summary>
        public Autoconsent Autoconsent;

        /// <summary>
        /// The level3 value.
        /// </summary>
        public uint Level3;

        /// <summary>
        /// Indicates if the helm is shown.
        /// </summary>
        public bool ShowHelm;

        /// <summary>
        /// The rest timer.
        /// </summary>
        public uint RestTimer;

        public PlayerProfile(Gender Gender, uint Race, uint Class, byte Level, Bind[] Binds, uint Deity, uint Intoxication, uint[] SpellSlotRefresh, uint AbilitySlotRefresh, byte HairColor, byte BeardColor, byte EyeColor1, byte EyeColor2, byte HairStyle, byte Beard, TextureProfile Equipment, TintProfile ItemTint, AAArray[] AAArray, uint Points, uint Mana, uint CurHP, uint STR, uint STA, uint CHA, uint DEX, uint INT, uint AGI, uint WIS, byte Face, uint[] SpellBook, uint[] MemSpells, Money PlayerMoney, Money CursorMoney, uint[] Skills, uint Toxicity, uint Thirst, uint Hunger, SpellBuff[] Buffs, uint[] Disciplines, uint[] RecastTimers, uint Endurance, uint AAPointsSpent, uint AAPoints, Bandolier[] Bandoliers, PotionBandolierItem[] PotionBelt, uint AvailableSlots, string Name, string LastName, uint GuildID, uint Birthday, uint LastLogin, uint AccountStartdate, uint TimePlayed, bool PVP, Roleplay Roleplay, bool GM, GuildRank GuildRank, uint GuildBanker, uint Experience, byte[] Languages, float X, float Y, float Z, float Heading, Money BankMoney, uint PlatinumShared, bool Autosplit, ushort ZoneID, ushort ZoneInstance, GroupMember[] GroupMembers, string GroupLeader, uint EntityID, bool LeadAAActive, LdonPoints LdonPoints, float TributeTimeRemaining, uint CareerTributePoints, uint TributePoints, bool TributeActive, Tribute[] Tributes, double GroupLeadershipExp, double RaidLeadershipExp, uint GroupLeadershipPoints, uint RaidLeadershipPoints, LeadershipAA LeaderAbilities, uint AirRemaining, PVPStats PVPStats, uint AAExperience, uint CurrentRadCrystals, uint CareerRadCrystals, uint CurrentEbonCrystals, uint CareerEbonCrystals, Autoconsent Autoconsent, uint Level3, bool ShowHelm, uint RestTimer) : this()
        {
            this.Gender = Gender;
            this.Race = Race;
            this.Class = Class;
            this.Level = Level;
            this.Binds = Binds;
            this.Deity = Deity;
            this.Intoxication = Intoxication;
            this.SpellSlotRefresh = SpellSlotRefresh;
            this.AbilitySlotRefresh = AbilitySlotRefresh;
            this.HairColor = HairColor;
            this.BeardColor = BeardColor;
            this.EyeColor1 = EyeColor1;
            this.EyeColor2 = EyeColor2;
            this.HairStyle = HairStyle;
            this.Beard = Beard;
            this.Equipment = Equipment;
            this.ItemTint = ItemTint;
            this.AAArray = AAArray;
            this.Points = Points;
            this.Mana = Mana;
            this.CurHP = CurHP;
            this.STR = STR;
            this.STA = STA;
            this.CHA = CHA;
            this.DEX = DEX;
            this.INT = INT;
            this.AGI = AGI;
            this.WIS = WIS;
            this.Face = Face;
            this.SpellBook = SpellBook;
            this.MemSpells = MemSpells;
            this.PlayerMoney = PlayerMoney;
            this.CursorMoney = CursorMoney;
            this.Skills = Skills;
            this.Toxicity = Toxicity;
            this.Thirst = Thirst;
            this.Hunger = Hunger;
            this.Buffs = Buffs;
            this.Disciplines = Disciplines;
            this.RecastTimers = RecastTimers;
            this.Endurance = Endurance;
            this.AAPointsSpent = AAPointsSpent;
            this.AAPoints = AAPoints;
            this.Bandoliers = Bandoliers;
            this.PotionBelt = PotionBelt;
            this.AvailableSlots = AvailableSlots;
            this.Name = Name;
            this.LastName = LastName;
            this.GuildID = GuildID;
            this.Birthday = Birthday;
            this.LastLogin = LastLogin;
            this.AccountStartdate = AccountStartdate;
            this.TimePlayed = TimePlayed;
            this.PVP = PVP;
            this.Roleplay = Roleplay;
            this.GM = GM;
            this.GuildRank = GuildRank;
            this.GuildBanker = GuildBanker;
            this.Experience = Experience;
            this.Languages = Languages;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Heading = Heading;
            this.BankMoney = BankMoney;
            this.PlatinumShared = PlatinumShared;
            this.Autosplit = Autosplit;
            this.ZoneID = ZoneID;
            this.ZoneInstance = ZoneInstance;
            this.GroupMembers = GroupMembers;
            this.GroupLeader = GroupLeader;
            this.EntityID = EntityID;
            this.LeadAAActive = LeadAAActive;
            this.LdonPoints = LdonPoints;
            this.TributeTimeRemaining = TributeTimeRemaining;
            this.CareerTributePoints = CareerTributePoints;
            this.TributePoints = TributePoints;
            this.TributeActive = TributeActive;
            this.Tributes = Tributes;
            this.GroupLeadershipExp = GroupLeadershipExp;
            this.RaidLeadershipExp = RaidLeadershipExp;
            this.GroupLeadershipPoints = GroupLeadershipPoints;
            this.RaidLeadershipPoints = RaidLeadershipPoints;
            this.LeaderAbilities = LeaderAbilities;
            this.AirRemaining = AirRemaining;
            this.PVPStats = PVPStats;
            this.AAExperience = AAExperience;
            this.CurrentRadCrystals = CurrentRadCrystals;
            this.CareerRadCrystals = CareerRadCrystals;
            this.CurrentEbonCrystals = CurrentEbonCrystals;
            this.CareerEbonCrystals = CareerEbonCrystals;
            this.Autoconsent = Autoconsent;
            this.Level3 = Level3;
            this.ShowHelm = ShowHelm;
            this.RestTimer = RestTimer;
        }

        public PlayerProfile(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public PlayerProfile(BinaryReader br) : this()
        {
            Unpack(br);
        }
        public void Unpack(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, data.Length - offset))
            {
                using (var br = new BinaryReader(ms))
                {
                    Unpack(br);
                }
            }
        }
        public void Unpack(BinaryReader br)
        {
            checksum = br.ReadUInt32();
            Gender = ((Gender)0).Unpack(br);
            Race = br.ReadUInt32();
            Class = br.ReadUInt32();
            br.ReadBytes(40);
            Level = br.ReadByte();
            unkLevel = br.ReadByte();
            br.ReadBytes(2);
            Binds = new Bind[5];
            for (var i = 0; i < 5; ++i)
            {
                Binds[i] = new Bind(br);
            }
            Deity = br.ReadUInt32();
            Intoxication = br.ReadUInt32();
            SpellSlotRefresh = new uint[MAX_PP_MEMSPELL];
            for (var i = 0; i < MAX_PP_MEMSPELL; ++i)
            {
                SpellSlotRefresh[i] = br.ReadUInt32();
            }
            br.ReadBytes(6);
            AbilitySlotRefresh = br.ReadUInt32();
            HairColor = br.ReadByte();
            BeardColor = br.ReadByte();
            EyeColor1 = br.ReadByte();
            EyeColor2 = br.ReadByte();
            HairStyle = br.ReadByte();
            Beard = br.ReadByte();
            br.ReadBytes(4);
            Equipment = new TextureProfile(br);
            br.ReadBytes(168);
            ItemTint = new TintProfile(br);
            AAArray = new AAArray[MAX_PP_AA_ARRAY];
            for (var i = 0; i < MAX_PP_AA_ARRAY; ++i)
            {
                AAArray[i] = new AAArray(br);
            }
            Points = br.ReadUInt32();
            Mana = br.ReadUInt32();
            CurHP = br.ReadUInt32();
            STR = br.ReadUInt32();
            STA = br.ReadUInt32();
            CHA = br.ReadUInt32();
            DEX = br.ReadUInt32();
            INT = br.ReadUInt32();
            AGI = br.ReadUInt32();
            WIS = br.ReadUInt32();
            br.ReadBytes(28);
            Face = br.ReadByte();
            br.ReadBytes(147);
            SpellBook = new uint[MAX_PP_SPELLBOOK];
            for (var i = 0; i < MAX_PP_SPELLBOOK; ++i)
            {
                SpellBook[i] = br.ReadUInt32();
            }
            MemSpells = new uint[MAX_PP_MEMSPELL];
            for (var i = 0; i < MAX_PP_MEMSPELL; ++i)
            {
                MemSpells[i] = br.ReadUInt32();
            }
            br.ReadBytes(20);
            PlayerMoney = new Money(br);
            CursorMoney = new Money(br);
            Skills = new uint[MAX_PP_SKILL];
            for (var i = 0; i < MAX_PP_SKILL; ++i)
            {
                Skills[i] = br.ReadUInt32();
            }
            br.ReadBytes(136);
            Toxicity = br.ReadUInt32();
            Thirst = br.ReadUInt32();
            Hunger = br.ReadUInt32();
            Buffs = new SpellBuff[BUFF_COUNT];
            for (var i = 0; i < BUFF_COUNT; ++i)
            {
                Buffs[i] = new SpellBuff(br);
            }
            Disciplines = new uint[MAX_PP_DISCIPLINES];
            for (var i = 0; i < MAX_PP_DISCIPLINES; ++i)
            {
                Disciplines[i] = br.ReadUInt32();
            }
            RecastTimers = new uint[MAX_RECAST_TYPES];
            for (var i = 0; i < MAX_RECAST_TYPES; ++i)
            {
                RecastTimers[i] = br.ReadUInt32();
            }
            br.ReadBytes(160);
            Endurance = br.ReadUInt32();
            br.ReadBytes(20);
            AAPointsSpent = br.ReadUInt32();
            AAPoints = br.ReadUInt32();
            br.ReadBytes(4);
            Bandoliers = new Bandolier[BANDOLIERS_SIZE];
            for (var i = 0; i < BANDOLIERS_SIZE; ++i)
            {
                Bandoliers[i] = new Bandolier(br);
            }
            PotionBelt = new PotionBandolierItem[POTIONBELT_SIZE];
            for (var i = 0; i < POTIONBELT_SIZE; ++i)
            {
                PotionBelt[i] = new PotionBandolierItem(br);
            }
            br.ReadBytes(8);
            AvailableSlots = br.ReadUInt32();
            br.ReadBytes(80);
            Name = br.ReadString(64);
            LastName = br.ReadString(32);
            br.ReadBytes(8);
            GuildID = br.ReadUInt32();
            Birthday = br.ReadUInt32();
            LastLogin = br.ReadUInt32();
            AccountStartdate = br.ReadUInt32();
            TimePlayed = br.ReadUInt32();
            PVP = br.ReadByte() != 0;
            Roleplay = ((Roleplay)0).Unpack(br);
            GM = br.ReadByte() != 0;
            GuildRank = ((GuildRank)0).Unpack(br);
            GuildBanker = br.ReadUInt32();
            br.ReadBytes(4);
            Experience = br.ReadUInt32();
            br.ReadBytes(8);
            timeEntitledOnAccount = br.ReadUInt32();
            Languages = new byte[MAX_PP_LANGUAGE];
            for (var i = 0; i < MAX_PP_LANGUAGE; ++i)
            {
                Languages[i] = br.ReadByte();
            }
            br.ReadBytes(7);
            X = br.ReadSingle();
            Y = br.ReadSingle();
            Z = br.ReadSingle();
            Heading = br.ReadSingle();
            br.ReadBytes(4);
            BankMoney = new Money(br);
            PlatinumShared = br.ReadUInt32();
            br.ReadBytes(1036);
            expansions = br.ReadUInt32();
            br.ReadBytes(12);
            Autosplit = br.ReadUInt32() != 0;
            br.ReadBytes(16);
            ZoneID = br.ReadUInt16();
            ZoneInstance = br.ReadUInt16();
            GroupMembers = new GroupMember[MAX_GROUP_MEMBERS];
            for (var i = 0; i < MAX_GROUP_MEMBERS; ++i)
            {
                GroupMembers[i] = new GroupMember(br);
            }
            GroupLeader = br.ReadString(64);
            br.ReadBytes(540);
            EntityID = br.ReadUInt32();
            LeadAAActive = br.ReadUInt32() != 0;
            br.ReadBytes(4);
            LdonPoints = new LdonPoints(br);
            br.ReadBytes(9 * 4 + 4 + 4 * 6 + 72);
            TributeTimeRemaining = br.ReadSingle();
            CareerTributePoints = br.ReadUInt32();
            br.ReadBytes(4);
            TributePoints = br.ReadUInt32();
            br.ReadBytes(4);
            TributeActive = br.ReadUInt32() != 0;
            Tributes = new Tribute[MAX_PLAYER_TRIBUTES];
            for (var i = 0; i < MAX_PLAYER_TRIBUTES; ++i)
            {
                Tributes[i] = new Tribute(br);
            }
            br.ReadBytes(4);
            GroupLeadershipExp = br.ReadDouble();
            RaidLeadershipExp = br.ReadDouble();
            GroupLeadershipPoints = br.ReadUInt32();
            RaidLeadershipPoints = br.ReadUInt32();
            LeaderAbilities = new LeadershipAA(br);
            br.ReadBytes(128);
            AirRemaining = br.ReadUInt32();
            PVPStats = new PVPStats(br);
            AAExperience = br.ReadUInt32();
            br.ReadBytes(40);
            CurrentRadCrystals = br.ReadUInt32();
            CareerRadCrystals = br.ReadUInt32();
            CurrentEbonCrystals = br.ReadUInt32();
            CareerEbonCrystals = br.ReadUInt32();
            Autoconsent = new Autoconsent(br);
            br.ReadBytes(1);
            Level3 = br.ReadUInt32();
            ShowHelm = br.ReadUInt32() != 0;
            RestTimer = br.ReadUInt32();
            br.ReadBytes(1036);
        }

        public byte[] Pack()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    Pack(bw);
                    return ms.ToArray();
                }
            }
        }
        public void Pack(BinaryWriter bw)
        {
            bw.Write(checksum);
            bw.Write((uint)Gender);
            bw.Write(Race);
            bw.Write(Class);
            bw.Write(new byte[40]);
            bw.Write(Level);
            bw.Write(unkLevel);
            bw.Write(new byte[2]);
            for (var i = 0; i < 5; ++i)
            {
                Binds[i].Pack(bw);
            }
            bw.Write(Deity);
            bw.Write(Intoxication);
            for (var i = 0; i < MAX_PP_MEMSPELL; ++i)
            {
                bw.Write(SpellSlotRefresh[i]);
            }
            bw.Write(new byte[6]);
            bw.Write(AbilitySlotRefresh);
            bw.Write(HairColor);
            bw.Write(BeardColor);
            bw.Write(EyeColor1);
            bw.Write(EyeColor2);
            bw.Write(HairStyle);
            bw.Write(Beard);
            bw.Write(new byte[4]);
            Equipment.Pack(bw);
            bw.Write(new byte[168]);
            ItemTint.Pack(bw);
            for (var i = 0; i < MAX_PP_AA_ARRAY; ++i)
            {
                AAArray[i].Pack(bw);
            }
            bw.Write(Points);
            bw.Write(Mana);
            bw.Write(CurHP);
            bw.Write(STR);
            bw.Write(STA);
            bw.Write(CHA);
            bw.Write(DEX);
            bw.Write(INT);
            bw.Write(AGI);
            bw.Write(WIS);
            bw.Write(new byte[28]);
            bw.Write(Face);
            bw.Write(new byte[147]);
            for (var i = 0; i < MAX_PP_SPELLBOOK; ++i)
            {
                bw.Write(SpellBook[i]);
            }
            for (var i = 0; i < MAX_PP_MEMSPELL; ++i)
            {
                bw.Write(MemSpells[i]);
            }
            bw.Write(new byte[20]);
            PlayerMoney.Pack(bw);
            CursorMoney.Pack(bw);
            for (var i = 0; i < MAX_PP_SKILL; ++i)
            {
                bw.Write(Skills[i]);
            }
            bw.Write(new byte[136]);
            bw.Write(Toxicity);
            bw.Write(Thirst);
            bw.Write(Hunger);
            for (var i = 0; i < BUFF_COUNT; ++i)
            {
                Buffs[i].Pack(bw);
            }
            for (var i = 0; i < MAX_PP_DISCIPLINES; ++i)
            {
                bw.Write(Disciplines[i]);
            }
            for (var i = 0; i < MAX_RECAST_TYPES; ++i)
            {
                bw.Write(RecastTimers[i]);
            }
            bw.Write(new byte[160]);
            bw.Write(Endurance);
            bw.Write(new byte[20]);
            bw.Write(AAPointsSpent);
            bw.Write(AAPoints);
            bw.Write(new byte[4]);
            for (var i = 0; i < BANDOLIERS_SIZE; ++i)
            {
                Bandoliers[i].Pack(bw);
            }
            for (var i = 0; i < POTIONBELT_SIZE; ++i)
            {
                PotionBelt[i].Pack(bw);
            }
            bw.Write(new byte[8]);
            bw.Write(AvailableSlots);
            bw.Write(new byte[80]);
            bw.Write(Name.ToBytes(64));
            bw.Write(LastName.ToBytes(32));
            bw.Write(new byte[8]);
            bw.Write(GuildID);
            bw.Write(Birthday);
            bw.Write(LastLogin);
            bw.Write(AccountStartdate);
            bw.Write(TimePlayed);
            bw.Write((byte)(PVP ? 1 : 0));
            bw.Write((byte)Roleplay);
            bw.Write((byte)(GM ? 1 : 0));
            bw.Write((sbyte)GuildRank);
            bw.Write(GuildBanker);
            bw.Write(new byte[4]);
            bw.Write(Experience);
            bw.Write(new byte[8]);
            bw.Write(timeEntitledOnAccount);
            for (var i = 0; i < MAX_PP_LANGUAGE; ++i)
            {
                bw.Write(Languages[i]);
            }
            bw.Write(new byte[7]);
            bw.Write(X);
            bw.Write(Y);
            bw.Write(Z);
            bw.Write(Heading);
            bw.Write(new byte[4]);
            BankMoney.Pack(bw);
            bw.Write(PlatinumShared);
            bw.Write(new byte[1036]);
            bw.Write(expansions);
            bw.Write(new byte[12]);
            bw.Write((uint)(Autosplit ? 1 : 0));
            bw.Write(new byte[16]);
            bw.Write(ZoneID);
            bw.Write(ZoneInstance);
            for (var i = 0; i < MAX_GROUP_MEMBERS; ++i)
            {
                GroupMembers[i].Pack(bw);
            }
            bw.Write(GroupLeader.ToBytes(64));
            bw.Write(new byte[540]);
            bw.Write(EntityID);
            bw.Write((uint)(LeadAAActive ? 1 : 0));
            bw.Write(new byte[4]);
            LdonPoints.Pack(bw);
            bw.Write(new byte[9 * 4 + 4 + 4 * 6 + 72]);
            bw.Write(TributeTimeRemaining);
            bw.Write(CareerTributePoints);
            bw.Write(new byte[4]);
            bw.Write(TributePoints);
            bw.Write(new byte[4]);
            bw.Write((uint)(TributeActive ? 1 : 0));
            for (var i = 0; i < MAX_PLAYER_TRIBUTES; ++i)
            {
                Tributes[i].Pack(bw);
            }
            bw.Write(new byte[4]);
            bw.Write(GroupLeadershipExp);
            bw.Write(RaidLeadershipExp);
            bw.Write(GroupLeadershipPoints);
            bw.Write(RaidLeadershipPoints);
            LeaderAbilities.Pack(bw);
            bw.Write(new byte[128]);
            bw.Write(AirRemaining);
            PVPStats.Pack(bw);
            bw.Write(AAExperience);
            bw.Write(new byte[40]);
            bw.Write(CurrentRadCrystals);
            bw.Write(CareerRadCrystals);
            bw.Write(CurrentEbonCrystals);
            bw.Write(CareerEbonCrystals);
            Autoconsent.Pack(bw);
            bw.Write(new byte[1]);
            bw.Write(Level3);
            bw.Write((uint)(ShowHelm ? 1 : 0));
            bw.Write(RestTimer);
            bw.Write(new byte[1036]);
        }

        public override string ToString()
        {
            var ret = "struct PlayerProfile {\n";
            ret += "\tGender = ";
            try
            {
                ret += $"{Indentify(Gender)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tRace = ";
            try
            {
                ret += $"{Indentify(Race)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tClass = ";
            try
            {
                ret += $"{Indentify(Class)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLevel = ";
            try
            {
                ret += $"{Indentify(Level)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tBinds = ";
            try
            {
                if (Binds != null)
                {
                    ret += "{\n";
                    for (int i = 0, e = Binds.Length; i < e; ++i)
                        ret += $"\t\t{Indentify(Binds[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
                    ret += "\t},\n";
                }
                else
                {
                    ret += "!!NULL!!\n";
                }
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDeity = ";
            try
            {
                ret += $"{Indentify(Deity)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tIntoxication = ";
            try
            {
                ret += $"{Indentify(Intoxication)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAbilitySlotRefresh = ";
            try
            {
                ret += $"{Indentify(AbilitySlotRefresh)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tHairColor = ";
            try
            {
                ret += $"{Indentify(HairColor)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tBeardColor = ";
            try
            {
                ret += $"{Indentify(BeardColor)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tEyeColor1 = ";
            try
            {
                ret += $"{Indentify(EyeColor1)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tEyeColor2 = ";
            try
            {
                ret += $"{Indentify(EyeColor2)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tHairStyle = ";
            try
            {
                ret += $"{Indentify(HairStyle)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tBeard = ";
            try
            {
                ret += $"{Indentify(Beard)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tEquipment = ";
            try
            {
                ret += $"{Indentify(Equipment)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tItemTint = ";
            try
            {
                ret += $"{Indentify(ItemTint)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPoints = ";
            try
            {
                ret += $"{Indentify(Points)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tMana = ";
            try
            {
                ret += $"{Indentify(Mana)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCurHP = ";
            try
            {
                ret += $"{Indentify(CurHP)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tSTR = ";
            try
            {
                ret += $"{Indentify(STR)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tSTA = ";
            try
            {
                ret += $"{Indentify(STA)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCHA = ";
            try
            {
                ret += $"{Indentify(CHA)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDEX = ";
            try
            {
                ret += $"{Indentify(DEX)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tINT = ";
            try
            {
                ret += $"{Indentify(INT)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAGI = ";
            try
            {
                ret += $"{Indentify(AGI)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tWIS = ";
            try
            {
                ret += $"{Indentify(WIS)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tFace = ";
            try
            {
                ret += $"{Indentify(Face)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPlayerMoney = ";
            try
            {
                ret += $"{Indentify(PlayerMoney)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCursorMoney = ";
            try
            {
                ret += $"{Indentify(CursorMoney)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tToxicity = ";
            try
            {
                ret += $"{Indentify(Toxicity)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tThirst = ";
            try
            {
                ret += $"{Indentify(Thirst)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tHunger = ";
            try
            {
                ret += $"{Indentify(Hunger)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tEndurance = ";
            try
            {
                ret += $"{Indentify(Endurance)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAAPointsSpent = ";
            try
            {
                ret += $"{Indentify(AAPointsSpent)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAAPoints = ";
            try
            {
                ret += $"{Indentify(AAPoints)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAvailableSlots = ";
            try
            {
                ret += $"{Indentify(AvailableSlots)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tName = ";
            try
            {
                ret += $"{Indentify(Name)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLastName = ";
            try
            {
                ret += $"{Indentify(LastName)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGuildID = ";
            try
            {
                ret += $"{Indentify(GuildID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tBirthday = ";
            try
            {
                ret += $"{Indentify(Birthday)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLastLogin = ";
            try
            {
                ret += $"{Indentify(LastLogin)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAccountStartdate = ";
            try
            {
                ret += $"{Indentify(AccountStartdate)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tTimePlayed = ";
            try
            {
                ret += $"{Indentify(TimePlayed)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPVP = ";
            try
            {
                ret += $"{Indentify(PVP)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tRoleplay = ";
            try
            {
                ret += $"{Indentify(Roleplay)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGM = ";
            try
            {
                ret += $"{Indentify(GM)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGuildRank = ";
            try
            {
                ret += $"{Indentify(GuildRank)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGuildBanker = ";
            try
            {
                ret += $"{Indentify(GuildBanker)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tExperience = ";
            try
            {
                ret += $"{Indentify(Experience)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tX = ";
            try
            {
                ret += $"{Indentify(X)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tY = ";
            try
            {
                ret += $"{Indentify(Y)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tZ = ";
            try
            {
                ret += $"{Indentify(Z)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tHeading = ";
            try
            {
                ret += $"{Indentify(Heading)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tBankMoney = ";
            try
            {
                ret += $"{Indentify(BankMoney)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPlatinumShared = ";
            try
            {
                ret += $"{Indentify(PlatinumShared)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAutosplit = ";
            try
            {
                ret += $"{Indentify(Autosplit)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tZoneID = ";
            try
            {
                ret += $"{Indentify(ZoneID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tZoneInstance = ";
            try
            {
                ret += $"{Indentify(ZoneInstance)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGroupMembers = ";
            try
            {
                if (GroupMembers != null)
                {
                    ret += "{\n";
                    for (int i = 0, e = GroupMembers.Length; i < e; ++i)
                        ret += $"\t\t{Indentify(GroupMembers[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
                    ret += "\t},\n";
                }
                else
                {
                    ret += "!!NULL!!\n";
                }
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGroupLeader = ";
            try
            {
                ret += $"{Indentify(GroupLeader)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tEntityID = ";
            try
            {
                ret += $"{Indentify(EntityID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLeadAAActive = ";
            try
            {
                ret += $"{Indentify(LeadAAActive)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLdonPoints = ";
            try
            {
                ret += $"{Indentify(LdonPoints)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tTributeTimeRemaining = ";
            try
            {
                ret += $"{Indentify(TributeTimeRemaining)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCareerTributePoints = ";
            try
            {
                ret += $"{Indentify(CareerTributePoints)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tTributePoints = ";
            try
            {
                ret += $"{Indentify(TributePoints)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tTributeActive = ";
            try
            {
                ret += $"{Indentify(TributeActive)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGroupLeadershipExp = ";
            try
            {
                ret += $"{Indentify(GroupLeadershipExp)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tRaidLeadershipExp = ";
            try
            {
                ret += $"{Indentify(RaidLeadershipExp)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGroupLeadershipPoints = ";
            try
            {
                ret += $"{Indentify(GroupLeadershipPoints)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tRaidLeadershipPoints = ";
            try
            {
                ret += $"{Indentify(RaidLeadershipPoints)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLeaderAbilities = ";
            try
            {
                ret += $"{Indentify(LeaderAbilities)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAirRemaining = ";
            try
            {
                ret += $"{Indentify(AirRemaining)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPVPStats = ";
            try
            {
                ret += $"{Indentify(PVPStats)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAAExperience = ";
            try
            {
                ret += $"{Indentify(AAExperience)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCurrentRadCrystals = ";
            try
            {
                ret += $"{Indentify(CurrentRadCrystals)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCareerRadCrystals = ";
            try
            {
                ret += $"{Indentify(CareerRadCrystals)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCurrentEbonCrystals = ";
            try
            {
                ret += $"{Indentify(CurrentEbonCrystals)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCareerEbonCrystals = ";
            try
            {
                ret += $"{Indentify(CareerEbonCrystals)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAutoconsent = ";
            try
            {
                ret += $"{Indentify(Autoconsent)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLevel3 = ";
            try
            {
                ret += $"{Indentify(Level3)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tShowHelm = ";
            try
            {
                ret += $"{Indentify(ShowHelm)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tRestTimer = ";
            try
            {
                ret += $"{Indentify(RestTimer)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}