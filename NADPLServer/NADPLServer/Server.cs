using NADPLServer.Networking.Core;
using NADPLServer.Networking.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NADPLServer.Accounts;
using NAPDL.League;

namespace NADPLServer {
    class Server {
        #region Private Variables
        private TCPManager TCP;
        private League league;
        #endregion

        #region Public Variables

        #endregion

        #region Public Functions
        public Server() {
            league = new League();
            league.OnBotStatusChanged += League_OnBotStatusChanged;
            league.OnBotLobbyChanged += League_OnBotLobbyChanged;
            league.OnBotGameStarted += League_OnBotGameStarted;
            league.OnBotHeroesPicked += League_OnBotHeroesPicked;

            TCP = TCPManager.CreateListener(3222);
            TCP.NewClient += TCP_NewClient;
            TCP.ClientLost += TCP_ClientLost;
            TCP.NewMessage += TCP_NewMessage;

            SQLite.Connect();
        }

        

        public void init() {
            Console.WriteLine("Starting Network Manager");
            TCP.StartListener();

            while (true) {
                Thread.Sleep(1000);
            }
        }
        #endregion

        #region Private Functions
        

        #endregion

        #region Callbacks
        private void TCP_NewMessage(Client c, Networking.Messages.NetworkMessage msg) {
            NetworkMessageType type = msg.getMsgType();
            byte[] raw = msg.getRawMsgData();
            switch (type) {
                case NetworkMessageType.Error: {
                        string message = Encoding.ASCII.GetString(msg.getRawMsgData());
                        Console.WriteLine("NError: " + c.getID().ToString() + " | " + message);
                        break;
                    }
                case NetworkMessageType.Ping: {
                        Console.WriteLine("Ping Received");
                        c.addMessage(new NetworkMessage(NetworkMessageType.Pong, new NetworkMessageData() { mData = BitConverter.GetBytes(1) })); //Pong the client
                        break;
                    }
                case NetworkMessageType.Register: {
                        List<byte> msgData = raw.ToList<byte>();
                        ulong steamid = BitConverter.ToUInt64(msgData.ToArray(), 0);
                        msgData.RemoveRange(0, 8);
                        string data = Encoding.ASCII.GetString(msgData.ToArray());
                        string[] parts = data.Split('\n');
                        string user = parts[0];
                        string pass = parts[1];

                        //if user logs in, send back their account info, otherwise send back a failcode
                        if (c.register(user, pass, steamid)) {
                            c.addMessage(new NetworkMessage(NetworkMessageType.Register, new NetworkMessageData() { mData = c.GetAccount().ToBytes() }));
                        } else {
                            c.addMessage(new NetworkMessage(NetworkMessageType.Register, new NetworkMessageData() { mData = new byte[] { 0, 0, 0, 0 } }));
                        }

                        Requests.UpdatePlayerList(TCP);
                        break;
                    }
                case NetworkMessageType.Login: {
                        string data = Encoding.ASCII.GetString(raw);
                        string[] parts = data.Split('\n');
                        string user = parts[0];
                        string pass = parts[1];

                        //if user logs in, send back their account info, otherwise send back a failcode
                        if(c.login(user, pass)) {
                            c.addMessage(new NetworkMessage(NetworkMessageType.Login, new NetworkMessageData() { mData = c.GetAccount().ToBytes() }));
                        } else {
                            c.addMessage(new NetworkMessage(NetworkMessageType.Login, new NetworkMessageData() { mData = new byte[] { 0, 0, 0, 0 } }));
                        }


                        Requests.SendBotData(c, league);
                        Requests.UpdatePlayerList(TCP);


                        break;
                    }
                case NetworkMessageType.JoinLobby: {
                        List<byte> data = raw.ToList<byte>();
                        int botIndex = BitConverter.ToInt32(data.ToArray(), 0);
                        data.RemoveRange(0, 4);
                        int teamNum = BitConverter.ToInt32(data.ToArray(), 0);
                        
                        if(c.isInLobby()) {
                            c.addMessage(new NetworkMessage(NetworkMessageType.JoinLobby, new NetworkMessageData() { mData = new byte[] { 0, 0, 0, 0 } }));
                            break;
                        }

                        LeagueBot[] bots = league.GetBots();
                        if(bots.Length > botIndex) {
                            LeagueBot bot = bots[botIndex];
                            if (bot.CanJoinLobby(c, teamNum == 0 ? true : false)) {
                                c.SetLobby(botIndex, teamNum);
                                c.addMessage(new NetworkMessage(NetworkMessageType.JoinLobby, new NetworkMessageData() { mData = new byte[] { 0, 0, 0, 0, 0 } }));
                                bot.JoinLobby(c, teamNum == 0 ? true : false);
                            } else {
                                c.addMessage(new NetworkMessage(NetworkMessageType.JoinLobby, new NetworkMessageData() { mData = new byte[] { 0, 0, 0, 0 } }));
                            }
                        } else {
                            c.addMessage(new NetworkMessage(NetworkMessageType.JoinLobby, new NetworkMessageData() { mData = new byte[] { 0, 0, 0, 0 } }));
                        }
                        break;
                    }
                case NetworkMessageType.LeaveLobby: {
                        List<byte> data = raw.ToList<byte>();
                        int botIndex = BitConverter.ToInt32(data.ToArray(), 0);
                        data.RemoveRange(0, 4);
                        int teamNum = BitConverter.ToInt32(data.ToArray(), 0);

                        if (!c.isInLobby()) {
                            c.addMessage(new NetworkMessage(NetworkMessageType.JoinLobby, new NetworkMessageData() { mData = new byte[] { 0, 0, 0, 0 } }));
                            break;
                        }

                        LeagueBot[] bots = league.GetBots();
                        if (bots.Length > botIndex) {
                            LeagueBot bot = bots[botIndex];
                            c.ClearLobbyData();
                            c.addMessage(new NetworkMessage(NetworkMessageType.JoinLobby, new NetworkMessageData() { mData = new byte[] { 0, 0, 0, 0, 0 } }));
                            bot.LeaveLobby(c);
                        } else {
                            c.addMessage(new NetworkMessage(NetworkMessageType.JoinLobby, new NetworkMessageData() { mData = new byte[] { 0, 0, 0, 0 } }));
                        }
                        break;
                    }
            }
        }

