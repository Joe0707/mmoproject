using Common.Data;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class NpcManager : Singleton<NpcManager>
    {
        public delegate bool NpcActionHandler(NpcDefine npc);
        Dictionary<NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();
        Dictionary<int, Vector3> npcPositions = new Dictionary<int, Vector3>();
        public void RegisterNpcEvent(NpcFunction function, NpcActionHandler action)
        {
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
            {
                eventMap[function] += action;
            }
        }
        public NpcDefine GetNpcDefine(int npcID)
        {
            return DataManager.Instance.Npcs[npcID];
        }

        public bool Interactive(int npcId)
        {
            if (DataManager.Instance.Npcs.ContainsKey(npcId))
            {
                var npc = DataManager.Instance.Npcs[npcId];
                return Interactive(npc);
            }
            return false;
        }

        public bool Interactive(NpcDefine npc)
        {
            if (DoTaskInteractive(npc))
            {
                return true;
            }else if(npc.Type==NpcType.Functional)
            {
                return DoFunctionInteractive(npc);
            }
            return false;
        }

        private bool DoTaskInteractive(NpcDefine npc)
        {
            var status = QuestManager.Instance.GetQuestStatusByNpc(npc.ID);
            if (status == NpcQuestStatus.None)
                return false;
            return QuestManager.Instance.OpenNpcQuest(npc.ID);
        }
        private bool DoFunctionInteractive(NpcDefine npc)
        {
            if(npc.Type!=NpcType.Functional)
            {
                return false;
            }
            if(!eventMap.ContainsKey(npc.Function))
            {
                return false;
            }
            return eventMap[npc.Function](npc);
        }

        public void UpdateNpcPosition(int npc,Vector3 pos)
        {
            this.npcPositions[npc] = pos;
        }

        public Vector3 GetNpcPosition(int npc)
        {
            return this.npcPositions[npc];
        }
    }
}