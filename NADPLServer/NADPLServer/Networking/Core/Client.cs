using NADPLServer.Accounts;
using NADPLServer.Networking.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NADPLServer.Networking.Core {
    class Client : NetworkClient {
        private UserAccount account;
        
        private int LobbyBotNum = -1;
        private int LobbyTeam = -1;


        
        
        public void ClearLobbyData() {
            LobbyTeam = -1;
            LobbyBotNum = -1;
        }
        public bool isInLobby() {
            return LobbyTeam != -1;
        }
        public int[] GetUserLobby() {
            return new int[] { LobbyBotNum, LobbyTeam };
        }
        public void SetLobby(int botnum,int team) {
            LobbyBotNum = botnum;
            LobbyTeam = team;
        }
        public UserAccount GetAccount() {
            return account;
        }
        public void setAccount(UserAccount account) {
            this.account = account;
        }
        public bool isLoggedIn() {
            return (account != null);
        }
        public bool login(string username,string password) {
            account = UserAccount.Login(username, password);
            return isLoggedIn();
        }
        public bool register(string username,string password,ulong steamid) {
            account = UserAccount.Register(username, password, steamid);
            return isLoggedIn();
        }


        
        public Client(TcpClient _tcp, int clientid) : base(_tcp,clientid) {}
        

    }
}
