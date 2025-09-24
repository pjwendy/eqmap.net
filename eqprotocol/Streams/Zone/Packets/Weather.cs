using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Weather_Struct {
// uint32	val1;	//generall 0x000000FF
// uint32	type;	//0x31=rain, 0x02=snow(i think), 0 = normal
// uint32	mode;
// };

// ENCODE/DECODE Section:
// Handler function not found.

//outapp = new EQApplicationPacket(OP_Weather, 12);
//Weather_Struct* ws = (Weather_Struct*)outapp->pBuffer;
//ws->val1 = 0x000000FF;

//if (zone->zone_weather == EQ::constants::WeatherTypes::Raining)
//{
//    ws->type = 0x31;
//}
//else if (zone->zone_weather == EQ::constants::WeatherTypes::Snowing)
//{
//    outapp->pBuffer[8] = 0x01;
//    ws->type = EQ::constants::WeatherTypes::Snowing;
//}

//outapp->priority = 6;
//QueuePacket(outapp);
//safe_delete(outapp);

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the Weather packet structure for EverQuest network communication.
	/// </summary>
	public struct Weather : IEQStruct {
		/// <summary>
		/// Gets or sets the val1 value.
		/// </summary>
		public uint Val1 { get; set; }

		/// <summary>
		/// Gets or sets the type value.
		/// </summary>
		public uint Type { get; set; }

		/// <summary>
		/// Gets or sets the mode value.
		/// </summary>
		public uint Mode { get; set; }

		/// <summary>
		/// Initializes a new instance of the Weather struct with specified field values.
		/// </summary>
		/// <param name="val1">The val1 value.</param>
		/// <param name="type">The type value.</param>
		/// <param name="mode">The mode value.</param>
		public Weather(uint val1, uint type, uint mode) : this() {
			Val1 = val1;
			Type = type;
			Mode = mode;
		}

		/// <summary>
		/// Initializes a new instance of the Weather struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public Weather(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the Weather struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public Weather(BinaryReader br) : this() {
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
			Val1 = br.ReadUInt32();
			Type = br.ReadUInt32();
			//Mode = br.ReadUInt32();
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
			bw.Write(Val1);
			bw.Write(Type);
			//bw.Write(Mode);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Weather {\n";
			ret += "	Val1 = ";
			try {
				ret += $"{ Indentify(Val1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Type = ";
			try {
				ret += $"{ Indentify(Type) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Mode = ";
			try {
				ret += $"{ Indentify(Mode) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}