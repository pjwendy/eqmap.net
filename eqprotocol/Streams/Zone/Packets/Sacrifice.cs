using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Sacrifice_Struct {
// /*000*/	uint32	CasterID;
// /*004*/	uint32	TargetID;
// /*008*/	uint32	Confirm;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Sacrifice packet structure for EverQuest network communication.
	/// </summary>
	public struct Sacrifice : IEQStruct {
		/// <summary>
		/// Gets or sets the casterid value.
		/// </summary>
		public uint Casterid { get; set; }

		/// <summary>
		/// Gets or sets the targetid value.
		/// </summary>
		public uint Targetid { get; set; }

		/// <summary>
		/// Gets or sets the confirm value.
		/// </summary>
		public uint Confirm { get; set; }

		/// <summary>
		/// Initializes a new instance of the Sacrifice struct with specified field values.
		/// </summary>
		/// <param name="CasterID">The casterid value.</param>
		/// <param name="TargetID">The targetid value.</param>
		/// <param name="Confirm">The confirm value.</param>
		public Sacrifice(uint CasterID, uint TargetID, uint Confirm) : this() {
			Casterid = CasterID;
			Targetid = TargetID;
			Confirm = Confirm;
		}

		/// <summary>
		/// Initializes a new instance of the Sacrifice struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Sacrifice(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Sacrifice struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Sacrifice(BinaryReader br) : this() {
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
			Casterid = br.ReadUInt32();
			Targetid = br.ReadUInt32();
			Confirm = br.ReadUInt32();
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
			bw.Write(Casterid);
			bw.Write(Targetid);
			bw.Write(Confirm);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Sacrifice {\n";
			ret += "	Casterid = ";
			try {
				ret += $"{ Indentify(Casterid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Targetid = ";
			try {
				ret += $"{ Indentify(Targetid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Confirm = ";
			try {
				ret += $"{ Indentify(Confirm) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}