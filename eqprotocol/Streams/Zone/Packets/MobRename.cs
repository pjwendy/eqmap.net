using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct MobRename_Struct {
// /*000*/	char	old_name[64];
// /*064*/	char	old_name_again[64];	//not sure what the difference is
// /*128*/	char	new_name[64];
// /*192*/	uint32	unknown192;		//set to 0
// /*196*/	uint32	unknown196;		//set to 1
// /*200*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the MobRename packet structure for EverQuest network communication.
	/// </summary>
	public struct MobRename : IEQStruct {
		/// <summary>
		/// Gets or sets the oldname value.
		/// </summary>
		public byte[] OldName { get; set; }

		/// <summary>
		/// Gets or sets the oldnameagain value.
		/// </summary>
		public byte[] OldNameAgain { get; set; }

		/// <summary>
		/// Gets or sets the newname value.
		/// </summary>
		public byte[] NewName { get; set; }

		/// <summary>
		/// Gets or sets the unknown192 value.
		/// </summary>
		public uint Unknown192 { get; set; }

		/// <summary>
		/// Gets or sets the unknown196 value.
		/// </summary>
		public uint Unknown196 { get; set; }

		/// <summary>
		/// Initializes a new instance of the MobRename struct with specified field values.
		/// </summary>
		/// <param name="old_name">The oldname value.</param>
		/// <param name="old_name_again">The oldnameagain value.</param>
		/// <param name="new_name">The newname value.</param>
		/// <param name="unknown192">The unknown192 value.</param>
		/// <param name="unknown196">The unknown196 value.</param>
		public MobRename(byte[] old_name, byte[] old_name_again, byte[] new_name, uint unknown192, uint unknown196) : this() {
			OldName = old_name;
			OldNameAgain = old_name_again;
			NewName = new_name;
			Unknown192 = unknown192;
			Unknown196 = unknown196;
		}

		/// <summary>
		/// Initializes a new instance of the MobRename struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public MobRename(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the MobRename struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public MobRename(BinaryReader br) : this() {
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
			// TODO: Array reading for OldName - implement based on actual array size
			// OldName = new byte[size];
			// TODO: Array reading for OldNameAgain - implement based on actual array size
			// OldNameAgain = new byte[size];
			// TODO: Array reading for NewName - implement based on actual array size
			// NewName = new byte[size];
			Unknown192 = br.ReadUInt32();
			Unknown196 = br.ReadUInt32();
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
			// TODO: Array writing for OldName - implement based on actual array size
			// foreach(var item in OldName) bw.Write(item);
			// TODO: Array writing for OldNameAgain - implement based on actual array size
			// foreach(var item in OldNameAgain) bw.Write(item);
			// TODO: Array writing for NewName - implement based on actual array size
			// foreach(var item in NewName) bw.Write(item);
			bw.Write(Unknown192);
			bw.Write(Unknown196);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct MobRename {\n";
			ret += "	OldName = ";
			try {
				ret += $"{ Indentify(OldName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	OldNameAgain = ";
			try {
				ret += $"{ Indentify(OldNameAgain) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NewName = ";
			try {
				ret += $"{ Indentify(NewName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown192 = ";
			try {
				ret += $"{ Indentify(Unknown192) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown196 = ";
			try {
				ret += $"{ Indentify(Unknown196) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}