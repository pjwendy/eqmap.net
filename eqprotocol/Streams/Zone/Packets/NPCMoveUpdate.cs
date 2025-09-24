using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:

// ENCODE/DECODE Section:

//public struct PlayerPositionUpdate : IEQStruct
//{
//    public ushort ID;
//    public UpdatePosition Position;

//    public PlayerPositionUpdate(ushort ID, UpdatePosition Position) : this()
//    {
//        this.ID = ID;
//        this.Position = Position;
//    }

//    public PlayerPositionUpdate(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public PlayerPositionUpdate(BinaryReader br) : this()
//    {
//        Unpack(br);
//    }
//    public void Unpack(byte[] data, int offset = 0)
//    {
//        using (var ms = new MemoryStream(data, offset, data.Length - offset))
//        {
//            using (var br = new BinaryReader(ms))
//            {
//                Unpack(br);
//            }
//        }
//    }
//    public void Unpack(BinaryReader br)
//    {
//        ID = br.ReadUInt16();
//        Position = new UpdatePosition(br);
//    }

//    public byte[] Pack()
//    {
//        using (var ms = new MemoryStream())
//        {
//            using (var bw = new BinaryWriter(ms))
//            {
//                Pack(bw);
//                return ms.ToArray();
//            }
//        }
//    }
//    public void Pack(BinaryWriter bw)
//    {
//        bw.Write(ID);
//        Position.Pack(bw);
//    }

//    public override string ToString()
//    {
//        var ret = "struct PlayerPositionUpdate {\n";
//        ret += "\tID = ";
//        try
//        {
//            ret += $"{Indentify(ID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPosition = ";
//        try
//        {
//            ret += $"{Indentify(Position)}\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        return ret + "}";
//    }
//}

namespace OpenEQ.Netcode
{
    /// <summary>
    /// Represents the NPCMoveUpdate packet structure for EverQuest network communication.
    /// </summary>
    public struct NPCMoveUpdate : IEQStruct
    {
        public ushort ID;
        public UpdatePositionFromServer Position;

        public NPCMoveUpdate(ushort ID, UpdatePositionFromServer Position) : this()
        {
            this.ID = ID;
            this.Position = Position;
        }

        public NPCMoveUpdate(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public NPCMoveUpdate(BinaryReader br) : this()
        {
            Unpack(br);
        }
        public void Unpack(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, data.Length - offset))
            {
                using (var br = new BinaryReader(ms))
                {
                    Unpack(br);
                }
            }
        }
        public void Unpack(BinaryReader br)
        {
            ID = br.ReadUInt16();
            Position = new UpdatePositionFromServer(br);
        }

        public byte[] Pack()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    Pack(bw);
                    return ms.ToArray();
                }
            }
        }
        public void Pack(BinaryWriter bw)
        {
            bw.Write(ID);
            Position.Pack(bw);
        }

        public override string ToString()
        {
            var ret = "struct NPCMoveUpdate {\n";
            ret += "\tID = ";
            try
            {
                ret += $"{Indentify(ID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPosition = ";
            try
            {
                ret += $"{Indentify(Position)}\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}