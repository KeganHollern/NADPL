using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NADPLClient.Account {
    struct UserAccountInformation {
        public int mmr;
        public string user;
        public string pass;
        public ulong steamid64;
    }
    class UserAccount {
        public static UserAccountInformation Information = new UserAccountInformation() { mmr = 100, steamid64 = 0, user = "", pass = "" };
        public static void UpdateInformationFromBytes(byte[] data) {
            List<byte> info = data.ToList<byte>();
            int mmr = BitConverter.ToInt32(info.ToArray(), 0);
            info.RemoveRange(0, 4);
            ulong steamid64 = BitConverter.ToUInt64(info.ToArray(), 0);
            info.RemoveRange(0, 8);
            string login = Encoding.ASCII.GetString(info.ToArray());
            string[] parts = login.Split('\n');
            string user = parts[0];
            string pass = parts[1];
            Information.mmr = mmr;
            Information.steamid64 = steamid64;
            Information.user = user;
            Information.pass = pass;
        }
    }
}
