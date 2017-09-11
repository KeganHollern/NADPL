using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NADPLServer.DotaBot.Dota_2 {
    #region JSON Objects
    public class Dota2HeroesRequest {
        public Result result { get; set; }
    }

    public class Result {
        public Hero[] heroes { get; set; }
        public int status { get; set; }
        public int count { get; set; }
    }

    public class Hero {
        public string name { get; set; }
        public int id { get; set; }
    }
    #endregion

    class SteamApiRequest {
        

        public static Dota2HeroesRequest getHeroData() {
            string data = "";
            using (WebClient client = new WebClient()) {
                data = client.DownloadString("https://api.steampowered.com/IEconDOTA2_570/GetHeroes/v0001/?key=C032FC1879DC46DB97759E41D5ED8D3B");
            }
            try {
                return JsonConvert.DeserializeObject<Dota2HeroesRequest>(data);
            } catch {
                return null;
            }
        }

    }
}
