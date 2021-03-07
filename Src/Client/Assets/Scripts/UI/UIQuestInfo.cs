using Manager;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class UIQuestInfo:MonoBehaviour
    {
        public Text title;
        public Text[] targets;
        public Text description;
        public UIIconItem rewardItems;
        public Text rewardMoney;
        public Text rewardExp;
        public Text overview;

        public Button navButton;
        private int npc = 0;
        private void Start()
        {
        }
        public void SetQuestInfo(Quest quest)
        {
            this.title.text = string.Format("{0}", quest.Define.Type);
            if (this.overview != null) this.overview.text = quest.Define.Overview;
            if (this.description != null) { 
            if(quest.Info==null)
            {
                this.description.text = quest.Define.Dialog;
            }
            else
            {
                if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
                {
                    this.description.text = quest.Define.DialogFinish;
                }
            }
            }
            this.rewardMoney.text = quest.Define.RewardGold.ToString();
            this.rewardExp.text = quest.Define.RewardExp.ToString();

            if(quest.Info==null)
            {
                this.npc = quest.Define.AcceptNPC;
            }else if(quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                this.npc = quest.Define.SubmitNPC;
            }
            this.navButton.gameObject.SetActive(this.npc > 0);


            foreach (var filter in this.GetComponentsInChildren<ContentSizeFitter>())
            {
                filter.SetLayoutVertical();
            }
        }

        private void Update()
        {
        }

        public void OnClickAbandon()
        {

        }

        public void OnClickNav()
        {
            Vector3 pos = NpcManager.Instance.GetNpcPosition(this.npc);
            User.Instance.CurrentCharacterObject.StartNav(pos);
            UIManager.Instance.Close<UIQuestSystem>();
        }
    }
}
