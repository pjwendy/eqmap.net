using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Camera_Struct
// {
// uint32	duration;	// Duration in ms
// float intensity;
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the CameraEffect packet structure for EverQuest network communication.
	/// </summary>
	public struct CameraEffect : IEQStruct {
		/// <summary>
		/// Gets or sets the duration value.
		/// </summary>
		public uint Duration { get; set; }

		/// <summary>
		/// Gets or sets the intensity value.
		/// </summary>
		public float Intensity { get; set; }

		/// <summary>
		/// Initializes a new instance of the CameraEffect struct with specified field values.
		/// </summary>
		/// <param name="duration">The duration value.</param>
		/// <param name="intensity">The intensity value.</param>
		public CameraEffect(uint duration, float intensity) : this() {
			Duration = duration;
			Intensity = intensity;
		}

		/// <summary>
		/// Initializes a new instance of the CameraEffect struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public CameraEffect(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the CameraEffect struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public CameraEffect(BinaryReader br) : this() {
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
			Duration = br.ReadUInt32();
			Intensity = br.ReadSingle();
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
			bw.Write(Duration);
			bw.Write(Intensity);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct CameraEffect {\n";
			ret += "	Duration = ";
			try {
				ret += $"{ Indentify(Duration) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Intensity = ";
			try {
				ret += $"{ Indentify(Intensity) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}