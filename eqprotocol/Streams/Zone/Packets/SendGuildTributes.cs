using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildTributeAbility_Struct {
// uint32	guild_id;
// TributeAbility_Struct ability;
// };

// ENCODE/DECODE Section:
// ENCODE(OP_SendGuildTributes)
// {
// ENCODE_LENGTH_ATLEAST(structs::GuildTributeAbility_Struct)
// SETUP_VAR_ENCODE(GuildTributeAbility_Struct)
// ALLOC_VAR_ENCODE(structs::GuildTributeAbility_Struct, sizeof(GuildTributeAbility_Struct) + strlen(emu->ability.name))
// 
// eq->guild_id           = emu->guild_id;
// eq->ability.tribute_id = emu->ability.tribute_id;
// eq->ability.tier_count = emu->ability.tier_count;
// strncpy(eq->ability.name, emu->ability.name, strlen(emu->ability.name));
// for (int i = 0; i < ntohl(emu->ability.tier_count); i++) {
// eq->ability.tiers[i].cost            = emu->ability.tiers[i].cost;
// eq->ability.tiers[i].level           = emu->ability.tiers[i].level;
// eq->ability.tiers[i].tribute_item_id = emu->ability.tiers[i].tribute_item_id;
// }
// FINISH_ENCODE()
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SendGuildTributes packet structure for EverQuest network communication.
	/// </summary>
	public struct SendGuildTributes : IEQStruct {
		/// <summary>
		/// Gets or sets the guildid value.
		/// </summary>
		public uint GuildId { get; set; }

		/// <summary>
		/// Gets or sets the ability value.
		/// </summary>
		public uint Ability { get; set; }

		/// <summary>
		/// Initializes a new instance of the SendGuildTributes struct with specified field values.
		/// </summary>
		/// <param name="guild_id">The guildid value.</param>
		/// <param name="ability">The ability value.</param>
		public SendGuildTributes(uint guild_id, uint ability) : this() {
			GuildId = guild_id;
			Ability = ability;
		}

		/// <summary>
		/// Initializes a new instance of the SendGuildTributes struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SendGuildTributes(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SendGuildTributes struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SendGuildTributes(BinaryReader br) : this() {
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
			GuildId = br.ReadUInt32();
			Ability = br.ReadUInt32();
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
			bw.Write(GuildId);
			bw.Write(Ability);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SendGuildTributes {\n";
			ret += "	GuildId = ";
			try {
				ret += $"{ Indentify(GuildId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Ability = ";
			try {
				ret += $"{ Indentify(Ability) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}