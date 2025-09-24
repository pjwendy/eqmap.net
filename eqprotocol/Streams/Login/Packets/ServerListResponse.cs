using EQProtocol.Streams.Common;
using EQProtocol.Streams.Login;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition: Not found or not applicable

// ENCODE/DECODE Section:
//public struct ServerListHeader : IEQStruct
//{
//    uint unk1;
//    uint unk2;
//    uint unk3;
//    uint unk4;
//    uint serverCount;
//    public List<ServerListElement> Servers;

//    public ServerListHeader(List<ServerListElement> Servers) : this()
//    {
//        this.Servers = Servers;
//    }

//    public ServerListHeader(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public ServerListHeader(BinaryReader br) : this()
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
//        unk1 = br.ReadUInt32();
//        unk2 = br.ReadUInt32();
//        unk3 = br.ReadUInt32();
//        unk4 = br.ReadUInt32();
//        serverCount = br.ReadUInt32();
//        Servers = new List<ServerListElement>();
//        for (var i = 0; i < serverCount; ++i)
//        {
//            Servers.Add(new ServerListElement(br));
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
//        bw.Write(unk1);
//        bw.Write(unk2);
//        bw.Write(unk3);
//        bw.Write(unk4);
//        bw.Write(serverCount);
//        for (var i = 0; i < serverCount; ++i)
//        {
//            Servers[i].Pack(bw);
//        }
//    }

//    public override string ToString()
//    {
//        var ret = "struct ServerListHeader {\n";
//        ret += "\tServers = ";
//        try
//        {
//            ret += "{\n";
//            for (int i = 0, e = Servers.Count; i < e; ++i)
//                ret += $"\t\t{Indentify(Servers[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
//            ret += "\t}\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        return ret + "}";
//    }
//}

namespace EQProtocol.Streams.Login.Packets {
	/// <summary>
	/// Represents the ServerListResponse packet structure for EverQuest network communication.
	/// </summary>
	public struct ServerListResponse : IEQStruct {
        uint unk1;
        uint unk2;
		uint unk3;
		uint unk4;
		uint serverCount;
		public List<ServerListElement> Servers;

		/// <summary>
		/// Initializes a new instance of the ServerListResponse struct with specified field values.
		/// </summary>
		public ServerListResponse(List<ServerListElement> Servers) : this()
		{
			this.Servers = Servers;
		}

		/// <summary>
		/// Initializes a new instance of the ServerListResponse struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ServerListResponse(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ServerListResponse struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ServerListResponse(BinaryReader br) : this() {
			Unpack(br);
		}

		/// <summary>
		/// Unpacks the struct data from a byte array.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public void Unpack(byte[] data, int offset = 0) {
			using(var ms = new MemoryStream(data, offset, data.Length - offset)) {
				using(var br = new BinaryReader(ms)) {
					Unpack(br);
				}
			}
		}

		/// <summary>
		/// Unpacks the struct data from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public void Unpack(BinaryReader br) {
			unk1 = br.ReadUInt32();
			unk2 = br.ReadUInt32();
			unk3 = br.ReadUInt32();
			unk4 = br.ReadUInt32();
			serverCount = br.ReadUInt32();
			Servers = new List<ServerListElement>();
			for (var i = 0; i < serverCount; ++i)
			{
				Servers.Add(new ServerListElement(br));
			}
		}

        /// <summary>
        /// Packs the struct data into a byte array.
        /// </summary>
        /// <returns>A byte array containing the packed struct data.</returns>
        public byte[] Pack() {
			using(var ms = new MemoryStream()) {
				using(var bw = new BinaryWriter(ms)) {
					Pack(bw);
					return ms.ToArray();
				}
			}
		}

		/// <summary>
		/// Packs the struct data into a BinaryWriter.
		/// </summary>
		/// <param name="bw">The BinaryWriter to write data to.</param>
		public void Pack(BinaryWriter bw) {
			bw.Write(unk1);
			bw.Write(unk2);
			bw.Write(unk3);
			bw.Write(unk4);
			bw.Write(serverCount);
			for (var i = 0; i < serverCount; ++i)
			{
				Servers[i].Pack(bw);
			}
		}

        /// <summary>
        /// Returns a string representation of the struct with all field values.
        /// </summary>
        /// <returns>A formatted string containing all field names and values.</returns>
        public override string ToString() {
			var ret = "struct ServerListHeader {\n";
			ret += "\tServers = ";
			try
			{
				ret += "{\n";
				for (int i = 0, e = Servers.Count; i < e; ++i)
					ret += $"\t\t{Indentify(Servers[i], 2)}" + (i != e - 1 ? "," : "") + "\n";
				ret += "\t}\n";
			}
			catch (NullReferenceException)
			{
				ret += "!!NULL!!\n";
			}
			return ret + "}";
		}
    }
}