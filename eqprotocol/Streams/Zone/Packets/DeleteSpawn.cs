using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct EntityId_Struct
// {
// /*00*/	uint32	entity_id;
// /*04*/
// };

// ENCODE/DECODE Section:
//// Handler function not found.
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct EntityId : IEQStruct
//{
//    public uint Id;

//    public EntityId(BinaryReader br)
//    {
//        Id = br.ReadUInt32();
//    }

//    public void Unpack(byte[] data, int offset = 0)
//    {
//        using (var ms = new MemoryStream(data, offset, 4))
//        using (var br = new BinaryReader(ms))
//        {
//            Id = br.ReadUInt32();
//        }
//    }

//    public void Unpack(BinaryReader br)
//    {
//        Id = br.ReadUInt32();
//    }

//    public byte[] Pack()
//    {
//        using (var ms = new MemoryStream())
//        using (var bw = new BinaryWriter(ms))
//        {
//            bw.Write(Id);
//            return ms.ToArray();
//        }
//    }

//    public void Pack(BinaryWriter bw)
//    {
//        bw.Write(Id);
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the DeleteSpawn packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeleteSpawn : IEQStruct
    {
        public uint Id;

        public DeleteSpawn(BinaryReader br)
        {
            Id = br.ReadUInt32();
        }

        public void Unpack(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, 4))
            using (var br = new BinaryReader(ms))
            {
                Id = br.ReadUInt32();
            }
        }

        public void Unpack(BinaryReader br)
        {
            Id = br.ReadUInt32();
        }

        public byte[] Pack()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(Id);
                return ms.ToArray();
            }
        }

        public void Pack(BinaryWriter bw)
        {
            bw.Write(Id);
        }
	
		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DeleteSpawn {\n";
			ret += "	Id = ";
			try {
				ret += $"{ Indentify(Id) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}