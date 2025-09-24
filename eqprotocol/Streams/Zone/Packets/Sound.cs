using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct QuestReward_Struct
// {
// /*000*/ uint32	mob_id;	// ID of mob awarding the client
// /*004*/ uint32	target_id;
// /*008*/ uint32	exp_reward;
// /*012*/ uint32	faction;
// /*016*/ int32	faction_mod;
// /*020*/ uint32	copper;		// Gives copper to the client
// /*024*/ uint32	silver;		// Gives silver to the client
// /*028*/ uint32	gold;		// Gives gold to the client
// /*032*/ uint32	platinum;	// Gives platinum to the client
// /*036*/ int32	item_id[QUESTREWARD_COUNT];	// -1 for nothing
// /*068*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the Sound packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Sound
    {
        public uint EntityID;   // Entity making the sound
        public byte SoundID;     // Sound ID

        public Sound(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 5)))
            using (var br = new BinaryReader(ms))
            {
                EntityID = br.ReadUInt32();
                SoundID = br.ReadByte();
            }
        }
	
		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Sound {\n";
			ret += "	EntityID = ";
			try {
				ret += $"{ Indentify(EntityID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SoundID = ";
			try {
				ret += $"{ Indentify(SoundID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}