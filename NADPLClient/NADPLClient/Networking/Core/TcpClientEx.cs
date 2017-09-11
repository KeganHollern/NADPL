using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NADPLClient.Networking.Core {
    class TcpClientEx : TcpClient {
        public static bool IsDead { get; set; }

        public TcpClientEx() {
            TcpClientEx.IsDead = false;
            try {
                this.ReceiveTimeout = 2000;
                this.SendTimeout = 2000;
            } catch (Exception ex) {
                ErrorSender.Send("DEBUG: Error\n\n" + ex.ToString());
            }
        }
        public void ConnectSafe(string ip, int port, int timeoutMS) {
            bool success = false;
            try {
                IAsyncResult ar = this.BeginConnect(ip, port, null, null);
                System.Threading.WaitHandle wh = ar.AsyncWaitHandle;

                try {
                    if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeoutMS), false)) {
                        this.Close();
                        throw new TimeoutException();
                    }

                    this.EndConnect(ar);
                    success = true;
                } finally {
                    wh.Close();
                }
            } catch (TimeoutException) {

            } catch (Exception ex) {
                ErrorSender.Send("DEBUG: Error\n\n" + ex.ToString());
            }
            if (!success) {
                throw new TimeoutException();
            }
        }
        protected override void Dispose(bool disposing) {
            try {
                TcpClientEx.IsDead = true;
                base.Dispose(disposing);
            } catch (Exception ex) {
                ErrorSender.Send("DEBUG: Error\n\n" + ex.ToString());
            }
        }
    }
}
