using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BeginCast_Struct
// {
// // len = 8
// /*004*/	uint16	caster_id;
// /*006*/	uint16	spell_id;
// /*016*/	uint32	cast_time;		// in miliseconds
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the BeginCast packet structure for EverQuest network communication.
	/// </summary>
	public struct BeginCast : IEQStruct {
		/// <summary>
		/// Gets or sets the casterid value.
		/// </summary>
		public ushort CasterId { get; set; }

		/// <summary>
		/// Gets or sets the spellid value.
		/// </summary>
		public ushort SpellId { get; set; }

		/// <summary>
		/// Gets or sets the casttime value.
		/// </summary>
		public uint CastTime { get; set; }

		/// <summary>
		/// Initializes a new instance of the BeginCast struct with specified field values.
		/// </summary>
		/// <param name="caster_id">The casterid value.</param>
		/// <param name="spell_id">The spellid value.</param>
		/// <param name="cast_time">The casttime value.</param>
		public BeginCast(ushort caster_id, ushort spell_id, uint cast_time) : this() {
			CasterId = caster_id;
			SpellId = spell_id;
			CastTime = cast_time;
		}

		/// <summary>
		/// Initializes a new instance of the BeginCast struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public BeginCast(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the BeginCast struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public BeginCast(BinaryReader br) : this() {
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
			CasterId = br.ReadUInt16();
			SpellId = br.ReadUInt16();
			CastTime = br.ReadUInt32();
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
			bw.Write(CasterId);
			bw.Write(SpellId);
			bw.Write(CastTime);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct BeginCast {\n";
			ret += "	CasterId = ";
			try {
				ret += $"{ Indentify(CasterId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpellId = ";
			try {
				ret += $"{ Indentify(SpellId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CastTime = ";
			try {
				ret += $"{ Indentify(CastTime) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}