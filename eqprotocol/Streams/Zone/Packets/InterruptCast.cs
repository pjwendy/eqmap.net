using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct InterruptCast_Struct
// {
// uint32 spawnid;
// uint32 messageid;
// char	message[0];
// };

// ENCODE/DECODE Section:
// Handler function not found.

//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct InterruptCast
//{
//    public uint SpawnID;        // Entity ID
//    public uint MessageID;      // Interrupt message ID

//    public InterruptCast(byte[] data, int offset = 0)
//    {
//        using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 8)))
//        using (var br = new BinaryReader(ms))
//        {
//            SpawnID = br.ReadUInt32();
//            MessageID = br.ReadUInt32();
//        }
//    }
//}


namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the InterruptCast packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct InterruptCast
    {
        public uint SpawnID;        // Entity ID
        public uint MessageID;      // Interrupt message ID

        public InterruptCast(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 8)))
            using (var br = new BinaryReader(ms))
            {
                SpawnID = br.ReadUInt32();
                MessageID = br.ReadUInt32();
            }
        }

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct InterruptCast {\n";
			ret += "	SpawnID = ";
			try {
				ret += $"{ Indentify(SpawnID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MessageID = ";
			try {
				ret += $"{ Indentify(MessageID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}