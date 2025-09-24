using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct CastSpell_Struct
// {
// uint32	slot;
// uint32	spell_id;
// uint32	inventoryslot;  // slot for clicky item, 0xFFFF = normal cast
// uint32	target_id;
// uint32  cs_unknown1;
// uint32  cs_unknown2;
// float   y_pos;
// float   x_pos;
// float   z_pos;
// };

// ENCODE/DECODE Section:
// DECODE(OP_CastSpell)
// {
// DECODE_LENGTH_EXACT(structs::CastSpell_Struct);
// SETUP_DIRECT_DECODE(CastSpell_Struct, structs::CastSpell_Struct);
// 
// emu->slot = static_cast<uint32>(UFToServerCastingSlot(static_cast<spells::CastingSlot>(eq->slot)));
// 
// IN(spell_id);
// emu->inventoryslot = UFToServerSlot(eq->inventoryslot);
// IN(target_id);
// IN(cs_unknown1);
// IN(cs_unknown2);
// IN(y_pos);
// IN(x_pos);
// IN(z_pos);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the CastSpell packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CastSpell
    {
        public uint SpellID;        // Spell ID
        public uint CasterID;       // Caster entity ID
        public uint TargetID;       // Target entity ID
        public uint Slot;           // Spell slot
        public uint CastTime;       // Cast time in ms
        public uint TargetType;     // Target type        
		uint Unknown1;
        uint Unknown2;
        public float YPos;			// Y Position of target
        public float XPos;			// X Position of target
        public float ZPos;			// Z Position of target    

        public CastSpell(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 24)))
            using (var br = new BinaryReader(ms))
            {
                SpellID = br.ReadUInt32();
                CasterID = br.ReadUInt32();
                TargetID = br.ReadUInt32();
                Slot = br.ReadUInt32();
                CastTime = br.ReadUInt32();
                TargetType = br.ReadUInt32();
                Unknown1 = br.ReadUInt32();
                Unknown2 = br.ReadUInt32();
				YPos = br.ReadSingle();
                XPos = br.ReadSingle();
                ZPos = br.ReadSingle();
            }
        }
	
		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct CastSpell {\n";
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpellID = ";
			try {
				ret += $"{ Indentify(SpellID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Slot = ";
			try {
				ret += $"{ Indentify(Slot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TargetID = ";
			try {
				ret += $"{ Indentify(TargetID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}			
			ret += "	YPos = ";
			try {
				ret += $"{ Indentify(YPos) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	XPos = ";
			try {
				ret += $"{ Indentify(XPos) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZPos = ";
			try {
				ret += $"{ Indentify(ZPos) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}