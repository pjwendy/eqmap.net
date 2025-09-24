using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MarkNPC_Struct
// {
// /*00*/	uint32	TargetID;	// Target EntityID
// /*04*/	uint32	Number;		// Number to mark them with (1, 2 or 3)
// // The following field is for SoD+
// /*08**/	char	Name[64];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MarkRaidNPC packet structure for EverQuest network communication.
	/// </summary>
	public struct MarkRaidNPC : IEQStruct {
		/// <summary>
		/// Gets or sets the targetid value.
		/// </summary>
		public uint Targetid { get; set; }

		/// <summary>
		/// Gets or sets the number value.
		/// </summary>
		public uint Number { get; set; }

		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte[] Name { get; set; }

		/// <summary>
		/// Initializes a new instance of the MarkRaidNPC struct with specified field values.
		/// </summary>
		/// <param name="TargetID">The targetid value.</param>
		/// <param name="Number">The number value.</param>
		/// <param name="Name">The name value.</param>
		public MarkRaidNPC(uint TargetID, uint Number, byte[] Name) : this() {
			Targetid = TargetID;
			Number = Number;
			Name = Name;
		}

		/// <summary>
		/// Initializes a new instance of the MarkRaidNPC struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MarkRaidNPC(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MarkRaidNPC struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MarkRaidNPC(BinaryReader br) : this() {
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
			Targetid = br.ReadUInt32();
			Number = br.ReadUInt32();
			// TODO: Array reading for Name - implement based on actual array size
			// Name = new byte[size];
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
			bw.Write(Targetid);
			bw.Write(Number);
			// TODO: Array writing for Name - implement based on actual array size
			// foreach(var item in Name) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct MarkRaidNPC {\n";
			ret += "	Targetid = ";
			try {
				ret += $"{ Indentify(Targetid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Number = ";
			try {
				ret += $"{ Indentify(Number) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}