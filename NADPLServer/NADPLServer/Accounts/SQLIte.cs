using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
namespace NADPLServer.Accounts {
    class SQLite {
        private static SQLiteConnection dbConnection;


        public static SQLiteDataReader Query(string query,params string[] paramters) {
            SQLiteCommand command = new SQLiteCommand(query, dbConnection);
            int i = 1;
            foreach(string param in paramters) {
                command.Parameters.Add(new SQLiteParameter("@" + i.ToString(), param));
                i++;
            }
            return command.ExecuteReader();
        }
        public static SQLiteDataReader Query(string query,bool getResult = false) {
            SQLiteCommand command = new SQLiteCommand(query, dbConnection);
            if(getResult) {
                return command.ExecuteReader();
            } else {
                command.ExecuteNonQuery();
                return null;
            }
        }
        public static void Connect() {
            CreateIfNotExist();
            dbConnection = new SQLiteConnection("Data Source=Users.sqlite;Version=3;");
            dbConnection.Open();
            FirstTimeSetup();
        }
        private static void FirstTimeSetup() {
            SQLiteCommand command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users (steamid64 VARCHAR(17),mmr VARCHAR(10),username VARCHAR(50),password VARCHAR(50))", dbConnection);
            command.ExecuteNonQuery();
        }
        private static void CreateIfNotExist() {
            if (!File.Exists("Users.sqlite")) {
                SQLiteConnection.CreateFile("Users.sqlite");
            }
        }
    }
}
