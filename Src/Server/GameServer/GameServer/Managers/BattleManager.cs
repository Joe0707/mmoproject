using Common;
using GameServer.Entities;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class BattleManager:Singleton<BattleManager>
    {
        static long bid = 0;
        public void Init()
        {

        }

        public void ProcessBattleMessage(NetConnection<NetSession> sender,SkillCastRequest request)
        {
            Log.InfoFormat("BattleManager.ProcessBattleMessage:skill:");
            Character character = sender.Session.Character;
            var battle = MapManager.Instance[character.Info.mapId].Battle;

            battle.ProcessBattleMessage(sender, request);
        }
    }
}
