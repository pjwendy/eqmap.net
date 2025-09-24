using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct RecipeAutoCombine_Struct {
// uint32 object_type;
// uint32 some_id;
// uint32 unknown1;		//echoed in reply
// uint32 recipe_id;
// uint32 reply_code;		// 93 64 e1 00 (junk) in request
// // 00 00 00 00 in successful reply
// // f5 ff ff ff in 'you dont have all the stuff' reply
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the RecipeDetails packet structure for EverQuest network communication.
	/// </summary>
	public struct RecipeDetails : IEQStruct {
		/// <summary>
		/// Gets or sets the objecttype value.
		/// </summary>
		public uint ObjectType { get; set; }

		/// <summary>
		/// Gets or sets the someid value.
		/// </summary>
		public uint SomeId { get; set; }

		/// <summary>
		/// Gets or sets the unknown1 value.
		/// </summary>
		public uint Unknown1 { get; set; }

		/// <summary>
		/// Gets or sets the recipeid value.
		/// </summary>
		public uint RecipeId { get; set; }

		/// <summary>
		/// Gets or sets the replycode value.
		/// </summary>
		public uint ReplyCode { get; set; }

		/// <summary>
		/// Initializes a new instance of the RecipeDetails struct with specified field values.
		/// </summary>
		/// <param name="object_type">The objecttype value.</param>
		/// <param name="some_id">The someid value.</param>
		/// <param name="unknown1">The unknown1 value.</param>
		/// <param name="recipe_id">The recipeid value.</param>
		/// <param name="reply_code">The replycode value.</param>
		public RecipeDetails(uint object_type, uint some_id, uint unknown1, uint recipe_id, uint reply_code) : this() {
			ObjectType = object_type;
			SomeId = some_id;
			Unknown1 = unknown1;
			RecipeId = recipe_id;
			ReplyCode = reply_code;
		}

		/// <summary>
		/// Initializes a new instance of the RecipeDetails struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public RecipeDetails(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the RecipeDetails struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public RecipeDetails(BinaryReader br) : this() {
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
			ObjectType = br.ReadUInt32();
			SomeId = br.ReadUInt32();
			Unknown1 = br.ReadUInt32();
			RecipeId = br.ReadUInt32();
			ReplyCode = br.ReadUInt32();
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
			bw.Write(ObjectType);
			bw.Write(SomeId);
			bw.Write(Unknown1);
			bw.Write(RecipeId);
			bw.Write(ReplyCode);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct RecipeDetails {\n";
			ret += "	ObjectType = ";
			try {
				ret += $"{ Indentify(ObjectType) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SomeId = ";
			try {
				ret += $"{ Indentify(SomeId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown1 = ";
			try {
				ret += $"{ Indentify(Unknown1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	RecipeId = ";
			try {
				ret += $"{ Indentify(RecipeId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ReplyCode = ";
			try {
				ret += $"{ Indentify(ReplyCode) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}