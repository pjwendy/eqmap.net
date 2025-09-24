using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition: Not found or not applicable
/*
** Client Zone Entry struct
** Length: 68 Octets
** OpCode: ZoneEntryCode (when direction == client)
*/
//struct ClientZoneEntry_Struct
//{
//    /*0000*/
//    uint32 unknown0000;            // ***Placeholder
//    /*0004*/
//    char char_name[64];               // Player firstname [32]
//                                      //*0036*/ uint8  unknown0036[28];        // ***Placeholder
//                                      //*0064*/ uint32 unknown0064;            // unknown
//};


// ENCODE/DECODE Section:
// ENCODE(OP_ZoneEntry) { ENCODE_FORWARD(OP_ZoneSpawns); }

//public struct Spawn : IEQStruct
//{
//    public string Name;
//    public uint SpawnID;
//    public byte Level;
//    public CharType CharType;
//    public SpawnBitfields SpawnFlags;
//    public byte OtherFlags;
//    public float Size;
//    public byte Face;
//    public float WalkSpeed;
//    public float RunSpeed;
//    public Race Race;
//    public byte ShowName;
//    public uint BodyType;
//    public byte CurHP;
//    public byte HairColor;
//    public byte BeardColor;
//    public byte EyeColor1;
//    public byte EyeColor2;
//    public byte HairStyle;
//    public byte Beard;
//    public uint DrakkinHeritage;
//    public uint DrakkinTattoo;
//    public uint DrakkinDetails;
//    public byte Statue;
//    public uint Deity;
//    public uint GuildID;
//    public GuildRank GuildRank;
//    public byte Class;
//    public bool PVP;
//    public byte StandState;
//    public byte Light;
//    public byte FlyMode;
//    public byte EquipChest2;
//    public byte Helm;
//    public string LastName;
//    public AATitle AATitle;
//    public uint PetOwnerID;
//    public uint PlayerState;
//    public SpawnPosition Position;
//    public TintProfile EquipmentTint;
//    public TextureProfile Equipment;
//    public string Title;
//    public string Suffix;
//    public bool IsMercenary;

//    public Spawn(string Name, uint SpawnID, byte Level, CharType CharType, SpawnBitfields SpawnFlags, byte OtherFlags, float Size, byte Face, float WalkSpeed, float RunSpeed, Race Race, byte ShowName, uint BodyType, byte CurHP, byte HairColor, byte BeardColor, byte EyeColor1, byte EyeColor2, byte HairStyle, byte Beard, uint DrakkinHeritage, uint DrakkinTattoo, uint DrakkinDetails, byte Statue, uint Deity, uint GuildID, GuildRank GuildRank, byte Class, bool PVP, byte StandState, byte Light, byte FlyMode, byte EquipChest2, byte Helm, string LastName, AATitle AATitle, uint PetOwnerID, uint PlayerState, SpawnPosition Position, TintProfile EquipmentTint, TextureProfile Equipment, string Title, string Suffix, bool IsMercenary) : this()
//    {
//        this.Name = Name;
//        this.SpawnID = SpawnID;
//        this.Level = Level;
//        this.CharType = CharType;
//        this.SpawnFlags = SpawnFlags;
//        this.OtherFlags = OtherFlags;
//        this.Size = Size;
//        this.Face = Face;
//        this.WalkSpeed = WalkSpeed;
//        this.RunSpeed = RunSpeed;
//        this.Race = Race;
//        this.ShowName = ShowName;
//        this.BodyType = BodyType;
//        this.CurHP = CurHP;
//        this.HairColor = HairColor;
//        this.BeardColor = BeardColor;
//        this.EyeColor1 = EyeColor1;
//        this.EyeColor2 = EyeColor2;
//        this.HairStyle = HairStyle;
//        this.Beard = Beard;
//        this.DrakkinHeritage = DrakkinHeritage;
//        this.DrakkinTattoo = DrakkinTattoo;
//        this.DrakkinDetails = DrakkinDetails;
//        this.Statue = Statue;
//        this.Deity = Deity;
//        this.GuildID = GuildID;
//        this.GuildRank = GuildRank;
//        this.Class = Class;
//        this.PVP = PVP;
//        this.StandState = StandState;
//        this.Light = Light;
//        this.FlyMode = FlyMode;
//        this.EquipChest2 = EquipChest2;
//        this.Helm = Helm;
//        this.LastName = LastName;
//        this.AATitle = AATitle;
//        this.PetOwnerID = PetOwnerID;
//        this.PlayerState = PlayerState;
//        this.Position = Position;
//        this.EquipmentTint = EquipmentTint;
//        this.Equipment = Equipment;
//        this.Title = Title;
//        this.Suffix = Suffix;
//        this.IsMercenary = IsMercenary;
//    }

