using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct TaskDescriptionHeader_Struct {
// uint32	SequenceNumber; // The order the tasks appear in the journal. 0 for first task, 1 for second, etc.
// uint32	TaskID;
// uint32	unknown2;
// uint32	unknown3;
// uint8	unknown4;
// };

// ENCODE/DECODE Section:
// ENCODE(OP_TaskDescription)
// {
// EQApplicationPacket *in = *p;
// *p = nullptr;
// 
// unsigned char *__emu_buffer = in->pBuffer;
// 
// char *InBuffer = (char *)in->pBuffer;
// char *block_start = InBuffer;
// 
// InBuffer += sizeof(TaskDescriptionHeader_Struct);
// uint32 title_size = strlen(InBuffer) + 1;
// InBuffer += title_size;
// InBuffer += sizeof(TaskDescriptionData1_Struct);
// uint32 description_size = strlen(InBuffer) + 1;
// InBuffer += description_size;
// InBuffer += sizeof(TaskDescriptionData2_Struct);
// 
// uint32 reward_size = strlen(InBuffer) + 1;
// InBuffer += reward_size;
// 
// std::string old_message = InBuffer; // start item link string
// std::string new_message;
// ServerToUFSayLink(new_message, old_message);
// 
// in->size = sizeof(TaskDescriptionHeader_Struct) + sizeof(TaskDescriptionData1_Struct)+
// sizeof(TaskDescriptionData2_Struct) + sizeof(TaskDescriptionTrailer_Struct)+
// title_size + description_size + reward_size + new_message.length() + 1;
// 
// in->pBuffer = new unsigned char[in->size];
// 
// char *OutBuffer = (char *)in->pBuffer;
// 
// memcpy(OutBuffer, block_start, (InBuffer - block_start));
// OutBuffer += (InBuffer - block_start);
// 
// VARSTRUCT_ENCODE_STRING(OutBuffer, new_message.c_str());
// 
// InBuffer += strlen(InBuffer) + 1;
// 
// memcpy(OutBuffer, InBuffer, sizeof(TaskDescriptionTrailer_Struct));
// 
// delete[] __emu_buffer;
// dest->FastQueuePacket(&in, ack_req);
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the TaskDescription packet structure for EverQuest network communication.
	/// </summary>
	public struct TaskDescription : IEQStruct {
		/// <summary>
		/// Gets or sets the sequencenumber value.
		/// </summary>
		public uint Sequencenumber { get; set; }

		/// <summary>
		/// Gets or sets the taskid value.
		/// </summary>
		public uint Taskid { get; set; }

		/// <summary>
		/// Gets or sets the unknown2 value.
		/// </summary>
		public uint Unknown2 { get; set; }

		/// <summary>
		/// Gets or sets the unknown3 value.
		/// </summary>
		public uint Unknown3 { get; set; }

		/// <summary>
		/// Gets or sets the unknown4 value.
		/// </summary>
		public byte Unknown4 { get; set; }

		/// <summary>
		/// Initializes a new instance of the TaskDescription struct with specified field values.
		/// </summary>
		/// <param name="SequenceNumber">The sequencenumber value.</param>
		/// <param name="TaskID">The taskid value.</param>
		/// <param name="unknown2">The unknown2 value.</param>
		/// <param name="unknown3">The unknown3 value.</param>
		/// <param name="unknown4">The unknown4 value.</param>
		public TaskDescription(uint SequenceNumber, uint TaskID, uint unknown2, uint unknown3, byte unknown4) : this() {
			Sequencenumber = SequenceNumber;
			Taskid = TaskID;
			Unknown2 = unknown2;
			Unknown3 = unknown3;
			Unknown4 = unknown4;
		}

		/// <summary>
		/// Initializes a new instance of the TaskDescription struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public TaskDescription(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the TaskDescription struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public TaskDescription(BinaryReader br) : this() {
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
			Sequencenumber = br.ReadUInt32();
			Taskid = br.ReadUInt32();
			Unknown2 = br.ReadUInt32();
			Unknown3 = br.ReadUInt32();
			Unknown4 = br.ReadByte();
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
			bw.Write(Sequencenumber);
			bw.Write(Taskid);
			bw.Write(Unknown2);
			bw.Write(Unknown3);
			bw.Write(Unknown4);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct TaskDescription {\n";
			ret += "	Sequencenumber = ";
			try {
				ret += $"{ Indentify(Sequencenumber) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Taskid = ";
			try {
				ret += $"{ Indentify(Taskid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown2 = ";
			try {
				ret += $"{ Indentify(Unknown2) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown3 = ";
			try {
				ret += $"{ Indentify(Unknown3) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown4 = ";
			try {
				ret += $"{ Indentify(Unknown4) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}