using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct NewZone_Struct {
// /*0000*/	char	char_name[64];			// Character Name
// /*0064*/	char	zone_short_name[32];	// Zone Short Name
// /*0096*/	char    unknown0096[96];
// /*0192*/	char	zone_long_name[278];	// Zone Long Name
// /*0470*/	uint8	ztype;					// Zone type (usually FF)
// /*0471*/	uint8	fog_red[4];				// Zone fog (red)
// /*0475*/	uint8	fog_green[4];				// Zone fog (green)
// /*0479*/	uint8	fog_blue[4];				// Zone fog (blue)
// /*0483*/	uint8	unknown323;
// /*0484*/	float	fog_minclip[4];
// /*0500*/	float	fog_maxclip[4];
// /*0516*/	float	gravity;
// /*0520*/	uint8	time_type;
// /*0521*/    uint8   rain_chance[4];
// /*0525*/    uint8   rain_duration[4];
// /*0529*/    uint8   snow_chance[4];
// /*0533*/    uint8   snow_duration[4];
// /*0537*/    uint8   unknown537[33];
// /*0570*/	uint8	sky;					// Sky Type
// /*0571*/	uint8	unknown571[13];			// ***Placeholder
// /*0584*/	float	zone_exp_multiplier;	// Experience Multiplier
// /*0588*/	float	safe_y;					// Zone Safe Y
// /*0592*/	float	safe_x;					// Zone Safe X
// /*0596*/	float	safe_z;					// Zone Safe Z
// /*0600*/	float	min_z;					// Guessed - NEW - Seen 0
// /*0604*/	float	max_z;					// Guessed
// /*0608*/	float	underworld;				// Underworld, min z (Not Sure?)
// /*0612*/	float	minclip;				// Minimum View Distance
// /*0616*/	float	maxclip;				// Maximum View DIstance
// /*0620*/	uint8	unknown620[84];		// ***Placeholder
// /*0704*/	char	zone_short_name2[96];	//zone file name? excludes instance number which can be in previous version.
// /*0800*/	int32	unknown800;	//seen -1
// /*0804*/	char	unknown804[40]; //
// /*0844*/	int32  unknown844;	//seen 600
// /*0848*/	int32  unknown848;
// /*0852*/	uint16 zone_id;
// /*0854*/	uint16 zone_instance;
// /*0856*/	uint32 scriptNPCReceivedanItem;
// /*0860*/	uint32 bCheck;					// padded bool
// /*0864*/	uint32 scriptIDSomething;
// /*0868*/	uint32 underworld_teleport_index; // > 0 teleports w/ zone point index, invalid succors, -1 affects some collisions
// /*0872*/	uint32 scriptIDSomething3;
// /*0876*/	uint32 suspend_buffs;
// /*0880*/	uint32 lava_damage;	//seen 50
// /*0884*/	uint32 min_lava_damage;	//seen 10
// /*0888*/	uint8  unknown888;	//seen 1
// /*0889*/	uint8  unknown889;	//seen 0 (POK) or 1 (rujj)
// /*0890*/	uint8  unknown890;	//seen 1
// /*0891*/	uint8  unknown891;	//seen 0
// /*0892*/	uint8  unknown892;	//seen 0
// /*0893*/	uint8  unknown893;	//seen 0 - 00
// /*0894*/	uint8  fall_damage;	// 0 = Fall Damage on, 1 = Fall Damage off
// /*0895*/	uint8  unknown895;	//seen 0 - 00
// /*0896*/	uint32 fast_regen_hp;	//seen 180
// /*0900*/	uint32 fast_regen_mana;	//seen 180
// /*0904*/	uint32 fast_regen_endurance;	//seen 180
// /*0908*/	uint32 unknown908;	//seen 2
// /*0912*/	uint32 unknown912;	//seen 2
// /*0916*/	float  fog_density;	//Of about 10 or so zones tested, all but one have this set to 0.33 Blightfire had 0.16
// /*0920*/	uint32 unknown920;	//seen 0
// /*0924*/	uint32 unknown924;	//seen 0
// /*0928*/	uint32 unknown928;	//seen 0
// /*0932*/	uint8  unknown932[12];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_NewZone)
// {
// SETUP_DIRECT_ENCODE(NewZone_Struct, structs::NewZone_Struct);
// 
// OUT_str(char_name);
// OUT_str(zone_short_name);
// OUT_str(zone_long_name);
// OUT(ztype);
// int r;
// for (r = 0; r < 4; r++) {
// OUT(fog_red[r]);
// OUT(fog_green[r]);
// OUT(fog_blue[r]);
// OUT(fog_minclip[r]);
// OUT(fog_maxclip[r]);
// }
// OUT(gravity);
// OUT(time_type);
// for (r = 0; r < 4; r++) {
// OUT(rain_chance[r]);
// }
// for (r = 0; r < 4; r++) {
// OUT(rain_duration[r]);
// }
// for (r = 0; r < 4; r++) {
// OUT(snow_chance[r]);
// }
// for (r = 0; r < 4; r++) {
// OUT(snow_duration[r]);
// }
// for (r = 0; r < 32; r++) {
// eq->unknown537[r] = 0xFF;	//observed
// }
// OUT(sky);
// OUT(zone_exp_multiplier);
// OUT(safe_y);
// OUT(safe_x);
// OUT(safe_z);
// OUT(max_z);
// OUT(underworld);
// OUT(minclip);
// OUT(maxclip);
// OUT_str(zone_short_name2);
// OUT(zone_id);
// OUT(zone_instance);
// OUT(suspend_buffs);
// OUT(fast_regen_hp);
// OUT(fast_regen_mana);
// OUT(fast_regen_endurance);
// OUT(underworld_teleport_index);
// 
// eq->fog_density = emu->fog_density;
// 
// /*fill in some unknowns with observed values, hopefully it will help */
// eq->unknown800 = -1;
// eq->unknown844 = 600;
// OUT(lava_damage);
// OUT(min_lava_damage);
// eq->unknown888 = 1;
// eq->unknown889 = 0;
// eq->unknown890 = 1;
// eq->unknown891 = 0;
// eq->unknown892 = 0;
// eq->unknown893 = 0;
// eq->fall_damage = 0;	// 0 = Fall Damage on, 1 = Fall Damage off
// eq->unknown895 = 0;
// eq->unknown908 = 2;
// eq->unknown912 = 2;
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the NewZone packet structure for EverQuest network communication.
	/// </summary>
	public struct NewZone : IEQStruct {
		/// <summary>
		/// Gets or sets the charname value.
		/// </summary>
		public byte[] CharName { get; set; }

		/// <summary>
		/// Gets or sets the zoneshortname value.
		/// </summary>
		public byte[] ZoneShortName { get; set; }

		/// <summary>
		/// Gets or sets the zonelongname value.
		/// </summary>
		public byte[] ZoneLongName { get; set; }

		/// <summary>
		/// Gets or sets the ztype value.
		/// </summary>
		public byte Ztype { get; set; }

		/// <summary>
		/// Gets or sets the fogred value.
		/// </summary>
		public byte[] FogRed { get; set; }

		/// <summary>
		/// Gets or sets the foggreen value.
		/// </summary>
		public byte[] FogGreen { get; set; }

		/// <summary>
		/// Gets or sets the fogblue value.
		/// </summary>
		public byte[] FogBlue { get; set; }

		/// <summary>
		/// Gets or sets the unknown323 value.
		/// </summary>
		public byte Unknown323 { get; set; }

		/// <summary>
		/// Gets or sets the fogminclip value.
		/// </summary>
		public float[] FogMinclip { get; set; }

		/// <summary>
		/// Gets or sets the fogmaxclip value.
		/// </summary>
		public float[] FogMaxclip { get; set; }

		/// <summary>
		/// Gets or sets the gravity value.
		/// </summary>
		public float Gravity { get; set; }

		/// <summary>
		/// Gets or sets the timetype value.
		/// </summary>
		public byte TimeType { get; set; }

		/// <summary>
		/// Gets or sets the rainchance value.
		/// </summary>
		public byte[] RainChance { get; set; }

		/// <summary>
		/// Gets or sets the rainduration value.
		/// </summary>
		public byte[] RainDuration { get; set; }

		/// <summary>
		/// Gets or sets the snowchance value.
		/// </summary>
		public byte[] SnowChance { get; set; }

		/// <summary>
		/// Gets or sets the snowduration value.
		/// </summary>
		public byte[] SnowDuration { get; set; }

		/// <summary>
		/// Gets or sets the unknown537 value.
		/// </summary>
		public byte[] Unknown537 { get; set; }

		/// <summary>
		/// Gets or sets the sky value.
		/// </summary>
		public byte Sky { get; set; }

		/// <summary>
		/// Gets or sets the unknown571 value.
		/// </summary>
		public byte[] Unknown571 { get; set; }

		/// <summary>
		/// Gets or sets the zoneexpmultiplier value.
		/// </summary>
		public float ZoneExpMultiplier { get; set; }

		/// <summary>
		/// Gets or sets the safey value.
		/// </summary>
		public float SafeY { get; set; }

		/// <summary>
		/// Gets or sets the safex value.
		/// </summary>
		public float SafeX { get; set; }

		/// <summary>
		/// Gets or sets the safez value.
		/// </summary>
		public float SafeZ { get; set; }

		/// <summary>
		/// Gets or sets the minz value.
		/// </summary>
		public float MinZ { get; set; }

		/// <summary>
		/// Gets or sets the maxz value.
		/// </summary>
		public float MaxZ { get; set; }

		/// <summary>
		/// Gets or sets the underworld value.
		/// </summary>
		public float Underworld { get; set; }

		/// <summary>
		/// Gets or sets the minclip value.
		/// </summary>
		public float Minclip { get; set; }

		/// <summary>
		/// Gets or sets the maxclip value.
		/// </summary>
		public float Maxclip { get; set; }

		/// <summary>
		/// Gets or sets the unknown620 value.
		/// </summary>
		public byte[] Unknown620 { get; set; }

		/// <summary>
		/// Gets or sets the zoneshortname2 value.
		/// </summary>
		public byte[] ZoneShortName2 { get; set; }

		/// <summary>
		/// Gets or sets the unknown800 value.
		/// </summary>
		public int Unknown800 { get; set; }

		/// <summary>
		/// Gets or sets the unknown804 value.
		/// </summary>
		public byte[] Unknown804 { get; set; }

		/// <summary>
		/// Gets or sets the unknown844 value.
		/// </summary>
		public int Unknown844 { get; set; }

		/// <summary>
		/// Gets or sets the unknown848 value.
		/// </summary>
		public int Unknown848 { get; set; }

		/// <summary>
		/// Gets or sets the zoneid value.
		/// </summary>
		public ushort ZoneId { get; set; }

		/// <summary>
		/// Gets or sets the zoneinstance value.
		/// </summary>
		public ushort ZoneInstance { get; set; }

		/// <summary>
		/// Gets or sets the scriptnpcreceivedanitem value.
		/// </summary>
		public uint Scriptnpcreceivedanitem { get; set; }

		/// <summary>
		/// Gets or sets the bcheck value.
		/// </summary>
		public uint Bcheck { get; set; }

		/// <summary>
		/// Gets or sets the scriptidsomething value.
		/// </summary>
		public uint Scriptidsomething { get; set; }

		/// <summary>
		/// Gets or sets the underworldteleportindex value.
		/// </summary>
		public uint UnderworldTeleportIndex { get; set; }

		/// <summary>
		/// Gets or sets the scriptidsomething3 value.
		/// </summary>
		public uint Scriptidsomething3 { get; set; }

		/// <summary>
		/// Gets or sets the suspendbuffs value.
		/// </summary>
		public uint SuspendBuffs { get; set; }

		/// <summary>
		/// Gets or sets the lavadamage value.
		/// </summary>
		public uint LavaDamage { get; set; }

		/// <summary>
		/// Gets or sets the minlavadamage value.
		/// </summary>
		public uint MinLavaDamage { get; set; }

		/// <summary>
		/// Gets or sets the unknown888 value.
		/// </summary>
		public byte Unknown888 { get; set; }

		/// <summary>
		/// Gets or sets the unknown889 value.
		/// </summary>
		public byte Unknown889 { get; set; }

		/// <summary>
		/// Gets or sets the unknown890 value.
		/// </summary>
		public byte Unknown890 { get; set; }

		/// <summary>
		/// Gets or sets the unknown891 value.
		/// </summary>
		public byte Unknown891 { get; set; }

		/// <summary>
		/// Gets or sets the unknown892 value.
		/// </summary>
		public byte Unknown892 { get; set; }

		/// <summary>
		/// Gets or sets the unknown893 value.
		/// </summary>
		public byte Unknown893 { get; set; }

		/// <summary>
		/// Gets or sets the falldamage value.
		/// </summary>
		public byte FallDamage { get; set; }

		/// <summary>
		/// Gets or sets the unknown895 value.
		/// </summary>
		public byte Unknown895 { get; set; }

		/// <summary>
		/// Gets or sets the fastregenhp value.
		/// </summary>
		public uint FastRegenHp { get; set; }

		/// <summary>
		/// Gets or sets the fastregenmana value.
		/// </summary>
		public uint FastRegenMana { get; set; }

		/// <summary>
		/// Gets or sets the fastregenendurance value.
		/// </summary>
		public uint FastRegenEndurance { get; set; }

		/// <summary>
		/// Gets or sets the unknown908 value.
		/// </summary>
		public uint Unknown908 { get; set; }

		/// <summary>
		/// Gets or sets the unknown912 value.
		/// </summary>
		public uint Unknown912 { get; set; }

		/// <summary>
		/// Gets or sets the fogdensity value.
		/// </summary>
		public float FogDensity { get; set; }

		/// <summary>
		/// Gets or sets the unknown920 value.
		/// </summary>
		public uint Unknown920 { get; set; }

		/// <summary>
		/// Gets or sets the unknown924 value.
		/// </summary>
		public uint Unknown924 { get; set; }

		/// <summary>
		/// Gets or sets the unknown928 value.
		/// </summary>
		public uint Unknown928 { get; set; }

		/// <summary>
		/// Gets or sets the unknown932 value.
		/// </summary>
		public byte[] Unknown932 { get; set; }

		/// <summary>
		/// Initializes a new instance of the NewZone struct with specified field values.
		/// </summary>
		/// <param name="char_name">The charname value.</param>
		/// <param name="zone_short_name">The zoneshortname value.</param>
		/// <param name="zone_long_name">The zonelongname value.</param>
		/// <param name="ztype">The ztype value.</param>
		/// <param name="fog_red">The fogred value.</param>
		/// <param name="fog_green">The foggreen value.</param>
		/// <param name="fog_blue">The fogblue value.</param>
		/// <param name="unknown323">The unknown323 value.</param>
		/// <param name="fog_minclip">The fogminclip value.</param>
		/// <param name="fog_maxclip">The fogmaxclip value.</param>
		/// <param name="gravity">The gravity value.</param>
		/// <param name="time_type">The timetype value.</param>
		/// <param name="rain_chance">The rainchance value.</param>
		/// <param name="rain_duration">The rainduration value.</param>
		/// <param name="snow_chance">The snowchance value.</param>
		/// <param name="snow_duration">The snowduration value.</param>
		/// <param name="unknown537">The unknown537 value.</param>
		/// <param name="sky">The sky value.</param>
		/// <param name="unknown571">The unknown571 value.</param>
		/// <param name="zone_exp_multiplier">The zoneexpmultiplier value.</param>
		/// <param name="safe_y">The safey value.</param>
		/// <param name="safe_x">The safex value.</param>
		/// <param name="safe_z">The safez value.</param>
		/// <param name="min_z">The minz value.</param>
		/// <param name="max_z">The maxz value.</param>
		/// <param name="underworld">The underworld value.</param>
		/// <param name="minclip">The minclip value.</param>
		/// <param name="maxclip">The maxclip value.</param>
		/// <param name="unknown620">The unknown620 value.</param>
		/// <param name="zone_short_name2">The zoneshortname2 value.</param>
		/// <param name="unknown800">The unknown800 value.</param>
		/// <param name="unknown804">The unknown804 value.</param>
		/// <param name="unknown844">The unknown844 value.</param>
		/// <param name="unknown848">The unknown848 value.</param>
		/// <param name="zone_id">The zoneid value.</param>
		/// <param name="zone_instance">The zoneinstance value.</param>
		/// <param name="scriptNPCReceivedanItem">The scriptnpcreceivedanitem value.</param>
		/// <param name="bCheck">The bcheck value.</param>
		/// <param name="scriptIDSomething">The scriptidsomething value.</param>
		/// <param name="underworld_teleport_index">The underworldteleportindex value.</param>
		/// <param name="scriptIDSomething3">The scriptidsomething3 value.</param>
		/// <param name="suspend_buffs">The suspendbuffs value.</param>
		/// <param name="lava_damage">The lavadamage value.</param>
		/// <param name="min_lava_damage">The minlavadamage value.</param>
		/// <param name="unknown888">The unknown888 value.</param>
		/// <param name="unknown889">The unknown889 value.</param>
		/// <param name="unknown890">The unknown890 value.</param>
		/// <param name="unknown891">The unknown891 value.</param>
		/// <param name="unknown892">The unknown892 value.</param>
		/// <param name="unknown893">The unknown893 value.</param>
		/// <param name="fall_damage">The falldamage value.</param>
		/// <param name="unknown895">The unknown895 value.</param>
		/// <param name="fast_regen_hp">The fastregenhp value.</param>
		/// <param name="fast_regen_mana">The fastregenmana value.</param>
		/// <param name="fast_regen_endurance">The fastregenendurance value.</param>
		/// <param name="unknown908">The unknown908 value.</param>
		/// <param name="unknown912">The unknown912 value.</param>
		/// <param name="fog_density">The fogdensity value.</param>
		/// <param name="unknown920">The unknown920 value.</param>
		/// <param name="unknown924">The unknown924 value.</param>
		/// <param name="unknown928">The unknown928 value.</param>
		/// <param name="unknown932">The unknown932 value.</param>
		public NewZone(byte[] char_name, byte[] zone_short_name, byte[] zone_long_name, byte ztype, byte[] fog_red, byte[] fog_green, byte[] fog_blue, byte unknown323, float[] fog_minclip, float[] fog_maxclip, float gravity, byte time_type, byte[] rain_chance, byte[] rain_duration, byte[] snow_chance, byte[] snow_duration, byte[] unknown537, byte sky, byte[] unknown571, float zone_exp_multiplier, float safe_y, float safe_x, float safe_z, float min_z, float max_z, float underworld, float minclip, float maxclip, byte[] unknown620, byte[] zone_short_name2, int unknown800, byte[] unknown804, int unknown844, int unknown848, ushort zone_id, ushort zone_instance, uint scriptNPCReceivedanItem, uint bCheck, uint scriptIDSomething, uint underworld_teleport_index, uint scriptIDSomething3, uint suspend_buffs, uint lava_damage, uint min_lava_damage, byte unknown888, byte unknown889, byte unknown890, byte unknown891, byte unknown892, byte unknown893, byte fall_damage, byte unknown895, uint fast_regen_hp, uint fast_regen_mana, uint fast_regen_endurance, uint unknown908, uint unknown912, float fog_density, uint unknown920, uint unknown924, uint unknown928, byte[] unknown932) : this() {
			CharName = char_name;
			ZoneShortName = zone_short_name;
			ZoneLongName = zone_long_name;
			Ztype = ztype;
			FogRed = fog_red;
			FogGreen = fog_green;
			FogBlue = fog_blue;
			Unknown323 = unknown323;
			FogMinclip = fog_minclip;
			FogMaxclip = fog_maxclip;
			Gravity = gravity;
			TimeType = time_type;
			RainChance = rain_chance;
			RainDuration = rain_duration;
			SnowChance = snow_chance;
			SnowDuration = snow_duration;
			Unknown537 = unknown537;
			Sky = sky;
			Unknown571 = unknown571;
			ZoneExpMultiplier = zone_exp_multiplier;
			SafeY = safe_y;
			SafeX = safe_x;
			SafeZ = safe_z;
			MinZ = min_z;
			MaxZ = max_z;
			Underworld = underworld;
			Minclip = minclip;
			Maxclip = maxclip;
			Unknown620 = unknown620;
			ZoneShortName2 = zone_short_name2;
			Unknown800 = unknown800;
			Unknown804 = unknown804;
			Unknown844 = unknown844;
			Unknown848 = unknown848;
			ZoneId = zone_id;
			ZoneInstance = zone_instance;
			Scriptnpcreceivedanitem = scriptNPCReceivedanItem;
			Bcheck = bCheck;
			Scriptidsomething = scriptIDSomething;
			UnderworldTeleportIndex = underworld_teleport_index;
			Scriptidsomething3 = scriptIDSomething3;
			SuspendBuffs = suspend_buffs;
			LavaDamage = lava_damage;
			MinLavaDamage = min_lava_damage;
			Unknown888 = unknown888;
			Unknown889 = unknown889;
			Unknown890 = unknown890;
			Unknown891 = unknown891;
			Unknown892 = unknown892;
			Unknown893 = unknown893;
			FallDamage = fall_damage;
			Unknown895 = unknown895;
			FastRegenHp = fast_regen_hp;
			FastRegenMana = fast_regen_mana;
			FastRegenEndurance = fast_regen_endurance;
			Unknown908 = unknown908;
			Unknown912 = unknown912;
			FogDensity = fog_density;
			Unknown920 = unknown920;
			Unknown924 = unknown924;
			Unknown928 = unknown928;
			Unknown932 = unknown932;
		}

		/// <summary>
		/// Initializes a new instance of the NewZone struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public NewZone(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the NewZone struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public NewZone(BinaryReader br) : this() {
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
			// TODO: Array reading for CharName - implement based on actual array size
			// CharName = new byte[size];
			// TODO: Array reading for ZoneShortName - implement based on actual array size
			// ZoneShortName = new byte[size];
			// TODO: Array reading for ZoneLongName - implement based on actual array size
			// ZoneLongName = new byte[size];
			Ztype = br.ReadByte();
			// TODO: Array reading for FogRed - implement based on actual array size
			// FogRed = new byte[size];
			// TODO: Array reading for FogGreen - implement based on actual array size
			// FogGreen = new byte[size];
			// TODO: Array reading for FogBlue - implement based on actual array size
			// FogBlue = new byte[size];
			Unknown323 = br.ReadByte();
			// TODO: Array reading for FogMinclip - implement based on actual array size
			// FogMinclip = new float[size];
			// TODO: Array reading for FogMaxclip - implement based on actual array size
			// FogMaxclip = new float[size];
			Gravity = br.ReadSingle();
			TimeType = br.ReadByte();
			// TODO: Array reading for RainChance - implement based on actual array size
			// RainChance = new byte[size];
			// TODO: Array reading for RainDuration - implement based on actual array size
			// RainDuration = new byte[size];
			// TODO: Array reading for SnowChance - implement based on actual array size
			// SnowChance = new byte[size];
			// TODO: Array reading for SnowDuration - implement based on actual array size
			// SnowDuration = new byte[size];
			// TODO: Array reading for Unknown537 - implement based on actual array size
			// Unknown537 = new byte[size];
			Sky = br.ReadByte();
			// TODO: Array reading for Unknown571 - implement based on actual array size
			// Unknown571 = new byte[size];
			ZoneExpMultiplier = br.ReadSingle();
			SafeY = br.ReadSingle();
			SafeX = br.ReadSingle();
			SafeZ = br.ReadSingle();
			MinZ = br.ReadSingle();
			MaxZ = br.ReadSingle();
			Underworld = br.ReadSingle();
			Minclip = br.ReadSingle();
			Maxclip = br.ReadSingle();
			// TODO: Array reading for Unknown620 - implement based on actual array size
			// Unknown620 = new byte[size];
			// TODO: Array reading for ZoneShortName2 - implement based on actual array size
			// ZoneShortName2 = new byte[size];
			Unknown800 = br.ReadInt32();
			// TODO: Array reading for Unknown804 - implement based on actual array size
			// Unknown804 = new byte[size];
			Unknown844 = br.ReadInt32();
			Unknown848 = br.ReadInt32();
			ZoneId = br.ReadUInt16();
			ZoneInstance = br.ReadUInt16();
			Scriptnpcreceivedanitem = br.ReadUInt32();
			Bcheck = br.ReadUInt32();
			Scriptidsomething = br.ReadUInt32();
			UnderworldTeleportIndex = br.ReadUInt32();
			Scriptidsomething3 = br.ReadUInt32();
			SuspendBuffs = br.ReadUInt32();
			LavaDamage = br.ReadUInt32();
			MinLavaDamage = br.ReadUInt32();
			Unknown888 = br.ReadByte();
			Unknown889 = br.ReadByte();
			Unknown890 = br.ReadByte();
			Unknown891 = br.ReadByte();
			Unknown892 = br.ReadByte();
			Unknown893 = br.ReadByte();
			FallDamage = br.ReadByte();
			Unknown895 = br.ReadByte();
			FastRegenHp = br.ReadUInt32();
			FastRegenMana = br.ReadUInt32();
			FastRegenEndurance = br.ReadUInt32();
			Unknown908 = br.ReadUInt32();
			Unknown912 = br.ReadUInt32();
			FogDensity = br.ReadSingle();
			Unknown920 = br.ReadUInt32();
			Unknown924 = br.ReadUInt32();
			Unknown928 = br.ReadUInt32();
			// TODO: Array reading for Unknown932 - implement based on actual array size
			// Unknown932 = new byte[size];
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
			// TODO: Array writing for CharName - implement based on actual array size
			// foreach(var item in CharName) bw.Write(item);
			// TODO: Array writing for ZoneShortName - implement based on actual array size
			// foreach(var item in ZoneShortName) bw.Write(item);
			// TODO: Array writing for ZoneLongName - implement based on actual array size
			// foreach(var item in ZoneLongName) bw.Write(item);
			bw.Write(Ztype);
			// TODO: Array writing for FogRed - implement based on actual array size
			// foreach(var item in FogRed) bw.Write(item);
			// TODO: Array writing for FogGreen - implement based on actual array size
			// foreach(var item in FogGreen) bw.Write(item);
			// TODO: Array writing for FogBlue - implement based on actual array size
			// foreach(var item in FogBlue) bw.Write(item);
			bw.Write(Unknown323);
			// TODO: Array writing for FogMinclip - implement based on actual array size
			// foreach(var item in FogMinclip) bw.Write(item);
			// TODO: Array writing for FogMaxclip - implement based on actual array size
			// foreach(var item in FogMaxclip) bw.Write(item);
			bw.Write(Gravity);
			bw.Write(TimeType);
			// TODO: Array writing for RainChance - implement based on actual array size
			// foreach(var item in RainChance) bw.Write(item);
			// TODO: Array writing for RainDuration - implement based on actual array size
			// foreach(var item in RainDuration) bw.Write(item);
			// TODO: Array writing for SnowChance - implement based on actual array size
			// foreach(var item in SnowChance) bw.Write(item);
			// TODO: Array writing for SnowDuration - implement based on actual array size
			// foreach(var item in SnowDuration) bw.Write(item);
			// TODO: Array writing for Unknown537 - implement based on actual array size
			// foreach(var item in Unknown537) bw.Write(item);
			bw.Write(Sky);
			// TODO: Array writing for Unknown571 - implement based on actual array size
			// foreach(var item in Unknown571) bw.Write(item);
			bw.Write(ZoneExpMultiplier);
			bw.Write(SafeY);
			bw.Write(SafeX);
			bw.Write(SafeZ);
			bw.Write(MinZ);
			bw.Write(MaxZ);
			bw.Write(Underworld);
			bw.Write(Minclip);
			bw.Write(Maxclip);
			// TODO: Array writing for Unknown620 - implement based on actual array size
			// foreach(var item in Unknown620) bw.Write(item);
			// TODO: Array writing for ZoneShortName2 - implement based on actual array size
			// foreach(var item in ZoneShortName2) bw.Write(item);
			bw.Write(Unknown800);
			// TODO: Array writing for Unknown804 - implement based on actual array size
			// foreach(var item in Unknown804) bw.Write(item);
			bw.Write(Unknown844);
			bw.Write(Unknown848);
			bw.Write(ZoneId);
			bw.Write(ZoneInstance);
			bw.Write(Scriptnpcreceivedanitem);
			bw.Write(Bcheck);
			bw.Write(Scriptidsomething);
			bw.Write(UnderworldTeleportIndex);
			bw.Write(Scriptidsomething3);
			bw.Write(SuspendBuffs);
			bw.Write(LavaDamage);
			bw.Write(MinLavaDamage);
			bw.Write(Unknown888);
			bw.Write(Unknown889);
			bw.Write(Unknown890);
			bw.Write(Unknown891);
			bw.Write(Unknown892);
			bw.Write(Unknown893);
			bw.Write(FallDamage);
			bw.Write(Unknown895);
			bw.Write(FastRegenHp);
			bw.Write(FastRegenMana);
			bw.Write(FastRegenEndurance);
			bw.Write(Unknown908);
			bw.Write(Unknown912);
			bw.Write(FogDensity);
			bw.Write(Unknown920);
			bw.Write(Unknown924);
			bw.Write(Unknown928);
			// TODO: Array writing for Unknown932 - implement based on actual array size
			// foreach(var item in Unknown932) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct NewZone {\n";
			ret += "	CharName = ";
			try {
				ret += $"{ Indentify(CharName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneShortName = ";
			try {
				ret += $"{ Indentify(ZoneShortName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneLongName = ";
			try {
				ret += $"{ Indentify(ZoneLongName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Ztype = ";
			try {
				ret += $"{ Indentify(Ztype) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FogRed = ";
			try {
				ret += $"{ Indentify(FogRed) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FogGreen = ";
			try {
				ret += $"{ Indentify(FogGreen) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FogBlue = ";
			try {
				ret += $"{ Indentify(FogBlue) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown323 = ";
			try {
				ret += $"{ Indentify(Unknown323) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FogMinclip = ";
			try {
				ret += $"{ Indentify(FogMinclip) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FogMaxclip = ";
			try {
				ret += $"{ Indentify(FogMaxclip) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Gravity = ";
			try {
				ret += $"{ Indentify(Gravity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TimeType = ";
			try {
				ret += $"{ Indentify(TimeType) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	RainChance = ";
			try {
				ret += $"{ Indentify(RainChance) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	RainDuration = ";
			try {
				ret += $"{ Indentify(RainDuration) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SnowChance = ";
			try {
				ret += $"{ Indentify(SnowChance) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SnowDuration = ";
			try {
				ret += $"{ Indentify(SnowDuration) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown537 = ";
			try {
				ret += $"{ Indentify(Unknown537) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Sky = ";
			try {
				ret += $"{ Indentify(Sky) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown571 = ";
			try {
				ret += $"{ Indentify(Unknown571) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneExpMultiplier = ";
			try {
				ret += $"{ Indentify(ZoneExpMultiplier) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SafeY = ";
			try {
				ret += $"{ Indentify(SafeY) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SafeX = ";
			try {
				ret += $"{ Indentify(SafeX) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SafeZ = ";
			try {
				ret += $"{ Indentify(SafeZ) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MinZ = ";
			try {
				ret += $"{ Indentify(MinZ) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MaxZ = ";
			try {
				ret += $"{ Indentify(MaxZ) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Underworld = ";
			try {
				ret += $"{ Indentify(Underworld) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Minclip = ";
			try {
				ret += $"{ Indentify(Minclip) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Maxclip = ";
			try {
				ret += $"{ Indentify(Maxclip) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown620 = ";
			try {
				ret += $"{ Indentify(Unknown620) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneShortName2 = ";
			try {
				ret += $"{ Indentify(ZoneShortName2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown800 = ";
			try {
				ret += $"{ Indentify(Unknown800) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown804 = ";
			try {
				ret += $"{ Indentify(Unknown804) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown844 = ";
			try {
				ret += $"{ Indentify(Unknown844) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown848 = ";
			try {
				ret += $"{ Indentify(Unknown848) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneId = ";
			try {
				ret += $"{ Indentify(ZoneId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneInstance = ";
			try {
				ret += $"{ Indentify(ZoneInstance) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Scriptnpcreceivedanitem = ";
			try {
				ret += $"{ Indentify(Scriptnpcreceivedanitem) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Bcheck = ";
			try {
				ret += $"{ Indentify(Bcheck) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Scriptidsomething = ";
			try {
				ret += $"{ Indentify(Scriptidsomething) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	UnderworldTeleportIndex = ";
			try {
				ret += $"{ Indentify(UnderworldTeleportIndex) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Scriptidsomething3 = ";
			try {
				ret += $"{ Indentify(Scriptidsomething3) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SuspendBuffs = ";
			try {
				ret += $"{ Indentify(SuspendBuffs) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	LavaDamage = ";
			try {
				ret += $"{ Indentify(LavaDamage) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MinLavaDamage = ";
			try {
				ret += $"{ Indentify(MinLavaDamage) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown888 = ";
			try {
				ret += $"{ Indentify(Unknown888) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown889 = ";
			try {
				ret += $"{ Indentify(Unknown889) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown890 = ";
			try {
				ret += $"{ Indentify(Unknown890) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown891 = ";
			try {
				ret += $"{ Indentify(Unknown891) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown892 = ";
			try {
				ret += $"{ Indentify(Unknown892) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown893 = ";
			try {
				ret += $"{ Indentify(Unknown893) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FallDamage = ";
			try {
				ret += $"{ Indentify(FallDamage) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown895 = ";
			try {
				ret += $"{ Indentify(Unknown895) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FastRegenHp = ";
			try {
				ret += $"{ Indentify(FastRegenHp) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FastRegenMana = ";
			try {
				ret += $"{ Indentify(FastRegenMana) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FastRegenEndurance = ";
			try {
				ret += $"{ Indentify(FastRegenEndurance) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown908 = ";
			try {
				ret += $"{ Indentify(Unknown908) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown912 = ";
			try {
				ret += $"{ Indentify(Unknown912) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FogDensity = ";
			try {
				ret += $"{ Indentify(FogDensity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown920 = ";
			try {
				ret += $"{ Indentify(Unknown920) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown924 = ";
			try {
				ret += $"{ Indentify(Unknown924) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown928 = ";
			try {
				ret += $"{ Indentify(Unknown928) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown932 = ";
			try {
				ret += $"{ Indentify(Unknown932) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}