//    public Spawn(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public Spawn(BinaryReader br) : this()
//    {
//        Unpack(br);
//    }
//    public void Unpack(byte[] data, int offset = 0)
//    {
//        using (var ms = new MemoryStream(data, offset, data.Length - offset))
//        {
//            using (var br = new BinaryReader(ms))
//            {
//                Unpack(br);
//            }
//        }
//    }
//    public void Unpack(BinaryReader br)
//    {
//        Name = br.ReadString(-1);
//        SpawnID = br.ReadUInt32();
//        Level = br.ReadByte();
//        br.ReadBytes(4);
//        CharType = ((CharType)0).Unpack(br);
//        SpawnFlags = new SpawnBitfields(br);
//        OtherFlags = br.ReadByte();
//        br.ReadBytes(8);
//        Size = br.ReadSingle();
//        Face = br.ReadByte();
//        WalkSpeed = br.ReadSingle();
//        RunSpeed = br.ReadSingle();
//        Race = ((Race)0).Unpack(br);
//        ShowName = br.ReadByte();
//        BodyType = br.ReadUInt32();
//        CurHP = br.ReadByte();
//        HairColor = br.ReadByte();
//        BeardColor = br.ReadByte();
//        EyeColor1 = br.ReadByte();
//        EyeColor2 = br.ReadByte();
//        HairStyle = br.ReadByte();
//        Beard = br.ReadByte();
//        DrakkinHeritage = br.ReadUInt32();
//        DrakkinTattoo = br.ReadUInt32();
//        DrakkinDetails = br.ReadUInt32();
//        Statue = br.ReadByte();
//        Deity = br.ReadUInt32();
//        GuildID = br.ReadUInt32();
//        GuildRank = ((GuildRank)0).Unpack(br);
//        br.ReadBytes(3);
//        Class = br.ReadByte();
//        PVP = br.ReadByte() != 0;
//        StandState = br.ReadByte();
//        Light = br.ReadByte();
//        FlyMode = br.ReadByte();
//        EquipChest2 = br.ReadByte();
//        br.ReadBytes(2);
//        Helm = br.ReadByte();
//        LastName = br.ReadString(-1);
//        AATitle = ((AATitle)0).Unpack(br);
//        br.ReadBytes(1);
//        PetOwnerID = br.ReadUInt32();
//        br.ReadBytes(1);
//        PlayerState = br.ReadUInt32();
//        br.ReadBytes(5 * 4);
//        Position = new SpawnPosition(br);
//        EquipmentTint = new TintProfile(br);
//        if (CharType == CharType.PC || (int)Race == 12 || (int)Race == 128 || (int)Race == 130 || (int)Race == 330 || (int)Race == 522)
//        {
//            Equipment = new TextureProfile(br);
//        }
//        if ((OtherFlags & 4) != 0)
//        {
//            Title = br.ReadString(-1);
//        }
//        if ((OtherFlags & 8) != 0)
//        {
//            Suffix = br.ReadString(-1);
//        }
//        br.ReadBytes(8);
//        IsMercenary = br.ReadByte() != 0;
//        br.ReadBytes(28);
//    }

