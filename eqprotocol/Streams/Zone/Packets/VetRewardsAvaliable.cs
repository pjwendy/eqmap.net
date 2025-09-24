using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct VeteranReward
// {
// /*000*/	uint32 claim_id;
// /*004*/	uint32 number_available;
// /*008*/	uint32 claim_count;
// /*012*/	VeteranRewardItem items[8];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_VetRewardsAvaliable)
// {
// EQApplicationPacket *inapp = *p;
// unsigned char * __emu_buffer = inapp->pBuffer;
// 
// uint32 count = ((*p)->Size() / sizeof(InternalVeteranReward));
// *p = nullptr;
// 
// auto outapp_create =
// new EQApplicationPacket(OP_VetRewardsAvaliable, (sizeof(structs::VeteranReward) * count));
// uchar *old_data = __emu_buffer;
// uchar *data = outapp_create->pBuffer;
// for (unsigned int i = 0; i < count; ++i)
// {
// structs::VeteranReward *vr = (structs::VeteranReward*)data;
// InternalVeteranReward *ivr = (InternalVeteranReward*)old_data;
// 
// vr->claim_count = ivr->claim_count;
// vr->claim_id = ivr->claim_id;
// vr->number_available = ivr->number_available;
// for (int x = 0; x < 8; ++x)
// {
// vr->items[x].item_id = ivr->items[x].item_id;
// strcpy(vr->items[x].item_name, ivr->items[x].item_name);
// vr->items[x].charges = ivr->items[x].charges;
// }
// 
// old_data += sizeof(InternalVeteranReward);
// data += sizeof(structs::VeteranReward);
// }
// 
// dest->FastQueuePacket(&outapp_create);
// delete inapp;
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the VetRewardsAvaliable packet structure for EverQuest network communication.
	/// </summary>
	public struct VetRewardsAvaliable : IEQStruct {
		/// <summary>
		/// Gets or sets the claimid value.
		/// </summary>
		public uint ClaimId { get; set; }

		/// <summary>
		/// Gets or sets the numberavailable value.
		/// </summary>
		public uint NumberAvailable { get; set; }

		/// <summary>
		/// Gets or sets the claimcount value.
		/// </summary>
		public uint ClaimCount { get; set; }

		/// <summary>
		/// Gets or sets the items value.
		/// </summary>
		public uint[] Items { get; set; }

		/// <summary>
		/// Initializes a new instance of the VetRewardsAvaliable struct with specified field values.
		/// </summary>
		/// <param name="claim_id">The claimid value.</param>
		/// <param name="number_available">The numberavailable value.</param>
		/// <param name="claim_count">The claimcount value.</param>
		/// <param name="items">The items value.</param>
		public VetRewardsAvaliable(uint claim_id, uint number_available, uint claim_count, uint[] items) : this() {
			ClaimId = claim_id;
			NumberAvailable = number_available;
			ClaimCount = claim_count;
			Items = items;
		}

		/// <summary>
		/// Initializes a new instance of the VetRewardsAvaliable struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public VetRewardsAvaliable(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the VetRewardsAvaliable struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public VetRewardsAvaliable(BinaryReader br) : this() {
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
			ClaimId = br.ReadUInt32();
			NumberAvailable = br.ReadUInt32();
			ClaimCount = br.ReadUInt32();
			// TODO: Array reading for Items - implement based on actual array size
			// Items = new uint[size];
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
			bw.Write(ClaimId);
			bw.Write(NumberAvailable);
			bw.Write(ClaimCount);
			// TODO: Array writing for Items - implement based on actual array size
			// foreach(var item in Items) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct VetRewardsAvaliable {\n";
			ret += "	ClaimId = ";
			try {
				ret += $"{ Indentify(ClaimId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NumberAvailable = ";
			try {
				ret += $"{ Indentify(NumberAvailable) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ClaimCount = ";
			try {
				ret += $"{ Indentify(ClaimCount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Items = ";
			try {
				ret += $"{ Indentify(Items) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}