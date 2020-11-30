using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    public class ItemManager:Singleton<ItemManager>
    {
        public Dictionary<int, Item> items = new Dictionary<int, Item>();

        void Init(List<NItemInfo> items)
        {
            this.items.Clear();
            foreach(var info in items)
            {
                Item item = new Item(info);
                this.items.Add(item.Id, item);
                Debug.LogFormat("ItemManager{0}", item);
            }
            StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
        }

        bool OnItemNotify(NStatus status)
        {
            if(status.Action==StatusAction.Add)
            {
                this.AddItem(status.Id, status.Value);
            }
            if(status.Action == StatusAction.Delete)
            {
                this.RemoveItem(status.Id, status.Value);
            }
        }

        void AddItem(int itemId,int count)
        {
            Item item = null;
            if(this.items.TryGetValue(itemId,out item))
            {
                item.Count += count;
            }
            else
            {
                item = new Item(itemId, count);
                this.items.Add(itemId, item);
            }
            BagManager.Instance.AddItem(itemId, count);
        }

        void Removeitem(int itemId,int count)
        {
            if(!this.items.ContainsKey(itemId))
            {
                return;
            }
            Item item = this.items[itemId];
            if (item.Count < count)
                return;
            item.Count -= count;
            BagManager.Instance.RemoveItem(itemId, count);
        }

        public ItemDefine GetItem(int itemId)
        {
            return null;
        }

        public bool HasItem(int itemId)
        {
            return false;
        }

        public bool UseItem(ItemDefine item)
        {
            return false;
        }
    }
}
