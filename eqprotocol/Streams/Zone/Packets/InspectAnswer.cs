using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct InspectResponse_Struct{
// /*000*/	uint32 TargetID;
// /*004*/	uint32 playerid;
// /*008*/	char itemnames[23][64];
// /*1480*/uint32 itemicons[23];
// /*1572*/char text[288];	// Max number of chars in Inspect Window appears to be 254
// /*1860*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the InspectAnswer packet structure for EverQuest network communication.
	/// </summary>
	public struct InspectAnswer : IEQStruct {
		/// <summary>
		/// Gets or sets the targetid value.
		/// </summary>
		public uint Targetid { get; set; }

		/// <summary>
		/// Gets or sets the playerid value.
		/// </summary>
		public uint Playerid { get; set; }

		/// <summary>
		/// Gets or sets the itemicons value.
		/// </summary>
		public uint[] Itemicons { get; set; }

		/// <summary>
		/// Gets or sets the text value.
		/// </summary>
		public byte[] Text { get; set; }

		/// <summary>
		/// Initializes a new instance of the InspectAnswer struct with specified field values.
		/// </summary>
		/// <param name="TargetID">The targetid value.</param>
		/// <param name="playerid">The playerid value.</param>
		/// <param name="itemicons">The itemicons value.</param>
		/// <param name="text">The text value.</param>
		public InspectAnswer(uint TargetID, uint playerid, uint[] itemicons, byte[] text) : this() {
			Targetid = TargetID;
			Playerid = playerid;
			Itemicons = itemicons;
			Text = text;
		}

		/// <summary>
		/// Initializes a new instance of the InspectAnswer struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public InspectAnswer(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the InspectAnswer struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public InspectAnswer(BinaryReader br) : this() {
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
			Playerid = br.ReadUInt32();
			// TODO: Array reading for Itemicons - implement based on actual array size
			// Itemicons = new uint[size];
			// TODO: Array reading for Text - implement based on actual array size
			// Text = new byte[size];
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
			bw.Write(Playerid);
			// TODO: Array writing for Itemicons - implement based on actual array size
			// foreach(var item in Itemicons) bw.Write(item);
			// TODO: Array writing for Text - implement based on actual array size
			// foreach(var item in Text) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct InspectAnswer {\n";
			ret += "	Targetid = ";
			try {
				ret += $"{ Indentify(Targetid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Playerid = ";
			try {
				ret += $"{ Indentify(Playerid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Itemicons = ";
			try {
				ret += $"{ Indentify(Itemicons) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Text = ";
			try {
				ret += $"{ Indentify(Text) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}