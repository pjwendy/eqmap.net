using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using static OpenEQ.Netcode.Utility;

namespace OpenEQ.Netcode {
	public class WorldStream : EQStream {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public event EventHandler<List<CharacterSelectEntry>> CharacterList;
		public event EventHandler<bool> CharacterCreateNameApproval;
		public event EventHandler<string> MOTD;
		public event EventHandler<ZoneServerInfo> ZoneServer;
		public event EventHandler<string> ChatServerList;
		public event EventHandler<GuildsList> GuildsList;
		public event EventHandler<LogServer> LogServer;
		public event EventHandler<ExpansionInfo> ExpansionInfo;
		public event EventHandler<ApproveWorld> ApproveWorld;
		public event EventHandler<EnterWorldStatus> EnterWorld;
		public event EventHandler<PostEnterWorld> PostEnterWorld;
		public event EventHandler<WorldComplete> WorldComplete;

		uint AccountID;
		string SessionKey;

		public WorldStream(string host, int port, uint accountID, string sessionKey) : base(host, port) {
			Debug = true; // Enable for initial connection handshake
			AccountID = accountID;
			SessionKey = sessionKey;
			Logger.Info("World | Starting world server connection...");
			Connect();
			SendSessionRequest();
		}

		protected override void HandleSessionResponse(Packet packet) {
			Send(packet);
		
			// LoginInfo_Struct must be exactly 464 bytes to match UI client
			var data = new byte[464];
			// Format: "accountid\0sessionkey" starting at byte 0
			var str = $"{AccountID}\0{SessionKey}";
			Array.Copy(Encoding.ASCII.GetBytes(str), data, str.Length);
			// Byte 192: zoning flag (0xCC for UF client based on server logs)
			data[192] = 0xCC; // UF client-specific flag value
			Logger.Info("World | Sending LoginInfo packet to authenticate with world server");
			Send(AppPacket.Create(WorldOp.SendLoginInfo, data));
			
			// Disable debug spam after initial handshake is complete
			Debug = false;
		}

		protected override void HandleAppPacket(AppPacket packet) {
			var opcodeName = ((WorldOp)packet.Opcode).ToString();
			Logger.Info($"World | Received | [{opcodeName}] [0x{packet.Opcode:X04}] Size [{packet.Size}]");
			
			switch((WorldOp) packet.Opcode) {
				case WorldOp.GuildsList:
					Logger.Info("World | GuildsList received - guild information loaded");
					var guilds = packet.Get<GuildsList>();
					Logger.Info($"World | Parsed {guilds.Guilds.Count} guilds from server");
					GuildsList?.Invoke(this, guilds);
					break;
				case WorldOp.LogServer:
					Logger.Info("World | LogServer received - logging server information");
					var logServer = packet.Get<LogServer>();
					Logger.Info($"World | LogServer - World: {logServer.WorldShortName}, PvP: {logServer.EnablePvp != 0}, FV: {logServer.EnableFV != 0}");
					LogServer?.Invoke(this, logServer);
					break;
				case WorldOp.ExpansionInfo:
					Logger.Info("World | ExpansionInfo received - expansion data loaded");
					var expansionInfo = packet.Get<ExpansionInfo>();
					Logger.Info($"World | ExpansionInfo - Expansions: 0x{expansionInfo.Expansions:X08}");
					ExpansionInfo?.Invoke(this, expansionInfo);
					break;
				case WorldOp.ApproveWorld:
					Logger.Info("World | ApproveWorld received - world login approved, ready for character selection");
					var approveWorld = packet.Get<ApproveWorld>();
					Logger.Info($"World | ApproveWorld - Size: {approveWorld.Data?.Length ?? 0} bytes");
					ApproveWorld?.Invoke(this, approveWorld);
					break;
				case WorldOp.EnterWorld:
					Logger.Info("World | EnterWorld received - entering world sequence");
					var enterWorld = packet.Get<EnterWorldStatus>();
					Logger.Info($"World | EnterWorld - Size: {enterWorld.Data?.Length ?? 0} bytes");
					EnterWorld?.Invoke(this, enterWorld);
					break;
				case WorldOp.PostEnterWorld:
					Logger.Info("World | PostEnterWorld received - world entry complete");
					var postEnterWorld = packet.Get<PostEnterWorld>();
					Logger.Info($"World | PostEnterWorld - Size: {postEnterWorld.Data?.Length ?? 0} bytes");
					PostEnterWorld?.Invoke(this, postEnterWorld);
					break;
				case WorldOp.WorldComplete:
					Logger.Info("World | WorldComplete received - world loading complete");
					var worldComplete = packet.Get<WorldComplete>();
					Logger.Info($"World | WorldComplete - Size: {worldComplete.Data?.Length ?? 0} bytes");
					WorldComplete?.Invoke(this, worldComplete);
					break;
				case WorldOp.SendCharInfo:
					Logger.Info("World | SendCharInfo received - character list available");
					var chars = new CharacterSelect(packet.Data);
					CharacterList?.Invoke(this, chars.Characters);
					break;
				case WorldOp.MessageOfTheDay:
					Logger.Info("World | MessageOfTheDay received");
					MOTD?.Invoke(this, Encoding.ASCII.GetString(packet.Data));
					break;
				case WorldOp.ZoneServerInfo:
					Logger.Info("World | ZoneServerInfo received - zone server details");
					var info = packet.Get<ZoneServerInfo>();
					ZoneServer?.Invoke(this, info);
					break;
				case WorldOp.SetChatServer:
				case WorldOp.SetChatServer2:
					Logger.Info("World | SetChatServer received - chat server configuration");
					ChatServerList?.Invoke(this, Encoding.ASCII.GetString(packet.Data));
					break;
				case WorldOp.ApproveName:
					Logger.Info("World | ApproveName received - character name approval result");
					CharacterCreateNameApproval?.Invoke(this, packet.Data[0] == 1);
					break;
				default:
					Logger.Warn($"World | UNHANDLED | [{opcodeName}] [0x{packet.Opcode:X04}] Size [{packet.Size}] - No handler implemented");
					if (packet.Data != null && packet.Data.Length > 0) {
						Logger.Debug("Packet data:");
						Hexdump(packet.Data);
					}
					break;
			}
		}

		public void SendNameApproval(NameApproval nameApproval) {
			Send(AppPacket.Create(WorldOp.ApproveName, nameApproval));
		}

		public void SendCharacterCreate(CharCreate charCreate) {
			Send(AppPacket.Create(WorldOp.CharacterCreate, charCreate));
		}

		public void SendEnterWorld(EnterWorld enterWorld) {
			Send(AppPacket.Create(WorldOp.EnterWorld, enterWorld));
		}
	}
}