using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildMakeLeader_Struct{
// char	requestor[64];
// char	new_leader[64];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GuildLeader packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildLeader : IEQStruct {
		/// <summary>
		/// Gets or sets the requestor value.
		/// </summary>
		public byte[] Requestor { get; set; }

		/// <summary>
		/// Gets or sets the newleader value.
		/// </summary>
		public byte[] NewLeader { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildLeader struct with specified field values.
		/// </summary>
		/// <param name="requestor">The requestor value.</param>
		/// <param name="new_leader">The newleader value.</param>
		public GuildLeader(byte[] requestor, byte[] new_leader) : this() {
			Requestor = requestor;
			NewLeader = new_leader;
		}

		/// <summary>
		/// Initializes a new instance of the GuildLeader struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildLeader(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildLeader struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildLeader(BinaryReader br) : this() {
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
			// TODO: Array reading for Requestor - implement based on actual array size
			// Requestor = new byte[size];
			// TODO: Array reading for NewLeader - implement based on actual array size
			// NewLeader = new byte[size];
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
			// TODO: Array writing for Requestor - implement based on actual array size
			// foreach(var item in Requestor) bw.Write(item);
			// TODO: Array writing for NewLeader - implement based on actual array size
			// foreach(var item in NewLeader) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildLeader {\n";
			ret += "	Requestor = ";
			try {
				ret += $"{ Indentify(Requestor) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NewLeader = ";
			try {
				ret += $"{ Indentify(NewLeader) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}