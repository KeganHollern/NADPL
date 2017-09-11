using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NADPLServer.Networking.Messages;
using System.Threading;

namespace NADPLServer.Networking.Core {
    class TCPManager: TcpListener {
        #region Private Variables

        private int clientids = 0;
        private bool listen = false;

        private List<Client> Clients = new List<Client>();
        #endregion

        #region Public Variables

        #endregion

        #region Events
        public event ClientConnected NewClient;
        public delegate void ClientConnected(Client c);

        public event ClientDisconnected ClientLost;
        public delegate void ClientDisconnected(Client c);

        public event MessageRecieved NewMessage;
        public delegate void MessageRecieved(Client c, NetworkMessage msg);
        #endregion

        #region Public Functions
        public Client[] getClients() {
            List<Client> goodClients = new List<Client>();
            foreach(Client c in Clients) {
                if(c.GetAccount() != null) {
                    goodClients.Add(c);
                }
            }
            return goodClients.ToArray();
        }

        public TCPManager(IPEndPoint EP) : base(EP) {
            listen = false;
        }
        public static TCPManager CreateListener(int port) {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
            return new TCPManager(ep);
        }

        public void StartListener() {
            listen = true;
            base.Start();
            AcceptClients();
        }
        public void StopListener() {
            listen = false;
            base.Stop();
            foreach (Client c in getClients()) {
                ClientLost(c);
            }
            Clients.Clear();
            clientids = 0;
        }
        public bool isListening() {
            return listen;
        }
        #endregion

        #region Private Functions
        private void RemoveClient(Client _client) {
            Clients.Remove(_client);
            ClientLost(_client);
        }
        private void AddClient(Client _client) {
            Clients.Add(_client);
            NewClient(_client);
        }
        private async Task AcceptClients() {
            try {
                if (!this.isListening()) { return; }
                TcpClient _tcp = await this.AcceptTcpClientAsync();
                _tcp.NoDelay = true;
                _tcp.SendTimeout = 5000;
                _tcp.ReceiveTimeout = 5000;
                Client _client = new Client(_tcp, clientids);
                clientids = clientids + 1;
                AddClient(_client);
                Thread thread = new Thread(() => Handshake(_client));
                thread.Start();
            } catch (Exception) {
            }
            AcceptClients();
        }

        private bool WriteToStream(NetworkStream stream, byte[] buffer) {
            int retry = 0;
retrywrite:
            if (retry > 4) {
                return false;
            }
            try {
                stream.Write(buffer, 0, buffer.Length);
            } catch (Exception) {
                retry = retry + 1;
                goto retrywrite;
            }
            return true;
        }
        private byte[] ReadFromStream(NetworkStream stream, int length) {
            byte[] buffer = new byte[length];
            int bytesread = 0;
            int retry = 0;
retryread:
            if (retry > 4) {
                return new byte[] { };
            }
            try {
                bytesread = stream.Read(buffer, 0, length);
            } catch (Exception) {
                retry = retry + 1;
                goto retryread;
            }


            return buffer.ToList<byte>().GetRange(0, bytesread).ToArray();
        }

        private void Handshake(Client c) {
            NetworkStream stream = c.Tcp.GetStream();
            stream.WriteTimeout = 1000;
            stream.ReadTimeout = 1000;
            byte[] buffer = new byte[] { };
            while (c.Tcp.Connected && listen) {
                buffer = c.getNextMsg().ToBytes();
                if (!WriteToStream(stream, buffer)) {
                    break;
                }
                buffer = ReadFromStream(stream, c.Tcp.ReceiveBufferSize);
                if (buffer.Length == 0) {
                    break;
                }
                NetworkMessage _recieved = NetworkMessage.BuildFromData(buffer);
                Thread TempThread = new Thread(() => {
                    NewMessage(c, _recieved);
                });
                TempThread.Start();
                Thread.Sleep(10);
            }
            if (listen) {
                RemoveClient(c);
            } else {
                c.Tcp.Close();
            }
        }
        #endregion




    }
}
