using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TradeskillFavorites_Struct {
// uint32 object_type;
// uint32 some_id;
// uint32 favorite_recipes[500];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the RecipesFavorite packet structure for EverQuest network communication.
	/// </summary>
	public struct RecipesFavorite : IEQStruct {
		/// <summary>
		/// Gets or sets the objecttype value.
		/// </summary>
		public uint ObjectType { get; set; }

		/// <summary>
		/// Gets or sets the someid value.
		/// </summary>
		public uint SomeId { get; set; }

		/// <summary>
		/// Gets or sets the favoriterecipes value.
		/// </summary>
		public uint[] FavoriteRecipes { get; set; }

		/// <summary>
		/// Initializes a new instance of the RecipesFavorite struct with specified field values.
		/// </summary>
		/// <param name="object_type">The objecttype value.</param>
		/// <param name="some_id">The someid value.</param>
		/// <param name="favorite_recipes">The favoriterecipes value.</param>
		public RecipesFavorite(uint object_type, uint some_id, uint[] favorite_recipes) : this() {
			ObjectType = object_type;
			SomeId = some_id;
			FavoriteRecipes = favorite_recipes;
		}

		/// <summary>
		/// Initializes a new instance of the RecipesFavorite struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public RecipesFavorite(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the RecipesFavorite struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public RecipesFavorite(BinaryReader br) : this() {
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
			// TODO: Array reading for FavoriteRecipes - implement based on actual array size
			// FavoriteRecipes = new uint[size];
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
			// TODO: Array writing for FavoriteRecipes - implement based on actual array size
			// foreach(var item in FavoriteRecipes) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct RecipesFavorite {\n";
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
			ret += "	FavoriteRecipes = ";
			try {
				ret += $"{ Indentify(FavoriteRecipes) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}