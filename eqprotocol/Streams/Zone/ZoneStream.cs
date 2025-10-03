using EQProtocol.Streams.Common;
using OpenEQ.Netcode;
using OpenEQ.Netcode.GameClient.Models;
using System;
using static OpenEQ.Netcode.Utility;

namespace EQProtocol.Streams.Zone
{
    public class ZoneStream : EQStream
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        readonly string CharName;
        bool Entering = true;
        ushort PlayerSpawnId;
        ushort UpdateSequence;

        public event EventHandler<ZoneEntry> ZoneEntry;
        public event EventHandler<ClientUpdateFromServer> ClientUpdate;
        public event EventHandler<MobUpdate> MobUpdate;
        public event EventHandler<NPCMoveUpdate> NPCMoveUpdate;
        public event EventHandler<PlayerProfile> PlayerProfile;
        public event EventHandler<ChannelMessage> Message;
        public event EventHandler<Death> Death;
        public event EventHandler<string> Zoned;
        public event EventHandler<Consider> Consider;
        public event EventHandler<MobHealth> MobHealth;
        public event EventHandler<Damage> Damage;
        public event EventHandler<CastSpell> CastSpell;
        public event EventHandler<InterruptCast> InterruptCast;
        public event EventHandler<Animation> Animation;
        public event EventHandler<Buff> Buff;
        public event EventHandler<GroundSpawn> GroundSpawn;
        public event EventHandler<Track> Track;
        public event EventHandler<Emote> Emote;
        public event EventHandler<uint> DeleteSpawn;
        public event EventHandler<ExpUpdate> ExpUpdate;
        public event EventHandler<LevelUpdate> LevelUpdate;
        public event EventHandler<SkillUpdate> SkillUpdate;
        public event EventHandler<WearChange> WearChange;
        public event EventHandler<MoveItem> MoveItem;
        public event EventHandler<Assist> Assist;
        public event EventHandler<byte[]> AutoAttack;
        public event EventHandler<Charm> Charm;
        public event EventHandler<Stun> Stun;
        public event EventHandler<Illusion> Illusion;
        public event EventHandler<Sound> Sound;
        public event EventHandler<byte[]> Hide;
        public event EventHandler<byte[]> Sneak;
        public event EventHandler<byte[]> FeignDeath;

        protected override string GetOpcodeNameForLogging(ushort opcode)
        {
            if (Enum.IsDefined(typeof(ZoneOp), opcode))
            {
                return Enum.GetName(typeof(ZoneOp), opcode);
            }
            return $"Unknown_0x{opcode:X04}";
        }

        public ZoneStream(string host, int port, string charName) : base(host, port)
        {
            SendKeepalives = true;
            CharName = charName;

            Logger.Debug("Starting zone connection...");
            Connect();
            SendSessionRequest();
        }

        protected override void HandleSessionResponse(Packet packet)
        {
            Send(packet);
            Send(AppPacket.Create(ZoneOp.ZoneEntry, new ZoneEntry(CharName)));
        }

