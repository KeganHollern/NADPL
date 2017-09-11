using NADPLClient.Networking.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NADPLClient.Networking.Messages {
    class MessagesCore {
        protected TCPManager manager;
        public MessagesCore(TCPManager _manager, byte[] raw) {
            manager = _manager;
            Thread thread = new Thread(() => RunSafe(raw));
            thread.Start();
        }
        public virtual void RunFunc(byte[] raw) {

        }
        private void RunSafe(byte[] raw) {
            try {
                RunFunc(raw);
            } catch (Exception ex) {
                manager.addMessage(NetworkMessage.BuildErrorMsg(ex.Message));
            }
        }
    }
}
