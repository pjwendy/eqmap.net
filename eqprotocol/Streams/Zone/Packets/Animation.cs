using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Animation_Struct {
// /*00*/	uint16 spawnid;
// /*02*/	uint8 speed;
// /*03*/	uint8 action;
// /*04*/
// };

// ENCODE/DECODE Section:
// Handler function not found.
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct Animation
//{
//    public ushort SpawnID;      // Entity ID
//    public byte Action;         // Animation action
//    public byte Speed;          // Animation speed

//    public Animation(byte[] data, int offset = 0)
//    {
//        using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 4)))
//        using (var br = new BinaryReader(ms))
//        {
//            SpawnID = br.ReadUInt16();
//            Action = br.ReadByte();
//            Speed = br.ReadByte();
//        }
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the Animation packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Animation
    {
        public ushort SpawnID;      // Entity ID
        public byte Action;         // Animation action
        public byte Speed;          // Animation speed

        public Animation(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 4)))
            using (var br = new BinaryReader(ms))
            {
                SpawnID = br.ReadUInt16();
                Action = br.ReadByte();
                Speed = br.ReadByte();
            }
        }
	
		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Animation {\n";
			ret += "	SpawnID = ";
			try {
				ret += $"{ Indentify(SpawnID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Speed = ";
			try {
				ret += $"{ Indentify(Speed) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}