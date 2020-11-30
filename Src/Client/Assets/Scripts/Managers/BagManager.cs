using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    class BagManager:Singleton<BagManager>
    {
        public int Unlocked;
        public BagItem[] Items;
        NBagInfo info;
        unsafe public void Init(NBagInfo info)
        {
            this.info = info;
            this.Unlocked = info.Unlocked;
            Items = new BagItem[this.Unlocked]; 
            if(info.Items!=null&&info.Items.Length>=this.Unlocked)
            {
                Analyze(info.Items);
            }
            else
            {
                info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }
        public void Reset()
        {
            int i = 0;
            foreach(var kv in ItemManager.Instance.items)
            {
                if (kv.Value.Count <= kv.Value.Define.StackLimit)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Count;
                }
                else
                {
                    int count = kv.Value.Count;
                    while (count > kv.Value.Define.StackLimit)
                    {
                        this.Items[i].ItemId = (ushort)kv.Key;
                        this.Items[i].Count = (ushort)kv.Value.Define.StackLimit;
                        i++;
                        count -= kv.Value.Define.StackLimit;

                    }
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)count;
                }
                i++;
            }
        }

        unsafe void Analyze(byte[]data)
        {
           fixed(byte*pt = data)
            {
                for( int i =0;i<this.Unlocked;i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    Items[i] = *item;
                }
            }
        }

        unsafe public NBagInfo GetBagInfo()
        {
            fixed(byte*pt = info.Items)
            {
                for(int i =0;i<this.Unlocked;i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = Items[i];
                }
            }
            return this.info;
        }

        public void AddItem(int itemId, int count)
        {
            ushort addCOUNT = (ushort)count;
            for(int i =0;i<Items.Length;i++)
            {
                if (this.Items[i].ItemId == itemId)
                {
                    ushort canAdd = (ushort)(DataManager.Instance.Items[itemId].StackLimit - Items.Count);
                    if(canAdd>=addCOUNT)
                    {
                        this.Items[i].Count += addCOUNT;
                        addCOUNT = 0;
                        break;
                    }
                    else
                    {
                        this.Items[i].Count += canAdd;
                        addCOUNT -= canAdd;
                    }
                }
            }
            if (addCOUNT > 0)
            {
                for(int i =0;i<Items.Length;i++)
                {
                    if(this.Items[i].ItemId==0)
                    {
                        this.Items[i].ItemId = (ushort)itemId;
                        this.Items[i].Count = addCOUNT;
                        break;
                    }
                }
            }
        }

        public void RemoveItem(int itemId,int count)
        {

        }


    }
}
