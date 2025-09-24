using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct DelegateAbility_Struct
// {
// /*000*/	uint32	DelegateAbility;
// /*004*/	uint32	MemberNumber;
// /*008*/	uint32	Action;
// /*012*/	uint32	Unknown012;
// /*016*/	uint32	Unknown016;
// /*020*/	uint32	EntityID;
// /*024*/	uint32	Unknown024;
// /*028*/	char	Name[64];
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the DelegateAbility packet structure for EverQuest network communication.
	/// </summary>
	public struct DelegateAbility : IEQStruct {
		/// <summary>
		/// Gets or sets the delegateabilityvalue value.
		/// </summary>
		public uint DelegateabilityValue { get; set; }

		/// <summary>
		/// Gets or sets the membernumber value.
		/// </summary>
		public uint Membernumber { get; set; }

		/// <summary>
		/// Gets or sets the action value.
		/// </summary>
		public uint Action { get; set; }

		/// <summary>
		/// Gets or sets the unknown012 value.
		/// </summary>
		public uint Unknown012 { get; set; }

		/// <summary>
		/// Gets or sets the unknown016 value.
		/// </summary>
		public uint Unknown016 { get; set; }

		/// <summary>
		/// Gets or sets the entityid value.
		/// </summary>
		public uint Entityid { get; set; }

		/// <summary>
		/// Gets or sets the unknown024 value.
		/// </summary>
		public uint Unknown024 { get; set; }

		/// <summary>
		/// Gets or sets the name value.
		/// </summary>
		public byte[] Name { get; set; }

		/// <summary>
		/// Initializes a new instance of the DelegateAbility struct with specified field values.
		/// </summary>
		/// <param name="DelegateAbility">The delegateabilityvalue value.</param>
		/// <param name="MemberNumber">The membernumber value.</param>
		/// <param name="Action">The action value.</param>
		/// <param name="Unknown012">The unknown012 value.</param>
		/// <param name="Unknown016">The unknown016 value.</param>
		/// <param name="EntityID">The entityid value.</param>
		/// <param name="Unknown024">The unknown024 value.</param>
		/// <param name="Name">The name value.</param>
		public DelegateAbility(uint DelegateAbility, uint MemberNumber, uint Action, uint Unknown012, uint Unknown016, uint EntityID, uint Unknown024, byte[] Name) : this() {
			DelegateabilityValue = DelegateAbility;
			Membernumber = MemberNumber;
			Action = Action;
			Unknown012 = Unknown012;
			Unknown016 = Unknown016;
			Entityid = EntityID;
			Unknown024 = Unknown024;
			Name = Name;
		}

		/// <summary>
		/// Initializes a new instance of the DelegateAbility struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public DelegateAbility(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the DelegateAbility struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public DelegateAbility(BinaryReader br) : this() {
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
			DelegateabilityValue = br.ReadUInt32();
			Membernumber = br.ReadUInt32();
			Action = br.ReadUInt32();
			Unknown012 = br.ReadUInt32();
			Unknown016 = br.ReadUInt32();
			Entityid = br.ReadUInt32();
			Unknown024 = br.ReadUInt32();
			// TODO: Array reading for Name - implement based on actual array size
			// Name = new byte[size];
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
			bw.Write(DelegateabilityValue);
			bw.Write(Membernumber);
			bw.Write(Action);
			bw.Write(Unknown012);
			bw.Write(Unknown016);
			bw.Write(Entityid);
			bw.Write(Unknown024);
			// TODO: Array writing for Name - implement based on actual array size
			// foreach(var item in Name) bw.Write(item);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct DelegateAbility {\n";
			ret += "	DelegateabilityValue = ";
			try {
				ret += $"{ Indentify(DelegateabilityValue) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Membernumber = ";
			try {
				ret += $"{ Indentify(Membernumber) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Action = ";
			try {
				ret += $"{ Indentify(Action) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown012 = ";
			try {
				ret += $"{ Indentify(Unknown012) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown016 = ";
			try {
				ret += $"{ Indentify(Unknown016) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Entityid = ";
			try {
				ret += $"{ Indentify(Entityid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown024 = ";
			try {
				ret += $"{ Indentify(Unknown024) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Name = ";
			try {
				ret += $"{ Indentify(Name) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}