        private void League_OnBotLobbyChanged(int botNumber, LeagueBot bot) {
            Requests.SendBotUpdate(TCP, botNumber, bot);
        }

        private void League_OnBotStatusChanged(int botNumber, LeagueBot bot) {
            Requests.SendBotUpdate(TCP, botNumber, bot);
        }
        private void League_OnBotGameStarted(int botNumber, LeagueBot bot, LeagueBotInfoType type) {
            Requests.SendBotGameInfo(TCP, botNumber,type,new byte[] { });
        }
        private void League_OnBotHeroesPicked(int botNumber, LeagueBot bot, LeagueBotInfoType type, List<string> RadiantHeroes, List<string> DireHeroes) {

            string data = "";
            for(int i = 0; i < 5;i++) {
                if(i < RadiantHeroes.Count) {
                    if(data != "") {
                        data += "\n";
                    }
                    data += RadiantHeroes[i];
                } else {
                    if (data != "") {
                        data += "\n";
                    }
                    data += "no_hero";
                }
            }
            for (int i = 0; i < 5; i++) {
                if (i < DireHeroes.Count) {
                    if (data != "") {
                        data += "\n";
                    }
                    data += DireHeroes[i];
                } else {
                    if (data != "") {
                        data += "\n";
                    }
                    data += "no_hero";
                }
            }

            Requests.SendBotGameInfo(TCP, botNumber, type, Encoding.ASCII.GetBytes(data));
        }

        private void TCP_ClientLost(Client c) {
            UserAccount account = c.GetAccount();
            if(account == null) {
                Console.WriteLine("Unknown Client Disconnected.");
            } else {
                Console.WriteLine("User Disconnected: " + account.Information.user);

                Requests.UpdatePlayerList(TCP);

            }


            if(c.isInLobby()) {
                int[] data = c.GetUserLobby();
                int botIndex = data[0];
                int teamnum = data[1];
                LeagueBot[] bots = league.GetBots();
                if (bots.Length > botIndex) {
                    LeagueBot bot = bots[botIndex];
                    bot.LeaveLobby(c);
                }
            }

        }
        private void TCP_NewClient(Client c) {
            Console.WriteLine("New Client Connected!");
        }
        #endregion
    }
}
