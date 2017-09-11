using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NADPLServer.Networking.Messages {

    enum NetworkMessageType {
        NoData,
        Error,
        Ping,
        Pong,
        Login,
        Register,
        UpdatePlayerList,
        UpdateBotList,
        UpdateBot,
        JoinLobby,
        LeaveLobby,
        GameInformation,
        GameStartInfo,
        GameOverInfo
    }

    struct NetworkMessageData {
        public byte[] mData;
    }
    

    class NetworkMessage {

        public byte[] ToBytes() {
            List<byte> dataList = new List<byte>();

            dataList.AddRange(BitConverter.GetBytes((int)this.getMsgType()));
            dataList.AddRange(this.getRawMsgData());
            return dataList.ToArray();
        }

        public static NetworkMessage BuildFromData(byte[] data) {
            int nmt = BitConverter.ToInt32(data, 0);
            byte[] raw = new byte[] { };
            if (data.Length > 4) {
                raw = data.ToList<byte>().GetRange(4, data.Length - 4).ToArray();
            }
            NetworkMessageData nmd = new NetworkMessageData() { mData = raw };
            NetworkMessageType type = (NetworkMessageType)nmt;
            return new NetworkMessage(type, nmd);
        }


        public static NetworkMessage BuildErrorMsg(string Error) {
            return new NetworkMessage(NetworkMessageType.Error, new NetworkMessageData() { mData = Encoding.ASCII.GetBytes(Error) });
        }
        public static NetworkMessage EMPTY = new NetworkMessage(NetworkMessageType.NoData, new NetworkMessageData() { mData = new byte[] { } });
        private NetworkMessageType _type;
        private NetworkMessageData _data;
        public NetworkMessage(NetworkMessageType type, NetworkMessageData data) {
            _type = type;
            _data = data;
        }
        public NetworkMessageType getMsgType() { return _type; }
        public NetworkMessageData getMsgData() { return _data; }
        public byte[] getRawMsgData() { return _data.mData; }
    }
}
