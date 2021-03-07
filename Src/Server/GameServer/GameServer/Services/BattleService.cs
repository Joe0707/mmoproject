using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class BattleService:Singleton<BattleService>
    {
        public BattleService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillCastRequest>(this.OnSkillCast);
        }

        public void Init()
        {

        }

        void OnSkillCast(NetConnection<NetSession> sender,SkillCastRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnSkillCast:skill");
            BattleManager.Instance.ProcessBattleMessage(sender, request);
        }
    }
}
