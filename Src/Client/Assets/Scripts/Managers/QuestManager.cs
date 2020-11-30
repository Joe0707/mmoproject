using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI;
using UnityEngine.Events;

namespace Managers
{
    public enum NpcQuestStatus
    {
        None,
        Complete,
        Available,
        Incomplete
    }
    class QuestManager:Singleton<QuestManager>
    {
        public List<NQuestInfo> questInfos;
        public Dictionary<int, Quest> allQuest = new Dictionary<int, Quest>();
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();
        public UnityAction<Quest> onQuestStatusChanged;
        public void Init(List<NQuestInfo> quests)
        {
            this.questInfos = quests;
            allQuest.Clear();
            this.npcQuests.Clear();
            InitQuests();
        }

        public void InitQuests()
        {
            //初始化已有任务
            foreach(var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                this.AddNpcQuest(quest.Define.AcceptNPC, quest);
                this.AddNpcQuest(quest.Define.SubmitNPC, quest);
                this.allQuest[quest.Info.QuestId] = quest;
            }
            this.CheckAvailableQuests();
            foreach(var kv in this.allQuest)
            {
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
                //if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacter.Class) continue;//不符合职业
                //if (kv.Value.LimitLevel > User.Instance.CurrentCharacter.Level)
                //    continue;//不符合等级
                //if (this.allQuest.ContainsKey(kv.Key))
                //    continue;//任务已经存在
            }
        }

         void CheckAvailableQuests()
        {
            foreach(var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacter.Class)
                    continue;
                if (kv.Value.LimitLevel > User.Instance.CurrentCharacter.Level)
                    continue;
                if (this.allQuest.ContainsKey(kv.Key))
                    continue;
                if(kv.Value.PreQuest>0)
                {
                    Quest preQuest;
                    if (this.allQuest.TryGetValue(kv.Value.PreQuest, out preQuest))//获取前置任务
                    {
                        if (preQuest.Info == null)
                            continue;//前置任务未获取
                        if (preQuest.Info.Status != QuestStatus.Finished)
                            continue;
                        //前置任务未完成
                    }
                    else
                        continue;//前置任务还没接
                }
                Quest quest = new Quest(kv.Value);
                this.allQuest[quest.Define.ID] = quest;
            }
        }

        void AddNpcQuest(int npcId,Quest quest)
        {
            if (!this.npcQuests.ContainsKey(npcId))
            {
                this.npcQuests[npcId] = new Dictionary<NpcQuestStatus, List<Quest>>();
            }

            List<Quest> availables;
            List<Quest> completes;
            List<Quest> incompletes;
            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Available] = availables;
            }
            if(!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Complete,out completes))
            {
                completes = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Complete] = completes;
            }
            if(!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplete,out incompletes))
            {
                incompletes = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Incomplete] = incompletes;
            }
        }
        public NpcQuestStatus GetQuestStatusByNpc(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if(this.npcQuests.TryGetValue(npcId,out status))//获取NPC任务
            {
                if(status[NpcQuestStatus.Complete].Count>0)
                {
                    return NpcQuestStatus.Complete;
                }
                if (status[NpcQuestStatus.Available].Count > 0)
                {
                    return NpcQuestStatus.Available;
                }
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                {
                    return NpcQuestStatus.Incomplete;
                }
            }
            return NpcQuestStatus.None;
        }

        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if(this.npcQuests.TryGetValue(npcId,out status))//获取NPC任务
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Complete].First());
                }
                if(status[NpcQuestStatus.Available].Count>0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Available].First());
                }
                if(status[NpcQuestStatus.Incomplete].Count>0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Incomplete].First());
                }
            }
            return false;
        }

        bool ShowQuestDialog(Quest quest)
        {
            if(quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                UIQuestDialog dlg = UIManager.Instance.Show<UIQuestDialog>();
                dlg.SetQuest(quest);
                dlg.OnClose += OnQuestDialogClose;
                return true;
            }
            if (quest.Info != null || quest.Info.Status != QuestStatus.Complated)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                {
                    MessageBox.Show(quest.Define.DialogIncomplete);
                }
            }
            return true;
        }
        void OnQuestDialogClose(UIWindow sender,UIWindow.WindowResult result)
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if (result == UIWindow.WindowResult.Yes)
            {
                MessageBox.Show(dlg.quest.Define.DialogAccept);
            }else if(result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(dlg.quest.Define.DialogDeny);
            }
        }

        public void OnQuestAccepted(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogAccept);
        }

        public void OnQuestSubmited(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogFinish);
        }

        Quest RefreshQuestStatus(NQuestInfo quest)
        {
            this.npcQuests.Clear();
            Quest result;
            if (this.allQuest.ContainsKey(quest.QuestId))
            {
                //更新新的任务状态
                this.allQuest[quest.QuestId].Info = quest;
                result = this.allQuest[quest.QuestId];
            }
            else
            {
                result = new Quest(quest);
                this.allQuest[quest.QuestId] = result;
            }
            CheckAvailableQuests();
            foreach(var kv in this.allQuest)
            {
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }
            if (onQuestStatusChanged != null)
                onQuestStatusChanged(result);
            return result;
        }
    }
}
