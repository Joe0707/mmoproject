using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class UICharEquip
    {
        public Text title;
        public Text money;
        public GameObject itemPrefab;
        public GameObject itemEquipedPrefab;

        public Transform itemListRoot;
        public List<Transform> slots;
        void Start()
        {
            RefreshUI();
            EquipManager.Instance.OnEquipChange += RefreshUI;
        }

        void OnDestroy()
        {
            EquipManager.Instance.OnEquipChanged -= RefreshUI;
        }

        void RefreshUI()
        {
            ClearAllEquipList();
            InitAllEquipItems();
            ClearEquipedList();
            InitEquipedItems();
            this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
        }

        void InitAllEquipItems()
        {
            foreach(var kv in ItemManager.Instance.items)
            {
                if (kv.Value.Define.Type == ItemType.Equip&&kv.Value.Define.LimitClass==User.Instance.CurrentCharacter.Class)
                {
                    if (EquipManager.Instance.Contains(kv.Key)) continue;
                    GameObject go = GameObject.Instantiate(itemPrefab, itemListRoot);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(kv.Key, kv.Value, this, false);
                }
            }
        }

        void ClearAllEquipList()
        {
            foreach (var item in itemListRoot.GetComponentsInChildren<UIEquipItem>())
            {
                GameObject.Destroy(item.gameObject);
            }
        }
        
        void ClearEquipedList()
        {
            foreach(var item in slots)
            {
                if(item.childCount>0)
                {
                    GameObject.Destroy(item.GetChild(0).gameObject);
                }
            }
        }

        void InitEquipedItems()
        {
            for(int i =0;i<(int)EquipSlot.SlotMax;i++)
            {
                var item = EquipManager.Instance.Equips[i];
                if(item!=null)
                {
                    GameObject go = GameObject.Instantiate(itemEquipedPrefab, slots[i]);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(i, item, this, true);
                }
            }
        }

        public void DoEquip(Item item)
        {
            EquipManager.Instance.EquipItem(item);
        }

        public void UnEquip(Item item)
        {
            EquipManager.Instance.UnEquipItem(item);
        }

    }
}
