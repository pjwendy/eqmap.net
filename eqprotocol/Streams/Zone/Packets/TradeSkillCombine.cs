using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct NewCombine_Struct {
// /*00*/	int16	container_slot;
// /*02*/	int16	guildtribute_slot;
// /*04*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_TradeSkillCombine)
// {
// DECODE_LENGTH_EXACT(structs::NewCombine_Struct);
// SETUP_DIRECT_DECODE(NewCombine_Struct, structs::NewCombine_Struct);
// 
// emu->container_slot = UFToServerSlot(eq->container_slot);
// IN(guildtribute_slot);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TradeSkillCombine packet structure for EverQuest network communication.
	/// </summary>
	public struct TradeSkillCombine : IEQStruct {
		/// <summary>
		/// Gets or sets the containerslot value.
		/// </summary>
		public short ContainerSlot { get; set; }

		/// <summary>
		/// Gets or sets the guildtributeslot value.
		/// </summary>
		public short GuildtributeSlot { get; set; }

		/// <summary>
		/// Initializes a new instance of the TradeSkillCombine struct with specified field values.
		/// </summary>
		/// <param name="container_slot">The containerslot value.</param>
		/// <param name="guildtribute_slot">The guildtributeslot value.</param>
		public TradeSkillCombine(short container_slot, short guildtribute_slot) : this() {
			ContainerSlot = container_slot;
			GuildtributeSlot = guildtribute_slot;
		}

		/// <summary>
		/// Initializes a new instance of the TradeSkillCombine struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TradeSkillCombine(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TradeSkillCombine struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TradeSkillCombine(BinaryReader br) : this() {
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
			ContainerSlot = br.ReadInt16();
			GuildtributeSlot = br.ReadInt16();
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
			bw.Write(ContainerSlot);
			bw.Write(GuildtributeSlot);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TradeSkillCombine {\n";
			ret += "	ContainerSlot = ";
			try {
				ret += $"{ Indentify(ContainerSlot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	GuildtributeSlot = ";
			try {
				ret += $"{ Indentify(GuildtributeSlot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}