        protected override void HandleAppPacket(AppPacket packet)
        {
            //Logger.Debug($"Zone app packet: {(ZoneOp) packet.Opcode}");
            switch ((ZoneOp)packet.Opcode)
            {
                case ZoneOp.PlayerProfile:
                    var player = packet.Get<PlayerProfile>();
                    Logger.Debug($"Zone | PlayerProfile received for {player.Name} (Level {player.Level}) at zone {player.ZoneID}");
                    PlayerProfile?.Invoke(this, player);
                    break;

                case ZoneOp.TimeOfDay:
                    var timeofday = packet.Get<TimeOfDay>();
                    //Logger.Debug(timeofday);
                    break;

                case ZoneOp.TaskActivity:
                    // XXX: Handle short activities!
                    //var activity = packet.Get<TaskActivity>();
                    //Logger.Debug(activity);
                    break;

                case ZoneOp.TaskDescription:
                    var desc = packet.Get<TaskDescription>();
                    //Logger.Debug(desc);
                    break;

                case ZoneOp.CompletedTasks:
                    var comp = packet.Get<CompletedTasks>();
                    //Logger.Debug(comp);
                    break;

                case ZoneOp.XTargetResponse:
                    var xt = packet.Get<XTargetResponse>();
                    //Logger.Debug(xt);
                    break;

                case ZoneOp.Weather:
                    var weather = packet.Get<Weather>();
                    //Logger.Debug(weather);

                    if (Entering)
                        Send(AppPacket.Create(ZoneOp.ReqNewZone));
                    break;

                case ZoneOp.TributeTimer:
                    var timer = packet.Get<TributeTimer>();
                    //Logger.Debug(timer);
                    break;

                case ZoneOp.TributeUpdate:
                    var update = packet.Get<TributeInfo>();
                    //Logger.Debug(update);
                    break;

                case ZoneOp.ZoneEntry:
                    var mob = packet.Get<ZoneEntry>();
                    if (mob.Name == CharName)
                        PlayerSpawnId = (ushort)mob.SpawnID;
                    Logger.Debug($"Zone | ZoneEntry - ID: {mob.SpawnID}, Pos: ({mob.Position.X:F1}, {mob.Position.Y:F1}, {mob.Position.Z:F1})");
                    Logger.Debug(mob.ToString());
                    ZoneEntry?.Invoke(this, mob);
                    break;

                case ZoneOp.NewZone:
                    Send(AppPacket.Create(ZoneOp.ReqClientSpawn));
                    Logger.Debug("Zone | NewZone packet received");
                    Zoned?.Invoke(this, "NewZone");
                    break;

                case ZoneOp.SendExpZonein:
                    if (Entering)
                    {
                        Send(AppPacket.Create(ZoneOp.ClientReady));
                        Entering = false;
                    }
                    break;

                case ZoneOp.CharInventory:
                    break;

                case ZoneOp.SendFindableNPCs:
                    var npc = packet.Get<SendFindableNPCs>();                    
                    break;

                case ZoneOp.ClientUpdate:
                    var pu = packet.Get<ClientUpdateFromServer>();
                    Logger.Debug($"Zone | ClientUpdate - ID: {pu.ID}, Pos: ({pu.Position.X:F1}, {pu.Position.Y:F1}, {pu.Position.Z:F1})");
                    Logger.Debug(pu.ToString());
                    HexdumpServerStyle(packet.Data);
                    ClientUpdate?.Invoke(this, pu);
                    break;

                case ZoneOp.HPUpdate:
                    break;

                case ZoneOp.MobUpdate:
                    // MobUpdate/SpawnPositionUpdate both use 0x4656 - this is for NPC/mob position updates
                    var mobUpdate = packet.Get<MobUpdate>();
                    Logger.Debug($"Zone | MobUpdate - ID: {mobUpdate.ID}, Pos: ({mobUpdate.Position.X:F1}, {mobUpdate.Position.Y:F1}, {mobUpdate.Position.Z:F1})");
                    MobUpdate?.Invoke(this, mobUpdate);
                    break;

                case ZoneOp.NPCMoveUpdate:
                    // Another NPC movement packet
                    var npcMove = packet.Get<NPCMoveUpdate>();
                    Logger.Debug($"Zone | NPCMoveUpdate - ID: {npcMove.ID}, Pos: ({npcMove.Position.X:F1}, {npcMove.Position.Y:F1}, {npcMove.Position.Z:F1})");
                    NPCMoveUpdate?.Invoke(this, npcMove);
                    break;

                case ZoneOp.SpawnDoor:
                    for (var i = 0; i < packet.Data.Length; i += 92)
                    {
                        var door = new SpawnDoor(packet.Data, i);
                        Logger.Debug(door);
                    }
                    break;

                case ZoneOp.ChannelMessage:
                    var msg = packet.Get<ChannelMessage>();
                    Logger.Debug($"Zone | Message - [{msg.ChanNum}] {msg.From}: {msg.Message}");
                    Message?.Invoke(this, msg);
                    break;

                case ZoneOp.Death:
                    var death = packet.Get<Death>();
                    Logger.Debug($"Zone | Death - SpawnID: {death.SpawnId}, CorpseId: {death.CorpseId}");
                    Death?.Invoke(this, death);
                    break;

                case ZoneOp.SpawnAppearance:
                    var appearance = packet.Get<SpawnAppearance>();
                    Logger.Debug($"Zone | SpawnAppearance - SpawnID: {appearance.SpawnId}, Type: {appearance.Type}, Parameter: {appearance.Parameter}");
                    // Could add an event for this if needed
                    break;

                case ZoneOp.WorldObjectsSent:
                    Logger.Debug("Zone | WorldObjectsSent - Server finished sending world objects");
                    // Could add an event for this if needed
                    break;

                case ZoneOp.Consider:
                    var consider = new Consider(packet.Data);
                    Logger.Debug($"Zone | Consider - PlayerID: {consider.PlayerID}, TargetID: {consider.TargetID}, Level: {consider.Level}");
                    Consider?.Invoke(this, consider);
                    break;

                case ZoneOp.MobHealth:
                    var mobHealth = new MobHealth(packet.Data);
                    Logger.Debug($"Zone | MobHealth - EntityID: {mobHealth.EntityID}, HP: {mobHealth.HP}%");
                    MobHealth?.Invoke(this, mobHealth);
                    break;

                case ZoneOp.Damage:
                    var damage = new Damage(packet.Data);
                    Logger.Debug($"Zone | Damage - Target: {damage.Target}, Source: {damage.Source}, Amount: {damage.Amount}");
                    Damage?.Invoke(this, damage);
                    break;

                case ZoneOp.CastSpell:
                    var castSpell = new CastSpell(packet.Data);
                    Logger.Debug($"Zone | CastSpell - SpellID: {castSpell.SpellID}, Caster: {castSpell.CasterID}, Target: {castSpell.TargetID}");
                    CastSpell?.Invoke(this, castSpell);
                    break;

                case ZoneOp.InterruptCast:
                    var interrupt = new InterruptCast(packet.Data);
                    Logger.Debug($"Zone | InterruptCast - SpawnID: {interrupt.SpawnID}");
                    InterruptCast?.Invoke(this, interrupt);
                    break;

                case ZoneOp.Animation:
                    var anim = new Animation(packet.Data);
                    Logger.Debug($"Zone | Animation - SpawnID: {anim.SpawnID}, Action: {anim.Action}");
                    Animation?.Invoke(this, anim);
                    break;

                case ZoneOp.Buff:
                    var buff = new Buff(packet.Data);
                    Logger.Debug($"Zone | Buff - EntityID: {buff.EntityID}, SpellID: {buff.SpellID}");
                    Buff?.Invoke(this, buff);
                    break;

                case ZoneOp.GroundSpawn:
                    var groundSpawn = new GroundSpawn(packet.Data);
                    Logger.Debug($"Zone | GroundSpawn - DropID: {groundSpawn.DropID}, Name: {groundSpawn.ObjectName}");
                    GroundSpawn?.Invoke(this, groundSpawn);
                    break;

                case ZoneOp.Track:
                    var track = new Track(packet.Data);
                    Logger.Debug($"Zone | Track - EntityID: {track.EntityID}, Pos: ({track.X}, {track.Y}, {track.Z})");
                    Track?.Invoke(this, track);
                    break;

                case ZoneOp.Emote:
                    var emote = new Emote(packet.Data);
                    Logger.Debug($"Zone | Emote - Message: {emote.Message}");
                    Emote?.Invoke(this, emote);
                    break;

                case ZoneOp.DeleteSpawn:
                    var deleteSpawn = packet.Get<DeleteSpawn>();
                    Logger.Debug($"Zone | DeleteSpawn - EntityID: {deleteSpawn.Id}");
                    DeleteSpawn?.Invoke(this, deleteSpawn.Id);
                    break;

                case ZoneOp.ExpUpdate:
                    var expUpdate = new ExpUpdate(packet.Data);
                    Logger.Debug($"Zone | ExpUpdate - Exp: {expUpdate.Exp}, AAExp: {expUpdate.AAExp}");
                    ExpUpdate?.Invoke(this, expUpdate);
                    break;

                case ZoneOp.LevelUpdate:
                    var levelUpdate = new LevelUpdate(packet.Data);
                    Logger.Debug($"Zone | LevelUpdate - Level: {levelUpdate.Level} (was {levelUpdate.LevelOld})");
                    LevelUpdate?.Invoke(this, levelUpdate);
                    break;

                case ZoneOp.SkillUpdate:
                    var skillUpdate = new SkillUpdate(packet.Data);
                    Logger.Debug($"Zone | SkillUpdate - SkillID: {skillUpdate.SkillId}, Value: {skillUpdate.Value}");
                    SkillUpdate?.Invoke(this, skillUpdate);
                    break;

                case ZoneOp.WearChange:
                    var wearChange = new WearChange(packet.Data);
                    Logger.Debug($"Zone | WearChange - SpawnID: {wearChange.SpawnId}, Slot: {wearChange.WearSlotId}");
                    WearChange?.Invoke(this, wearChange);
                    break;

                case ZoneOp.MoveItem:
                    var moveItem = new MoveItem(packet.Data);
                    Logger.Debug($"Zone | MoveItem - From: {moveItem.FromSlot}, To: {moveItem.ToSlot}");
                    MoveItem?.Invoke(this, moveItem);
                    break;

                case ZoneOp.Assist:
                    var assist = new Assist(packet.Data);
                    Logger.Debug($"Zone | Assist - Target: {assist.EntityId}");
                    Assist?.Invoke(this, assist);
                    break;

                case ZoneOp.AutoAttack:
                case ZoneOp.AutoAttack2:
                    Logger.Debug($"Zone | AutoAttack");
                    AutoAttack?.Invoke(this, packet.Data);
                    break;

                case ZoneOp.Charm:
                    var charm = new Charm(packet.Data);
                    Logger.Debug($"Zone | Charm - Owner: {charm.OwnerId}, Pet: {charm.PetId}, Cmd: {charm.Command}");
                    Charm?.Invoke(this, charm);
                    break;

                case ZoneOp.Stun:
                    var stun = new Stun(packet.Data);
                    Logger.Debug($"Zone | Stun - Duration: {stun.Duration}");
                    Stun?.Invoke(this, stun);
                    break;

                case ZoneOp.Illusion:
                    var illusion = new Illusion(packet.Data);
                    Logger.Debug($"Zone | Illusion - SpawnID: {illusion.SpawnId}, Race: {illusion.Race}");
                    Illusion?.Invoke(this, illusion);
                    break;

                case ZoneOp.Sound:
                    var sound = new Sound(packet.Data);
                    Logger.Debug($"Zone | Sound - EntityID: {sound.EntityID}, SoundID: {sound.SoundID}");
                    Sound?.Invoke(this, sound);
                    break;

                case ZoneOp.Hide:
                    Logger.Debug($"Zone | Hide");
                    Hide?.Invoke(this, packet.Data);
                    break;

                case ZoneOp.Sneak:
                    Logger.Debug($"Zone | Sneak");
                    Sneak?.Invoke(this, packet.Data);
                    break;

                case ZoneOp.FeignDeath:
                    Logger.Debug($"Zone | FeignDeath");
                    FeignDeath?.Invoke(this, packet.Data);
                    break;

                default:
                    Logger.Debug($"Unhandled packet in ZoneStream: {(ZoneOp)packet.Opcode} (0x{packet.Opcode:X04})");
                    HexdumpServerStyle(packet.Data);
                    break;
            }
        }

        public void SendChatMessage(string to, string from, string message, uint channel = 0)
        {
            var chatMsg = new ChannelMessage(to, from, 0, channel, 100, message);
            Send(AppPacket.Create(ZoneOp.ChannelMessage, chatMsg));
            Logger.Debug($"Zone | Sent chat message - Channel: {channel}, Message: {message}");
        }

        public void UpdatePosition(Tuple<float, float, float, float> Position)
        {
            Logger.Debug($"Zone | Sent UpdatePosition message - X:{Position.Item1} Y:{Position.Item2} Z:{ Position.Item3} Heading:{Position.Item4}");
            var update = new ClientUpdateToServer
            {
                ID = PlayerSpawnId,
                Vehicle = 0,
                Position = new UpdatePositionToServer
                {
                    X = Position.Item1,
                    Y = Position.Item2,
                    Z = Position.Item3,
                    Heading = (ushort)Position.Item4,
                    DeltaX = 0,
                    DeltaY = 0,
                    DeltaZ = 0,
                    DeltaHeading = 0,
                    Animation = 0
                },                
            };
            Send(AppPacket.Create(ZoneOp.ClientUpdate, update));
        }
    }
}