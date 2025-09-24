using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ExpeditionInvite_Struct
// {
// /*000*/ uint32 client_id;            // unique character id
// /*004*/ uint32 unknown004;
// /*008*/ char   inviter_name[64];
// /*072*/ char   expedition_name[128];
// /*200*/ uint8  swapping;             // 0: adding 1: swapping
// /*201*/ char   swap_name[64];        // if swapping, swap name being removed
// /*265*/ uint8  padding[3];
// /*268*/ uint16 dz_zone_id;           // dz_id zone/instance pair, sent back in reply
// /*270*/ uint16 dz_instance_id;
// };

// ENCODE/DECODE Section:
// ENCODE(OP_DzExpeditionInvite)
// {
// ENCODE_LENGTH_EXACT(ExpeditionInvite_Struct);
// SETUP_DIRECT_ENCODE(ExpeditionInvite_Struct, structs::ExpeditionInvite_Struct);
// 
// OUT(client_id);
// strn0cpy(eq->inviter_name, emu->inviter_name, sizeof(eq->inviter_name));
// strn0cpy(eq->expedition_name, emu->expedition_name, sizeof(eq->expedition_name));
// OUT(swapping);
// strn0cpy(eq->swap_name, emu->swap_name, sizeof(eq->swap_name));
// OUT(dz_zone_id);
// OUT(dz_instance_id);
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DzExpeditionInvite packet structure for EverQuest network communication.
	/// </summary>
	public struct DzExpeditionInvite : IEQStruct {
		/// <summary>
		/// Gets or sets the clientid value.
		/// </summary>
		public uint ClientId { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public uint Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the invitername value.
		/// </summary>
		public byte[] InviterName { get; set; }

		/// <summary>
		/// Gets or sets the expeditionname value.
		/// </summary>
		public byte[] ExpeditionName { get; set; }

		/// <summary>
		/// Gets or sets the swapping value.
		/// </summary>
		public byte Swapping { get; set; }

		/// <summary>
		/// Gets or sets the swapname value.
		/// </summary>
		public byte[] SwapName { get; set; }

		/// <summary>
		/// Gets or sets the padding value.
		/// </summary>
		public byte[] Padding { get; set; }

		/// <summary>
		/// Gets or sets the dzzoneid value.
		/// </summary>
		public ushort DzZoneId { get; set; }

		/// <summary>
		/// Gets or sets the dzinstanceid value.
		/// </summary>
		public ushort DzInstanceId { get; set; }

		/// <summary>
		/// Initializes a new instance of the DzExpeditionInvite struct with specified field values.
		/// </summary>
		/// <param name="client_id">The clientid value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="inviter_name">The invitername value.</param>
		/// <param name="expedition_name">The expeditionname value.</param>
		/// <param name="swapping">The swapping value.</param>
		/// <param name="swap_name">The swapname value.</param>
		/// <param name="padding">The padding value.</param>
		/// <param name="dz_zone_id">The dzzoneid value.</param>
		/// <param name="dz_instance_id">The dzinstanceid value.</param>
		public DzExpeditionInvite(uint client_id, uint unknown004, byte[] inviter_name, byte[] expedition_name, byte swapping, byte[] swap_name, byte[] padding, ushort dz_zone_id, ushort dz_instance_id) : this() {
			ClientId = client_id;
			Unknown004 = unknown004;
			InviterName = inviter_name;
			ExpeditionName = expedition_name;
			Swapping = swapping;
			SwapName = swap_name;
			Padding = padding;
			DzZoneId = dz_zone_id;
			DzInstanceId = dz_instance_id;
		}

		/// <summary>
		/// Initializes a new instance of the DzExpeditionInvite struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DzExpeditionInvite(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DzExpeditionInvite struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DzExpeditionInvite(BinaryReader br) : this() {
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
			Unknown004 = br.ReadUInt32();
			// TODO: Array reading for InviterName - implement based on actual array size
			// InviterName = new byte[size];
			// TODO: Array reading for ExpeditionName - implement based on actual array size
			// ExpeditionName = new byte[size];
			Swapping = br.ReadByte();
			// TODO: Array reading for SwapName - implement based on actual array size
			// SwapName = new byte[size];
			// TODO: Array reading for Padding - implement based on actual array size
			// Padding = new byte[size];
			DzZoneId = br.ReadUInt16();
			DzInstanceId = br.ReadUInt16();
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
			bw.Write(Unknown004);
			// TODO: Array writing for InviterName - implement based on actual array size
			// foreach(var item in InviterName) bw.Write(item);
			// TODO: Array writing for ExpeditionName - implement based on actual array size
			// foreach(var item in ExpeditionName) bw.Write(item);
			bw.Write(Swapping);
			// TODO: Array writing for SwapName - implement based on actual array size
			// foreach(var item in SwapName) bw.Write(item);
			// TODO: Array writing for Padding - implement based on actual array size
			// foreach(var item in Padding) bw.Write(item);
			bw.Write(DzZoneId);
			bw.Write(DzInstanceId);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DzExpeditionInvite {\n";
			ret += "	ClientId = ";
			try {
				ret += $"{ Indentify(ClientId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	InviterName = ";
			try {
				ret += $"{ Indentify(InviterName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ExpeditionName = ";
			try {
				ret += $"{ Indentify(ExpeditionName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Swapping = ";
			try {
				ret += $"{ Indentify(Swapping) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SwapName = ";
			try {
				ret += $"{ Indentify(SwapName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Padding = ";
			try {
				ret += $"{ Indentify(Padding) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DzZoneId = ";
			try {
				ret += $"{ Indentify(DzZoneId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DzInstanceId = ";
			try {
				ret += $"{ Indentify(DzInstanceId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}