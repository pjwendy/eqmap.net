using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildInviteAccept_Struct {
// char inviter[64];
// char newmember[64];
// uint32 response;
// uint32 guildeqid;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GuildInviteAccept packet structure for EverQuest network communication.
	/// </summary>
	public struct GuildInviteAccept : IEQStruct {
		/// <summary>
		/// Gets or sets the inviter value.
		/// </summary>
		public byte[] Inviter { get; set; }

		/// <summary>
		/// Gets or sets the newmember value.
		/// </summary>
		public byte[] Newmember { get; set; }

		/// <summary>
		/// Gets or sets the response value.
		/// </summary>
		public uint Response { get; set; }

		/// <summary>
		/// Gets or sets the guildeqid value.
		/// </summary>
		public uint Guildeqid { get; set; }

		/// <summary>
		/// Initializes a new instance of the GuildInviteAccept struct with specified field values.
		/// </summary>
		/// <param name="inviter">The inviter value.</param>
		/// <param name="newmember">The newmember value.</param>
		/// <param name="response">The response value.</param>
		/// <param name="guildeqid">The guildeqid value.</param>
		public GuildInviteAccept(byte[] inviter, byte[] newmember, uint response, uint guildeqid) : this() {
			Inviter = inviter;
			Newmember = newmember;
			Response = response;
			Guildeqid = guildeqid;
		}

		/// <summary>
		/// Initializes a new instance of the GuildInviteAccept struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GuildInviteAccept(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GuildInviteAccept struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GuildInviteAccept(BinaryReader br) : this() {
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
			// TODO: Array reading for Inviter - implement based on actual array size
			// Inviter = new byte[size];
			// TODO: Array reading for Newmember - implement based on actual array size
			// Newmember = new byte[size];
			Response = br.ReadUInt32();
			Guildeqid = br.ReadUInt32();
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
			// TODO: Array writing for Inviter - implement based on actual array size
			// foreach(var item in Inviter) bw.Write(item);
			// TODO: Array writing for Newmember - implement based on actual array size
			// foreach(var item in Newmember) bw.Write(item);
			bw.Write(Response);
			bw.Write(Guildeqid);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GuildInviteAccept {\n";
			ret += "	Inviter = ";
			try {
				ret += $"{ Indentify(Inviter) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Newmember = ";
			try {
				ret += $"{ Indentify(Newmember) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Response = ";
			try {
				ret += $"{ Indentify(Response) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Guildeqid = ";
			try {
				ret += $"{ Indentify(Guildeqid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}