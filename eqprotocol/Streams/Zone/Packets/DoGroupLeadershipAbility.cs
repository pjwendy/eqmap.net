using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DoGroupLeadershipAbility_Struct
// {
// /*000*/	uint32	Ability;
// /*000*/	uint32	Parameter;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DoGroupLeadershipAbility packet structure for EverQuest network communication.
	/// </summary>
	public struct DoGroupLeadershipAbility : IEQStruct {
		/// <summary>
		/// Gets or sets the ability value.
		/// </summary>
		public uint Ability { get; set; }

		/// <summary>
		/// Gets or sets the parameter value.
		/// </summary>
		public uint Parameter { get; set; }

		/// <summary>
		/// Initializes a new instance of the DoGroupLeadershipAbility struct with specified field values.
		/// </summary>
		/// <param name="Ability">The ability value.</param>
		/// <param name="Parameter">The parameter value.</param>
		public DoGroupLeadershipAbility(uint Ability, uint Parameter) : this() {
			Ability = Ability;
			Parameter = Parameter;
		}

		/// <summary>
		/// Initializes a new instance of the DoGroupLeadershipAbility struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DoGroupLeadershipAbility(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DoGroupLeadershipAbility struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DoGroupLeadershipAbility(BinaryReader br) : this() {
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
			Ability = br.ReadUInt32();
			Parameter = br.ReadUInt32();
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
			bw.Write(Ability);
			bw.Write(Parameter);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DoGroupLeadershipAbility {\n";
			ret += "	Ability = ";
			try {
				ret += $"{ Indentify(Ability) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Parameter = ";
			try {
				ret += $"{ Indentify(Parameter) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}