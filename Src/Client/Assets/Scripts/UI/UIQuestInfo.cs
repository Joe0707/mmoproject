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
        private void Start()
        {
        }
        public void SetQuestInfo(Quest quest)
        {
            this.title.text = string.Format("{0}", quest.Define.Type);
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
            this.rewardMoney.text = quest.Define.RewardGold.ToString();
            this.rewardExp.text = quest.Define.RewardExp.ToString();
            foreach (var filter in this.GetComponentsInChildren<ContentSizeFitter>())
            {
                filter.SetLayoutVertical();
            }
        }

        private void Update()
        {
        }
    }
}
