using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AugmentInfo_Struct
// {
// /*000*/ uint32	itemid;			// id of the solvent needed
// /*004*/ uint32	window;			// window to display the information in
// /*008*/ char	augment_info[64];	// total packet length 76, all the rest were always 00
// /*072*/ uint32	unknown072;
// };

// ENCODE/DECODE Section:
// DECODE(OP_AugmentInfo)
// {
// DECODE_LENGTH_EXACT(structs::AugmentInfo_Struct);
// SETUP_DIRECT_DECODE(AugmentInfo_Struct, structs::AugmentInfo_Struct);
// 
// IN(itemid);
// IN(window);
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AugmentInfo packet structure for EverQuest network communication.
	/// </summary>
	public struct AugmentInfo : IEQStruct {
		/// <summary>
		/// Gets or sets the itemid value.
		/// </summary>
		public uint Itemid { get; set; }

		/// <summary>
		/// Gets or sets the window value.
		/// </summary>
		public uint Window { get; set; }

		/// <summary>
		/// Gets or sets the augmentinfovalue value.
		/// </summary>
		public byte[] AugmentInfoValue { get; set; }

		/// <summary>
		/// Gets or sets the unknown072 value.
		/// </summary>
		public uint Unknown072 { get; set; }

		/// <summary>
		/// Initializes a new instance of the AugmentInfo struct with specified field values.
		/// </summary>
		/// <param name="itemid">The itemid value.</param>
		/// <param name="window">The window value.</param>
		/// <param name="augment_info">The augmentinfovalue value.</param>
		/// <param name="unknown072">The unknown072 value.</param>
		public AugmentInfo(uint itemid, uint window, byte[] augment_info, uint unknown072) : this() {
			Itemid = itemid;
			Window = window;
			AugmentInfoValue = augment_info;
			Unknown072 = unknown072;
		}

		/// <summary>
		/// Initializes a new instance of the AugmentInfo struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AugmentInfo(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AugmentInfo struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AugmentInfo(BinaryReader br) : this() {
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
			Itemid = br.ReadUInt32();
			Window = br.ReadUInt32();
			// TODO: Array reading for AugmentInfoValue - implement based on actual array size
			// AugmentInfoValue = new byte[size];
			Unknown072 = br.ReadUInt32();
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
			bw.Write(Itemid);
			bw.Write(Window);
			// TODO: Array writing for AugmentInfoValue - implement based on actual array size
			// foreach(var item in AugmentInfoValue) bw.Write(item);
			bw.Write(Unknown072);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AugmentInfo {\n";
			ret += "	Itemid = ";
			try {
				ret += $"{ Indentify(Itemid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Window = ";
			try {
				ret += $"{ Indentify(Window) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AugmentInfoValue = ";
			try {
				ret += $"{ Indentify(AugmentInfoValue) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown072 = ";
			try {
				ret += $"{ Indentify(Unknown072) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}