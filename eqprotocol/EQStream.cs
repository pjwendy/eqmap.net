﻿using System;
using System.Threading.Tasks;
using static System.Console;
using static OpenEQ.Netcode.Utility;

namespace OpenEQ.Netcode {
	public abstract class EQStream {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public bool Debug = true;

		public bool Compressing, Validating;
		public byte[] CRCKey;
		public ushort OutSequence, InSequence;

		public bool SendKeepalives = false;
		float lastRecvSendTime;

		AsyncUDPConnection conn;
		uint sessionID;

		ushort lastAckRecieved, lastAckSent;
		bool resendAck;
		Packet[] sentPackets, futurePackets;

		public bool Disconnecting;

		public EQStream(string host, int port) {
			conn = new AsyncUDPConnection(host, port);

			AsyncHelper.Run(CheckerAsync, longRunning: true);
			AsyncHelper.Run(ReceiverAsync, longRunning: true);
		}

		protected void Connect() {
			Compressing = Validating = false;
			OutSequence = InSequence = 0;
			lastAckRecieved = 65535;
			lastAckSent = 0;
			sentPackets = new Packet[65536];
			futurePackets = new Packet[65536];
		}

		public void Disconnect() => Disconnecting = true;

		public void SendSessionRequest() {
			var sr = new SessionRequest((uint) new Random().Next());
			sessionID = sr.sessionID;
			Send(Packet.Create(SessionOp.Request, sr, bare: true));
		}

		public void ResetAckForZone() {
			OutSequence = InSequence = 0;
			lastAckRecieved = 65535;
			lastAckSent = 0;
//            sentPackets = new Packet[65536];
//            futurePackets = new Packet[65536];
		}

		async void CheckerAsync() {
			while(!Disconnecting) {
				if(sentPackets != null) {
					lock(sentPackets) {
						var last = lastAckRecieved + 1;
						for(var i = last; i < last + 65536; ++i) {
							var packet = sentPackets[i % 65536];
							if(packet == null || packet.Acked)
								break;
							if(Time.Now - packet.SentTime > 2) {
								if(Debug)
									Logger.Debug(
										$"Packet {packet.Sequence} not acked in {Time.Now - packet.SentTime}; resending.");
								Send(packet);
							}
						}

						if(lastAckSent != InSequence) {
							if(Debug)
								Logger.Debug($"ACKing up to {(ushort) ((InSequence + 65536 - 1) % 65536)}");
							Send(Packet.Create(SessionOp.Ack, sequence: (ushort) ((InSequence + 65536 - 1) % 65536)));
							lastAckSent = InSequence;
						} else if(resendAck) {
							Send(Packet.Create(SessionOp.Ack, sequence: (ushort) ((InSequence + 65536) % 65536)));
							resendAck = false;
						}
					}
				}

				if(SendKeepalives && Time.Now - lastRecvSendTime > 5) {
					Logger.Debug("Sending keepalive");
					Send(Packet.Create(SessionOp.Ack, sequence: (ushort) ((lastAckSent + 65536 - 1) % 65536)));
				}

				await Task.Delay(100);
			}
		}

		async void ReceiverAsync() {
			try {
				while(!Disconnecting) {
					if(Debug)
						Logger.Debug($"Waiting for packets on wire ({this})");
					var data = await conn.Receive();
					//ForegroundColor = ConsoleColor.Blue;
					lastRecvSendTime = Time.Now;

					if(Debug) {
						//ForegroundColor = ConsoleColor.DarkMagenta;
						Logger.Debug($"Received packet ({this})");
						Hexdump(data);
						//ResetColor();
					}

					var packet = new Packet(this, data);
					if(packet.Valid)
						ProcessSessionPacket(packet);
				}
			} catch(Exception e) {
				Logger.Debug($"Got exception in receiver thread for {this}");
				Logger.Debug(e);
				Environment.Exit(0);
			}
		}

		void ProcessSessionPacket(Packet packet) {
			var op = (SessionOp) packet.Opcode;
			switch(op) {
				case SessionOp.Response:
					var response = packet.Get<SessionResponse>();
					Compressing = (response.filterMode & FilterMode.Compressed) != 0;
					Validating = (response.validationMode & ValidationMode.Crc) != 0;

					CRCKey = new[] {
						(byte) (response.crcKey >> 24),
						(byte) (response.crcKey >> 16),
						(byte) (response.crcKey >> 8),
						(byte) response.crcKey
					};
					HandleSessionResponse(packet);
					break;
				case SessionOp.Ack:
					if(lastAckRecieved > packet.Sequence)
						for(var i = lastAckRecieved + 1; i <= packet.Sequence + 65536; ++i)
							sentPackets[i % 65536].Acked = true;
					else
						for(var i = lastAckRecieved + 1; i <= packet.Sequence; ++i)
							sentPackets[i].Acked = true;
					lastAckRecieved = packet.Sequence;
					break;
				case SessionOp.KeepAlive:
					Send(packet);
					break;
				case SessionOp.Single:
				case SessionOp.Fragment:
				case SessionOp.Bare:
					QueueOrProcess(packet);
					break;
				case SessionOp.Combined:
					if(Debug)
						Logger.Debug("Processing combined packet: {");
					for(var i = 0; i < packet.Data.Length;) {
						var slen = packet.Data[i];
						var sub = new Packet(this, packet.Data.Sub(i + 1, i + 1 + slen), combined: true);
						ProcessSessionPacket(sub);
						i += slen + 1;
					}

					if(Debug)
						Logger.Debug("} END OF COMBINED");
					break;
				default:
					if(Debug) {
						Logger.Debug($"Unknown packet received: {op} (0x{packet.Opcode:X04})");
						Hexdump(packet.Data);
					}

					break;
			}
		}

