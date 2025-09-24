using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BookButton_Struct
// {
// /*0000*/ int32 invslot;
// /*0004*/ int32 target_id; // client's target when using the book
// /*0008*/ int32 unused;    // always 0 from button packets
// };

// ENCODE/DECODE Section:
// DECODE(OP_BookButton)
// {
// DECODE_LENGTH_EXACT(structs::BookButton_Struct);
// SETUP_DIRECT_DECODE(BookButton_Struct, structs::BookButton_Struct);
// 
// emu->invslot = static_cast<int16_t>(UFToServerSlot(eq->invslot));
// IN(target_id);
// 
// FINISH_DIRECT_DECODE();
// }

namespace EQProtocol.Streams.Common.UnUsed_Packets {
	/// <summary>
	/// Represents the BookButton packet structure for EverQuest network communication.
	/// </summary>
	public struct BookButton : IEQStruct {
		/// <summary>
		/// Gets or sets the invslot value.
		/// </summary>
		public int Invslot { get; set; }

		/// <summary>
		/// Gets or sets the targetid value.
		/// </summary>
		public int TargetId { get; set; }

		/// <summary>
		/// Gets or sets the unused value.
		/// </summary>
		public int Unused { get; set; }

		/// <summary>
		/// Initializes a new instance of the BookButton struct with specified field values.
		/// </summary>
		/// <param name="invslot">The invslot value.</param>
		/// <param name="target_id">The targetid value.</param>
		/// <param name="unused">The unused value.</param>
		public BookButton(int invslot, int target_id, int unused) : this() {
			Invslot = invslot;
			TargetId = target_id;
			Unused = unused;
		}

		/// <summary>
		/// Initializes a new instance of the BookButton struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public BookButton(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the BookButton struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public BookButton(BinaryReader br) : this() {
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
			Invslot = br.ReadInt32();
			TargetId = br.ReadInt32();
			Unused = br.ReadInt32();
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
			bw.Write(Invslot);
			bw.Write(TargetId);
			bw.Write(Unused);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct BookButton {\n";
			ret += "	Invslot = ";
			try {
				ret += $"{ Indentify(Invslot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TargetId = ";
			try {
				ret += $"{ Indentify(TargetId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unused = ";
			try {
				ret += $"{ Indentify(Unused) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}