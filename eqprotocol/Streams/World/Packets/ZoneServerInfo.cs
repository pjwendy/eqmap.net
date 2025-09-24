using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ZoneServerInfo_Struct
// {
// /*0000*/	char	ip[128];
// /*0128*/	uint16	port;
// };

// ENCODE/DECODE Section:
// ENCODE(OP_ZoneServerInfo)
// {
// SETUP_DIRECT_ENCODE(ZoneServerInfo_Struct, ZoneServerInfo_Struct);
// 
// OUT_str(ip);
// OUT(port);
// 
// FINISH_ENCODE();
// }
//public struct ZoneServerInfo : IEQStruct
//{
//    public string Host;
//    public ushort Port;

//    public ZoneServerInfo(string Host, ushort Port) : this()
//    {
//        this.Host = Host;
//        this.Port = Port;
//    }

//    public ZoneServerInfo(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public ZoneServerInfo(BinaryReader br) : this()
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
//        Host = br.ReadString(128);
//        Port = br.ReadUInt16();
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
//        bw.Write(Host.ToBytes(128));
//        bw.Write(Port);
//    }

//    public override string ToString()
//    {
//        var ret = "struct ZoneServerInfo {\n";
//        ret += "\tHost = ";
//        try
//        {
//            ret += $"{Indentify(Host)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPort = ";
//        try
//        {
//            ret += $"{Indentify(Port)}\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        return ret + "}";
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the ZoneServerInfo packet structure for EverQuest network communication.
    /// </summary>
    public struct ZoneServerInfo : IEQStruct
    {
        /// <summary>
        /// The IP address or hostname of the zone server.
        /// </summary>
        public string Host;

        /// <summary>
        /// The port number of the zone server.
        /// </summary>
        public ushort Port;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneServerInfo"/> struct with the specified host and port.
        /// </summary>
        /// <param name="Host">The IP address or hostname.</param>
        /// <param name="Port">The port number.</param>
        public ZoneServerInfo(string Host, ushort Port) : this()
        {
            this.Host = Host;
            this.Port = Port;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneServerInfo"/> struct from a byte array.
        /// </summary>
        /// <param name="data">The byte array containing the packed data.</param>
        /// <param name="offset">The offset in the array to start reading from.</param>
        public ZoneServerInfo(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneServerInfo"/> struct from a <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br">The binary reader containing the packed data.</param>
        public ZoneServerInfo(BinaryReader br) : this()
        {
            Unpack(br);
        }

        /// <summary>
        /// Unpacks the structure from a byte array.
        /// </summary>
        /// <param name="data">The byte array containing the packed data.</param>
        /// <param name="offset">The offset in the array to start reading from.</param>
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

        /// <summary>
        /// Unpacks the structure from a <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br">The binary reader containing the packed data.</param>
        public void Unpack(BinaryReader br)
        {
            Host = br.ReadString(128);
            Port = br.ReadUInt16();
        }

        /// <summary>
        /// Packs the structure into a byte array.
        /// </summary>
        /// <returns>A byte array containing the packed data.</returns>
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

        /// <summary>
        /// Packs the structure into a <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="bw">The binary writer to write the packed data to.</param>
        public void Pack(BinaryWriter bw)
        {
            bw.Write(Host.ToBytes(128));
            bw.Write(Port);
        }

        /// <summary>
        /// Returns a string representation of the <see cref="ZoneServerInfo"/> structure.
        /// </summary>
        /// <returns>A string describing the contents of the structure.</returns>
        public override string ToString()
        {
            var ret = "struct ZoneServerInfo {\n";
            ret += "\tHost = ";
            try
            {
                ret += $"{Indentify(Host)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPort = ";
            try
            {
                ret += $"{Indentify(Port)}\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}