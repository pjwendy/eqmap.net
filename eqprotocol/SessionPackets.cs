﻿using System;
using System.Runtime.InteropServices;

namespace OpenEQ.Netcode {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SessionRequest {
        uint unknown;
        public uint sessionID;
        uint maxLength;

        public SessionRequest(uint sessID) {
            unknown = 2;
            sessionID = sessID;
            maxLength = 0x00020000; //512; // Need network order
        }
    }

    [Flags]
    public enum ValidationMode : byte {
        None = 0,
        Crc = 2
    }

    [Flags]
    public enum FilterMode : byte {
        None = 0,
        Compressed = 1,
        Encoded = 4
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SessionResponse {
        public uint sessionId;
        public uint crcKey;
        public ValidationMode validationMode;
        public FilterMode filterMode;
        public byte unknownA;
        public uint maxLength;
    }
}