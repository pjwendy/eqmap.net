using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GroupInvite_Struct {
// /*0000*/	char	invitee_name[64];
// /*0064*/	char	inviter_name[64];
// /*0128*/	uint32	unknown0128;
// /*0132*/	uint32	unknown0132;
// /*0136*/	uint32	unknown0136;
// /*0140*/	uint32	unknown0140;
// /*0144*/	uint32	unknown0144;
// /*0148*/
// };

// ENCODE/DECODE Section:
// DECODE(OP_GroupInvite)
// {
// //EQApplicationPacket *in = __packet;
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] Received incoming OP_GroupInvite");
// //Log.Hex(Logs::Netcode, in->pBuffer, in->size);
// DECODE_LENGTH_EXACT(structs::GroupInvite_Struct);
// SETUP_DIRECT_DECODE(GroupGeneric_Struct, structs::GroupInvite_Struct);
// 
// memcpy(emu->name1, eq->invitee_name, sizeof(emu->name1));
// memcpy(emu->name2, eq->inviter_name, sizeof(emu->name2));
// 
// FINISH_DIRECT_DECODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GroupInvite packet structure for EverQuest network communication.
	/// </summary>
	public struct GroupInvite : IEQStruct {
		/// <summary>
		/// Gets or sets the inviteename value.
		/// </summary>
		public byte[] InviteeName { get; set; }

		/// <summary>
		/// Gets or sets the invitername value.
		/// </summary>
		public byte[] InviterName { get; set; }

		/// <summary>
		/// Initializes a new instance of the GroupInvite struct with specified field values.
		/// </summary>
		/// <param name="invitee_name">The inviteename value.</param>
		/// <param name="inviter_name">The invitername value.</param>
		public GroupInvite(byte[] invitee_name, byte[] inviter_name) : this() {
			InviteeName = invitee_name;
			InviterName = inviter_name;
		}

		/// <summary>
		/// Initializes a new instance of the GroupInvite struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GroupInvite(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GroupInvite struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GroupInvite(BinaryReader br) : this() {
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
			// TODO: Array reading for InviteeName - implement based on actual array size
			// InviteeName = new byte[size];
			// TODO: Array reading for InviterName - implement based on actual array size
			// InviterName = new byte[size];
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
			// TODO: Array writing for InviteeName - implement based on actual array size
			// foreach(var item in InviteeName) bw.Write(item);
			// TODO: Array writing for InviterName - implement based on actual array size
			// foreach(var item in InviterName) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GroupInvite {\n";
			ret += "	InviteeName = ";
			try {
				ret += $"{ Indentify(InviteeName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	InviterName = ";
			try {
				ret += $"{ Indentify(InviterName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}