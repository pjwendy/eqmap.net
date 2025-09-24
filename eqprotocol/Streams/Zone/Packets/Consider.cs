using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition: Not found or not applicable

// ENCODE/DECODE Section:
// DECODE(OP_Consider)
// {
// DECODE_LENGTH_EXACT(structs::Consider_Struct);
// SETUP_DIRECT_DECODE(Consider_Struct, structs::Consider_Struct);
// 
// IN(playerid);
// IN(targetid);
// IN(faction);
// IN(level);
// //emu->cur_hp = 1;
// //emu->max_hp = 2;
// //emu->pvpcon = 0;
// 
// FINISH_DIRECT_DECODE();
// }
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct Consider
//{
//    public uint PlayerID;      // PlayerID
//    public uint TargetID;      // TargetID  
//    public uint Faction;       // Faction
//    public uint Level;         // Level
//    public byte PvpCon;        // Pvp con flag 0/1
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
//    public byte[] Unknown017;  // Unknown bytes

//    public Consider(byte[] data, int offset = 0)
//    {
//        var availableBytes = data.Length - offset;
//        if (availableBytes < 20)
//        {
//            // Initialize with defaults if not enough data
//            PlayerID = 0;
//            TargetID = 0;
//            Faction = 0;
//            Level = 0;
//            PvpCon = 0;
//            Unknown017 = new byte[3];
//            return;
//        }

//        using (var ms = new MemoryStream(data, offset, 20))
//        using (var br = new BinaryReader(ms))
//        {
//            PlayerID = br.ReadUInt32();
//            TargetID = br.ReadUInt32();
//            Faction = br.ReadUInt32();
//            Level = br.ReadUInt32();
//            PvpCon = br.ReadByte();
//            Unknown017 = br.ReadBytes(3);
//        }
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the Consider packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Consider
    {
        public uint PlayerID;      // PlayerID
        public uint TargetID;      // TargetID  
        public uint Faction;       // Faction
        public uint Level;         // Level
        public byte PvpCon;        // Pvp con flag 0/1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Unknown017;  // Unknown bytes

        public Consider(byte[] data, int offset = 0)
        {
            var availableBytes = data.Length - offset;
            if (availableBytes < 20)
            {
                // Initialize with defaults if not enough data
                PlayerID = 0;
                TargetID = 0;
                Faction = 0;
                Level = 0;
                PvpCon = 0;
                Unknown017 = new byte[3];
                return;
            }

            using (var ms = new MemoryStream(data, offset, 20))
            using (var br = new BinaryReader(ms))
            {
                PlayerID = br.ReadUInt32();
                TargetID = br.ReadUInt32();
                Faction = br.ReadUInt32();
                Level = br.ReadUInt32();
                PvpCon = br.ReadByte();
                Unknown017 = br.ReadBytes(3);
            }
        }
    }
}