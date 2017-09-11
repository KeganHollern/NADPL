using NADPLServer.Networking.Core;
using NADPLServer.Networking.Messages;
using NAPDL.Dota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace NAPDL.League {
    


    struct BotLobby {
        public List<Client> Radiant;
        public List<Client> Dire;
    }


    class LeagueBot {

        private const int RADIANT_TEAM_SIZE = 3;
        private const int DIRE_TEAM_SIZE = 3;

        private DotaClient client;
        private string username;
        private string password;
        private bool inuse;
        private DotaGameResult MatchResult;
        private bool MatchOver;
        private string CurrentLobby;
        private bool ready;
        private string LobbyName;
        private Random randomizer;
        
        private BotLobby Lobby;

        public delegate void BotStatusChangedEvent(LeagueBot bot, bool isActive);
        public event BotStatusChangedEvent OnBotStatusChanged;

        public delegate void BotLobbyChangedEvent(LeagueBot bot, BotLobby newLobby);
        public event BotLobbyChangedEvent OnBotLobbyChanged;

        public delegate void HeroesPickedEventHandler(LeagueBot bot, List<string> RadiantHeroes, List<string> DireHeroes);
        public event HeroesPickedEventHandler OnHeroesPicked;

        public delegate void OnGameStartedEvent(LeagueBot bot, BotLobby FinalLobby);
        public event OnGameStartedEvent OnBotGameStarted;

        //TODO: eventually make this "request start match" to make the "ready up buttons" appear
        public void TryStartMatch() {

            //if bot is not ready, do not start the match.
            if (!ready) {
                return;
            }

            if(Lobby.Dire.Count == DIRE_TEAM_SIZE && Lobby.Radiant.Count == RADIANT_TEAM_SIZE) {
                Thread MatchThread = new Thread(() => DoMatch());
                MatchThread.Start();
            }
        }
        public void DoMatch() {
            

            const string chars = "abcdefghjkmnopqrxyz";
            string serverpassword = new string(Enumerable.Repeat(chars, 8).Select(s => s[randomizer.Next(s.Length)]).ToArray());

            
            string LobbyInfo = LobbyName + "\n" + serverpassword;

            List<ulong> RadiantTeam = new List<ulong>();
            List<ulong> DireTeam = new List<ulong>();

            foreach(Client c in Lobby.Radiant) {
                c.addMessage(new NetworkMessage(NetworkMessageType.GameStartInfo, new NetworkMessageData() { mData = Encoding.ASCII.GetBytes(LobbyInfo) }));
                RadiantTeam.Add(c.GetAccount().Information.steamid64);
            }
            foreach (Client c in Lobby.Dire) {
                c.addMessage(new NetworkMessage(NetworkMessageType.GameStartInfo, new NetworkMessageData() { mData = Encoding.ASCII.GetBytes(LobbyInfo) }));
                DireTeam.Add(c.GetAccount().Information.steamid64);
            }

            DotaGameResult result = RunMatch(RadiantTeam, DireTeam, LobbyName, serverpassword);

            //sends 0 for win, 1 for loss, 2 for error
            int status = 2;
            if (result == DotaGameResult.Radiant) { status = 0; }
            if(result == DotaGameResult.Dire) { status = 1; }
            foreach (Client c in Lobby.Radiant) {
                c.addMessage(new NetworkMessage(NetworkMessageType.GameOverInfo, new NetworkMessageData() { mData = BitConverter.GetBytes((int)(status)) }));
            }

            status = 2;
            if (result == DotaGameResult.Radiant) { status = 0; }
            if (result == DotaGameResult.Dire) { status = 1; }
            foreach (Client c in Lobby.Dire) {
                c.addMessage(new NetworkMessage(NetworkMessageType.GameOverInfo, new NetworkMessageData() { mData = BitConverter.GetBytes((int)(status)) }));
            }


            foreach (Client c in Lobby.Dire.ToArray()) {
                c.ClearLobbyData();
                LeaveLobbySilent(c);
            }
            foreach (Client c in Lobby.Radiant.ToArray()) {
                c.ClearLobbyData();
                LeaveLobbySilent(c);
            }

            ResetClient();
            inuse = false;
            if (OnBotStatusChanged != null) {
                OnBotStatusChanged(this, inuse);
            }
            
        }



        public bool CanJoinLobby(Client client, bool isRadiant) {
            if (isRadiant) {
                if (Lobby.Radiant.Count < 5) {
                    return true;
                }
            } else {
                if (Lobby.Dire.Count < 5) {
                    return true;
                }
            }
            return false;
        }
        public void JoinLobby(Client client, bool isRadiant) {
            if(!CanJoinLobby(client,isRadiant))
                return;

            if(isRadiant) {
                Lobby.Radiant.Add(client);
            } else {
                Lobby.Dire.Add(client);
            }
            if (OnBotLobbyChanged != null) {
                OnBotLobbyChanged(this, Lobby);
            }

            TryStartMatch();
        }
        public BotLobby getLobby() {
            return Lobby;
        }
        public void CleanLobby() {
            Lobby.Dire.Remove(null);
            Lobby.Radiant.Remove(null);
            foreach(Client c in Lobby.Radiant) {
                if(c.GetAccount() == null) {
                    Lobby.Radiant.Remove(c);
                }
            }
            foreach (Client c in Lobby.Dire) {
                if (c.GetAccount() == null) {
                    Lobby.Dire.Remove(c);
                }
            }
        }
        private void LeaveLobbySilent(Client client) {
            Lobby.Radiant.Remove(client);
            Lobby.Dire.Remove(client);
        }
        public void LeaveLobby(Client client) {
            Lobby.Radiant.Remove(client);
            Lobby.Dire.Remove(client);
            if (OnBotLobbyChanged != null) {
                OnBotLobbyChanged(this, Lobby);
            }
        }
        
        public LeagueBot(string username,string password, string lobbyname) {
            randomizer = new Random();
            inuse = false;
            ready = false;
            LobbyName = lobbyname;

            Lobby = new BotLobby();

            this.username = username;
            this.password = password;
            CurrentLobby = "No Lobby";
            InitNewClient();
        }
        public bool isInUse() {
            return inuse;
        }
        public DotaGameResult RunMatch(ulong[] Radiant,ulong[] Dire, string LobbyName, string LobbyPassword) {
            return RunMatch(Radiant.ToList<ulong>(), Dire.ToList<ulong>(), LobbyName, LobbyPassword);
        }
        public DotaGameResult RunMatch(List<ulong> Radiant, List<ulong> Dire, string LobbyName, string LobbyPassword) {
            if(!ready) {
                Console.WriteLine("Bot Not Ready. Waiting For Connection..");
            }
            while(!ready) {
                Thread.Sleep(10);
            }
            inuse = true;
            if(OnBotStatusChanged != null) {
                OnBotStatusChanged(this, inuse);
            }
            MatchOver = false;
            CurrentLobby = LobbyName;
            
            client.CreateLobby(LobbyName, LobbyPassword, new DotaLobbyParams(Radiant, Dire));

            while(!MatchOver) {
                Thread.Sleep(10);
            }

            return MatchResult;
        }
        private void InitLobby() {
            Lobby.Dire = new List<Client>();
            Lobby.Radiant = new List<Client>();
        }

        public void ResetClient() {
            client.Reset();
        }

        private void InitNewClient() {
            InitLobby();

            client = DotaClient.Create(username, password, new DotaClientParams());
            client.OnLobbyCreated += Client_OnLobbyCreated;
            client.OnGameStarted += Client_OnGameStarted;
            client.OnGameFinished += Client_OnGameFinished;
            client.OnStatusChanged += Client_OnStatusChanged;
            client.OnHeroesPicked += Client_OnHeroesPicked;
            Thread Temp = new Thread(() => {
                client.Connect();
                ready = true;
            });
            Temp.Start();
        }

        private void Client_OnHeroesPicked(List<string> RadiantHeroes, List<string> DireHeroes) {
            if(OnHeroesPicked != null) {
                OnHeroesPicked(this, RadiantHeroes, DireHeroes);
            }
        }

        private void Client_OnStatusChanged(DotaClientStatus status, string message) {
            if(status == DotaClientStatus.Warning) {
                Console.WriteLine(CurrentLobby + ": -WARNING- " + message);
            } else if(status == DotaClientStatus.Fatal) {
                Console.WriteLine(CurrentLobby + ": -FATAL- " + message);
            }

        }
        private void Client_OnGameFinished(DotaGameResult MatchInfo) {
            Console.WriteLine(CurrentLobby + ": Match Completed. Winner: " + Enum.GetName(typeof(DotaGameResult), MatchInfo));
            MatchResult = MatchInfo;
            MatchOver = true;
        }
        private void Client_OnGameStarted(ulong MatchID) {
            Console.WriteLine(CurrentLobby + ": Game Started: " + MatchID.ToString());

        }
        private void Client_OnLobbyCreated(ulong LobbyID) {
            Console.WriteLine(CurrentLobby + ": Lobby Created: " + LobbyID.ToString());

            foreach(Client c in Lobby.Radiant) {
                client.InviteToLobby(c.GetAccount().Information.steamid64);
            }
            foreach (Client c in Lobby.Dire) {
                client.InviteToLobby(c.GetAccount().Information.steamid64);
            }

            if(OnBotGameStarted != null) {
                OnBotGameStarted(this, Lobby);
            }
        }
    }
}
