using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct AuraCreate_Struct {
// /* 00 */	uint32 action; // 0 = add, 1 = delete, 2 = reset
// /* 04 */	uint32 type; // unsure -- normal auras show 1 clicky (ex. Circle of Power) show 0
// /* 08 */	char aura_name[64];
// /* 72 */	uint32 entity_id;
// /* 76 */	uint32 icon;
// /* 80 */
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the UpdateAura packet structure for EverQuest network communication.
	/// </summary>
	public struct UpdateAura : IEQStruct {
		/// <summary>
		/// Gets or sets the action value.
		/// </summary>
		public uint Action { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Gets or sets the auraname value.
		/// </summary>
		public byte[] AuraName { get; set; }

		/// <summary>
		/// Gets or sets the entityid value.
		/// </summary>
		public uint EntityId { get; set; }

		/// <summary>
		/// Gets or sets the icon value.
		/// </summary>
		public uint Icon { get; set; }

		/// <summary>
		/// Initializes a new instance of the UpdateAura struct with specified field values.
		/// </summary>
		/// <param name="action">The action value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="aura_name">The auraname value.</param>
		/// <param name="entity_id">The entityid value.</param>
		/// <param name="icon">The icon value.</param>
		public UpdateAura(uint action, uint type, byte[] aura_name, uint entity_id, uint icon) : this() {
			Action = action;
			Type = type;
			AuraName = aura_name;
			EntityId = entity_id;
			Icon = icon;
		}

		/// <summary>
		/// Initializes a new instance of the UpdateAura struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public UpdateAura(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the UpdateAura struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public UpdateAura(BinaryReader br) : this() {
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
			Action = br.ReadUInt32();
			Type = br.ReadUInt32();
			// TODO: Array reading for AuraName - implement based on actual array size
			// AuraName = new byte[size];
			EntityId = br.ReadUInt32();
			Icon = br.ReadUInt32();
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
			bw.Write(Action);
			bw.Write(Type);
			// TODO: Array writing for AuraName - implement based on actual array size
			// foreach(var item in AuraName) bw.Write(item);
			bw.Write(EntityId);
			bw.Write(Icon);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct UpdateAura {\n";
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	AuraName = ";
			try {
				ret += $"{ Indentify(AuraName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	EntityId = ";
			try {
				ret += $"{ Indentify(EntityId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Icon = ";
			try {
				ret += $"{ Indentify(Icon) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}