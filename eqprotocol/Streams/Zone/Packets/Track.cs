using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Track_Struct {
// uint16 entityid;
// uint16 y;
// uint16 x;
// uint16 z;
// };

// ENCODE/DECODE Section:
// ENCODE(OP_Track)
// {
// EQApplicationPacket *in = *p;
// *p = nullptr;
// 
// unsigned char *__emu_buffer = in->pBuffer;
// Track_Struct *emu = (Track_Struct *)__emu_buffer;
// 
// int EntryCount = in->size / sizeof(Track_Struct);
// 
// if (EntryCount == 0 || ((in->size % sizeof(Track_Struct))) != 0)
// {
// LogNetcode("[STRUCTS] Wrong size on outbound [{}]: Got [{}], expected multiple of [{}]", opcodes->EmuToName(in->GetOpcode()), in->size, sizeof(Track_Struct));
// delete in;
// return;
// }
// 
// int PacketSize = 2;
// 
// for (int i = 0; i < EntryCount; ++i, ++emu)
// PacketSize += (12 + strlen(emu->name));
// 
// emu = (Track_Struct *)__emu_buffer;
// 
// in->size = PacketSize;
// in->pBuffer = new unsigned char[in->size];
// 
// char *Buffer = (char *)in->pBuffer;
// 
// VARSTRUCT_ENCODE_TYPE(uint16, Buffer, EntryCount);
// 
// for (int i = 0; i < EntryCount; ++i, ++emu)
// {
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, emu->entityid);
// VARSTRUCT_ENCODE_TYPE(float, Buffer, emu->distance);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->level);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->is_npc);
// VARSTRUCT_ENCODE_STRING(Buffer, emu->name);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, emu->is_merc);
// }
// 
// delete[] __emu_buffer;
// dest->FastQueuePacket(&in, ack_req);
// }
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct Track
//{
//    public ushort EntityID;
//    public ushort Y;
//    public ushort X;
//    public ushort Z;

//    public Track(byte[] data, int offset = 0)
//    {
//        var availableBytes = data.Length - offset;
//        if (availableBytes < 8)
//        {
//            // Initialize with defaults if not enough data
//            EntityID = 0;
//            Y = 0;
//            X = 0;
//            Z = 0;
//            return;
//        }

//        using (var ms = new MemoryStream(data, offset, 8))
//        using (var br = new BinaryReader(ms))
//        {
//            EntityID = br.ReadUInt16();
//            Y = br.ReadUInt16();
//            X = br.ReadUInt16();
//            Z = br.ReadUInt16();
//        }
//    }
//}
namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the Track packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Track
    {
        public ushort EntityID;
        public ushort Y;
        public ushort X;
        public ushort Z;

        public Track(byte[] data, int offset = 0)
        {
            var availableBytes = data.Length - offset;
            if (availableBytes < 8)
            {
                // Initialize with defaults if not enough data
                EntityID = 0;
                Y = 0;
                X = 0;
                Z = 0;
                return;
            }

            using (var ms = new MemoryStream(data, offset, 8))
            using (var br = new BinaryReader(ms))
            {
                EntityID = br.ReadUInt16();
                Y = br.ReadUInt16();
                X = br.ReadUInt16();
                Z = br.ReadUInt16();
            }
        }
	
		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Track {\n";
			ret += "	EntityID = ";
			try {
				ret += $"{ Indentify(EntityID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Y = ";
			try {
				ret += $"{ Indentify(Y) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	X = ";
			try {
				ret += $"{ Indentify(X) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Z = ";
			try {
				ret += $"{ Indentify(Z) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}