using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Weblink_Struct{
// /*000*/ char weblink[1];
// /*004*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

//public struct XTarget : IEQStruct
//{
//    public uint MaxXTargets;
//    public bool RestFlag;
//    public uint Slot;
//    public byte Role;
//    public uint TargetID;
//    public string Name;

//    public XTarget(uint MaxXTargets, bool RestFlag, uint Slot, byte Role, uint TargetID, string Name) : this()
//    {
//        this.MaxXTargets = MaxXTargets;
//        this.RestFlag = RestFlag;
//        this.Slot = Slot;
//        this.Role = Role;
//        this.TargetID = TargetID;
//        this.Name = Name;
//    }

//    public XTarget(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public XTarget(BinaryReader br) : this()
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
//        MaxXTargets = br.ReadUInt32();
//        RestFlag = br.ReadUInt32() != 0;
//        if (RestFlag)
//        {
//            Slot = br.ReadUInt32();
//        }
//        if (RestFlag)
//        {
//            Role = br.ReadByte();
//        }
//        if (RestFlag)
//        {
//            TargetID = br.ReadUInt32();
//        }
//        if (RestFlag)
//        {
//            Name = br.ReadString(-1);
//        }
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
//        bw.Write(MaxXTargets);
//        bw.Write((uint)(RestFlag ? 1 : 0));
//        if (RestFlag)
//        {
//            bw.Write(Slot);
//        }
//        if (RestFlag)
//        {
//            bw.Write(Role);
//        }
//        if (RestFlag)
//        {
//            bw.Write(TargetID);
//        }
//        if (RestFlag)
//        {
//            bw.Write(Name.ToBytes());
//        }
//    }

//    public override string ToString()
//    {
//        var ret = "struct XTarget {\n";
//        ret += "\tMaxXTargets = ";
//        try
//        {
//            ret += $"{Indentify(MaxXTargets)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tRestFlag = ";
//        try
//        {
//            ret += $"{Indentify(RestFlag)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        if (RestFlag)
//        {
//            ret += "\tSlot = ";
//            try
//            {
//                ret += $"{Indentify(Slot)},\n";
//            }
//            catch (NullReferenceException)
//            {
//                ret += "!!NULL!!\n";
//            }
//        }
//        if (RestFlag)
//        {
//            ret += "\tRole = ";
//            try
//            {
//                ret += $"{Indentify(Role)},\n";
//            }
//            catch (NullReferenceException)
//            {
//                ret += "!!NULL!!\n";
//            }
//        }
//        if (RestFlag)
//        {
//            ret += "\tTargetID = ";
//            try
//            {
//                ret += $"{Indentify(TargetID)},\n";
//            }
//            catch (NullReferenceException)
//            {
//                ret += "!!NULL!!\n";
//            }
//        }
//        if (RestFlag)
//        {
//            ret += "\tName = ";
//            try
//            {
//                ret += $"{Indentify(Name)}\n";
//            }
//            catch (NullReferenceException)
//            {
//                ret += "!!NULL!!\n";
//            }
//        }
//        return ret + "}";
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the XTargetResponse packet structure for EverQuest network communication.
    /// </summary>
    public struct XTargetResponse : IEQStruct
    {
        public uint MaxXTargets;
        public bool RestFlag;
        public uint Slot;
        public byte Role;
        public uint TargetID;
        public string Name;

        public XTargetResponse(uint MaxXTargets, bool RestFlag, uint Slot, byte Role, uint TargetID, string Name) : this()
        {
            this.MaxXTargets = MaxXTargets;
            this.RestFlag = RestFlag;
            this.Slot = Slot;
            this.Role = Role;
            this.TargetID = TargetID;
            this.Name = Name;
        }

        public XTargetResponse(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public XTargetResponse(BinaryReader br) : this()
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
            MaxXTargets = br.ReadUInt32();
            RestFlag = br.ReadUInt32() != 0;
            if (RestFlag)
            {
                Slot = br.ReadUInt32();
            }
            if (RestFlag)
            {
                Role = br.ReadByte();
            }
            if (RestFlag)
            {
                TargetID = br.ReadUInt32();
            }
            if (RestFlag)
            {
                Name = br.ReadString(-1);
            }
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
            bw.Write(MaxXTargets);
            bw.Write((uint)(RestFlag ? 1 : 0));
            if (RestFlag)
            {
                bw.Write(Slot);
            }
            if (RestFlag)
            {
                bw.Write(Role);
            }
            if (RestFlag)
            {
                bw.Write(TargetID);
            }
            if (RestFlag)
            {
                bw.Write(Name.ToBytes());
            }
        }

        public override string ToString()
        {
            var ret = "struct XTargetResponse {\n";
            ret += "\tMaxXTargets = ";
            try
            {
                ret += $"{Indentify(MaxXTargets)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tRestFlag = ";
            try
            {
                ret += $"{Indentify(RestFlag)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            if (RestFlag)
            {
                ret += "\tSlot = ";
                try
                {
                    ret += $"{Indentify(Slot)},\n";
                }
                catch (NullReferenceException)
                {
                    ret += "!!NULL!!\n";
                }
            }
            if (RestFlag)
            {
                ret += "\tRole = ";
                try
                {
                    ret += $"{Indentify(Role)},\n";
                }
                catch (NullReferenceException)
                {
                    ret += "!!NULL!!\n";
                }
            }
            if (RestFlag)
            {
                ret += "\tTargetID = ";
                try
                {
                    ret += $"{Indentify(TargetID)},\n";
                }
                catch (NullReferenceException)
                {
                    ret += "!!NULL!!\n";
                }
            }
            if (RestFlag)
            {
                ret += "\tName = ";
                try
                {
                    ret += $"{Indentify(Name)}\n";
                }
                catch (NullReferenceException)
                {
                    ret += "!!NULL!!\n";
                }
            }
            return ret + "}";
        }
    }
}