		void QueueOrProcess(Packet packet) {
			lock(sentPackets) {
				if(packet.Bare || packet.Sequence == InSequence) // Present
					ProcessPacket(packet);
				else if((packet.Sequence > InSequence && packet.Sequence - InSequence < 2048) ||
				        (packet.Sequence + 65536) - InSequence < 2048) {
					// Future
					Logger.Debug($"Future packet :( Got {packet.Sequence}, need {InSequence}");
					futurePackets[packet.Sequence] = packet;
					if(futurePackets[InSequence]?.Opcode == (ushort) SessionOp.Fragment
					) // Maybe we have enough for the current fragment?
						ProcessPacket(futurePackets[InSequence]);
				} else if((packet.Sequence < InSequence && InSequence - packet.Sequence < 2048) ||
				          packet.Sequence - (InSequence + 65536) < 2048) {
					// Past
					if(Debug)
						Logger.Debug(
							$"Got packet in the past... expect {InSequence} got {packet.Sequence}.  Sending ACK up to {(ushort) ((InSequence + 65536) % 65536)}");
					resendAck = true;
				}
			}
		}

		void HandleAppPacketProxy(AppPacket packet) {
			if(Debug) {
				//ForegroundColor = ConsoleColor.Magenta;
				Logger.Debug($"Received app packet (opcode {packet.Opcode:X04}, {this}):");
				if(packet.Data == null)
					Logger.Debug("!Null data!");
				else
					Hexdump(packet.Data);
				//ResetColor();
			}

			HandleAppPacket(packet);
		}

		bool ProcessPacket(Packet packet, bool self = false) {
			switch((SessionOp) packet.Opcode) {
				case SessionOp.Bare:
					var bapp = new AppPacket(packet.Data);
					HandleAppPacketProxy(bapp);
					break;
				case SessionOp.Single:
					futurePackets[packet.Sequence] = null;
					var app = new AppPacket(packet.Data);
					HandleAppPacketProxy(app);
					InSequence = (ushort) ((packet.Sequence + 1) % 65536);
					if(Debug)
						Logger.Debug($"Single packet updated sequence from {packet.Sequence} to {InSequence}");
					break;
				case SessionOp.Fragment:
					var tlen = packet.Data.NetU32(0);
					var rlen = -4;
					for(var i = packet.Sequence; futurePackets[i] != null && rlen < tlen; ++i)
						rlen += futurePackets[i].Data.Length;
					if(rlen < tlen) {
						// Don't have all the pieces yet
						futurePackets[packet.Sequence] = packet;
						return false;
					}

					var tdata = new byte[rlen];
					rlen = 0;
					var last = 0;
					for(var i = packet.Sequence; rlen < tlen; ++i) {
						var off = i == packet.Sequence ? 4 : 0;
						var fdata = futurePackets[i % 65536].Data;
						Array.Copy(fdata, off, tdata, rlen, fdata.Length - off);
						rlen += fdata.Length - off;
						futurePackets[i] = null;
						last = i;
					}

					InSequence = (ushort) ((last + 1) % 65536);
					if(Debug)
						Logger.Debug(
							$"Fragmented packet updated our sequence from {packet.Sequence} to {InSequence} ({last - packet.Sequence} packets)");
					HandleAppPacketProxy(new AppPacket(tdata));
					break;
			}

			while(!self && futurePackets[InSequence] != null)
				if(!ProcessPacket(futurePackets[InSequence], self: true))
					break;
			return true;
		}

		protected void Send(Packet packet) {
			lastRecvSendTime = Time.Now;

			if(packet.Baked == null && packet.SentTime == 0 && !packet.Bare &&
			   packet.Opcode != (ushort) SessionOp.Ack && packet.Opcode != (ushort) SessionOp.Stats) {
				packet.Sequence = OutSequence;
				sentPackets[OutSequence++] = packet;
			}

			packet.SentTime = Time.Now;
			var data = packet.Bake(this);
			if(data.Length > 512) {
                Logger.Debug("Overlong packet!");
				Hexdump(data);
			}

			if(Debug) {
                //ForegroundColor = ConsoleColor.DarkGreen;
                Logger.Debug($"Sending connection packet (from {this}):");
				Hexdump(data);
				//ResetColor();
			}

			conn.Send(data);
		}
		protected void Send(AppPacket packet) {
			if(packet.Size > 512 - 7) {
                // Fragment
                Logger.Debug("Fragment :(");
			} else {
				if(Debug) {
                    //ForegroundColor = ConsoleColor.Green;
                    Logger.Debug($"Sending app packet (opcode {packet.Opcode:X04}, {this}):");
					if(packet.Data == null)
                        Logger.Debug("!Null data!");
					else
						Hexdump(packet);
					//ResetColor();
				}

				var data = new byte[packet.Size];
				data[1] = (byte) (packet.Opcode >> 8);
				data[0] = (byte) packet.Opcode;
				if(packet.Data != null)
					Array.Copy(packet.Data, 0, data, 2, packet.Data.Length);
				Send(new Packet(SessionOp.Single, data));
			}
		}

		void SendRaw(byte[] packet) {
			conn.Send(packet);
		}

		protected abstract void HandleSessionResponse(Packet packet);
		protected abstract void HandleAppPacket(AppPacket packet);
	}
}