//    public byte[] Pack()
//    {
//        using (var ms = new MemoryStream())
//        {
//            using (var bw = new BinaryWriter(ms))
//            {
//                Pack(bw);
//                return ms.ToArray();
//            }
//        }
//    }
//    public void Pack(BinaryWriter bw)
//    {
//        bw.Write(Name.ToBytes());
//        bw.Write(SpawnID);
//        bw.Write(Level);
//        bw.Write(new byte[4]);
//        bw.Write((byte)CharType);
//        SpawnFlags.Pack(bw);
//        bw.Write(OtherFlags);
//        bw.Write(new byte[8]);
//        bw.Write(Size);
//        bw.Write(Face);
//        bw.Write(WalkSpeed);
//        bw.Write(RunSpeed);
//        bw.Write((uint)Race);
//        bw.Write(ShowName);
//        bw.Write(BodyType);
//        bw.Write(CurHP);
//        bw.Write(HairColor);
//        bw.Write(BeardColor);
//        bw.Write(EyeColor1);
//        bw.Write(EyeColor2);
//        bw.Write(HairStyle);
//        bw.Write(Beard);
//        bw.Write(DrakkinHeritage);
//        bw.Write(DrakkinTattoo);
//        bw.Write(DrakkinDetails);
//        bw.Write(Statue);
//        bw.Write(Deity);
//        bw.Write(GuildID);
//        bw.Write((sbyte)GuildRank);
//        bw.Write(new byte[3]);
//        bw.Write(Class);
//        bw.Write((byte)(PVP ? 1 : 0));
//        bw.Write(StandState);
//        bw.Write(Light);
//        bw.Write(FlyMode);
//        bw.Write(EquipChest2);
//        bw.Write(new byte[2]);
//        bw.Write(Helm);
//        bw.Write(LastName.ToBytes());
//        bw.Write((uint)AATitle);
//        bw.Write(new byte[1]);
//        bw.Write(PetOwnerID);
//        bw.Write(new byte[1]);
//        bw.Write(PlayerState);
//        bw.Write(new byte[5 * 4]);
//        Position.Pack(bw);
//        EquipmentTint.Pack(bw);
//        if (CharType == CharType.PC || (int)Race == 12 || (int)Race == 128 || (int)Race == 130 || (int)Race == 330 || (int)Race == 522)
//        {
//            Equipment.Pack(bw);
//        }
//        if ((OtherFlags & 4) != 0)
//        {
//            bw.Write(Title.ToBytes());
//        }
//        if ((OtherFlags & 8) != 0)
//        {
//            bw.Write(Suffix.ToBytes());
//        }
//        bw.Write(new byte[8]);
//        bw.Write((byte)(IsMercenary ? 1 : 0));
//        bw.Write(new byte[28]);
//    }

