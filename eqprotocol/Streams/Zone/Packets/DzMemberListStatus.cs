using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DynamicZoneMemberList_Struct
// {
// /*000*/ uint32 client_id;
// /*004*/ uint32 member_count; // number of players in window
// /*008*/ DynamicZoneMemberEntry_Struct members[0]; // variable length
// };

// ENCODE/DECODE Section:
// ENCODE(OP_DzMemberListStatus)
// {
// auto emu = reinterpret_cast<DynamicZoneMemberList_Struct*>((*p)->pBuffer);
// if (emu->member_count == 1)
// {
// ENCODE_FORWARD(OP_DzMemberList);
// }
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DzMemberListStatus packet structure for EverQuest network communication.
	/// </summary>
	public struct DzMemberListStatus : IEQStruct {
		/// <summary>
		/// Gets or sets the clientid value.
		/// </summary>
		public uint ClientId { get; set; }

		/// <summary>
		/// Gets or sets the membercount value.
		/// </summary>
		public uint MemberCount { get; set; }

		/// <summary>
		/// Gets or sets the members value.
		/// </summary>
		public uint Members { get; set; }

		/// <summary>
		/// Initializes a new instance of the DzMemberListStatus struct with specified field values.
		/// </summary>
		/// <param name="client_id">The clientid value.</param>
		/// <param name="member_count">The membercount value.</param>
		/// <param name="members">The members value.</param>
		public DzMemberListStatus(uint client_id, uint member_count, uint members) : this() {
			ClientId = client_id;
			MemberCount = member_count;
			Members = members;
		}

		/// <summary>
		/// Initializes a new instance of the DzMemberListStatus struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DzMemberListStatus(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DzMemberListStatus struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DzMemberListStatus(BinaryReader br) : this() {
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
			ClientId = br.ReadUInt32();
			MemberCount = br.ReadUInt32();
			Members = br.ReadUInt32();
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
			bw.Write(ClientId);
			bw.Write(MemberCount);
			bw.Write(Members);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DzMemberListStatus {\n";
			ret += "	ClientId = ";
			try {
				ret += $"{ Indentify(ClientId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MemberCount = ";
			try {
				ret += $"{ Indentify(MemberCount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Members = ";
			try {
				ret += $"{ Indentify(Members) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}