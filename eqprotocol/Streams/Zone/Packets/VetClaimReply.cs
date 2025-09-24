using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct VeteranClaim
// {
// /*000*/	char name[64]; //name + other data
// /*064*/	uint32 claim_id;
// /*068*/	uint32 unknown068;
// /*072*/	uint32 action;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the VetClaimReply packet structure for EverQuest network communication.
	/// </summary>
	public struct VetClaimReply : IEQStruct {
		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte[] Name { get; set; }

		/// <summary>
		/// Gets or sets the claimid value.
		/// </summary>
		public uint ClaimId { get; set; }

		/// <summary>
		/// Gets or sets the unknown068 value.
		/// </summary>
		public uint Unknown068 { get; set; }

		/// <summary>
		/// Gets or sets the action value.
		/// </summary>
		public uint Action { get; set; }

		/// <summary>
		/// Initializes a new instance of the VetClaimReply struct with specified field values.
		/// </summary>
		/// <param name="name">The name value.</param>
		/// <param name="claim_id">The claimid value.</param>
		/// <param name="unknown068">The unknown068 value.</param>
		/// <param name="action">The action value.</param>
		public VetClaimReply(byte[] name, uint claim_id, uint unknown068, uint action) : this() {
			Name = name;
			ClaimId = claim_id;
			Unknown068 = unknown068;
			Action = action;
		}

		/// <summary>
		/// Initializes a new instance of the VetClaimReply struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public VetClaimReply(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the VetClaimReply struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public VetClaimReply(BinaryReader br) : this() {
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
			// TODO: Array reading for Name - implement based on actual array size
			// Name = new byte[size];
			ClaimId = br.ReadUInt32();
			Unknown068 = br.ReadUInt32();
			Action = br.ReadUInt32();
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
			// TODO: Array writing for Name - implement based on actual array size
			// foreach(var item in Name) bw.Write(item);
			bw.Write(ClaimId);
			bw.Write(Unknown068);
			bw.Write(Action);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct VetClaimReply {\n";
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ClaimId = ";
			try {
				ret += $"{ Indentify(ClaimId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown068 = ";
			try {
				ret += $"{ Indentify(Unknown068) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}