//    public override string ToString()
//    {
//        var ret = "struct Spawn {\n";
//        ret += "\tName = ";
//        try
//        {
//            ret += $"{Indentify(Name)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tSpawnID = ";
//        try
//        {
//            ret += $"{Indentify(SpawnID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLevel = ";
//        try
//        {
//            ret += $"{Indentify(Level)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCharType = ";
//        try
//        {
//            ret += $"{Indentify(CharType)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tSpawnFlags = ";
//        try
//        {
//            ret += $"{Indentify(SpawnFlags)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tOtherFlags = ";
//        try
//        {
//            ret += $"{Indentify(OtherFlags)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tSize = ";
//        try
//        {
//            ret += $"{Indentify(Size)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tFace = ";
//        try
//        {
//            ret += $"{Indentify(Face)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tWalkSpeed = ";
//        try
//        {
//            ret += $"{Indentify(WalkSpeed)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tRunSpeed = ";
//        try
//        {
//            ret += $"{Indentify(RunSpeed)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tRace = ";
//        try
//        {
//            ret += $"{Indentify(Race)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tShowName = ";
//        try
//        {
//            ret += $"{Indentify(ShowName)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tBodyType = ";
//        try
//        {
//            ret += $"{Indentify(BodyType)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCurHP = ";
//        try
//        {
//            ret += $"{Indentify(CurHP)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tHairColor = ";
//        try
//        {
//            ret += $"{Indentify(HairColor)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tBeardColor = ";
//        try
//        {
//            ret += $"{Indentify(BeardColor)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tEyeColor1 = ";
//        try
//        {
//            ret += $"{Indentify(EyeColor1)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tEyeColor2 = ";
//        try
//        {
//            ret += $"{Indentify(EyeColor2)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tHairStyle = ";
//        try
//        {
//            ret += $"{Indentify(HairStyle)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tBeard = ";
//        try
//        {
//            ret += $"{Indentify(Beard)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tDrakkinHeritage = ";
//        try
//        {
//            ret += $"{Indentify(DrakkinHeritage)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tDrakkinTattoo = ";
//        try
//        {
//            ret += $"{Indentify(DrakkinTattoo)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tDrakkinDetails = ";
//        try
//        {
//            ret += $"{Indentify(DrakkinDetails)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tStatue = ";
//        try
//        {
//            ret += $"{Indentify(Statue)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tDeity = ";
//        try
//        {
//            ret += $"{Indentify(Deity)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tGuildID = ";
//        try
//        {
//            ret += $"{Indentify(GuildID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tGuildRank = ";
//        try
//        {
//            ret += $"{Indentify(GuildRank)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tClass = ";
//        try
//        {
//            ret += $"{Indentify(Class)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPVP = ";
//        try
//        {
//            ret += $"{Indentify(PVP)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tStandState = ";
//        try
//        {
//            ret += $"{Indentify(StandState)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLight = ";
//        try
//        {
//            ret += $"{Indentify(Light)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tFlyMode = ";
//        try
//        {
//            ret += $"{Indentify(FlyMode)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tEquipChest2 = ";
//        try
//        {
//            ret += $"{Indentify(EquipChest2)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tHelm = ";
//        try
//        {
//            ret += $"{Indentify(Helm)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tLastName = ";
//        try
//        {
//            ret += $"{Indentify(LastName)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAATitle = ";
//        try
//        {
//            ret += $"{Indentify(AATitle)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPetOwnerID = ";
//        try
//        {
//            ret += $"{Indentify(PetOwnerID)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPlayerState = ";
//        try
//        {
//            ret += $"{Indentify(PlayerState)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tPosition = ";
//        try
//        {
//            ret += $"{Indentify(Position)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tEquipmentTint = ";
//        try
//        {
//            ret += $"{Indentify(EquipmentTint)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        if (CharType == CharType.PC || (int)Race == 12 || (int)Race == 128 || (int)Race == 130 || (int)Race == 330 || (int)Race == 522)
//        {
//            ret += "\tEquipment = ";
//            try
//            {
//                ret += $"{Indentify(Equipment)},\n";
//            }
//            catch (NullReferenceException)
//            {
//                ret += "!!NULL!!\n";
//            }
//        }
//        if ((OtherFlags & 4) != 0)
//        {
//            ret += "\tTitle = ";
//            try
//            {
//                ret += $"{Indentify(Title)},\n";
//            }
//            catch (NullReferenceException)
//            {
//                ret += "!!NULL!!\n";
//            }
//        }
//        if ((OtherFlags & 8) != 0)
//        {
//            ret += "\tSuffix = ";
//            try
//            {
//                ret += $"{Indentify(Suffix)},\n";
//            }
//            catch (NullReferenceException)
//            {
//                ret += "!!NULL!!\n";
//            }
//        }
//        ret += "\tIsMercenary = ";
//        try
//        {
//            ret += $"{Indentify(IsMercenary)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        return ret + "}";
//    }
//}

