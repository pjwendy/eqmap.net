using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ChangeSize_Struct
// {
// /*00*/ uint32 EntityID;
// /*04*/ float Size;
// /*08*/ uint32 Unknown08;	// Observed 0
// /*12*/ float Unknown12;		// Observed 1.0f
// /*16*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ChangeSize packet structure for EverQuest network communication.
	/// </summary>
	public struct ChangeSize : IEQStruct {
		/// <summary>
		/// Gets or sets the entityid value.
		/// </summary>
		public uint Entityid { get; set; }

		/// <summary>
		/// Gets or sets the size value.
		/// </summary>
		public float Size { get; set; }

		/// <summary>
		/// Gets or sets the unknown08 value.
		/// </summary>
		public uint Unknown08 { get; set; }

		/// <summary>
		/// Gets or sets the unknown12 value.
		/// </summary>
		public float Unknown12 { get; set; }

		/// <summary>
		/// Initializes a new instance of the ChangeSize struct with specified field values.
		/// </summary>
		/// <param name="EntityID">The entityid value.</param>
		/// <param name="Size">The size value.</param>
		/// <param name="Unknown08">The unknown08 value.</param>
		/// <param name="Unknown12">The unknown12 value.</param>
		public ChangeSize(uint EntityID, float Size, uint Unknown08, float Unknown12) : this() {
			Entityid = EntityID;
			Size = Size;
			Unknown08 = Unknown08;
			Unknown12 = Unknown12;
		}

		/// <summary>
		/// Initializes a new instance of the ChangeSize struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ChangeSize(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ChangeSize struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ChangeSize(BinaryReader br) : this() {
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
			Entityid = br.ReadUInt32();
			Size = br.ReadSingle();
			Unknown08 = br.ReadUInt32();
			Unknown12 = br.ReadSingle();
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
			bw.Write(Entityid);
			bw.Write(Size);
			bw.Write(Unknown08);
			bw.Write(Unknown12);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ChangeSize {\n";
			ret += "	Entityid = ";
			try {
				ret += $"{ Indentify(Entityid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Size = ";
			try {
				ret += $"{ Indentify(Size) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown08 = ";
			try {
				ret += $"{ Indentify(Unknown08) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown12 = ";
			try {
				ret += $"{ Indentify(Unknown12) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}