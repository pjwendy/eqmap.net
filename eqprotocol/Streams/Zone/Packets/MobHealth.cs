using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DeleteSpawn_Struct
// {
// /*00*/ uint32 spawn_id;             // Spawn ID to delete
// /*04*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the MobHealth packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MobHealth
    {
        public byte HP;         // HP percentage (0-100)
        public ushort EntityID; // Mob's ID (was incorrectly ordered)

        public MobHealth(byte[] data, int offset = 0)
        {
            var availableBytes = data.Length - offset;
            if (availableBytes < 3)
            {
                // Initialize with defaults if not enough data
                HP = 0;
                EntityID = 0;
                return;
            }

            using (var ms = new MemoryStream(data, offset, 3))
            using (var br = new BinaryReader(ms))
            {
                HP = br.ReadByte();
                EntityID = br.ReadUInt16();
            }
        }
        /// <summary>
        /// Returns a string representation of the struct with all field values.
        /// </summary>
        /// <returns>A formatted string containing all field names and values.</returns>
        public override string ToString()
        {
            var ret = "struct MobHealth {\n";
            ret += "	EntityID = ";
            try
            {
                ret += $"{Indentify(EntityID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "	HP = ";
            try
            {
                ret += $"{Indentify(HP)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret;
        }
    }
}