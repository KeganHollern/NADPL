using NAPDL.League;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NADPLServer {
    enum LeagueBotInfoType {
        GameStarted,
        HeroList
    }

    class League {
        #region Private Variables
        private List<LeagueBot> Bots;

        #endregion

        #region Public Variables

        #endregion

        #region Events
        public delegate void BotStatusChangedEvent(int botNumber, LeagueBot bot);
        public event BotStatusChangedEvent OnBotStatusChanged;

        public delegate void BotLobbyChangedEvent(int botNumber, LeagueBot bot);
        public event BotLobbyChangedEvent OnBotLobbyChanged;

        public delegate void BotHeroesPickedEvent(int botNumber, LeagueBot bot, LeagueBotInfoType type, List<string> RadiantHeroes, List<string> DireHeroes);
        public event BotHeroesPickedEvent OnBotHeroesPicked;

        public delegate void BotGameStartedEvent(int botNumber, LeagueBot bot, LeagueBotInfoType type);
        public event BotGameStartedEvent OnBotGameStarted;
        #endregion

        #region Public Functions
        public League() {
            Bots = new List<LeagueBot>();

            LeagueBot bot;

            bot = new LeagueBot("napdlbot1", "keganandgoku", "NA Dota Pleb League #1");
            bot.OnBotStatusChanged += Bot_OnBotStatusChanged;
            bot.OnBotLobbyChanged += Bot_OnBotLobbyChanged;
            bot.OnBotGameStarted += Bot_OnBotGameStarted;
            bot.OnHeroesPicked += Bot_OnHeroesPicked;
            Bots.Add(bot);

            bot = new LeagueBot("napdlbot2", "keganandgoku12", "NA Dota Pleb League #2");
            bot.OnBotStatusChanged += Bot_OnBotStatusChanged;
            bot.OnBotLobbyChanged += Bot_OnBotLobbyChanged;
            bot.OnBotGameStarted += Bot_OnBotGameStarted;
            bot.OnHeroesPicked += Bot_OnHeroesPicked;
            Bots.Add(bot);

            bot = new LeagueBot("napdl3", "keganandgoku20", "NA Dota Pleb League #3");
            bot.OnBotStatusChanged += Bot_OnBotStatusChanged;
            bot.OnBotLobbyChanged += Bot_OnBotLobbyChanged;
            bot.OnBotGameStarted += Bot_OnBotGameStarted;
            bot.OnHeroesPicked += Bot_OnHeroesPicked;
            Bots.Add(bot);

            bot = new LeagueBot("NAPDLBOT4", "keganandgoku35", "NA Dota Pleb League #4");
            bot.OnBotStatusChanged += Bot_OnBotStatusChanged;
            bot.OnBotLobbyChanged += Bot_OnBotLobbyChanged;
            bot.OnBotGameStarted += Bot_OnBotGameStarted;
            bot.OnHeroesPicked += Bot_OnHeroesPicked;
            Bots.Add(bot);

        }

        private void Bot_OnHeroesPicked(LeagueBot bot, List<string> RadiantHeroes, List<string> DireHeroes) {
            if(OnBotHeroesPicked != null) {
                OnBotHeroesPicked(Bots.IndexOf(bot), bot, LeagueBotInfoType.HeroList, RadiantHeroes,DireHeroes);
            }
        }

        private void Bot_OnBotGameStarted(LeagueBot bot, BotLobby FinalLobby) {
            if (OnBotGameStarted != null) {
                OnBotGameStarted(Bots.IndexOf(bot), bot, LeagueBotInfoType.GameStarted);
            }
        }

        private void Bot_OnBotLobbyChanged(LeagueBot bot, BotLobby newLobby) {
            if (OnBotLobbyChanged != null) {
                OnBotLobbyChanged(Bots.IndexOf(bot), bot);
            }
        }

        private void Bot_OnBotStatusChanged(LeagueBot bot, bool isActive) {
            if(OnBotStatusChanged != null) {
                OnBotStatusChanged(Bots.IndexOf(bot), bot);
            }
        }
        public LeagueBot[] GetBots() {
            return Bots.ToArray();
        }
        #endregion

        #region Private Functions

        #endregion





    }
}
