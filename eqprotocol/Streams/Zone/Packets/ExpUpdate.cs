using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ExpUpdate_Struct
// {
// /*0000*/ uint32 exp;                    // Current experience ratio from 0 to 330
// /*0004*/ uint32 aaxp; // @BP ??
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the ExpUpdate packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ExpUpdate
    {
        public uint Exp;   // Current experience ratio from 0 to 330
        public uint AAExp; // AA experience points

        public ExpUpdate(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 8)))
            using (var br = new BinaryReader(ms))
            {
                Exp = br.ReadUInt32();
                AAExp = br.ReadUInt32();
            }
        }
	
		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ExpUpdate {\n";
			ret += "	Exp = ";
			try {
				ret += $"{ Indentify(Exp) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AAExp = ";
			try {
				ret += $"{ Indentify(AAExp) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}