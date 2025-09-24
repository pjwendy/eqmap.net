using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct RecipeReply_Struct {
// uint32 object_type;
// uint32 some_id;	 //same as in favorites
// uint32 component_count;
// uint32 recipe_id;
// uint32 trivial;
// char recipe_name[64];
// /*84*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the RecipeReply packet structure for EverQuest network communication.
	/// </summary>
	public struct RecipeReply : IEQStruct {
		/// <summary>
		/// Gets or sets the objecttype value.
		/// </summary>
		public uint ObjectType { get; set; }

		/// <summary>
		/// Gets or sets the someid value.
		/// </summary>
		public uint SomeId { get; set; }

		/// <summary>
		/// Gets or sets the componentcount value.
		/// </summary>
		public uint ComponentCount { get; set; }

		/// <summary>
		/// Gets or sets the recipeid value.
		/// </summary>
		public uint RecipeId { get; set; }

		/// <summary>
		/// Gets or sets the trivial value.
		/// </summary>
		public uint Trivial { get; set; }

		/// <summary>
		/// Gets or sets the recipename value.
		/// </summary>
		public byte[] RecipeName { get; set; }

		/// <summary>
		/// Initializes a new instance of the RecipeReply struct with specified field values.
		/// </summary>
		/// <param name="object_type">The objecttype value.</param>
		/// <param name="some_id">The someid value.</param>
		/// <param name="component_count">The componentcount value.</param>
		/// <param name="recipe_id">The recipeid value.</param>
		/// <param name="trivial">The trivial value.</param>
		/// <param name="recipe_name">The recipename value.</param>
		public RecipeReply(uint object_type, uint some_id, uint component_count, uint recipe_id, uint trivial, byte[] recipe_name) : this() {
			ObjectType = object_type;
			SomeId = some_id;
			ComponentCount = component_count;
			RecipeId = recipe_id;
			Trivial = trivial;
			RecipeName = recipe_name;
		}

		/// <summary>
		/// Initializes a new instance of the RecipeReply struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public RecipeReply(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the RecipeReply struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public RecipeReply(BinaryReader br) : this() {
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
			ComponentCount = br.ReadUInt32();
			RecipeId = br.ReadUInt32();
			Trivial = br.ReadUInt32();
			// TODO: Array reading for RecipeName - implement based on actual array size
			// RecipeName = new byte[size];
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
			bw.Write(ComponentCount);
			bw.Write(RecipeId);
			bw.Write(Trivial);
			// TODO: Array writing for RecipeName - implement based on actual array size
			// foreach(var item in RecipeName) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct RecipeReply {\n";
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
			ret += "	ComponentCount = ";
			try {
				ret += $"{ Indentify(ComponentCount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	RecipeId = ";
			try {
				ret += $"{ Indentify(RecipeId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Trivial = ";
			try {
				ret += $"{ Indentify(Trivial) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	RecipeName = ";
			try {
				ret += $"{ Indentify(RecipeName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}