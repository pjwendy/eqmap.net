using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct fling_struct {
// /* 00 */ uint32 collision; // 0 collision is off, anything else it's on
// /* 04 */ int32 travel_time; // ms -- UF we need to calc this, RoF+ -1 auto calcs
// /* 08 */ uint8 unk3; // bool, set to 1 has something to do with z-axis or something weird things happen if the new Z is above or equal to yours
// /* 09 */ uint8 disable_fall_damage; // 1 you take no fall damage, 0 you take fall damage
// /* 10 */ uint8 padding[2];
// /* 12 */ float speed_z;
// /* 16 */ float new_y;
// /* 20 */ float new_x;
// /* 24 */ float new_z;
// /* 28 */
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Fling packet structure for EverQuest network communication.
	/// </summary>
	public struct Fling : IEQStruct {
		/// <summary>
		/// Gets or sets the collision value.
		/// </summary>
		public uint Collision { get; set; }

		/// <summary>
		/// Gets or sets the traveltime value.
		/// </summary>
		public int TravelTime { get; set; }

		/// <summary>
		/// Gets or sets the unk3 value.
		/// </summary>
		public byte Unk3 { get; set; }

		/// <summary>
		/// Gets or sets the disablefalldamage value.
		/// </summary>
		public byte DisableFallDamage { get; set; }

		/// <summary>
		/// Gets or sets the padding value.
		/// </summary>
		public byte[] Padding { get; set; }

		/// <summary>
		/// Gets or sets the speedz value.
		/// </summary>
		public float SpeedZ { get; set; }

		/// <summary>
		/// Gets or sets the newy value.
		/// </summary>
		public float NewY { get; set; }

		/// <summary>
		/// Gets or sets the newx value.
		/// </summary>
		public float NewX { get; set; }

		/// <summary>
		/// Gets or sets the newz value.
		/// </summary>
		public float NewZ { get; set; }

		/// <summary>
		/// Initializes a new instance of the Fling struct with specified field values.
		/// </summary>
		/// <param name="collision">The collision value.</param>
		/// <param name="travel_time">The traveltime value.</param>
		/// <param name="unk3">The unk3 value.</param>
		/// <param name="disable_fall_damage">The disablefalldamage value.</param>
		/// <param name="padding">The padding value.</param>
		/// <param name="speed_z">The speedz value.</param>
		/// <param name="new_y">The newy value.</param>
		/// <param name="new_x">The newx value.</param>
		/// <param name="new_z">The newz value.</param>
		public Fling(uint collision, int travel_time, byte unk3, byte disable_fall_damage, byte[] padding, float speed_z, float new_y, float new_x, float new_z) : this() {
			Collision = collision;
			TravelTime = travel_time;
			Unk3 = unk3;
			DisableFallDamage = disable_fall_damage;
			Padding = padding;
			SpeedZ = speed_z;
			NewY = new_y;
			NewX = new_x;
			NewZ = new_z;
		}

		/// <summary>
		/// Initializes a new instance of the Fling struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Fling(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Fling struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Fling(BinaryReader br) : this() {
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
			Collision = br.ReadUInt32();
			TravelTime = br.ReadInt32();
			Unk3 = br.ReadByte();
			DisableFallDamage = br.ReadByte();
			// TODO: Array reading for Padding - implement based on actual array size
			// Padding = new byte[size];
			SpeedZ = br.ReadSingle();
			NewY = br.ReadSingle();
			NewX = br.ReadSingle();
			NewZ = br.ReadSingle();
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
			bw.Write(Collision);
			bw.Write(TravelTime);
			bw.Write(Unk3);
			bw.Write(DisableFallDamage);
			// TODO: Array writing for Padding - implement based on actual array size
			// foreach(var item in Padding) bw.Write(item);
			bw.Write(SpeedZ);
			bw.Write(NewY);
			bw.Write(NewX);
			bw.Write(NewZ);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Fling {\n";
			ret += "	Collision = ";
			try {
				ret += $"{ Indentify(Collision) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	TravelTime = ";
			try {
				ret += $"{ Indentify(TravelTime) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unk3 = ";
			try {
				ret += $"{ Indentify(Unk3) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DisableFallDamage = ";
			try {
				ret += $"{ Indentify(DisableFallDamage) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Padding = ";
			try {
				ret += $"{ Indentify(Padding) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpeedZ = ";
			try {
				ret += $"{ Indentify(SpeedZ) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NewY = ";
			try {
				ret += $"{ Indentify(NewY) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NewX = ";
			try {
				ret += $"{ Indentify(NewX) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	NewZ = ";
			try {
				ret += $"{ Indentify(NewZ) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}