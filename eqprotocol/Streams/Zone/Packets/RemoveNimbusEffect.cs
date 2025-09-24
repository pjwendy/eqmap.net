using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct RemoveNimbusEffect_Struct
// {
// /*00*/ uint32 spawnid;			// Spawn ID
// /*04*/ int32 nimbus_effect;	// Nimbus Effect Number
// /*08*/
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the RemoveNimbusEffect packet structure for EverQuest network communication.
	/// </summary>
	public struct RemoveNimbusEffect : IEQStruct {
		/// <summary>
		/// Gets or sets the spawnid value.
		/// </summary>
		public uint Spawnid { get; set; }

		/// <summary>
		/// Gets or sets the nimbuseffect value.
		/// </summary>
		public int NimbusEffect { get; set; }

		/// <summary>
		/// Initializes a new instance of the RemoveNimbusEffect struct with specified field values.
		/// </summary>
		/// <param name="spawnid">The spawnid value.</param>
		/// <param name="nimbus_effect">The nimbuseffect value.</param>
		public RemoveNimbusEffect(uint spawnid, int nimbus_effect) : this() {
			Spawnid = spawnid;
			NimbusEffect = nimbus_effect;
		}

		/// <summary>
		/// Initializes a new instance of the RemoveNimbusEffect struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public RemoveNimbusEffect(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the RemoveNimbusEffect struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public RemoveNimbusEffect(BinaryReader br) : this() {
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
			Spawnid = br.ReadUInt32();
			NimbusEffect = br.ReadInt32();
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
			bw.Write(Spawnid);
			bw.Write(NimbusEffect);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct RemoveNimbusEffect {\n";
			ret += "	Spawnid = ";
			try {
				ret += $"{ Indentify(Spawnid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NimbusEffect = ";
			try {
				ret += $"{ Indentify(NimbusEffect) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}