using System;
using System.IO;
using System.Threading.Tasks;
using MoreLinq;
using NLog;
using OpenEQ.Netcode;
using static System.Console;
using static System.Net.Mime.MediaTypeNames;

namespace NetClient {
	internal class Program {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static string Input(string prompt) {
			Logger.Info($"{prompt} > ");
			return ReadLine().TrimEnd();
		}
		
		static void Main(string[] args) {
            if (File.Exists("log.txt"))
                File.Delete("log.txt");

            // Create config for NLog so we can log to file as well as to file
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt" };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            Logger.Info("Starting eqconsole");

            var host = GetIndexValueOrDefault(args, 0, "127.0.0.1", (string value) => value);
            var port = GetIndexValueOrDefault(args, 1, 5999,        (string value) => int.Parse(value));
            Logger.Info($"Connecting to LoginServer @ {host}:{port}");

            while(true) {
				var loginStream = new LoginStream(host, port);
				loginStream.LoginSuccess += (_, success) => {
					if(success) {
                        Logger.Info($"Login succeeded (accountID={loginStream.AccountID}).  Requesting server list");
						loginStream.RequestServerList();
					} else {
                        Logger.Error("Login failed");
						loginStream.Disconnect();
					}
				};

				loginStream.ServerList += (_, servers) => {
					servers.ForEach((serv, i) =>
                        Logger.Info($"{i + 1}: {serv.Longname} ({serv.PlayersOnline} players online)"));
					int ret;
					while(!int.TryParse(Input("Server number"), out ret) || ret < 1 || servers.Count < ret) {}
					loginStream.Play(servers[ret - 1]);
				};
				
				loginStream.PlaySuccess += (_, server) => {
					if(server == null) {
                        Logger.Error("Failed to connect to server.  Try everything again.");
						loginStream.Disconnect();
						return;
					}
					
					ConnectWorld(loginStream, server.Value);
				};

                var username = Input("Username");
                var password = Input("Password");

                loginStream.Login(username, password);

				while(!loginStream.Disconnecting)
					Task.Delay(100).Wait();
			}
		}

        private static TResult GetIndexValueOrDefault<TResult>(string[] args, int index, TResult defaultValue, Func<string, TResult> parser)
        {
            if (args.Length <= index)
            {
                return defaultValue;
            }
            return parser(args[index]);
        }

        static void ConnectWorld(LoginStream ls, ServerListElement server) {
            Logger.Info($"Selected {server}.  Connecting.");
			var worldStream = new WorldStream(server.WorldIP, 9000, ls.AccountID, ls.SessionKey);
			
			string charName = null;
			worldStream.CharacterList += (_, chars) => {
                Logger.Info("Select a character:");
                Logger.Info("0: Create a new character");
				chars.ForEach((@char, i) => WriteLine($"{i + 1}: {@char.Name} - Level {@char.Level}"));
				int ret;
				while(!int.TryParse(Input("Character number"), out ret) || ret < 0 || chars.Count < ret) {}
				if(ret == 0)
					CreateCharacter();
				else
					worldStream.SendEnterWorld(new EnterWorld(charName = chars[ret - 1].Name, false, false));
			};

			void CreateCharacter() {
				charName = Input("Name");
				worldStream.SendNameApproval(new NameApproval {
					Name = charName, 
					Class = 3, 
					Race = 1, 
					Unknown = 214
				});
			}

			worldStream.CharacterCreateNameApproval += (_, success) => {
				if(!success) {
                    Logger.Info("Name not approved by server");
					CreateCharacter();
				} else {
                    Logger.Info("Name approved, creating");
					worldStream.SendCharacterCreate(new CharCreate {
						Class_ = 1,
						Haircolor = 255,
						BeardColor = 255,
						Beard = 255,
						Gender = 0,
						Race = 2,
						StartZone = 29,
						HairStyle = 255,
						Deity = 211,
						STR = 113,
						STA = 130,
						AGI = 87,
						DEX = 70,
						WIS = 70,
						INT = 60,
						CHA = 55,
						Face = 0,
						EyeColor1 = 9,
						EyeColor2 = 9,
						DrakkinHeritage = 1,
						DrakkinTattoo = 0,
						DrakkinDetails = 0,
						Tutorial = 0
					});
				}
			};

			worldStream.ZoneServer += (_, zs) => {
                Logger.Info($"Got zone server at {zs.Host}:{zs.Port}.  Connecting");
				ConnectZone(charName, zs.Host, zs.Port);
			};
		}

		static void ConnectZone(string charName, string host, ushort port) {
			var zoneStream = new ZoneStream(host, port, charName);
			zoneStream.Spawned += (_, mob) => {
                Logger.Info($"Spawn {mob.Name}");
			};
			zoneStream.PositionUpdated += (_, update) => {
                Logger.Info($"Position updated: {update.ID} {update.Position}");
			};
		}
    }
}