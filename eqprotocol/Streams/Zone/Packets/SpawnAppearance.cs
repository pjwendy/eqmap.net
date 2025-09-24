using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct SpawnAppearance_Struct
// {
// /*0000*/ uint16 spawn_id;          // ID of the spawn
// /*0002*/ uint16 type;              // Values associated with the type
// /*0004*/ uint32 parameter;         // Type of data sent
// /*0008*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_SpawnAppearance)
// {
// ENCODE_LENGTH_EXACT(SpawnAppearance_Struct);
// SETUP_DIRECT_ENCODE(SpawnAppearance_Struct, structs::SpawnAppearance_Struct);
// 
// OUT(spawn_id);
// OUT(type);
// OUT(parameter);
// switch (emu->type) {
// case AppearanceType::GuildRank: {
// //Translate new ranks to old values* /
// switch (emu->parameter) {
// case GUILD_SENIOR_MEMBER:
// case GUILD_MEMBER:
// case GUILD_JUNIOR_MEMBER:
// case GUILD_INITIATE:
// case GUILD_RECRUIT: {
// eq->parameter = GUILD_MEMBER_TI;
// break;
// }
// case GUILD_OFFICER:
// case GUILD_SENIOR_OFFICER: {
// eq->parameter = GUILD_OFFICER_TI;
// break;
// }
// case GUILD_LEADER: {
// eq->parameter = GUILD_LEADER_TI;
// break;
// }
// default: {
// eq->parameter = GUILD_RANK_NONE_TI;
// break;
// }
// }
// break;
// }
// case AppearanceType::GuildShow: {
// FAIL_ENCODE();
// return;
// }
// default: {
// break;
// }
// }
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SpawnAppearance packet structure for EverQuest network communication.
	/// </summary>
	public struct SpawnAppearance : IEQStruct {
		/// <summary>
		/// Gets or sets the spawnid value.
		/// </summary>
		public ushort SpawnId { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public ushort Type { get; set; }

		/// <summary>
		/// Gets or sets the parameter value.
		/// </summary>
		public uint Parameter { get; set; }

		/// <summary>
		/// Initializes a new instance of the SpawnAppearance struct with specified field values.
		/// </summary>
		/// <param name="spawn_id">The spawnid value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="parameter">The parameter value.</param>
		public SpawnAppearance(ushort spawn_id, ushort type, uint parameter) : this() {
			SpawnId = spawn_id;
			Type = type;
			Parameter = parameter;
		}

		/// <summary>
		/// Initializes a new instance of the SpawnAppearance struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SpawnAppearance(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SpawnAppearance struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SpawnAppearance(BinaryReader br) : this() {
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
			SpawnId = br.ReadUInt16();
			Type = br.ReadUInt16();
			Parameter = br.ReadUInt32();
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
			bw.Write(SpawnId);
			bw.Write(Type);
			bw.Write(Parameter);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SpawnAppearance {\n";
			ret += "	SpawnId = ";
			try {
				ret += $"{ Indentify(SpawnId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Parameter = ";
			try {
				ret += $"{ Indentify(Parameter) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}