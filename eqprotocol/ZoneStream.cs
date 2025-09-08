using System;
using static OpenEQ.Netcode.Utility;

namespace OpenEQ.Netcode
{
    public class ZoneStream : EQStream
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        readonly string CharName;
        bool Entering = true;
        ushort PlayerSpawnId;
        ushort UpdateSequence;

        public event EventHandler<Spawn> Spawned;
        public event EventHandler<PlayerPositionUpdate> PositionUpdated;
        public event EventHandler<PlayerProfile> PlayerProfile;
        public event EventHandler<ChannelMessage> Message;
        public event EventHandler<Death> Death;
        public event EventHandler<string> Zoned;

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

            Send(AppPacket.Create(ZoneOp.ZoneEntry, new ClientZoneEntry(CharName)));
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
                    var xt = packet.Get<XTarget>();
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
                    var mob = packet.Get<Spawn>();
                    if (mob.Name == CharName)
                        PlayerSpawnId = (ushort)mob.SpawnID;
                    Spawned?.Invoke(this, mob);
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
                    var npc = packet.Get<FindableNPC>();
                    //Logger.Debug(npc);
                    break;

                case ZoneOp.ClientUpdate:
                    var pu = packet.Get<PlayerPositionUpdate>();
                    Logger.Debug($"Zone | ClientUpdate - ID: {pu.ID}, Pos: ({pu.Position.X:F1}, {pu.Position.Y:F1}, {pu.Position.Z:F1})");
                    PositionUpdated?.Invoke(this, pu);
                    break;

                case ZoneOp.HPUpdate:
                    break;

                case ZoneOp.MobUpdate:
                    // MobUpdate/SpawnPositionUpdate both use 0x4656 - this is for NPC/mob position updates
                    var mobUpdate = packet.Get<PlayerPositionUpdate>();
                    Logger.Debug($"Zone | MobUpdate - ID: {mobUpdate.ID}, Pos: ({mobUpdate.Position.X:F1}, {mobUpdate.Position.Y:F1}, {mobUpdate.Position.Z:F1})");
                    PositionUpdated?.Invoke(this, mobUpdate);
                    break;

                case ZoneOp.NPCMoveUpdate:
                    // Another NPC movement packet
                    var npcMove = packet.Get<PlayerPositionUpdate>();
                    Logger.Debug($"Zone | NPCMoveUpdate - ID: {npcMove.ID}, Pos: ({npcMove.Position.X:F1}, {npcMove.Position.Y:F1}, {npcMove.Position.Z:F1})");
                    PositionUpdated?.Invoke(this, npcMove);
                    break;

                case ZoneOp.SpawnDoor:
                    for (var i = 0; i < packet.Data.Length; i += 92)
                    {
                        var door = new Door(packet.Data, i);
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

                default:
                    Logger.Debug($"Unhandled packet in ZoneStream: {(ZoneOp)packet.Opcode} (0x{packet.Opcode:X04})");
                    HexdumpServerStyle(packet.Data);
                    break;
            }
        }

        public void UpdatePosition(Tuple<float, float, float, float> Position)
        {
            var update = new ClientPlayerPositionUpdate
            {
                ID = PlayerSpawnId,
                Sequence = UpdateSequence++,
                X = Position.Item1,
                Y = Position.Item2,
                Sub1 = new ClientUpdatePositionSub1(),
                Z = Position.Item3,
                Sub2 = new ClientUpdatePositionSub2(0, (ushort)(Position.Item4 * 8f * 255f))
            };
            Send(AppPacket.Create(ZoneOp.ClientUpdate, update));
        }
    }
}