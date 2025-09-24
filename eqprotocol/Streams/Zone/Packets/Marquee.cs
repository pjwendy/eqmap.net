using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct ClientMarqueeMessage_Struct {
// uint32 type;
// uint32 unk04; // no idea, didn't notice a change when altering it.
// //According to asm the following are hard coded values: 2, 4, 5, 6, 7, 10, 12, 13, 14, 15, 16, 18, 20
// //There is also a non-hardcoded fall through but to be honest i don't know enough about what it does yet
// uint32 priority; //needs a better name but it does:
// //opacity = (priority / 255) - floor(priority / 255)
// //# of fade in/out blinks = (int)((priority - 1) / 255)
// //so 510 would have 100% opacity and 1 extra blink at end
// uint32 fade_in_time; //The fade in time, in ms
// uint32 fade_out_time; //The fade out time, in ms
// uint32 duration; //in ms
// char msg[1]; //message plus null terminator
// 
// };

// ENCODE/DECODE Section:
// Handler function not found.

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Marquee packet structure for EverQuest network communication.
	/// </summary>
	public struct Marquee : IEQStruct {
		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Gets or sets the unk04 value.
		/// </summary>
		public uint Unk04 { get; set; }

		/// <summary>
		/// Gets or sets the priority value.
		/// </summary>
		public uint Priority { get; set; }

		/// <summary>
		/// Gets or sets the fadeintime value.
		/// </summary>
		public uint FadeInTime { get; set; }

		/// <summary>
		/// Gets or sets the fadeouttime value.
		/// </summary>
		public uint FadeOutTime { get; set; }

		/// <summary>
		/// Gets or sets the duration value.
		/// </summary>
		public uint Duration { get; set; }

		/// <summary>
		/// Gets or sets the msg value.
		/// </summary>
		public byte Msg { get; set; }

		/// <summary>
		/// Initializes a new instance of the Marquee struct with specified field values.
		/// </summary>
		/// <param name="type">The type value.</param>
		/// <param name="unk04">The unk04 value.</param>
		/// <param name="priority">The priority value.</param>
		/// <param name="fade_in_time">The fadeintime value.</param>
		/// <param name="fade_out_time">The fadeouttime value.</param>
		/// <param name="duration">The duration value.</param>
		/// <param name="msg">The msg value.</param>
		public Marquee(uint type, uint unk04, uint priority, uint fade_in_time, uint fade_out_time, uint duration, byte msg) : this() {
			Type = type;
			Unk04 = unk04;
			Priority = priority;
			FadeInTime = fade_in_time;
			FadeOutTime = fade_out_time;
			Duration = duration;
			Msg = msg;
		}

		/// <summary>
		/// Initializes a new instance of the Marquee struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Marquee(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Marquee struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Marquee(BinaryReader br) : this() {
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
			Type = br.ReadUInt32();
			Unk04 = br.ReadUInt32();
			Priority = br.ReadUInt32();
			FadeInTime = br.ReadUInt32();
			FadeOutTime = br.ReadUInt32();
			Duration = br.ReadUInt32();
			Msg = br.ReadByte();
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
			bw.Write(Type);
			bw.Write(Unk04);
			bw.Write(Priority);
			bw.Write(FadeInTime);
			bw.Write(FadeOutTime);
			bw.Write(Duration);
			bw.Write(Msg);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Marquee {\n";
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unk04 = ";
			try {
				ret += $"{ Indentify(Unk04) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Priority = ";
			try {
				ret += $"{ Indentify(Priority) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FadeInTime = ";
			try {
				ret += $"{ Indentify(FadeInTime) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	FadeOutTime = ";
			try {
				ret += $"{ Indentify(FadeOutTime) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Duration = ";
			try {
				ret += $"{ Indentify(Duration) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Msg = ";
			try {
				ret += $"{ Indentify(Msg) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}