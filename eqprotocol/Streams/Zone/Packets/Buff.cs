using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SpellBuffPacket_Struct {
// /*000*/	uint32 entityid;	// Player id who cast the buff
// /*004*/	SpellBuff_Struct buff;
// /*080*/	uint32 slotid;
// /*084*/	uint32 bufffade;
// /*088*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_Buff)
// {
// DECODE_LENGTH_EXACT(structs::SpellBuffPacket_Struct);
// SETUP_DIRECT_DECODE(SpellBuffPacket_Struct, structs::SpellBuffPacket_Struct);
// 
// IN(entityid);
// IN(buff.effect_type);
// IN(buff.level);
// IN(buff.unknown003);
// IN(buff.spellid);
// IN(buff.duration);
// emu->slotid = UFToServerBuffSlot(eq->slotid);
// IN(slotid);
// IN(bufffade);
// 
// FINISH_DIRECT_DECODE();
// }
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct Buff
//{
//    public uint EntityID;       // Entity ID
//    public ushort SpellID;      // Spell ID
//    public uint Duration;       // Duration in ticks
//    public byte Level;          // Caster level
//    public uint BardModifier;   // Bard modifier
//    public uint Effect;         // Effect
//    public uint DamageShield;   // Damage shield amount

//    public Buff(byte[] data, int offset = 0)
//    {
//        var availableBytes = data.Length - offset;
//        if (availableBytes < 23)  // 4+2+4+1+4+4+4 = 23 bytes
//        {
//            // Initialize with defaults if not enough data
//            EntityID = 0;
//            SpellID = 0;
//            Duration = 0;
//            Level = 0;
//            BardModifier = 0;
//            Effect = 0;
//            DamageShield = 0;
//            return;
//        }

//        using (var ms = new MemoryStream(data, offset, 23))
//        using (var br = new BinaryReader(ms))
//        {
//            EntityID = br.ReadUInt32();
//            SpellID = br.ReadUInt16();
//            Duration = br.ReadUInt32();
//            Level = br.ReadByte();
//            BardModifier = br.ReadUInt32();
//            Effect = br.ReadUInt32();
//            DamageShield = br.ReadUInt32();
//        }
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the Buff packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Buff
    {
        public uint EntityID;       // Entity ID
        public ushort SpellID;      // Spell ID
        public uint Duration;       // Duration in ticks
        public byte Level;          // Caster level
        public uint BardModifier;   // Bard modifier
        public uint Effect;         // Effect
        public uint DamageShield;   // Damage shield amount

        public Buff(byte[] data, int offset = 0)
        {
            var availableBytes = data.Length - offset;
            if (availableBytes < 23)  // 4+2+4+1+4+4+4 = 23 bytes
            {
                // Initialize with defaults if not enough data
                EntityID = 0;
                SpellID = 0;
                Duration = 0;
                Level = 0;
                BardModifier = 0;
                Effect = 0;
                DamageShield = 0;
                return;
            }

            using (var ms = new MemoryStream(data, offset, 23))
            using (var br = new BinaryReader(ms))
            {
                EntityID = br.ReadUInt32();
                SpellID = br.ReadUInt16();
                Duration = br.ReadUInt32();
                Level = br.ReadByte();
                BardModifier = br.ReadUInt32();
                Effect = br.ReadUInt32();
                DamageShield = br.ReadUInt32();
            }
        }
	
		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Buff {\n";
			ret += "	EntityID = ";
			try {
				ret += $"{ Indentify(EntityID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
            ret += "	SpellID = ";
            try
            {
                ret += $"{Indentify(SpellID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "	Duration = ";
            try
            {
                ret += $"{Indentify(Duration)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "	Level = ";
            try
            {
                ret += $"{Indentify(Level)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "	BardModifier = ";
            try
            {
                ret += $"{Indentify(BardModifier)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "	Effect = ";
            try
            {
                ret += $"{Indentify(Effect)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "	DamageShield = ";
            try
            {
                ret += $"{Indentify(DamageShield)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }

            return ret;
		}
	}
}