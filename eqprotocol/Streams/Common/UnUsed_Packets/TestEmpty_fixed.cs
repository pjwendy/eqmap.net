using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition: Not found or not applicable

// ENCODE/DECODE Section:
// Handler function not found.

namespace EQProtocol.Streams.Common.UnUsed_Packets {
	public struct TestEmpty : IEQStruct {
				// No properties - structure definition not found or empty
		// C# 8.0: Parameterless struct constructors not supported - uncomment if using C# 10.0+
		// public TestEmpty() : this() {
		// 		// No assignments needed
		// }

		public TestEmpty(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}
		public TestEmpty(BinaryReader br) : this() {
			Unpack(br);
		}
		public void Unpack(byte[] data, int offset = 0) {
			using(var ms = new MemoryStream(data, offset, data.Length - offset)) {
				using(var br = new BinaryReader(ms)) {
					Unpack(br);
				}
			}
		}
		public void Unpack(BinaryReader br) {
						// No data to read
		}

		public byte[] Pack() {
			using(var ms = new MemoryStream()) {
				using(var bw = new BinaryWriter(ms)) {
					Pack(bw);
					return ms.ToArray();
				}
			}
		}
		public void Pack(BinaryWriter bw) {
						// No data to write
		}

		public override string ToString() {
			var ret = "struct TestEmpty {\n";
						ret += "	// No properties\n";
			ret += "}";
			
			return ret;
		}
	}
}