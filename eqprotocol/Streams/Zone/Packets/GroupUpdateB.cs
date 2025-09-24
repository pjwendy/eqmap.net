using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct GuildBankClear_Struct
// {
// /*00*/	uint32	Action;
// /*04*/	uint32	Unknown04;
// /*08*/	uint32	DepositAreaCount;
// /*12*/	uint32	MainAreaCount;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the GroupUpdateB packet structure for EverQuest network communication.
	/// </summary>
	public struct GroupUpdateB : IEQStruct {
		/// <summary>
		/// Gets or sets the action value.
		/// </summary>
		public uint Action { get; set; }

		/// <summary>
		/// Gets or sets the unknown04 value.
		/// </summary>
		public uint Unknown04 { get; set; }

		/// <summary>
		/// Gets or sets the depositareacount value.
		/// </summary>
		public uint Depositareacount { get; set; }

		/// <summary>
		/// Gets or sets the mainareacount value.
		/// </summary>
		public uint Mainareacount { get; set; }

		/// <summary>
		/// Initializes a new instance of the GroupUpdateB struct with specified field values.
		/// </summary>
		/// <param name="Action">The action value.</param>
		/// <param name="Unknown04">The unknown04 value.</param>
		/// <param name="DepositAreaCount">The depositareacount value.</param>
		/// <param name="MainAreaCount">The mainareacount value.</param>
		public GroupUpdateB(uint Action, uint Unknown04, uint DepositAreaCount, uint MainAreaCount) : this() {
			Action = Action;
			Unknown04 = Unknown04;
			Depositareacount = DepositAreaCount;
			Mainareacount = MainAreaCount;
		}

		/// <summary>
		/// Initializes a new instance of the GroupUpdateB struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public GroupUpdateB(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the GroupUpdateB struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public GroupUpdateB(BinaryReader br) : this() {
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
			Unknown04 = br.ReadUInt32();
			Depositareacount = br.ReadUInt32();
			Mainareacount = br.ReadUInt32();
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
			bw.Write(Unknown04);
			bw.Write(Depositareacount);
			bw.Write(Mainareacount);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GroupUpdateB {\n";
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown04 = ";
			try {
				ret += $"{ Indentify(Unknown04) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Depositareacount = ";
			try {
				ret += $"{ Indentify(Depositareacount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mainareacount = ";
			try {
				ret += $"{ Indentify(Mainareacount) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}