namespace OpenEQ.Netcode {
    /// <summary>
    /// Represents the ZoneEntry packet structure for EverQuest network communication.
    /// </summary>
    public struct ZoneEntry : IEQStruct
    {
        public string Name;
        public uint SpawnID;
        public byte Level;
        public CharType CharType;
        public SpawnBitfields SpawnFlags;
        public byte OtherFlags;
        public float Size;
        public byte Face;
        public float WalkSpeed;
        public float RunSpeed;
        public Race Race;
        public byte ShowName;
        public uint BodyType;
        public byte CurHP;
        public byte HairColor;
        public byte BeardColor;
        public byte EyeColor1;
        public byte EyeColor2;
        public byte HairStyle;
        public byte Beard;
        public uint DrakkinHeritage;
        public uint DrakkinTattoo;
        public uint DrakkinDetails;
        public byte Statue;
        public uint Deity;
        public uint GuildID;
        public GuildRank GuildRank;
        public byte Class;
        public bool PVP;
        public byte StandState;
        public byte Light;
        public byte FlyMode;
        public byte EquipChest2;
        public byte Helm;
        public string LastName;
        public AATitle AATitle;
        public uint PetOwnerID;
        public uint PlayerState;
        public SpawnPosition Position;
        public TintProfile EquipmentTint;
        public TextureProfile Equipment;
        public string Title;
        public string Suffix;
        public bool IsMercenary;

        public ZoneEntry(string Name) : this()
        {
            this.Name = Name;
        }
        public ZoneEntry(string Name, uint SpawnID, byte Level, CharType CharType, SpawnBitfields SpawnFlags, byte OtherFlags, float Size, byte Face, float WalkSpeed, float RunSpeed, Race Race, byte ShowName, uint BodyType, byte CurHP, byte HairColor, byte BeardColor, byte EyeColor1, byte EyeColor2, byte HairStyle, byte Beard, uint DrakkinHeritage, uint DrakkinTattoo, uint DrakkinDetails, byte Statue, uint Deity, uint GuildID, GuildRank GuildRank, byte Class, bool PVP, byte StandState, byte Light, byte FlyMode, byte EquipChest2, byte Helm, string LastName, AATitle AATitle, uint PetOwnerID, uint PlayerState, SpawnPosition Position, TintProfile EquipmentTint, TextureProfile Equipment, string Title, string Suffix, bool IsMercenary) : this()
        {
            this.Name = Name;
            this.SpawnID = SpawnID;
            this.Level = Level;
            this.CharType = CharType;
            this.SpawnFlags = SpawnFlags;
            this.OtherFlags = OtherFlags;
            this.Size = Size;
            this.Face = Face;
            this.WalkSpeed = WalkSpeed;
            this.RunSpeed = RunSpeed;
            this.Race = Race;
            this.ShowName = ShowName;
            this.BodyType = BodyType;
            this.CurHP = CurHP;
            this.HairColor = HairColor;
            this.BeardColor = BeardColor;
            this.EyeColor1 = EyeColor1;
            this.EyeColor2 = EyeColor2;
            this.HairStyle = HairStyle;
            this.Beard = Beard;
            this.DrakkinHeritage = DrakkinHeritage;
            this.DrakkinTattoo = DrakkinTattoo;
            this.DrakkinDetails = DrakkinDetails;
            this.Statue = Statue;
            this.Deity = Deity;
            this.GuildID = GuildID;
            this.GuildRank = GuildRank;
            this.Class = Class;
            this.PVP = PVP;
            this.StandState = StandState;
            this.Light = Light;
            this.FlyMode = FlyMode;
            this.EquipChest2 = EquipChest2;
            this.Helm = Helm;
            this.LastName = LastName;
            this.AATitle = AATitle;
            this.PetOwnerID = PetOwnerID;
            this.PlayerState = PlayerState;
            this.Position = Position;
            this.EquipmentTint = EquipmentTint;
            this.Equipment = Equipment;
            this.Title = Title;
            this.Suffix = Suffix;
            this.IsMercenary = IsMercenary;
        }

