using NADPLServer.Accounts;
using NADPLServer.Networking.Core;
using NAPDL.League;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NADPLServer.Networking.Messages {
    class Requests {

        public static void UpdatePlayerList(TCPManager TCP) {

            string requestdata = "";
            foreach(Client c in TCP.getClients()) {
                if(requestdata != "") {
                    requestdata += "\n";
                }

                UserAccount account = c.GetAccount();
                if(account != null) {
                    requestdata += account.Information.steamid64.ToString() + "\r" + account.Information.user;
                }
            }
            byte[] data = Encoding.ASCII.GetBytes(requestdata);

            foreach(Client c in TCP.getClients()) {
                c.addMessage(new NetworkMessage(NetworkMessageType.UpdatePlayerList, new NetworkMessageData() { mData = data }));
            }
        }

        /// <summary>
        /// Send bot update to all clients
        /// </summary>
        /// <param name="TCP"></param>
        /// <param name="index"></param>
        /// <param name="bot"></param>
        public static void SendBotUpdate(TCPManager TCP, int index, LeagueBot bot) {
            List<byte> data = new List<byte>();

            bool cleanuprequired = false;

            BotLobby lobby = bot.getLobby();
            bool inUse = bot.isInUse();

            data.AddRange(BitConverter.GetBytes(index));
            data.AddRange(BitConverter.GetBytes(inUse));

            for (int i = 0; i < 5; i++) {
                if (lobby.Radiant.Count > i) {
                    if (lobby.Radiant[i].GetAccount() != null) {
                        data.AddRange(BitConverter.GetBytes(lobby.Radiant[i].GetAccount().Information.steamid64));
                    } else {
                        data.AddRange(BitConverter.GetBytes((ulong)0));
                        cleanuprequired = true;
                    }
                } else {
                    data.AddRange(BitConverter.GetBytes((ulong)0));
                }
            }
            for (int i = 0; i < 5; i++) {
                if (lobby.Dire.Count > i) {
                    if (lobby.Dire[i].GetAccount() != null) {
                        data.AddRange(BitConverter.GetBytes(lobby.Dire[i].GetAccount().Information.steamid64));
                    } else {
                        data.AddRange(BitConverter.GetBytes((ulong)0));
                    }
                } else {
                    data.AddRange(BitConverter.GetBytes((ulong)0));
                    cleanuprequired = true;
                }
            }

            foreach (Client c in TCP.getClients()) {
                c.addMessage(new NetworkMessage(NetworkMessageType.UpdateBot, new NetworkMessageData() { mData = data.ToArray() }));
            }

            if(cleanuprequired) {
                bot.CleanLobby();
            }
        }

        public static void SendBotGameInfo(TCPManager TCP, int index, LeagueBotInfoType type, byte[] extradata) {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(index));
            data.AddRange(BitConverter.GetBytes((int)type));
            data.AddRange(extradata);

            foreach (Client c in TCP.getClients()) {
                c.addMessage(new NetworkMessage(NetworkMessageType.GameInformation, new NetworkMessageData() { mData = data.ToArray() }));
            }
        }

        /// <summary>
        /// Send all bot info to client (used on connect)
        /// </summary>
        /// <param name="c"></param>
        /// <param name="league"></param>
        public static void SendBotData(Client c, League league) {
            LeagueBot[] bots = league.GetBots();

            List<byte> data = new List<byte>();

            int index = 0;
            foreach(LeagueBot bot in bots) {
                BotLobby lobby = bot.getLobby();
                bool inUse = bot.isInUse();

                data.AddRange(BitConverter.GetBytes(index));
                data.AddRange(BitConverter.GetBytes(inUse));

                for(int i = 0; i < 5;i++) {
                    if(lobby.Radiant.Count > i) {
                        data.AddRange(BitConverter.GetBytes(lobby.Radiant[i].GetAccount().Information.steamid64));
                    } else {
                        data.AddRange(BitConverter.GetBytes((ulong)0));
                    }
                }
                for (int i = 0; i < 5; i++) {
                    if (lobby.Dire.Count > i) {
                        data.AddRange(BitConverter.GetBytes(lobby.Dire[i].GetAccount().Information.steamid64));
                    } else {
                        data.AddRange(BitConverter.GetBytes((ulong)0));
                    }
                }

                index++;
            }
            c.addMessage(new NetworkMessage(NetworkMessageType.UpdateBotList, new NetworkMessageData() { mData = data.ToArray() }));
        }

    }
}
