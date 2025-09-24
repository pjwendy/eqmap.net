using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BookRequest_Struct {
// /*0000*/ uint32 window;       // where to display the text (0xFFFFFFFF means new window).
// /*0004*/ uint32 invslot;      // The inventory slot the book is in
// /*0008*/ uint32 type;         // 0 = Scroll, 1 = Book, 2 = Item Info. Possibly others
// /*0012*/ uint32 target_id;
// /*0016*/ uint8 can_cast;
// /*0017*/ uint8 can_scribe;
// /*0018*/ char txtfile[8194];
// };

// ENCODE/DECODE Section:
// DECODE(OP_ReadBook)
// {
// DECODE_LENGTH_EXACT(structs::BookRequest_Struct);
// SETUP_DIRECT_DECODE(BookRequest_Struct, structs::BookRequest_Struct);
// 
// IN(type);
// emu->invslot = static_cast<int16_t>(UFToServerSlot(eq->invslot));
// IN(target_id);
// emu->window = (uint8)eq->window;
// strn0cpy(emu->txtfile, eq->txtfile, sizeof(emu->txtfile));
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the ReadBook packet structure for EverQuest network communication.
	/// </summary>
	public struct ReadBook : IEQStruct {
		/// <summary>
		/// Gets or sets the window value.
		/// </summary>
		public uint Window { get; set; }

		/// <summary>
		/// Gets or sets the invslot value.
		/// </summary>
		public uint Invslot { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Gets or sets the targetid value.
		/// </summary>
		public uint TargetId { get; set; }

		/// <summary>
		/// Gets or sets the cancast value.
		/// </summary>
		public byte CanCast { get; set; }

		/// <summary>
		/// Gets or sets the canscribe value.
		/// </summary>
		public byte CanScribe { get; set; }

		/// <summary>
		/// Gets or sets the txtfile value.
		/// </summary>
		public byte[] Txtfile { get; set; }

		/// <summary>
		/// Initializes a new instance of the ReadBook struct with specified field values.
		/// </summary>
		/// <param name="window">The window value.</param>
		/// <param name="invslot">The invslot value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="target_id">The targetid value.</param>
		/// <param name="can_cast">The cancast value.</param>
		/// <param name="can_scribe">The canscribe value.</param>
		/// <param name="txtfile">The txtfile value.</param>
		public ReadBook(uint window, uint invslot, uint type, uint target_id, byte can_cast, byte can_scribe, byte[] txtfile) : this() {
			Window = window;
			Invslot = invslot;
			Type = type;
			TargetId = target_id;
			CanCast = can_cast;
			CanScribe = can_scribe;
			Txtfile = txtfile;
		}

		/// <summary>
		/// Initializes a new instance of the ReadBook struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public ReadBook(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the ReadBook struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public ReadBook(BinaryReader br) : this() {
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
			Window = br.ReadUInt32();
			Invslot = br.ReadUInt32();
			Type = br.ReadUInt32();
			TargetId = br.ReadUInt32();
			CanCast = br.ReadByte();
			CanScribe = br.ReadByte();
			// TODO: Array reading for Txtfile - implement based on actual array size
			// Txtfile = new byte[size];
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
			bw.Write(Window);
			bw.Write(Invslot);
			bw.Write(Type);
			bw.Write(TargetId);
			bw.Write(CanCast);
			bw.Write(CanScribe);
			// TODO: Array writing for Txtfile - implement based on actual array size
			// foreach(var item in Txtfile) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct ReadBook {\n";
			ret += "	Window = ";
			try {
				ret += $"{ Indentify(Window) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Invslot = ";
			try {
				ret += $"{ Indentify(Invslot) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TargetId = ";
			try {
				ret += $"{ Indentify(TargetId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CanCast = ";
			try {
				ret += $"{ Indentify(CanCast) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CanScribe = ";
			try {
				ret += $"{ Indentify(CanScribe) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Txtfile = ";
			try {
				ret += $"{ Indentify(Txtfile) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}