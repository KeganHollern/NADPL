using NADPLServer.Networking.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NADPLServer.Networking.Core {
    class NetworkClient {
        public TcpClient Tcp;
        private int clientID;
        private List<NetworkMessage> messageQueue = new List<NetworkMessage>();

        public void addMessage(NetworkMessage msg) {
            messageQueue.Add(msg);
        }
        public NetworkMessage getNextMsg() {
            if (messageQueue.Count == 0) { return NetworkMessage.EMPTY; }
            NetworkMessage msg = messageQueue.ElementAt(0);
            messageQueue.RemoveAt(0);
            return msg;
        }
        public void setID(int id) {
            clientID = id;
        }
        public int getID() {
            return clientID;
        }
        public NetworkClient(TcpClient _tcp, int clientid) {
            this.Tcp = _tcp;
            this.clientID = clientid;
        }
    }
}
