using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NADPLServer.Accounts {
    struct UserAccountInformation {
        public int mmr;
        public string user;
        public string pass;
        public ulong steamid64;
    }
    class UserAccount {

        public UserAccountInformation Information;

        public byte[] ToBytes() {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(Information.mmr));
            bytes.AddRange(BitConverter.GetBytes(Information.steamid64));
            bytes.AddRange(Encoding.ASCII.GetBytes(Information.user + "\n" + Information.pass));
            return bytes.ToArray();
        }

        public void ModifyMMR(int change) {
            Information.mmr += change;
            UpdateInDB();
        }
        public void UpdateInDB() {
            SQLite.Query("UPDATE users SET mmr=@1 WHERE username=@2", Information.mmr.ToString(), Information.user);
        }

        public static UserAccount FromBytes(byte[] data) {
            UserAccountInformation uai = new UserAccountInformation();
            List<byte> info = data.ToList<byte>();
            int mmr = BitConverter.ToInt32(info.ToArray(), 0);
            info.RemoveRange(0, 4);
            ulong steamid64 = BitConverter.ToUInt64(info.ToArray(), 0);
            info.RemoveRange(0, 8);
            string login = Encoding.ASCII.GetString(info.ToArray());
            string[] parts = login.Split('\n');
            string user = parts[0];
            string pass = parts[1];
            uai.mmr = mmr;
            uai.steamid64 = steamid64;
            uai.user = user;
            uai.pass = pass;

            UserAccount account = new UserAccount();
            account.Information = uai;
            return account;
        }
        public static UserAccount Register(string username,string password,ulong steamid64) {
            SQLiteDataReader reader = SQLite.Query("SELECT * FROM users WHERE username=@1", username);
            if (reader.Read()) {
                return null; // failed to create account (user already exists)
            }
            SQLite.Query("INSERT INTO users(steamid64, mmr, username, password) VALUES (@1, @2, @3, @4);", steamid64.ToString(), "1000", username, password);

            UserAccountInformation uai = new UserAccountInformation();

            uai.mmr = 1000;
            uai.steamid64 = steamid64;
            uai.user = username;
            uai.pass = password;

            Console.WriteLine("Registration: " + username);

            UserAccount account = new UserAccount();
            account.Information = uai;
            return account;
        }
        public static UserAccount Login(string username,string password) {
            

            SQLiteDataReader reader = SQLite.Query("SELECT * FROM users WHERE username=@1 AND password=@2", username, password);
            if(!reader.Read()) {
                return null;
            }
            
            UserAccountInformation uai = new UserAccountInformation();
            
            uai.mmr = int.Parse((string)reader["mmr"]);
            uai.steamid64 = ulong.Parse((string)reader["steamid64"]);
            uai.user = username;
            uai.pass = password;

            Console.WriteLine("User Login: " + username);

            UserAccount account = new UserAccount();
            account.Information = uai;
            return account;
        }
    }
}
