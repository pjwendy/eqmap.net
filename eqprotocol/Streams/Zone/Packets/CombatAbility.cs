using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct CombatAbility_Struct {
// uint32 m_target;		//the ID of the target mob
// uint32 m_atk;
// uint32 m_skill;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the CombatAbility packet structure for EverQuest network communication.
	/// </summary>
	public struct CombatAbility : IEQStruct {
		/// <summary>
		/// Gets or sets the mtarget value.
		/// </summary>
		public uint MTarget { get; set; }

		/// <summary>
		/// Gets or sets the matk value.
		/// </summary>
		public uint MAtk { get; set; }

		/// <summary>
		/// Gets or sets the mskill value.
		/// </summary>
		public uint MSkill { get; set; }

		/// <summary>
		/// Initializes a new instance of the CombatAbility struct with specified field values.
		/// </summary>
		/// <param name="m_target">The mtarget value.</param>
		/// <param name="m_atk">The matk value.</param>
		/// <param name="m_skill">The mskill value.</param>
		public CombatAbility(uint m_target, uint m_atk, uint m_skill) : this() {
			MTarget = m_target;
			MAtk = m_atk;
			MSkill = m_skill;
		}

		/// <summary>
		/// Initializes a new instance of the CombatAbility struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public CombatAbility(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the CombatAbility struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public CombatAbility(BinaryReader br) : this() {
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
			MTarget = br.ReadUInt32();
			MAtk = br.ReadUInt32();
			MSkill = br.ReadUInt32();
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
			bw.Write(MTarget);
			bw.Write(MAtk);
			bw.Write(MSkill);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct CombatAbility {\n";
			ret += "	MTarget = ";
			try {
				ret += $"{ Indentify(MTarget) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MAtk = ";
			try {
				ret += $"{ Indentify(MAtk) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	MSkill = ";
			try {
				ret += $"{ Indentify(MSkill) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}