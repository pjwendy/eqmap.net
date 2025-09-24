using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SkillUpdate_Struct {
// /*00*/	uint32 skillId;
// /*04*/	uint32 value;
// /*08*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the SkillUpdate packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SkillUpdate
    {
        public uint SkillId;
        public uint Value;

        public SkillUpdate(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 8)))
            using (var br = new BinaryReader(ms))
            {
                SkillId = br.ReadUInt32();
                Value = br.ReadUInt32();
            }
        }

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SkillUpdate {\n";
			ret += "	SkillId = ";
			try {
				ret += $"{ Indentify(SkillId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Value = ";
			try {
				ret += $"{ Indentify(Value) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}