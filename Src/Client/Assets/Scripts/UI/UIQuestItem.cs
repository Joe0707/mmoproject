using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class UIQuestItem:ListView.ListViewItem
    {
        public Text title;
        public Image background;
        public Sprite normalBg;
        public Sprite selectedBg;
        public Quest quest;
        public override void onSelected(bool selected)
        {
            this.background.overrideSprite = selected ? selectedBg : normalBg;
        }
        void Start()
        {

        }

        bool isEquiped = false;
        public void SetQuestInfo(Quest item)
        {
            this.quest = item;
            if (this.title != null) this.title.text = this.quest.Define.Name;
        }
    }
}
