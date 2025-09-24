using EQProtocol.Streams.Common;
using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ExpansionInfo_Struct {
// /*000*/	char	Unknown000[64];
// /*064*/	uint32	Expansions;
// };

// ENCODE/DECODE Section:
// Handler function not found.


//public struct EnterWorld : IEQStruct {
//	public string Name;
//	public bool Tutorial;
//	public bool GoHome;

//	public EnterWorld(string Name, bool Tutorial, bool GoHome) : this() {
//		this.Name = Name;
//		this.Tutorial = Tutorial;
//		this.GoHome = GoHome;
//	}

//	public EnterWorld(byte[] data, int offset = 0) : this() {
//		Unpack(data, offset);
//	}
//	public EnterWorld(BinaryReader br) : this() {
//		Unpack(br);
//	}
//	public void Unpack(byte[] data, int offset = 0) {
//		using(var ms = new MemoryStream(data, offset, data.Length - offset)) {
//			using(var br = new BinaryReader(ms)) {
//				Unpack(br);
//			}
//		}
//	}
//	public void Unpack(BinaryReader br) {
//		Name = br.ReadString(64);
//		Tutorial = br.ReadUInt32() != 0;
//		GoHome = br.ReadUInt32() != 0;
//	}

//	public byte[] Pack() {
//		using(var ms = new MemoryStream()) {
//			using(var bw = new BinaryWriter(ms)) {
//				Pack(bw);
//				return ms.ToArray();
//			}
//		}
//	}
//	public void Pack(BinaryWriter bw) {
//		bw.Write(Name.ToBytes(64));
//		bw.Write((uint) (Tutorial ? 1 : 0));
//		bw.Write((uint) (GoHome ? 1 : 0));
//	}

//	public override string ToString() {
//		var ret = "struct EnterWorld {\n";
//		ret += "\tName = ";
//		try {
//			ret += $"{ Indentify(Name) },\n";
//		} catch(NullReferenceException) {
//			ret += "!!NULL!!\n";
//		}
//		ret += "\tTutorial = ";
//		try {
//			ret += $"{ Indentify(Tutorial) },\n";
//		} catch(NullReferenceException) {
//			ret += "!!NULL!!\n";
//		}
//		ret += "\tGoHome = ";
//		try {
//			ret += $"{ Indentify(GoHome) }\n";
//		} catch(NullReferenceException) {
//			ret += "!!NULL!!\n";
//		}
//		return ret + "}";
//	}
//}

namespace EQProtocol.Streams.World.Packets
{
    /// <summary>
    /// Represents the EnterWorld packet structure for EverQuest network communication.
    /// </summary>
    public struct EnterWorld : IEQStruct
    {
        /// <summary>
        /// The name of the character entering the world.
        /// </summary>
        public string Name;

        /// <summary>
        /// Indicates whether the player should enter the tutorial.
        /// </summary>
        public bool Tutorial;

        /// <summary>
        /// Indicates whether the player should go home upon entering the world.
        /// </summary>
        public bool GoHome;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterWorld"/> struct with the specified values.
        /// </summary>
        /// <param name="Name">The character name.</param>
        /// <param name="Tutorial">Whether to enter the tutorial.</param>
        /// <param name="GoHome">Whether to go home.</param>
        public EnterWorld(string Name, bool Tutorial, bool GoHome) : this()
        {
            this.Name = Name;
            this.Tutorial = Tutorial;
            this.GoHome = GoHome;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterWorld"/> struct from a byte array.
        /// </summary>
        /// <param name="data">The byte array containing the packet data.</param>
        /// <param name="offset">The offset in the array to start reading from.</param>
        public EnterWorld(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterWorld"/> struct from a <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br">The binary reader containing the packet data.</param>
        public EnterWorld(BinaryReader br) : this()
        {
            Unpack(br);
        }

        /// <summary>
        /// Unpacks the packet data from a byte array.
        /// </summary>
        /// <param name="data">The byte array containing the packet data.</param>
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
        /// Unpacks the packet data from a <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="br">The binary reader containing the packet data.</param>
        public void Unpack(BinaryReader br)
        {
            try
            {
                Name = br.ReadString(64);
                Tutorial = br.ReadUInt32() != 0;
                GoHome = br.ReadUInt32() != 0;
            }
            catch (Exception ex)
            {
                // Looks like this packet has multiple formats and can be sent empty. Looking at the server code,
                // it seems that if the player is entering from a different zone, the packet is empty.
                // world/client.cpp
                // void Client::SendEnterWorld(std::string name)
                // {
                //      std::string live_name {};
                //      if (is_player_zoning)
                //      {
                //          live_name = database.GetLiveChar(GetAccountID());
                //          if (database.GetAccountIDByChar(live_name) != GetAccountID())
                //          {
                //              eqs->Close();
                //              return;
                //          }
                //          LogInfo("Zoning with live_name [{}] account_id [{}]", live_name, GetAccountID());
                //      }
                //      if (!is_player_zoning && RuleB(World, EnableAutoLogin))
                //      {
                //          live_name = AccountRepository::GetAutoLoginCharacterNameByAccountID(database, GetAccountID());
                //          LogInfo("Attempting to auto login with live_name [{}] account_id [{}]", live_name, GetAccountID());
                //      }
                //      LogInfo("Enter World with live_name [{}] account_id [{}]", live_name, GetAccountID());
                //      auto outapp = new EQApplicationPacket(OP_EnterWorld, live_name.length() + 1);
                //      memcpy(outapp->pBuffer, live_name.c_str(), live_name.length() + 1);
                //      QueuePacket(outapp);
                //      safe_delete(outapp);
                // }
            }
        }

        /// <summary>
        /// Packs the current structure into a byte array.
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
        /// Packs the current structure into a <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="bw">The binary writer to write the packed data to.</param>
        public void Pack(BinaryWriter bw)
        {
            bw.Write(Name.ToBytes(64));
            bw.Write((uint)(Tutorial ? 1 : 0));
            bw.Write((uint)(GoHome ? 1 : 0));
        }

        /// <summary>
        /// Returns a string representation of the <see cref="EnterWorld"/> structure.
        /// </summary>
        /// <returns>A string describing the structure.</returns>
        public override string ToString()
        {
            var ret = "struct EnterWorld {\n";
            ret += "\tName = ";
            try
            {
                ret += $"{Indentify(Name)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tTutorial = ";
            try
            {
                ret += $"{Indentify(Tutorial)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGoHome = ";
            try
            {
                ret += $"{Indentify(GoHome)}\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}