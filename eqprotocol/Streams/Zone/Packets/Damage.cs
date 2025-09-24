using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct CombatDamage_Struct
// {
// /* 00 */	uint16	target;
// /* 02 */	uint16	source;
// /* 04 */	uint8	type;			//slashing, etc.  231 (0xE7) for spells
// /* 05 */	uint16	spellid;
// /* 07 */	int32	damage;
// /* 11 */	float	force;		// cd cc cc 3d
// /* 15 */	float	hit_heading;		// see above notes in Action_Struct
// /* 19 */	float	hit_pitch;
// /* 23 */	uint8	secondary;	// 0 for primary hand, 1 for secondary
// /* 24 */	uint32	special; // 2 = Rampage, 1 = Wild Rampage
// /* 28 */
// };

// ENCODE/DECODE Section:
// DECODE(OP_Damage)
// {
// DECODE_LENGTH_EXACT(structs::CombatDamage_Struct);
// SETUP_DIRECT_DECODE(CombatDamage_Struct, structs::CombatDamage_Struct);
// 
// IN(target);
// IN(source);
// IN(type);
// IN(spellid);
// IN(damage);
// IN(hit_heading);
// 
// FINISH_DIRECT_DECODE();
//// }
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct Damage
//{
//    public ushort Target;       // Target entity ID
//    public ushort Source;       // Source entity ID
//    public byte Type;           // Damage type (231/0xE7 for spells)
//    public ushort SpellID;      // Spell ID if spell damage
//    public int Amount;          // Damage amount
//    public float Force;         // Force/knockback
//    public float HitHeading;    // Hit heading
//    public float HitPitch;      // Hit pitch
//    public byte Secondary;      // 0 for primary hand, 1 for secondary
//    public uint Special;        // 2 = Rampage, 1 = Wild Rampage

//    public Damage(byte[] data, int offset = 0)
//    {
//        var availableBytes = data.Length - offset;
//        if (availableBytes < 28)  // CombatDamage_Struct is 28 bytes
//        {
//            // Initialize with defaults if not enough data
//            Target = 0;
//            Source = 0;
//            Type = 0;
//            SpellID = 0;
//            Amount = 0;
//            Force = 0;
//            HitHeading = 0;
//            HitPitch = 0;
//            Secondary = 0;
//            Special = 0;
//            return;
//        }

//        using (var ms = new MemoryStream(data, offset, 28))
//        using (var br = new BinaryReader(ms))
//        {
//            Target = br.ReadUInt16();
//            Source = br.ReadUInt16();
//            Type = br.ReadByte();
//            SpellID = br.ReadUInt16();
//            Amount = br.ReadInt32();
//            Force = br.ReadSingle();
//            HitHeading = br.ReadSingle();
//            HitPitch = br.ReadSingle();
//            Secondary = br.ReadByte();
//            Special = br.ReadUInt32();
//        }
//    }
//}
namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the Damage packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Damage
    {
        public ushort Target;       // Target entity ID
        public ushort Source;       // Source entity ID
        public byte Type;           // Damage type (231/0xE7 for spells)
        public ushort SpellID;      // Spell ID if spell damage
        public int Amount;          // Damage amount
        public float Force;         // Force/knockback
        public float HitHeading;    // Hit heading
        public float HitPitch;      // Hit pitch
        public byte Secondary;      // 0 for primary hand, 1 for secondary
        public uint Special;        // 2 = Rampage, 1 = Wild Rampage

        public Damage(byte[] data, int offset = 0)
        {
            var availableBytes = data.Length - offset;
            if (availableBytes < 28)  // CombatDamage_Struct is 28 bytes
            {
                // Initialize with defaults if not enough data
                Target = 0;
                Source = 0;
                Type = 0;
                SpellID = 0;
                Amount = 0;
                Force = 0;
                HitHeading = 0;
                HitPitch = 0;
                Secondary = 0;
                Special = 0;
                return;
            }

            using (var ms = new MemoryStream(data, offset, 28))
            using (var br = new BinaryReader(ms))
            {
                Target = br.ReadUInt16();
                Source = br.ReadUInt16();
                Type = br.ReadByte();
                SpellID = br.ReadUInt16();
                Amount = br.ReadInt32();
                Force = br.ReadSingle();
                HitHeading = br.ReadSingle();
                HitPitch = br.ReadSingle();
                Secondary = br.ReadByte();
                Special = br.ReadUInt32();
            }
        }	

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Damage {\n";
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
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpellID = ";
			try {
				ret += $"{ Indentify(SpellID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Amount = ";
			try {
				ret += $"{ Indentify(Amount) },\n";
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
			ret += "	Secondary = ";
			try {
				ret += $"{ Indentify(Secondary) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Special = ";
			try {
				ret += $"{ Indentify(Special) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}