        public ZoneEntry(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public ZoneEntry(BinaryReader br) : this()
        {
            Unpack(br);
        }
        public void Unpack(byte[] data, int offset = 0)
        {
            using (var ms = new MemoryStream(data, offset, data.Length - offset))
            {
                using (var br = new BinaryReader(ms))
                {
                    Unpack(br);
                }
            }
        }
        public void Unpack(BinaryReader br)
        {
            Name = br.ReadString(-1);
            SpawnID = br.ReadUInt32();
            Level = br.ReadByte();
            br.ReadBytes(4);
            CharType = ((CharType)0).Unpack(br);
            SpawnFlags = new SpawnBitfields(br);
            OtherFlags = br.ReadByte();
            br.ReadBytes(8);
            Size = br.ReadSingle();
            Face = br.ReadByte();
            WalkSpeed = br.ReadSingle();
            RunSpeed = br.ReadSingle();
            Race = ((Race)0).Unpack(br);
            ShowName = br.ReadByte();
            BodyType = br.ReadUInt32();
            CurHP = br.ReadByte();
            HairColor = br.ReadByte();
            BeardColor = br.ReadByte();
            EyeColor1 = br.ReadByte();
            EyeColor2 = br.ReadByte();
            HairStyle = br.ReadByte();
            Beard = br.ReadByte();
            DrakkinHeritage = br.ReadUInt32();
            DrakkinTattoo = br.ReadUInt32();
            DrakkinDetails = br.ReadUInt32();
            Statue = br.ReadByte();
            Deity = br.ReadUInt32();
            GuildID = br.ReadUInt32();
            GuildRank = ((GuildRank)0).Unpack(br);
            br.ReadBytes(3);
            Class = br.ReadByte();
            PVP = br.ReadByte() != 0;
            StandState = br.ReadByte();
            Light = br.ReadByte();
            FlyMode = br.ReadByte();
            EquipChest2 = br.ReadByte();
            br.ReadBytes(2);
            Helm = br.ReadByte();
            LastName = br.ReadString(-1);
            AATitle = ((AATitle)0).Unpack(br);
            br.ReadBytes(1);
            PetOwnerID = br.ReadUInt32();
            br.ReadBytes(1);
            PlayerState = br.ReadUInt32();
            br.ReadBytes(5 * 4);
            Position = new SpawnPosition(br);
            EquipmentTint = new TintProfile(br);
            if (CharType == CharType.PC || (int)Race == 12 || (int)Race == 128 || (int)Race == 130 || (int)Race == 330 || (int)Race == 522)
            {
                Equipment = new TextureProfile(br);
            }
            if ((OtherFlags & 4) != 0)
            {
                Title = br.ReadString(-1);
            }
            if ((OtherFlags & 8) != 0)
            {
                Suffix = br.ReadString(-1);
            }
            br.ReadBytes(8);
            IsMercenary = br.ReadByte() != 0;
            br.ReadBytes(28);
        }

        public byte[] Pack()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    Pack(bw);
                    return ms.ToArray();
                }
            }
        }
        public void Pack(BinaryWriter bw)
        {
            bw.Write(new uint());
            bw.Write(Name.ToBytes(64));           
        }

        public override string ToString()
        {
            var ret = "struct ZoneEntry {\n";
            ret += "\tName = ";
            try
            {
                ret += $"{Indentify(Name)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tSpawnID = ";
            try
            {
                ret += $"{Indentify(SpawnID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLevel = ";
            try
            {
                ret += $"{Indentify(Level)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCharType = ";
            try
            {
                ret += $"{Indentify(CharType)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tSpawnFlags = ";
            try
            {
                ret += $"{Indentify(SpawnFlags)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tOtherFlags = ";
            try
            {
                ret += $"{Indentify(OtherFlags)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tSize = ";
            try
            {
                ret += $"{Indentify(Size)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tFace = ";
            try
            {
                ret += $"{Indentify(Face)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tWalkSpeed = ";
            try
            {
                ret += $"{Indentify(WalkSpeed)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tRunSpeed = ";
            try
            {
                ret += $"{Indentify(RunSpeed)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tRace = ";
            try
            {
                ret += $"{Indentify(Race)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tShowName = ";
            try
            {
                ret += $"{Indentify(ShowName)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tBodyType = ";
            try
            {
                ret += $"{Indentify(BodyType)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCurHP = ";
            try
            {
                ret += $"{Indentify(CurHP)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tHairColor = ";
            try
            {
                ret += $"{Indentify(HairColor)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tBeardColor = ";
            try
            {
                ret += $"{Indentify(BeardColor)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tEyeColor1 = ";
            try
            {
                ret += $"{Indentify(EyeColor1)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tEyeColor2 = ";
            try
            {
                ret += $"{Indentify(EyeColor2)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tHairStyle = ";
            try
            {
                ret += $"{Indentify(HairStyle)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tBeard = ";
            try
            {
                ret += $"{Indentify(Beard)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDrakkinHeritage = ";
            try
            {
                ret += $"{Indentify(DrakkinHeritage)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDrakkinTattoo = ";
            try
            {
                ret += $"{Indentify(DrakkinTattoo)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDrakkinDetails = ";
            try
            {
                ret += $"{Indentify(DrakkinDetails)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tStatue = ";
            try
            {
                ret += $"{Indentify(Statue)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDeity = ";
            try
            {
                ret += $"{Indentify(Deity)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGuildID = ";
            try
            {
                ret += $"{Indentify(GuildID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tGuildRank = ";
            try
            {
                ret += $"{Indentify(GuildRank)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tClass = ";
            try
            {
                ret += $"{Indentify(Class)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPVP = ";
            try
            {
                ret += $"{Indentify(PVP)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tStandState = ";
            try
            {
                ret += $"{Indentify(StandState)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLight = ";
            try
            {
                ret += $"{Indentify(Light)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tFlyMode = ";
            try
            {
                ret += $"{Indentify(FlyMode)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tEquipChest2 = ";
            try
            {
                ret += $"{Indentify(EquipChest2)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tHelm = ";
            try
            {
                ret += $"{Indentify(Helm)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tLastName = ";
            try
            {
                ret += $"{Indentify(LastName)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAATitle = ";
            try
            {
                ret += $"{Indentify(AATitle)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPetOwnerID = ";
            try
            {
                ret += $"{Indentify(PetOwnerID)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPlayerState = ";
            try
            {
                ret += $"{Indentify(PlayerState)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tPosition = ";
            try
            {
                ret += $"{Indentify(Position)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tEquipmentTint = ";
            try
            {
                ret += $"{Indentify(EquipmentTint)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            if (CharType == CharType.PC || (int)Race == 12 || (int)Race == 128 || (int)Race == 130 || (int)Race == 330 || (int)Race == 522)
            {
                ret += "\tEquipment = ";
                try
                {
                    ret += $"{Indentify(Equipment)},\n";
                }
                catch (NullReferenceException)
                {
                    ret += "!!NULL!!\n";
                }
            }
            if ((OtherFlags & 4) != 0)
            {
                ret += "\tTitle = ";
                try
                {
                    ret += $"{Indentify(Title)},\n";
                }
                catch (NullReferenceException)
                {
                    ret += "!!NULL!!\n";
                }
            }
            if ((OtherFlags & 8) != 0)
            {
                ret += "\tSuffix = ";
                try
                {
                    ret += $"{Indentify(Suffix)},\n";
                }
                catch (NullReferenceException)
                {
                    ret += "!!NULL!!\n";
                }
            }
            ret += "\tIsMercenary = ";
            try
            {
                ret += $"{Indentify(IsMercenary)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}