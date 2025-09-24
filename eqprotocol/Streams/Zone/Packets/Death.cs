using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct Death_Struct
// {
// /*000*/	uint32	spawn_id;
// /*004*/	uint32	killer_id;
// /*008*/	uint32	corpseid;	// was corpseid
// /*012*/	uint32	attack_skill;	// was type
// /*016*/	uint32	spell_id;
// /*020*/ uint32	bindzoneid;	//bindzoneid?
// /*024*/	uint32	damage;
// /*028*/	uint32	unknown028;
// };

// ENCODE/DECODE Section:
// Handler function not found.

//public struct Death : IEQStruct
//{
//    public uint SpawnId;
//    public uint KillerId;
//    public uint CorpseId;
//    public uint AttackSkill;
//    public uint SpellId;
//    public uint BindZoneId;
//    public uint Damage;
//    uint unknown028;

//    public Death(uint SpawnId, uint KillerId, uint CorpseId, uint AttackSkill, uint SpellId, uint BindZoneId, uint Damage) : this()
//    {
//        this.SpawnId = SpawnId;
//        this.KillerId = KillerId;
//        this.CorpseId = CorpseId;
//        this.AttackSkill = AttackSkill;
//        this.SpellId = SpellId;
//        this.BindZoneId = BindZoneId;
//        this.Damage = Damage;
//    }

//    public Death(byte[] data, int offset = 0) : this()
//    {
//        Unpack(data, offset);
//    }
//    public Death(BinaryReader br) : this()
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
//        SpawnId = br.ReadUInt32();
//        KillerId = br.ReadUInt32();
//        CorpseId = br.ReadUInt32();
//        AttackSkill = br.ReadUInt32();
//        SpellId = br.ReadUInt32();
//        BindZoneId = br.ReadUInt32();
//        Damage = br.ReadUInt32();
//        unknown028 = br.ReadUInt32();
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
//        bw.Write(SpawnId);
//        bw.Write(KillerId);
//        bw.Write(CorpseId);
//        bw.Write(AttackSkill);
//        bw.Write(SpellId);
//        bw.Write(BindZoneId);
//        bw.Write(Damage);
//        bw.Write(unknown028);
//    }

//    public override string ToString()
//    {
//        var ret = "struct Death {\n";
//        ret += "\tSpawnId = ";
//        try
//        {
//            ret += $"{Indentify(SpawnId)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tKillerId = ";
//        try
//        {
//            ret += $"{Indentify(KillerId)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tCorpseId = ";
//        try
//        {
//            ret += $"{Indentify(CorpseId)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tAttackSkill = ";
//        try
//        {
//            ret += $"{Indentify(AttackSkill)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tSpellId = ";
//        try
//        {
//            ret += $"{Indentify(SpellId)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tBindZoneId = ";
//        try
//        {
//            ret += $"{Indentify(BindZoneId)},\n";
//        }
//        catch (NullReferenceException)
//        {
//            ret += "!!NULL!!\n";
//        }
//        ret += "\tDamage = ";
//        try
//        {
//            ret += $"{Indentify(Damage)},\n";
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
    /// Represents the Death packet structure for EverQuest network communication.
    /// </summary>
    public struct Death : IEQStruct
    {
        public uint SpawnId;
        public uint KillerId;
        public uint CorpseId;
        public uint AttackSkill;
        public uint SpellId;
        public uint BindZoneId;
        public uint Damage;
        uint unknown028;

        public Death(uint SpawnId, uint KillerId, uint CorpseId, uint AttackSkill, uint SpellId, uint BindZoneId, uint Damage) : this()
        {
            this.SpawnId = SpawnId;
            this.KillerId = KillerId;
            this.CorpseId = CorpseId;
            this.AttackSkill = AttackSkill;
            this.SpellId = SpellId;
            this.BindZoneId = BindZoneId;
            this.Damage = Damage;
        }

        public Death(byte[] data, int offset = 0) : this()
        {
            Unpack(data, offset);
        }
        public Death(BinaryReader br) : this()
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
            SpawnId = br.ReadUInt32();
            KillerId = br.ReadUInt32();
            CorpseId = br.ReadUInt32();
            AttackSkill = br.ReadUInt32();
            SpellId = br.ReadUInt32();
            BindZoneId = br.ReadUInt32();
            Damage = br.ReadUInt32();
            unknown028 = br.ReadUInt32();
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
            bw.Write(SpawnId);
            bw.Write(KillerId);
            bw.Write(CorpseId);
            bw.Write(AttackSkill);
            bw.Write(SpellId);
            bw.Write(BindZoneId);
            bw.Write(Damage);
            bw.Write(unknown028);
        }

        public override string ToString()
        {
            var ret = "struct Death {\n";
            ret += "\tSpawnId = ";
            try
            {
                ret += $"{Indentify(SpawnId)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tKillerId = ";
            try
            {
                ret += $"{Indentify(KillerId)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tCorpseId = ";
            try
            {
                ret += $"{Indentify(CorpseId)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tAttackSkill = ";
            try
            {
                ret += $"{Indentify(AttackSkill)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tSpellId = ";
            try
            {
                ret += $"{Indentify(SpellId)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tBindZoneId = ";
            try
            {
                ret += $"{Indentify(BindZoneId)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            ret += "\tDamage = ";
            try
            {
                ret += $"{Indentify(Damage)},\n";
            }
            catch (NullReferenceException)
            {
                ret += "!!NULL!!\n";
            }
            return ret + "}";
        }
    }
}