using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GroupGeneric_Struct {
// /*0000*/	char	name1[64];
// /*0064*/	char	name2[64];
// /*0128*/	uint32	unknown0128;
// /*0132*/	uint32	unknown0132;
// /*0136*/	uint32	unknown0136;
// /*0140*/	uint32	unknown0140;
// /*0144*/	uint32	unknown0144;
// /*0148*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_GroupUpdate)
// {
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] OP_GroupUpdate");
// EQApplicationPacket *in = *p;
// GroupJoin_Struct *gjs = (GroupJoin_Struct*)in->pBuffer;
// 
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] Received outgoing OP_GroupUpdate with action code %i", gjs->action);
// if ((gjs->action == groupActLeave) || (gjs->action == groupActDisband))
// {
// if ((gjs->action == groupActDisband) || !strcmp(gjs->yourname, gjs->membername))
// {
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] Group Leave, yourname = %s, member_name = %s", gjs->yourname, gjs->member_name);
// 
// auto outapp =
// new EQApplicationPacket(OP_GroupDisbandYou, sizeof(structs::GroupGeneric_Struct));
// 
// structs::GroupGeneric_Struct *ggs = (structs::GroupGeneric_Struct*)outapp->pBuffer;
// memcpy(ggs->name1, gjs->yourname, sizeof(ggs->name1));
// memcpy(ggs->name2, gjs->membername, sizeof(ggs->name1));
// dest->FastQueuePacket(&outapp);
// 
// // Make an empty GLAA packet to clear out their useable GLAAs
// //
// outapp = new EQApplicationPacket(OP_GroupLeadershipAAUpdate, sizeof(GroupLeadershipAAUpdate_Struct));
// 
// dest->FastQueuePacket(&outapp);
// 
// delete in;
// return;
// }
// //if(gjs->action == groupActLeave)
// //	Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] Group Leave, yourname = %s, member_name = %s", gjs->yourname, gjs->member_name);
// 
// auto outapp =
// new EQApplicationPacket(OP_GroupDisbandOther, sizeof(structs::GroupGeneric_Struct));
// 
// structs::GroupGeneric_Struct *ggs = (structs::GroupGeneric_Struct*)outapp->pBuffer;
// memcpy(ggs->name1, gjs->yourname, sizeof(ggs->name1));
// memcpy(ggs->name2, gjs->membername, sizeof(ggs->name2));
// //Log.Hex(Logs::Netcode, outapp->pBuffer, outapp->size);
// dest->FastQueuePacket(&outapp);
// 
// delete in;
// return;
// }
// 
// if (in->size == sizeof(GroupUpdate2_Struct))
// {
// // Group Update2
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] Struct is GroupUpdate2");
// 
// unsigned char *__emu_buffer = in->pBuffer;
// GroupUpdate2_Struct *gu2 = (GroupUpdate2_Struct*)__emu_buffer;
// 
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] Yourname is %s", gu2->yourname);
// 
// int MemberCount = 1;
// int PacketLength = 8 + strlen(gu2->leadersname) + 1 + 22 + strlen(gu2->yourname) + 1;
// 
// for (int i = 0; i < 5; ++i)
// {
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] Membername[%i] is %s", i,  gu2->member_name[i]);
// if (gu2->membername[i][0] != '\0')
// {
// PacketLength += (22 + strlen(gu2->membername[i]) + 1);
// ++MemberCount;
// }
// }
// 
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] Leadername is %s", gu2->leadersname);
// 
// auto outapp = new EQApplicationPacket(OP_GroupUpdateB, PacketLength);
// 
// char *Buffer = (char *)outapp->pBuffer;
// 
// // Header
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);	// Think this should be SpawnID, but it doesn't seem to matter
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, MemberCount);
// VARSTRUCT_ENCODE_STRING(Buffer, gu2->leadersname);
// 
// // Leader
// //
// 
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// VARSTRUCT_ENCODE_STRING(Buffer, gu2->yourname);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);
// //VARSTRUCT_ENCODE_STRING(Buffer, "");
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);	// This is a string
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0x46);	// Observed 0x41 and 0x46 here
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint16, Buffer, 0);
// 
// int MemberNumber = 1;
// 
// for (int i = 0; i < 5; ++i)
// {
// if (gu2->membername[i][0] == '\0')
// continue;
// 
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, MemberNumber++);
// VARSTRUCT_ENCODE_STRING(Buffer, gu2->membername[i]);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);
// //VARSTRUCT_ENCODE_STRING(Buffer, "");
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);	// This is a string
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0x41);	// Observed 0x41 and 0x46 here
// VARSTRUCT_ENCODE_TYPE(uint8, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);	// Low byte is Main Assist Flag
// VARSTRUCT_ENCODE_TYPE(uint32, Buffer, 0);
// VARSTRUCT_ENCODE_TYPE(uint16, Buffer, 0);
// }
// 
// //Log.Hex(Logs::Netcode, outapp->pBuffer, outapp->size);
// dest->FastQueuePacket(&outapp);
// 
// outapp = new EQApplicationPacket(OP_GroupLeadershipAAUpdate, sizeof(GroupLeadershipAAUpdate_Struct));
// 
// GroupLeadershipAAUpdate_Struct *GLAAus = (GroupLeadershipAAUpdate_Struct*)outapp->pBuffer;
// 
// GLAAus->NPCMarkerID = gu2->NPCMarkerID;
// memcpy(&GLAAus->LeaderAAs, &gu2->leader_aas, sizeof(GLAAus->LeaderAAs));
// 
// dest->FastQueuePacket(&outapp);
// delete in;
// return;
// }
// //Log.LogDebugType(Logs::General, Logs::Netcode, "[ERROR] Generic GroupUpdate, yourname = %s, member_name = %s", gjs->yourname, gjs->member_name);
// ENCODE_LENGTH_EXACT(GroupJoin_Struct);
// SETUP_DIRECT_ENCODE(GroupJoin_Struct, structs::GroupJoin_Struct);
// 
// memcpy(eq->membername, emu->membername, sizeof(eq->membername));
// 
// auto outapp =
// new EQApplicationPacket(OP_GroupLeadershipAAUpdate, sizeof(GroupLeadershipAAUpdate_Struct));
// GroupLeadershipAAUpdate_Struct *GLAAus = (GroupLeadershipAAUpdate_Struct*)outapp->pBuffer;
// 
// GLAAus->NPCMarkerID = emu->NPCMarkerID;
// 
// memcpy(&GLAAus->LeaderAAs, &emu->leader_aas, sizeof(GLAAus->LeaderAAs));
// //Log.Hex(Logs::Netcode, __packet->pBuffer, __packet->size);
// 
// FINISH_ENCODE();
// 
// dest->FastQueuePacket(&outapp);
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GroupUpdate packet structure for EverQuest network communication.
	/// </summary>
	public struct GroupUpdate : IEQStruct {
		/// <summary>
		/// Gets or sets the name1 value.
		/// </summary>
		public byte[] Name1 { get; set; }

		/// <summary>
		/// Gets or sets the name2 value.
		/// </summary>
		public byte[] Name2 { get; set; }

		/// <summary>
		/// Initializes a new instance of the GroupUpdate struct with specified field values.
		/// </summary>
		/// <param name="name1">The name1 value.</param>
		/// <param name="name2">The name2 value.</param>
		public GroupUpdate(byte[] name1, byte[] name2) : this() {
			Name1 = name1;
			Name2 = name2;
		}

		/// <summary>
		/// Initializes a new instance of the GroupUpdate struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GroupUpdate(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GroupUpdate struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GroupUpdate(BinaryReader br) : this() {
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
			// TODO: Array reading for Name1 - implement based on actual array size
			// Name1 = new byte[size];
			// TODO: Array reading for Name2 - implement based on actual array size
			// Name2 = new byte[size];
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
			// TODO: Array writing for Name1 - implement based on actual array size
			// foreach(var item in Name1) bw.Write(item);
			// TODO: Array writing for Name2 - implement based on actual array size
			// foreach(var item in Name2) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GroupUpdate {\n";
			ret += "	Name1 = ";
			try {
				ret += $"{ Indentify(Name1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Name2 = ";
			try {
				ret += $"{ Indentify(Name2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}