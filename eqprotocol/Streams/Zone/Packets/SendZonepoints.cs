using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ZonePoints {
// /*0000*/	uint32	count;
// /*0004*/	struct	ZonePoint_Entry zpe[0]; // Always add one extra to the end after all zonepoints
// //*0xxx*/    uint8     unknown0xxx[24]; //New from SEQ
// };

// ENCODE/DECODE Section:
// ENCODE(OP_SendZonepoints)
// {
// SETUP_VAR_ENCODE(ZonePoints);
// ALLOC_VAR_ENCODE(structs::ZonePoints, sizeof(structs::ZonePoints) + sizeof(structs::ZonePoint_Entry) * (emu->count + 1));
// 
// eq->count = emu->count;
// for (uint32 i = 0; i < emu->count; ++i)
// {
// eq->zpe[i].iterator = emu->zpe[i].iterator;
// eq->zpe[i].x = emu->zpe[i].x;
// eq->zpe[i].y = emu->zpe[i].y;
// eq->zpe[i].z = emu->zpe[i].z;
// eq->zpe[i].heading = emu->zpe[i].heading;
// eq->zpe[i].zoneid = emu->zpe[i].zoneid;
// eq->zpe[i].zoneinstance = emu->zpe[i].zoneinstance;
// }
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the SendZonepoints packet structure for EverQuest network communication.
	/// </summary>
	public struct SendZonepoints : IEQStruct {
		/// <summary>
		/// Gets or sets the count value.
		/// </summary>
		public uint Count { get; set; }

		/// <summary>
		/// Initializes a new instance of the SendZonepoints struct with specified field values.
		/// </summary>
		/// <param name="count">The count value.</param>
		public SendZonepoints(uint count) : this() {
			Count = count;
		}

		/// <summary>
		/// Initializes a new instance of the SendZonepoints struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public SendZonepoints(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the SendZonepoints struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public SendZonepoints(BinaryReader br) : this() {
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
			Count = br.ReadUInt32();
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
			bw.Write(Count);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct SendZonepoints {\n";
			ret += "	Count = ";
			try {
				ret += $"{ Indentify(Count) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}