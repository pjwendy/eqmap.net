﻿using System;
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

		uint AccountID;
		string SessionKey;

		public WorldStream(string host, int port, uint accountID, string sessionKey) : base(host, port) {
			//Debug = true;
			AccountID = accountID;
			SessionKey = sessionKey;
			Logger.Debug("Starting world connection...");
			Connect();
			SendSessionRequest();
		}

		protected override void HandleSessionResponse(Packet packet) {
			Send(packet);

			var data = new byte[464];
			var str = $"{AccountID}\0{SessionKey}";
			Array.Copy(Encoding.ASCII.GetBytes(str), data, str.Length);
			Send(AppPacket.Create(WorldOp.SendLoginInfo, data));
		}

		protected override void HandleAppPacket(AppPacket packet) {
			Logger.Debug($"WorldStream: {(WorldOp) packet.Opcode} : {((WorldOp)packet.Opcode).ToString()} ");
			switch((WorldOp) packet.Opcode) {
				case WorldOp.GuildsList:				
				case WorldOp.LogServer:
				case WorldOp.ApproveWorld:
				case WorldOp.EnterWorld:
				case WorldOp.PostEnterWorld:
				case WorldOp.ExpansionInfo:
					break;
				case WorldOp.SendCharInfo:
					var chars = new CharacterSelect(packet.Data);
					CharacterList?.Invoke(this, chars.Characters);
					// The emu doesn't do anything with ApproveWorld and WorldClientReady so we may be able to just skip them both.
					Send(AppPacket.Create(WorldOp.ApproveWorld));
					Send(AppPacket.Create(WorldOp.AckPacket));
					Send(AppPacket.Create(WorldOp.WorldClientReady));
					break;
				case WorldOp.MessageOfTheDay:
					MOTD?.Invoke(this, Encoding.ASCII.GetString(packet.Data));
					break;
				case WorldOp.ZoneServerInfo:
					var info = packet.Get<ZoneServerInfo>();
					ZoneServer?.Invoke(this, info);
					break;
				case WorldOp.SetChatServer:
				case WorldOp.SetChatServer2:
					ChatServerList?.Invoke(this, Encoding.ASCII.GetString(packet.Data));
					break;
				case WorldOp.ApproveName:
					CharacterCreateNameApproval?.Invoke(this, packet.Data[0] == 1);
					break;
				default:
					Logger.Debug($"Unhandled packet in WorldStream: {(WorldOp) packet.Opcode} (0x{packet.Opcode:X04})");
					Hexdump(packet.Data);
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