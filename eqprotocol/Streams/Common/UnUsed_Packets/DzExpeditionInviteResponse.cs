using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ExpeditionInviteResponse_Struct
// {
// /*000*/ uint32 unknown000;
// /*004*/ uint32 unknown004;
// /*008*/ uint16 dz_zone_id;     // dz_id pair sent in invite
// /*010*/ uint16 dz_instance_id;
// /*012*/ uint8  accepted;       // 0: declined 1: accepted
// /*013*/ uint8  swapping;       // 0: adding 1: swapping (sent in invite)
// /*014*/ char   swap_name[64];  // swap name sent in invite
// /*078*/ uint8  unknown078;     // padding garbage?
// /*079*/ uint8  unknown079;     // padding garbage?
// };

// ENCODE/DECODE Section:
// DECODE(OP_DzExpeditionInviteResponse)
// {
// DECODE_LENGTH_EXACT(structs::ExpeditionInviteResponse_Struct);
// SETUP_DIRECT_DECODE(ExpeditionInviteResponse_Struct, structs::ExpeditionInviteResponse_Struct);
// 
// IN(dz_zone_id);
// IN(dz_instance_id);
// IN(accepted);
// IN(swapping);
// strn0cpy(emu->swap_name, eq->swap_name, sizeof(emu->swap_name));
// 
// FINISH_DIRECT_DECODE();
// }

namespace EQProtocol.Streams.Common.UnUsed_Packets {
	/// <summary>
	/// Represents the DzExpeditionInviteResponse packet structure for EverQuest network communication.
	/// </summary>
	public struct DzExpeditionInviteResponse : IEQStruct {
		/// <summary>
		/// Gets or sets the unknown000 value.
		/// </summary>
		public uint Unknown000 { get; set; }

		/// <summary>
		/// Gets or sets the unknown004 value.
		/// </summary>
		public uint Unknown004 { get; set; }

		/// <summary>
		/// Gets or sets the dzzoneid value.
		/// </summary>
		public ushort DzZoneId { get; set; }

		/// <summary>
		/// Gets or sets the dzinstanceid value.
		/// </summary>
		public ushort DzInstanceId { get; set; }

		/// <summary>
		/// Gets or sets the accepted value.
		/// </summary>
		public byte Accepted { get; set; }

		/// <summary>
		/// Gets or sets the swapping value.
		/// </summary>
		public byte Swapping { get; set; }

		/// <summary>
		/// Gets or sets the swapname value.
		/// </summary>
		public byte[] SwapName { get; set; }

		/// <summary>
		/// Gets or sets the unknown078 value.
		/// </summary>
		public byte Unknown078 { get; set; }

		/// <summary>
		/// Gets or sets the unknown079 value.
		/// </summary>
		public byte Unknown079 { get; set; }

		/// <summary>
		/// Initializes a new instance of the DzExpeditionInviteResponse struct with specified field values.
		/// </summary>
		/// <param name="unknown000">The unknown000 value.</param>
		/// <param name="unknown004">The unknown004 value.</param>
		/// <param name="dz_zone_id">The dzzoneid value.</param>
		/// <param name="dz_instance_id">The dzinstanceid value.</param>
		/// <param name="accepted">The accepted value.</param>
		/// <param name="swapping">The swapping value.</param>
		/// <param name="swap_name">The swapname value.</param>
		/// <param name="unknown078">The unknown078 value.</param>
		/// <param name="unknown079">The unknown079 value.</param>
		public DzExpeditionInviteResponse(uint unknown000, uint unknown004, ushort dz_zone_id, ushort dz_instance_id, byte accepted, byte swapping, byte[] swap_name, byte unknown078, byte unknown079) : this() {
			Unknown000 = unknown000;
			Unknown004 = unknown004;
			DzZoneId = dz_zone_id;
			DzInstanceId = dz_instance_id;
			Accepted = accepted;
			Swapping = swapping;
			SwapName = swap_name;
			Unknown078 = unknown078;
			Unknown079 = unknown079;
		}

		/// <summary>
		/// Initializes a new instance of the DzExpeditionInviteResponse struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DzExpeditionInviteResponse(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DzExpeditionInviteResponse struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DzExpeditionInviteResponse(BinaryReader br) : this() {
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
			Unknown000 = br.ReadUInt32();
			Unknown004 = br.ReadUInt32();
			DzZoneId = br.ReadUInt16();
			DzInstanceId = br.ReadUInt16();
			Accepted = br.ReadByte();
			Swapping = br.ReadByte();
			// TODO: Array reading for SwapName - implement based on actual array size
			// SwapName = new byte[size];
			Unknown078 = br.ReadByte();
			Unknown079 = br.ReadByte();
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
			bw.Write(Unknown000);
			bw.Write(Unknown004);
			bw.Write(DzZoneId);
			bw.Write(DzInstanceId);
			bw.Write(Accepted);
			bw.Write(Swapping);
			// TODO: Array writing for SwapName - implement based on actual array size
			// foreach(var item in SwapName) bw.Write(item);
			bw.Write(Unknown078);
			bw.Write(Unknown079);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DzExpeditionInviteResponse {\n";
			ret += "	Unknown000 = ";
			try {
				ret += $"{ Indentify(Unknown000) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown004 = ";
			try {
				ret += $"{ Indentify(Unknown004) },\n";
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
			ret += "	Accepted = ";
			try {
				ret += $"{ Indentify(Accepted) },\n";
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
			ret += "	Unknown078 = ";
			try {
				ret += $"{ Indentify(Unknown078) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown079 = ";
			try {
				ret += $"{ Indentify(Unknown079) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}