using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AdventureFinish_Struct{
// uint32 win_lose;//Cofruben: 00 is a lose,01 is win.
// uint32 points;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the AdventureFinish packet structure for EverQuest network communication.
	/// </summary>
	public struct AdventureFinish : IEQStruct {
		/// <summary>
		/// Gets or sets the winlose value.
		/// </summary>
		public uint WinLose { get; set; }

		/// <summary>
		/// Gets or sets the points value.
		/// </summary>
		public uint Points { get; set; }

		/// <summary>
		/// Initializes a new instance of the AdventureFinish struct with specified field values.
		/// </summary>
		/// <param name="win_lose">The winlose value.</param>
		/// <param name="points">The points value.</param>
		public AdventureFinish(uint win_lose, uint points) : this() {
			WinLose = win_lose;
			Points = points;
		}

		/// <summary>
		/// Initializes a new instance of the AdventureFinish struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public AdventureFinish(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the AdventureFinish struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public AdventureFinish(BinaryReader br) : this() {
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
			WinLose = br.ReadUInt32();
			Points = br.ReadUInt32();
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
			bw.Write(WinLose);
			bw.Write(Points);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct AdventureFinish {\n";
			ret += "	WinLose = ";
			try {
				ret += $"{ Indentify(WinLose) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Points = ";
			try {
				ret += $"{ Indentify(Points) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}