using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BuffIcon_Struct {
// /*000*/ uint32 entity_id;
// /*004*/ uint32 unknown004;
// /*008*/ uint8  all_buffs; // 1 when updating all buffs, 0 when doing one
// /*009*/ uint16 count;
// /*011*/ BuffIconEntry_Struct entires[0];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_BuffCreate)
// {
// SETUP_VAR_ENCODE(BuffIcon_Struct);
// 
// uint32 sz = 12 + (17 * emu->count) + emu->name_lengths; // 17 includes nullterm
// __packet->size = sz;
// __packet->pBuffer = new unsigned char[sz];
// memset(__packet->pBuffer, 0, sz);
// 
// __packet->WriteUInt32(emu->entity_id);
// __packet->WriteUInt32(emu->tic_timer);
// __packet->WriteUInt8(emu->all_buffs); // 1 = all buffs, 0 = 1 buff
// __packet->WriteUInt16(emu->count);
// 
// for (int i = 0; i < emu->count; ++i)
// {
// __packet->WriteUInt32(emu->type == 0 ? ServerToUFBuffSlot(emu->entries[i].buff_slot) : emu->entries[i].buff_slot);
// __packet->WriteUInt32(emu->entries[i].spell_id);
// __packet->WriteUInt32(emu->entries[i].tics_remaining);
// __packet->WriteUInt32(emu->entries[i].num_hits);
// __packet->WriteString(emu->entries[i].caster);
// }
// __packet->WriteUInt8(emu->type);
// 
// FINISH_ENCODE();
// /*
// uint32 write_var32 = 60;
// uint8 write_var8 = 1;
// ss.write((const char*)&emu->entity_id, sizeof(uint32));
// ss.write((const char*)&write_var32, sizeof(uint32));
// ss.write((const char*)&write_var8, sizeof(uint8));
// ss.write((const char*)&emu->count, sizeof(uint16));
// write_var32 = 0;
// write_var8 = 0;
// for(uint16 i = 0; i < emu->count; ++i)
// {
// if(emu->entries[i].buff_slot >= 25 && emu->entries[i].buff_slot < 37)
// {
// emu->entries[i].buff_slot += 5;
// }
// else if(emu->entries[i].buff_slot >= 37)
// {
// emu->entries[i].buff_slot += 14;
// }
// ss.write((const char*)&emu->entries[i].buff_slot, sizeof(uint32));
// ss.write((const char*)&emu->entries[i].spell_id, sizeof(uint32));
// ss.write((const char*)&emu->entries[i].tics_remaining, sizeof(uint32));
// ss.write((const char*)&write_var32, sizeof(uint32));
// ss.write((const char*)&write_var8, sizeof(uint8));
// }
// ss.write((const char*)&write_var8, sizeof(uint8));
// */
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the BuffCreate packet structure for EverQuest network communication.
	/// </summary>
	public struct BuffCreate : IEQStruct {
		/// <summary>
		/// Gets or sets the entityid value.
		/// </summary>
		public uint EntityId { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public uint Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the allbuffs value.
		/// </summary>
		public byte AllBuffs { get; set; }

		/// <summary>
		/// Gets or sets the count value.
		/// </summary>
		public ushort Count { get; set; }

		/// <summary>
		/// Gets or sets the entires value.
		/// </summary>
		public uint Entires { get; set; }

		/// <summary>
		/// Initializes a new instance of the BuffCreate struct with specified field values.
		/// </summary>
		/// <param name="entity_id">The entityid value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="all_buffs">The allbuffs value.</param>
		/// <param name="count">The count value.</param>
		/// <param name="entires">The entires value.</param>
		public BuffCreate(uint entity_id, uint unknown004, byte all_buffs, ushort count, uint entires) : this() {
			EntityId = entity_id;
			Unknown004 = unknown004;
			AllBuffs = all_buffs;
			Count = count;
			Entires = entires;
		}

		/// <summary>
		/// Initializes a new instance of the BuffCreate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public BuffCreate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the BuffCreate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public BuffCreate(BinaryReader br) : this() {
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
			EntityId = br.ReadUInt32();
			Unknown004 = br.ReadUInt32();
			AllBuffs = br.ReadByte();
			Count = br.ReadUInt16();
			Entires = br.ReadUInt32();
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
			bw.Write(EntityId);
			bw.Write(Unknown004);
			bw.Write(AllBuffs);
			bw.Write(Count);
			bw.Write(Entires);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct BuffCreate {\n";
			ret += "	EntityId = ";
			try {
				ret += $"{ Indentify(EntityId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AllBuffs = ";
			try {
				ret += $"{ Indentify(AllBuffs) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Count = ";
			try {
				ret += $"{ Indentify(Count) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Entires = ";
			try {
				ret += $"{ Indentify(Entires) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}