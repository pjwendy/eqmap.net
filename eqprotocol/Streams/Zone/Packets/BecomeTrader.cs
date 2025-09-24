using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct BecomeTrader_Struct {
// uint32 entity_id;
// uint32 action;
// char   trader_name[64];
// };

// ENCODE/DECODE Section:
// ENCODE(OP_BecomeTrader)
// {
// uint32 action = *(uint32 *)(*p)->pBuffer;
// 
// switch (action)
// {
// case TraderOff:
// {
// ENCODE_LENGTH_EXACT(BecomeTrader_Struct);
// SETUP_DIRECT_ENCODE(BecomeTrader_Struct, structs::BecomeTrader_Struct);
// LogTrading(
// "Encode OP_BecomeTrader(UF) TraderOff action <green>[{}] entity_id <green>[{}] trader_name "
// "<green>[{}]",
// emu->action,
// emu->entity_id,
// emu->trader_name
// );
// eq->action    = structs::UFBazaarTraderBuyerActions::Zero;
// eq->entity_id = emu->entity_id;
// FINISH_ENCODE();
// break;
// }
// case TraderOn:
// {
// ENCODE_LENGTH_EXACT(BecomeTrader_Struct);
// SETUP_DIRECT_ENCODE(BecomeTrader_Struct, structs::BecomeTrader_Struct);
// LogTrading(
// "Encode OP_BecomeTrader(UF) TraderOn action <green>[{}] entity_id <green>[{}] trader_name "
// "<green>[{}]",
// emu->action,
// emu->entity_id,
// emu->trader_name
// );
// eq->action    = structs::UFBazaarTraderBuyerActions::BeginTraderMode;
// eq->entity_id = emu->entity_id;
// strn0cpy(eq->trader_name, emu->trader_name, sizeof(eq->trader_name));
// FINISH_ENCODE();
// break;
// }
// default:
// {
// LogTrading(
// "Encode OP_BecomeTrader(UF) unhandled action <red>[{}] Sending packet as is.",
// action
// );
// EQApplicationPacket *in = *p;
// *p = nullptr;
// dest->FastQueuePacket(&in, ack_req);
// }
// }
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the BecomeTrader packet structure for EverQuest network communication.
	/// </summary>
	public struct BecomeTrader : IEQStruct {
		/// <summary>
		/// Gets or sets the entityid value.
		/// </summary>
		public uint EntityId { get; set; }

		/// <summary>
		/// Gets or sets the action value.
		/// </summary>
		public uint Action { get; set; }

		/// <summary>
		/// Gets or sets the tradername value.
		/// </summary>
		public byte[] TraderName { get; set; }

		/// <summary>
		/// Initializes a new instance of the BecomeTrader struct with specified field values.
		/// </summary>
		/// <param name="entity_id">The entityid value.</param>
		/// <param name="action">The action value.</param>
		/// <param name="trader_name">The tradername value.</param>
		public BecomeTrader(uint entity_id, uint action, byte[] trader_name) : this() {
			EntityId = entity_id;
			Action = action;
			TraderName = trader_name;
		}

		/// <summary>
		/// Initializes a new instance of the BecomeTrader struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public BecomeTrader(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the BecomeTrader struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public BecomeTrader(BinaryReader br) : this() {
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
			EntityId = br.ReadUInt32();
			Action = br.ReadUInt32();
			// TODO: Array reading for TraderName - implement based on actual array size
			// TraderName = new byte[size];
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
			bw.Write(EntityId);
			bw.Write(Action);
			// TODO: Array writing for TraderName - implement based on actual array size
			// foreach(var item in TraderName) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct BecomeTrader {\n";
			ret += "	EntityId = ";
			try {
				ret += $"{ Indentify(EntityId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TraderName = ";
			try {
				ret += $"{ Indentify(TraderName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}