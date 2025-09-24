using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ItemRecastDelay_Struct {
// /*000*/	uint32	recast_delay;	// in seconds
// /*004*/	uint32	recast_type;
// /*008*/	bool	ignore_casting_requirement; //Ignores recast times allows items to be reset?
// /*012*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace EQProtocol.Streams.Common.UnUsed_Packets {
	/// <summary>
	/// Represents the ItemRecastDelay packet structure for EverQuest network communication.
	/// </summary>
	public struct ItemRecastDelay : IEQStruct {
		/// <summary>
		/// Gets or sets the recastdelay value.
		/// </summary>
		public uint RecastDelay { get; set; }

		/// <summary>
		/// Gets or sets the recasttype value.
		/// </summary>
		public uint RecastType { get; set; }

		/// <summary>
		/// Gets or sets the ignorecastingrequirement value.
		/// </summary>
		public uint IgnoreCastingRequirement { get; set; }

		/// <summary>
		/// Initializes a new instance of the ItemRecastDelay struct with specified field values.
		/// </summary>
		/// <param name="recast_delay">The recastdelay value.</param>
		/// <param name="recast_type">The recasttype value.</param>
		/// <param name="ignore_casting_requirement">The ignorecastingrequirement value.</param>
		public ItemRecastDelay(uint recast_delay, uint recast_type, uint ignore_casting_requirement) : this() {
			RecastDelay = recast_delay;
			RecastType = recast_type;
			IgnoreCastingRequirement = ignore_casting_requirement;
		}

		/// <summary>
		/// Initializes a new instance of the ItemRecastDelay struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ItemRecastDelay(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ItemRecastDelay struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ItemRecastDelay(BinaryReader br) : this() {
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
			RecastDelay = br.ReadUInt32();
			RecastType = br.ReadUInt32();
			IgnoreCastingRequirement = br.ReadUInt32();
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
			bw.Write(RecastDelay);
			bw.Write(RecastType);
			bw.Write(IgnoreCastingRequirement);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ItemRecastDelay {\n";
			ret += "	RecastDelay = ";
			try {
				ret += $"{ Indentify(RecastDelay) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	RecastType = ";
			try {
				ret += $"{ Indentify(RecastType) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	IgnoreCastingRequirement = ";
			try {
				ret += $"{ Indentify(IgnoreCastingRequirement) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}