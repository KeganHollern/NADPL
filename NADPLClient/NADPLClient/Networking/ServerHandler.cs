using NADPLClient.Account;
using NADPLClient.Networking.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NADPLClient.Networking {
    public class ServerHandler {
        private TCPManager TCP;
        private Form1 mainForm;
        private byte[] RequestData;

        public Dictionary<ulong, string> OnlineUsers;


        public bool Connected { get; set; }


        public ServerHandler(Form1 mainForm) {
            OnlineUsers = new Dictionary<ulong, string>();
            RequestData = new byte[] { };
            this.mainForm = mainForm;
            Connected = false;
            TCP = TCPManager.CreateClient();
            TCP.NewMessage += TCP_NewMessage;
            TCP.ServerFound += TCP_ServerFound;
            TCP.ServerLost += TCP_ServerLost;
        }
        public bool JoinLobby(int index,int team) {
            if (!Connected)
                return false;

            DateTime timeout = DateTime.Now.AddSeconds(10);
            RequestData = new byte[] { };
            List<byte> msgData = new List<byte>();
            msgData.AddRange(BitConverter.GetBytes(index));
            msgData.AddRange(BitConverter.GetBytes(team));

            TCP.addMessage(new NetworkMessage(NetworkMessageType.JoinLobby, new NetworkMessageData() { mData = msgData.ToArray() }));

            while (DateTime.Now < timeout) {
                Thread.Sleep(100);

                if (RequestData.Length > 0) {
                    break;
                }
            }
            if (RequestData.Length == 0) {
                return false;
            }

            if (RequestData.Length != 4) {
                return true;
            } else {
                return false;
            }
        }
        public bool LeaveLobby(int index, int team) {
            if (!Connected)
                return false;

            DateTime timeout = DateTime.Now.AddSeconds(10);
            RequestData = new byte[] { };
            List<byte> msgData = new List<byte>();
            msgData.AddRange(BitConverter.GetBytes(index));
            msgData.AddRange(BitConverter.GetBytes(team));

            TCP.addMessage(new NetworkMessage(NetworkMessageType.LeaveLobby, new NetworkMessageData() { mData = msgData.ToArray() }));

            while (DateTime.Now < timeout) {
                Thread.Sleep(100);

                if (RequestData.Length > 0) {
                    break;
                }
            }
            if (RequestData.Length == 0) {
                return false;
            }

            if (RequestData.Length != 4) {
                return true;
            } else {
                return false;
            }
        }
        public bool Register(string username,string password,ulong steamid) {
            if (!Connected)
                return false;

            DateTime timeout = DateTime.Now.AddSeconds(10);
            RequestData = new byte[] { };
            List<byte> msgData = new List<byte>();
            msgData.AddRange(BitConverter.GetBytes(steamid));
            msgData.AddRange(Encoding.ASCII.GetBytes(username + "\n" + password));

            TCP.addMessage(new NetworkMessage(NetworkMessageType.Register, new NetworkMessageData() { mData = msgData.ToArray() }));

            while (DateTime.Now < timeout) {
                Thread.Sleep(100);

                if (RequestData.Length > 0) {
                    break;
                }
            }
            if (RequestData.Length == 0) {
                return false;
            }

            if (RequestData.Length != 4) {
                UserAccount.UpdateInformationFromBytes(RequestData);
                return true;
            } else {
                return false;
            }
        }
        public bool Login(string username,string password) {
            if (!Connected)
                return false;
            
            DateTime timeout = DateTime.Now.AddSeconds(10);
            RequestData = new byte[] { };
            byte[] msgData = Encoding.ASCII.GetBytes(username + "\n" + password);

            TCP.addMessage(new NetworkMessage(NetworkMessageType.Login, new NetworkMessageData() { mData = msgData }));

            while(DateTime.Now < timeout) {
                Thread.Sleep(100);
                
                if(RequestData.Length > 0) {
                    break;
                }
            }
            if(RequestData.Length == 0) {
                return false;
            }
            
            if(RequestData.Length != 4) {
                UserAccount.UpdateInformationFromBytes(RequestData);
                return true;
            } else {
                return false;
            }

        }

        private void TCP_ServerLost() {
            Connected = false;
        }

        private void TCP_ServerFound() {
            Connected = true;
        }

        private void TCP_NewMessage(TCPManager _Tcp, NetworkMessage _Msg) {
            NetworkMessageType type = _Msg.getMsgType();
            byte[] raw = _Msg.getRawMsgData();

            switch(type) {
                case NetworkMessageType.Pong: {
                        Notification.Show("Network Test", "Pong Received");
                        break;
                    }
                //this is a catch-all for requests that require some response code
                case NetworkMessageType.JoinLobby:
                case NetworkMessageType.LeaveLobby:
                case NetworkMessageType.Register:
                case NetworkMessageType.Login: {
                        RequestData = raw; //set data response
                        break;
                    }
                case NetworkMessageType.UpdatePlayerList: {
                        string text = Encoding.ASCII.GetString(raw);
                        string[] userdata = text.Split('\n');

                        OnlineUsers = new Dictionary<ulong, string>();
                        List<string> usernames = new List<string>();

                        foreach(string data in userdata) {
                            string[] parts = data.Split('\r');
                            usernames.Add(parts[1]);
                            ulong steamid = ulong.Parse(parts[0]);
                            string username = parts[1];
                            if (!OnlineUsers.ContainsKey(steamid))
                                OnlineUsers.Add(steamid, username);
                        }

                        mainForm.UpdatePlayerList(usernames.ToArray());

                        break;
                    }
                case NetworkMessageType.UpdateBotList: {

                        List<byte> data = raw.ToList<byte>();

                        // 4 bots
                        for(int i = 0; i < 4; i++) {
                            int index = BitConverter.ToInt32(data.ToArray(), 0);
                            data.RemoveRange(0, 4);

                            bool inUse = BitConverter.ToBoolean(data.ToArray(), 0);
                            data.RemoveRange(0, 1);

                            List<ulong> RadiantSteamIDS = new List<ulong>();

                            for(int j = 0; j < 5;j++) {
                                ulong steamid = BitConverter.ToUInt64(data.ToArray(), 0);
                                data.RemoveRange(0, 8);
                                if(steamid != 0) {
                                    RadiantSteamIDS.Add(steamid);
                                }
                            }

                            List<ulong> DireSteamIDS = new List<ulong>();

                            for (int j = 0; j < 5; j++) {
                                ulong steamid = BitConverter.ToUInt64(data.ToArray(), 0);
                                data.RemoveRange(0, 8);
                                if (steamid != 0) {
                                    DireSteamIDS.Add(steamid);
                                }
                            }

                            mainForm.UpdateBotInfo(index, inUse, RadiantSteamIDS, DireSteamIDS);

                        }


                        break;
                    }
                case NetworkMessageType.UpdateBot: {
                        //data from one specific bot is updated
                        List<byte> data = raw.ToList<byte>();
                        int index = BitConverter.ToInt32(data.ToArray(), 0);
                        data.RemoveRange(0, 4);

                        bool inUse = BitConverter.ToBoolean(data.ToArray(), 0);
                        data.RemoveRange(0, 1);

                        List<ulong> RadiantSteamIDS = new List<ulong>();

                        for (int j = 0; j < 5; j++) {
                            ulong steamid = BitConverter.ToUInt64(data.ToArray(), 0);
                            data.RemoveRange(0, 8);
                            if (steamid != 0) {
                                RadiantSteamIDS.Add(steamid);
                            }
                        }

                        List<ulong> DireSteamIDS = new List<ulong>();

                        for (int j = 0; j < 5; j++) {
                            ulong steamid = BitConverter.ToUInt64(data.ToArray(), 0);
                            data.RemoveRange(0, 8);
                            if (steamid != 0) {
                                DireSteamIDS.Add(steamid);
                            }
                        }

                        mainForm.UpdateBotInfo(index, inUse, RadiantSteamIDS, DireSteamIDS);

                        break;
                    }
                case NetworkMessageType.GameInformation: {
                        List<byte> data = raw.ToList<byte>();
                        int botindex = BitConverter.ToInt32(data.ToArray(), 0);
                        data.RemoveRange(0, 4);
                        int infotype = BitConverter.ToInt32(data.ToArray(), 0);
                        data.RemoveRange(0, 4);

                        if (infotype == 0) {
                            mainForm.ResetLeaveButton(botindex);
                            TabUI.SetHeroes(botindex, new string[] { });
                        } else {
                            string info = Encoding.ASCII.GetString(data.ToArray());
                            string[] parts = info.Split('\n');
                            TabUI.SetHeroes(botindex, parts);
                        }

                        break;
                    }
                case NetworkMessageType.GameStartInfo: {
                        //lobby name and password
                        string data = Encoding.ASCII.GetString(raw);
                        string[] parts = data.Split('\n');
                        string lobby = parts[0];
                        string pass = parts[1];
                        Notification.Show("Connection Info", "Lobby: " + lobby + "\nPassword: " + pass);
                        break;
                    }
                case NetworkMessageType.GameOverInfo: {
                        //game winner (0 = win, 1 = loss, 2 = error)
                        int result = BitConverter.ToInt32(raw, 0);
                        if(result == 0) {
                            Notification.Show("Match Result", "Your team has won!");
                        } else if(result == 1) {
                            Notification.Show("Match Result", "Your team has lost!");
                        } else {
                            Notification.Show("Match Error", "Data corrupted.\nMatch will not be scored.");
                        }
                        break;
                    }
            }
        }

        public void TestConnection() {
            if(TCP.active) {
                TCP.addMessage(new NetworkMessage(NetworkMessageType.Ping, new NetworkMessageData() { mData = BitConverter.GetBytes(1) }));
            } else {
                Notification.Show("Network Error", "Not Connected! Can Not Test");
            }
        }
        public void DisconnectFromServer() {
            TCP.Stop();
        }
        public void ConnectToServer() {
            TCP.Start();
        }

    }
}
