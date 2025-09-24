using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Object_Struct {
// /*00*/	uint32	linked_list_addr[2];// They are, get this, prev and next, ala linked list
// /*08*/	uint32	unknown008;			// Something related to the linked list?
// /*12*/	uint32	drop_id;			// Unique object id for zone
// /*16*/	uint16	zone_id;			// Redudant, but: Zone the object appears in
// /*18*/	uint16	zone_instance;		//
// /*20*/	uint32	unknown020;			// 00 00 00 00
// /*24*/	uint32	unknown024;			// 53 9e f9 7e - same for all objects in the zone?
// /*40*/	float	heading;			// heading
// /*00*/	float	x_tilt;				//Tilt entire object on X axis
// /*00*/	float	y_tilt;				//Tilt entire object on Y axis
// /*28*/	float	size;			// Size - default 1
// /*44*/	float	z;					// z coord
// /*48*/	float	x;					// x coord
// /*52*/	float	y;					// y coord
// /*56*/	char	object_name[32];	// Name of object, usually something like IT63_ACTORDEF was [20]
// /*88*/	uint32	unknown088;			// unique ID?  Maybe for a table that includes the contents?
// /*92*/	uint32	object_type;		// Type of object, not directly translated to OP_OpenObject
// /*96*/	uint8	unknown096[4];		// ff ff ff ff
// /*100*/	uint32	spawn_id;			// Spawn Id of client interacting with object
// /*104*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_GroundSpawn)
// {
// // We are not encoding the spawn_id field here, or a size but it doesn't appear to matter.
// //
// EQApplicationPacket *in = *p;
// *p = nullptr;
// 
// Object_Struct *emu = (Object_Struct *)in->pBuffer;
// unsigned char *__emu_buffer = in->pBuffer;
// in->size = strlen(emu->object_name) + 58;
// in->pBuffer = new unsigned char[in->size];
// char *OutBuffer = (char *)in->pBuffer;
// 
// VARSTRUCT_ENCODE_TYPE(uint32, OutBuffer, emu->drop_id);
// VARSTRUCT_ENCODE_STRING(OutBuffer, emu->object_name);
// VARSTRUCT_ENCODE_TYPE(uint16, OutBuffer, emu->zone_id);
// VARSTRUCT_ENCODE_TYPE(uint16, OutBuffer, emu->zone_instance);
// VARSTRUCT_ENCODE_TYPE(uint32, OutBuffer, 0);	// Unknown, observed 0x00006762
// VARSTRUCT_ENCODE_TYPE(uint32, OutBuffer, 0);	// Unknown, observer 0x7fffbb64
// VARSTRUCT_ENCODE_TYPE(float, OutBuffer, emu->heading);
// // This next field is actually a float. There is a groundspawn in freeportwest (sack of money sitting on some barrels) which requires this
// // field to be set to (float)255.0 to appear at all, and also the size field below to be 5, to be the correct size. I think SoD has the same
// // issue.
// VARSTRUCT_ENCODE_TYPE(float, OutBuffer, emu->tilt_x); //X tilt
// VARSTRUCT_ENCODE_TYPE(float, OutBuffer, emu->tilt_y);	//Y tilt
// VARSTRUCT_ENCODE_TYPE(float, OutBuffer, emu->size != 0 && (float)emu->size < 5000.f ? (float)((float)emu->size / 100.0f) : 1.f );	// This appears to be the size field. Hackish logic because some PEQ DB items were corrupt.
// VARSTRUCT_ENCODE_TYPE(float, OutBuffer, emu->y);
// VARSTRUCT_ENCODE_TYPE(float, OutBuffer, emu->x);
// VARSTRUCT_ENCODE_TYPE(float, OutBuffer, emu->z);
// VARSTRUCT_ENCODE_TYPE(uint32, OutBuffer, emu->object_type);	// Unknown, observed 0x00000014
// VARSTRUCT_ENCODE_TYPE(uint32, OutBuffer, 0xffffffff);	// Unknown, observed 0xffffffff
// VARSTRUCT_ENCODE_TYPE(uint32, OutBuffer, 0);	// Unknown, observed 0x00000014
// VARSTRUCT_ENCODE_TYPE(uint8, OutBuffer, 0);	// Unknown, observed 0x00
// 
// delete[] __emu_buffer;
// dest->FastQueuePacket(&in, ack_req);
// }
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct GroundSpawn
//{
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
//    public uint[] LinkedListAddr;  // Prev and next pointers (2 * 4 = 8 bytes)
//    public uint Unknown008;         // Something related to the linked list
//    public uint DropID;             // Unique object id for zone
//    public ushort ZoneID;           // Zone the object appears in
//    public ushort ZoneInstance;     // Zone instance
//    public uint Unknown020;         // 00 00 00 00
//    public uint Unknown024;         // Seems to be zone-specific
//    public float Size;              // Size - default 1
//    public float XTilt;             // Tilt entire object on X axis
//    public float YTilt;             // Tilt entire object on Y axis
//    public float Heading;           // Heading
//    public float Z;                 // Z coord
//    public float X;                 // X coord
//    public float Y;                 // Y coord
//    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
//    public string ObjectName;       // Name of object
//    public uint Unknown088;         // Unique ID for table contents
//    public uint ObjectType;         // Type of object
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
//    public byte[] Unknown096;       // ff ff ff ff
//    public uint SpawnID;            // Spawn Id of client interacting

//    public GroundSpawn(byte[] data, int offset = 0)
//    {
//        var availableBytes = data.Length - offset;
//        if (availableBytes < 104)  // Object_Struct is 104 bytes
//        {
//            // Initialize with defaults if not enough data
//            LinkedListAddr = new uint[2];
//            Unknown008 = 0;
//            DropID = 0;
//            ZoneID = 0;
//            ZoneInstance = 0;
//            Unknown020 = 0;
//            Unknown024 = 0;
//            Size = 1.0f;
//            XTilt = 0;
//            YTilt = 0;
//            Heading = 0;
//            Z = 0;
//            X = 0;
//            Y = 0;
//            ObjectName = "";
//            Unknown088 = 0;
//            ObjectType = 0;
//            Unknown096 = new byte[4];
//            SpawnID = 0;
//            return;
//        }

//        using (var ms = new MemoryStream(data, offset, 104))
//        using (var br = new BinaryReader(ms))
//        {
//            LinkedListAddr = new uint[] { br.ReadUInt32(), br.ReadUInt32() };
//            Unknown008 = br.ReadUInt32();
//            DropID = br.ReadUInt32();
//            ZoneID = br.ReadUInt16();
//            ZoneInstance = br.ReadUInt16();
//            Unknown020 = br.ReadUInt32();
//            Unknown024 = br.ReadUInt32();
//            Size = br.ReadSingle();
//            XTilt = br.ReadSingle();
//            YTilt = br.ReadSingle();
//            Heading = br.ReadSingle();
//            Z = br.ReadSingle();
//            X = br.ReadSingle();
//            Y = br.ReadSingle();

//            var nameBytes = br.ReadBytes(32);
//            ObjectName = System.Text.Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');

//            Unknown088 = br.ReadUInt32();
//            ObjectType = br.ReadUInt32();
//            Unknown096 = br.ReadBytes(4);
//            SpawnID = br.ReadUInt32();
//        }
//    }
//}
namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the GroundSpawn packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GroundSpawn
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] LinkedListAddr;  // Prev and next pointers (2 * 4 = 8 bytes)
        public uint Unknown008;         // Something related to the linked list
        public uint DropID;             // Unique object id for zone
        public ushort ZoneID;           // Zone the object appears in
        public ushort ZoneInstance;     // Zone instance
        public uint Unknown020;         // 00 00 00 00
        public uint Unknown024;         // Seems to be zone-specific
        public float Size;              // Size - default 1
        public float XTilt;             // Tilt entire object on X axis
        public float YTilt;             // Tilt entire object on Y axis
        public float Heading;           // Heading
        public float Z;                 // Z coord
        public float X;                 // X coord
        public float Y;                 // Y coord
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string ObjectName;       // Name of object
        public uint Unknown088;         // Unique ID for table contents
        public uint ObjectType;         // Type of object
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Unknown096;       // ff ff ff ff
        public uint SpawnID;            // Spawn Id of client interacting

        public GroundSpawn(byte[] data, int offset = 0)
        {
            var availableBytes = data.Length - offset;
            if (availableBytes < 104)  // Object_Struct is 104 bytes
            {
                // Initialize with defaults if not enough data
                LinkedListAddr = new uint[2];
                Unknown008 = 0;
                DropID = 0;
                ZoneID = 0;
                ZoneInstance = 0;
                Unknown020 = 0;
                Unknown024 = 0;
                Size = 1.0f;
                XTilt = 0;
                YTilt = 0;
                Heading = 0;
                Z = 0;
                X = 0;
                Y = 0;
                ObjectName = "";
                Unknown088 = 0;
                ObjectType = 0;
                Unknown096 = new byte[4];
                SpawnID = 0;
                return;
            }

            using (var ms = new MemoryStream(data, offset, 104))
            using (var br = new BinaryReader(ms))
            {
                LinkedListAddr = new uint[] { br.ReadUInt32(), br.ReadUInt32() };
                Unknown008 = br.ReadUInt32();
                DropID = br.ReadUInt32();
                ZoneID = br.ReadUInt16();
                ZoneInstance = br.ReadUInt16();
                Unknown020 = br.ReadUInt32();
                Unknown024 = br.ReadUInt32();
                Size = br.ReadSingle();
                XTilt = br.ReadSingle();
                YTilt = br.ReadSingle();
                Heading = br.ReadSingle();
                Z = br.ReadSingle();
                X = br.ReadSingle();
                Y = br.ReadSingle();

                var nameBytes = br.ReadBytes(32);
                ObjectName = System.Text.Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');

                Unknown088 = br.ReadUInt32();
                ObjectType = br.ReadUInt32();
                Unknown096 = br.ReadBytes(4);
                SpawnID = br.ReadUInt32();
            }
        }
	
		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct GroundSpawn {\n";
			ret += "	LinkedListAddr = ";
			try {
				ret += $"{ Indentify(LinkedListAddr) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown008 = ";
			try {
				ret += $"{ Indentify(Unknown008) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	DropID = ";
			try {
				ret += $"{ Indentify(DropID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneID = ";
			try {
				ret += $"{ Indentify(ZoneID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ZoneInstance = ";
			try {
				ret += $"{ Indentify(ZoneInstance) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown020 = ";
			try {
				ret += $"{ Indentify(Unknown020) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown024 = ";
			try {
				ret += $"{ Indentify(Unknown024) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Heading = ";
			try {
				ret += $"{ Indentify(Heading) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	XTilt = ";
			try {
				ret += $"{ Indentify(XTilt) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	YTilt = ";
			try {
				ret += $"{ Indentify(YTilt) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Size = ";
			try {
				ret += $"{ Indentify(Size) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Z = ";
			try {
				ret += $"{ Indentify(Z) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	X = ";
			try {
				ret += $"{ Indentify(X) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Y = ";
			try {
				ret += $"{ Indentify(Y) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ObjectName = ";
			try {
				ret += $"{ Indentify(ObjectName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown088 = ";
			try {
				ret += $"{ Indentify(Unknown088) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	ObjectType = ";
			try {
				ret += $"{ Indentify(ObjectType) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown096 = ";
			try {
				ret += $"{ Indentify(Unknown096) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	SpawnID = ";
			try {
				ret += $"{ Indentify(SpawnID) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}