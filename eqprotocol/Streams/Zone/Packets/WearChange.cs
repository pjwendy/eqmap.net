using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition: Not found or not applicable

// ENCODE/DECODE Section:
// DECODE(OP_WearChange)
// {
// DECODE_LENGTH_EXACT(structs::WearChange_Struct);
// SETUP_DIRECT_DECODE(WearChange_Struct, structs::WearChange_Struct);
// 
// IN(spawn_id);
// IN(material);
// IN(unknown06);
// IN(elite_material);
// IN(color.Color);
// IN(wear_slot_id);
// emu->hero_forge_model = 0;
// emu->unknown18 = 0;
// 
// FINISH_DIRECT_DECODE();
// }
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct WearChange
//{
//    public ushort SpawnId;
//    public uint Material;
//    public uint Unknown06;
//    public uint EliteMaterial;
//    public uint Color;
//    public byte WearSlotId;

//    public WearChange(byte[] data, int offset = 0)
//    {
//        using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 19)))
//        using (var br = new BinaryReader(ms))
//        {
//            SpawnId = br.ReadUInt16();
//            Material = br.ReadUInt32();
//            Unknown06 = br.ReadUInt32();
//            EliteMaterial = br.ReadUInt32();
//            Color = br.ReadUInt32();
//            WearSlotId = br.ReadByte();
//        }
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the WearChange packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WearChange
    {
        public ushort SpawnId;
        public uint Material;
        public uint Unknown06;
        public uint EliteMaterial;
        public uint Color;
        public byte WearSlotId;

        public WearChange(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 19)))
            using (var br = new BinaryReader(ms))
            {
                SpawnId = br.ReadUInt16();
                Material = br.ReadUInt32();
                Unknown06 = br.ReadUInt32();
                EliteMaterial = br.ReadUInt32();
                Color = br.ReadUInt32();
                WearSlotId = br.ReadByte();
            }
        }

        /// <summary>
        /// Returns a string representation of the struct with all field values.
        /// </summary>
        /// <returns>A formatted string containing all field names and values.</returns>
        public override string ToString()
        {
            var ret = "struct WearChange {\n";
            ret += "	SpawnId = ";
            try
            {
                ret += $"{Indentify(SpawnId)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "	Material = ";
            try
            {
                ret += $"{Indentify(Material)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "	EliteMaterial = ";
            try
            {
                ret += $"{Indentify(EliteMaterial)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "	Color = ";
            try
            {
                ret += $"{Indentify(Color)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "	WearSlotId = ";
            try
            {
                ret += $"{Indentify(WearSlotId)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }

            return ret;
        }
    }
}