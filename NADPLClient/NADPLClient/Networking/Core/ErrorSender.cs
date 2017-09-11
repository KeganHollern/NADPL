using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NADPLClient.Networking.Core {
    class ErrorSender {
        private static bool isDebugMode = true;
        public static TCPManager tcp = null;

        public static void Send(string msg) {

            if (tcp != null) {
                if (tcp.active) {
                    tcp.addMessage(NetworkMessage.BuildErrorMsg(msg));
                    tcp.addMessage(NetworkMessage.BuildErrorMsg("An error has been reported! Check ErrorLog.txt"));
                } else {
                    //--- store error msg in tcp manager w/ error time so it can be better logged.
                    tcp.addMessage(NetworkMessage.BuildErrorMsg("Error While Disconnected... Time: " + DateTime.UtcNow.ToShortTimeString() + " | Error: " + msg));
                    tcp.addMessage(NetworkMessage.BuildErrorMsg("An error has been reported! Check ErrorLog.txt"));
                }
            } else {
                //--- tcp client not established, this is not a msg we can tell the server. If we are in debug mode then show a messagebox on the client.
                if (isDebugMode) {
                    Notification.Show("DEBUG Error", msg);
                }
            }
        }
    }
}
