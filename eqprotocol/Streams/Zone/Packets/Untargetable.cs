using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Untargetable_Struct {
// /*000*/	uint32 id;
// /*004*/	uint32 targetable_flag; //0 = not targetable, 1 or higher = targetable
// /*008*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Untargetable packet structure for EverQuest network communication.
	/// </summary>
	public struct Untargetable : IEQStruct {
		/// <summary>
		/// Gets or sets the id value.
		/// </summary>
		public uint Id { get; set; }

		/// <summary>
		/// Gets or sets the targetableflag value.
		/// </summary>
		public uint TargetableFlag { get; set; }

		/// <summary>
		/// Initializes a new instance of the Untargetable struct with specified field values.
		/// </summary>
		/// <param name="id">The id value.</param>
		/// <param name="targetable_flag">The targetableflag value.</param>
		public Untargetable(uint id, uint targetable_flag) : this() {
			Id = id;
			TargetableFlag = targetable_flag;
		}

		/// <summary>
		/// Initializes a new instance of the Untargetable struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Untargetable(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Untargetable struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Untargetable(BinaryReader br) : this() {
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
			Id = br.ReadUInt32();
			TargetableFlag = br.ReadUInt32();
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
			bw.Write(Id);
			bw.Write(TargetableFlag);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Untargetable {\n";
			ret += "	Id = ";
			try {
				ret += $"{ Indentify(Id) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TargetableFlag = ";
			try {
				ret += $"{ Indentify(TargetableFlag) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}