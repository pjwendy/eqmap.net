using System;
using System.Resources;
using static System.Console;
using static OpenEQ.Netcode.Utility;

namespace OpenEQ.Netcode {
	public class ZoneStream : EQStream {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        readonly string CharName;
		bool Entering = true;
		ushort PlayerSpawnId;
		ushort UpdateSequence;

		public event EventHandler<Spawn> Spawned;
		public event EventHandler<PlayerPositionUpdate> PositionUpdated;
        public event EventHandler<PlayerProfile> PlayerProfile;
        public event EventHandler<Death> Death;
		public event EventHandler<ChannelMessage> Message;
		public event EventHandler Zoned;

		public ZoneStream(string host, int port, string charName) : base(host, port) {
			SendKeepalives = true;
			CharName = charName;

			Logger.Debug("Starting zone connection...");
			Connect();
			SendSessionRequest();
		}

		protected override void HandleSessionResponse(Packet packet) {
			Send(packet);
			Send(AppPacket.Create(ZoneOp.ZoneEntry, new ClientZoneEntry(CharName)));
		}

		protected override void HandleAppPacket(AppPacket packet) {
			Logger.Debug($"Zone app packet: {(ZoneOp) packet.Opcode} : {((ZoneOp)packet.Opcode).ToString()}");
			switch((ZoneOp) packet.Opcode) {
				case ZoneOp.PlayerProfile:
					var player = packet.Get<PlayerProfile>();
                    Logger.Debug(player);
                    PlayerProfile?.Invoke(this, player);
                    break;

				case ZoneOp.TimeOfDay:
					var timeofday = packet.Get<TimeOfDay>();
					Logger.Debug(timeofday);
					break;

				case ZoneOp.TaskActivity:
					//var activity = packet.Get<TaskActivity>();
					//Logger.Debug(activity);
					break;

				case ZoneOp.TaskDescription:
					var desc = packet.Get<TaskDescription>();
					Logger.Debug(desc);
					break;

				case ZoneOp.CompletedTasks:
					var comp = packet.Get<CompletedTasks>();
					Logger.Debug(comp);
					break;

				case ZoneOp.XTargetResponse:
					var xt = packet.Get<XTarget>();
					Logger.Debug(xt);
					break;

				case ZoneOp.Weather:
					var weather = packet.Get<Weather>();
					Logger.Debug(weather);
					if(Entering)
                    {
						Send(AppPacket.Create(ZoneOp.AckPacket));
						Send(AppPacket.Create(ZoneOp.ReqNewZone));
					}						
					break;

				case ZoneOp.TributeTimer:
					var timer = packet.Get<TributeTimer>();
					Logger.Debug(timer);
					break;

				case ZoneOp.TributeUpdate:
					var update = packet.Get<TributeInfo>();
					Logger.Debug(update);
					break;

				case ZoneOp.ZoneEntry:
					var mob = packet.Get<Spawn>();
					if(mob.Name == CharName)
						PlayerSpawnId = (ushort) mob.SpawnID;
					Spawned?.Invoke(this, mob);
					break;

				case ZoneOp.NewZone:
					Send(AppPacket.Create(ZoneOp.ReqClientSpawn));
					break;

				case ZoneOp.SendExpZonein:
					if(Entering) {
						Logger.Debug("SendExpZonein received, sending ClientReady");
						Send(AppPacket.Create(ZoneOp.ClientReady));
						Entering = false;
					}
					break;
				
				case ZoneOp.CharInventory:
					var inventory = packet.Get<CharInventory>();
					Logger.Debug(inventory);
					break;

				case ZoneOp.SendFindableNPCs:
					var npc = packet.Get<FindableNPC>();
					Logger.Debug(npc);
					break;

				case ZoneOp.ClientUpdate:
					var pu = packet.Get<PlayerPositionUpdate>();
					PositionUpdated?.Invoke(this, pu);
					break;

				case ZoneOp.HPUpdate:
					break;

				case ZoneOp.ChannelMessage:
					ChannelMessage msg = new ChannelMessage(packet.Data);
					Logger.Debug(msg.ToString());
					Message?.Invoke(this, msg);
					break;

				case ZoneOp.RequestClientZoneChange:
					RequestClientZoneChange chg = new RequestClientZoneChange(packet.Data);
					Logger.Debug(chg.ToString());					
					SendZoneChange(chg.InstanceId, chg.ZoneID, chg.Y, chg.X, chg.Z);
					break;

				case ZoneOp.ZoneChange:
					ZoneChange zoneChg = new ZoneChange(packet.Data);
					Logger.Debug(zoneChg.ToString());
					SendSaveOnZoneReq();
					SendDeleteSpawn(PlayerSpawnId);
					break;

				case ZoneOp.SpawnDoor:
					for(var i = 0; i < packet.Data.Length; i += 92) {
						var door = new Door(packet.Data, i);
						Logger.Debug(door);
					}
					break;

                case ZoneOp.Death:
                    var dp = packet.Get<Death>();
                    Death?.Invoke(this, dp);
                    break;

				case ZoneOp.PreLogoutReply:
					Logger.Debug("Logging Out of Zone");
					break;

				case ZoneOp.LogoutReply:
					Logger.Debug("Logged Out of Zone");
					Zoned?.Invoke(this, null);
					break;

				default:
					Logger.Debug($"Unhandled packet in ZoneStream: {(ZoneOp) packet.Opcode} (0x{packet.Opcode:X04}) : {((ZoneOp)packet.Opcode).ToString()}");
					Hexdump(packet.Data);
					break;
			}
		}

		public void UpdatePosition(Tuple<float, float, float, float> Position) {
			var update = new ClientPlayerPositionUpdate {
				ID = PlayerSpawnId,
				Sequence = UpdateSequence++,
				X = Position.Item1,
				Y = Position.Item2,
				Sub1 = new ClientUpdatePositionSub1(),
				Z = Position.Item3,
				Sub2 = new ClientUpdatePositionSub2(0, (ushort) (Position.Item4 * 8f * 255f))
			};
			Send(AppPacket.Create(ZoneOp.ClientUpdate, update));
		}
         
        public void SendChatMessage(string from, string to, string message)
        {
            var chat = new ChannelMessage
            {
                From = from,
                To = to,
                Language = 0,
                LanguageSkill = 100,
                ChanNum = (ushort) ChatChannel.Say,
                Message = message
            };
            Send(AppPacket.Create(ZoneOp.ChannelMessage, chat));
        }

		public void SendZoneChange(ushort instanceId, ushort zoneId, float y, float x, float z)
		{
			var zone = new ZoneChange
			{
				Name = CharName,
				InstanceId = instanceId,
				ZoneID = zoneId,
				Y = y,
				X = x,
				Z = z
			};
			Send(AppPacket.Create(ZoneOp.ZoneChange, zone));
		}

		public void SendSaveOnZoneReq()
		{
			var save = new SaveOnZoneReq
			{
				Part1 = new byte[192],
				Part2 = new byte[176]
			};
			Send(AppPacket.Create(ZoneOp.SaveOnZoneReq, save));
		}

		public void SendDeleteSpawn(uint spawnId)
		{
			var spawn = new DeleteSpawn
			{
				SpawnId = spawnId
			};
			Send(AppPacket.Create(ZoneOp.DeleteSpawn, spawn));
		}
	}
}