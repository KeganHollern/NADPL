using Dota2.GC.Dota.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAPDL.Dota {

    class DotaLobbyParams {
        private List<ulong> RadiantTeam;
        private List<ulong> DireTeam;
        private int MinPlayers = 0;

        public bool hasAllPlayers(int readyPlayers) {
            return (MinPlayers == readyPlayers);
        }

        public bool isReadyPlayer(CDOTALobbyMember member) {
            if(RadiantTeam.Contains(member.id)) {
                return (member.team == DOTA_GC_TEAM.DOTA_GC_TEAM_GOOD_GUYS); 
            }
            if (DireTeam.Contains(member.id)) {
                return (member.team == DOTA_GC_TEAM.DOTA_GC_TEAM_BAD_GUYS);
            }
            return false;
        }
        public DotaLobbyParams(ulong[] Radiant,ulong[] Dire) {
            RadiantTeam = Radiant.ToList<ulong>();
            DireTeam = Dire.ToList<ulong>();

            MinPlayers = DireTeam.Count + RadiantTeam.Count;
            if(MinPlayers == 0) { MinPlayers = 1; }
        }
        public DotaLobbyParams(List<ulong> Radiant, List<ulong> Dire) {
            RadiantTeam = Radiant;
            DireTeam = Dire;

            MinPlayers = DireTeam.Count + RadiantTeam.Count;
            if (MinPlayers == 0) { MinPlayers = 1; }
        }
    }
}
