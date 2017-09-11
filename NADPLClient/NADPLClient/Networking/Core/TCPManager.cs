using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NADPLClient.Networking.Core {
    class TCPManager {
        #region Private Variables
        private TcpClientEx activeClient;
        private List<NetworkMessage> messageQueue = new List<NetworkMessage>();
        private Thread thread;
        private bool connect;
        private int clientids = 0;
        #endregion

        #region Public Variables
        public bool active;
        #endregion

        #region Events
        public event ServerConnected ServerFound;
        public delegate void ServerConnected();

        public event ServerDisconnected ServerLost;
        public delegate void ServerDisconnected();

        public event MessageRecieved NewMessage;
        public delegate void MessageRecieved(TCPManager _Tcp, NetworkMessage _Msg);
        #endregion

        #region Public Functions
        public TCPManager() {
            connect = true;
            active = false;
        }
        public List<NetworkMessage> allMessages() {
            return messageQueue;
        }
        public void addMessage(NetworkMessage msg) {
            messageQueue.Add(msg);
        }
        public NetworkMessage getNextMsg() {
            if (messageQueue.Count == 0) { return NetworkMessage.EMPTY; }
            NetworkMessage msg = messageQueue.ElementAt(0);
            messageQueue.RemoveAt(0);
            return msg;
        }
        public static TCPManager CreateClient() {
            return new TCPManager();
        }
        public void Start() {
            try {
                activeClient = new TcpClientEx();

                thread = new Thread(() => connector());
                thread.Start();
            } catch (Exception ex) {
                ErrorSender.Send("DEBUG: Error\n\n" + ex.ToString());
            }
        }
        public void Stop() {
            try {
                connect = false;
                thread.Join();
            } catch (Exception ex) {
                ErrorSender.Send("DEBUG: Error\n\n" + ex.ToString());
            }
        }
        #endregion

        #region Private Functions
        private void connector() {
            while (connect) {
                try {
                    if (TcpClientEx.IsDead) {
                        activeClient = new TcpClientEx();
                    }
                    activeClient.ConnectSafe("173.64.64.131", 3222, 1000);
                    active = true;
                    ServerFound();
                    Handshake();
                } catch (Exception) {
                    Thread.Sleep(100);
                }
            }

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
                if (connect) {
                    goto retrywrite;
                }
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
                if (connect) {
                    goto retryread;
                }
            }


            return buffer.ToList<byte>().GetRange(0, bytesread).ToArray();
        }
        private void Handshake() {
            try {
                NetworkStream stream = activeClient.GetStream();
                stream.WriteTimeout = 1000;
                stream.ReadTimeout = 1000;
                byte[] buffer = new byte[] { };



                while (activeClient.Connected && connect) {
                    //--- has to get locked in here somewhere
                    buffer = ReadFromStream(stream, activeClient.ReceiveBufferSize);
                    if (buffer.Length == 0) {
                        break;
                    } else if (buffer.Length != 1) {
                        NetworkMessage _recieved = NetworkMessage.BuildFromData(buffer);
                        Thread TempThread = new Thread(() => {
                            NewMessage(this, _recieved);
                        });
                        TempThread.Start();
                    }

                    buffer = this.getNextMsg().ToBytes();
                    if (!WriteToStream(stream, buffer)) {
                        break;
                    }
                    Thread.Sleep(10);
                }
                if (connect) {
                    active = false;
                    ServerLost();
                }
                activeClient.Close();
            } catch (Exception ex) {
                ErrorSender.Send("DEBUG: Error\n\n" + ex.ToString());
            }
        }
        #endregion



    }
}
