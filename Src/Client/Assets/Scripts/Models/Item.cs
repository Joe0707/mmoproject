using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Item
    {
        public int Id;
        public int Count;
        public ItemDefine Define;
        public EquipDefine EquipInfo;
        public Item(int id,int count)
        {
            this.Id = id;
            this.Count = count;
            this.Define = DataManager.Instance.Items[id];
            DataManager.Instance.Equips.TryGetValue(this.Id, out this.EquipInfo);
        }

        public Item(NItemInfo info)
        {
            this.Id = info.Id;
            this.Count = info.Count;
            this.Define = DataManager.Instance.Items[info.Id];
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
