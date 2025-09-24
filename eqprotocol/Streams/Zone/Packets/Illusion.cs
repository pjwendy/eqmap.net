using OpenEQ.Netcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Illusion_Struct {  //size: 256
// /*000*/	uint32	spawnid;
// /*004*/	char	charname[64];	//
// /*068*/	uint16	race;			//
// /*070*/	char	unknown006[2];	// Weird green name
// /*072*/	uint8	gender;
// /*073*/	uint8	texture;
// /*074*/	uint8	unknown074;		//
// /*075*/	uint8	unknown075;		//
// /*076*/	uint8	helmtexture;	//
// /*077*/	uint8	unknown077;		//
// /*078*/	uint8	unknown078;		//
// /*079*/	uint8	unknown079;		//
// /*080*/	uint32	face;			//
// /*084*/	uint8	hairstyle;		// Some Races don't change Hair Style Properly in SoF
// /*085*/	uint8	haircolor;		//
// /*086*/	uint8	beard;			//
// /*087*/	uint8	beardcolor;		//
// /*088*/ float	size;			//
// /*092*/	uint8	unknown092[148];
// /*240*/ uint32	unknown240;		// Removes armor?
// /*244*/ uint32	drakkin_heritage;	//
// /*248*/ uint32	drakkin_tattoo;		//
// /*252*/ uint32	drakkin_details;	//
// /*256*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_Illusion)
// {
// ENCODE_LENGTH_EXACT(Illusion_Struct);
// SETUP_DIRECT_ENCODE(Illusion_Struct, structs::Illusion_Struct);
// 
// OUT(spawnid);
// OUT_str(charname);
// OUT(race);
// OUT(unknown006[0]);
// OUT(unknown006[1]);
// OUT(gender);
// OUT(texture);
// OUT(helmtexture);
// OUT(face);
// OUT(hairstyle);
// OUT(haircolor);
// OUT(beard);
// OUT(beardcolor);
// OUT(size);
// OUT(drakkin_heritage);
// OUT(drakkin_tattoo);
// OUT(drakkin_details);
// 
// FINISH_ENCODE();
// }
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct Illusion
//{
//    public uint SpawnId;
//    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
//    public string CharName;
//    public ushort Race;
//    public ushort Unknown006;
//    public byte Gender;
//    public byte Texture;
//    public byte Unknown074;
//    public byte Unknown075;
//    public byte HelmTexture;
//    public byte Unknown077;
//    public byte Unknown078;
//    public byte Unknown079;
//    public uint Face;
//    public byte HairStyle;
//    public byte HairColor;
//    public byte Beard;
//    public byte BeardColor;
//    public float Size;

//    public Illusion(byte[] data, int offset = 0)
//    {
//        using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 92)))
//        using (var br = new BinaryReader(ms))
//        {
//            SpawnId = br.ReadUInt32();
//            var nameBytes = br.ReadBytes(64);
//            CharName = System.Text.Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');
//            Race = br.ReadUInt16();
//            Unknown006 = br.ReadUInt16();
//            Gender = br.ReadByte();
//            Texture = br.ReadByte();
//            Unknown074 = br.ReadByte();
//            Unknown075 = br.ReadByte();
//            HelmTexture = br.ReadByte();
//            Unknown077 = br.ReadByte();
//            Unknown078 = br.ReadByte();
//            Unknown079 = br.ReadByte();
//            Face = br.ReadUInt32();
//            HairStyle = br.ReadByte();
//            HairColor = br.ReadByte();
//            Beard = br.ReadByte();
//            BeardColor = br.ReadByte();
//            Size = br.ReadSingle();
//        }
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the Illusion packet structure for EverQuest network communication.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Illusion
    {
        public uint SpawnId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string CharName;
        public ushort Race;
        public ushort Unknown006;
        public byte Gender;
        public byte Texture;
        public byte Unknown074;
        public byte Unknown075;
        public byte HelmTexture;
        public byte Unknown077;
        public byte Unknown078;
        public byte Unknown079;
        public uint Face;
        public byte HairStyle;
        public byte HairColor;
        public byte Beard;
        public byte BeardColor;
        public float Size;

        public Illusion(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, Math.Min(data.Length - offset, 92)))
            using (var br = new BinaryReader(ms))
            {
                SpawnId = br.ReadUInt32();
                var nameBytes = br.ReadBytes(64);
                CharName = System.Text.Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');
                Race = br.ReadUInt16();
                Unknown006 = br.ReadUInt16();
                Gender = br.ReadByte();
                Texture = br.ReadByte();
                Unknown074 = br.ReadByte();
                Unknown075 = br.ReadByte();
                HelmTexture = br.ReadByte();
                Unknown077 = br.ReadByte();
                Unknown078 = br.ReadByte();
                Unknown079 = br.ReadByte();
                Face = br.ReadUInt32();
                HairStyle = br.ReadByte();
                HairColor = br.ReadByte();
                Beard = br.ReadByte();
                BeardColor = br.ReadByte();
                Size = br.ReadSingle();                
            }
        }
	
		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct Illusion {\n";
			ret += "	SpawnId = ";
			try {
				ret += $"{ Indentify(SpawnId) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	CharName = ";
			try {
				ret += $"{ Indentify(CharName) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Race = ";
			try {
				ret += $"{ Indentify(Race) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown006 = ";
			try {
				ret += $"{ Indentify(Unknown006) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Gender = ";
			try {
				ret += $"{ Indentify(Gender) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Texture = ";
			try {
				ret += $"{ Indentify(Texture) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown074 = ";
			try {
				ret += $"{ Indentify(Unknown074) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown075 = ";
			try {
				ret += $"{ Indentify(Unknown075) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	HelmTexture = ";
			try {
				ret += $"{ Indentify(HelmTexture) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown077 = ";
			try {
				ret += $"{ Indentify(Unknown077) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown078 = ";
			try {
				ret += $"{ Indentify(Unknown078) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Unknown079 = ";
			try {
				ret += $"{ Indentify(Unknown079) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Face = ";
			try {
				ret += $"{ Indentify(Face) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	HairStyle = ";
			try {
				ret += $"{ Indentify(HairStyle) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	HairColor = ";
			try {
				ret += $"{ Indentify(HairColor) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Beard = ";
			try {
				ret += $"{ Indentify(Beard) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	BeardColor = ";
			try {
				ret += $"{ Indentify(BeardColor) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Size = ";
			try {
				ret += $"{ Indentify(Size) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}			
			
			return ret;
		}
	}
}