﻿using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindow
{
    public Text decript;
    public GameObject itemPrefab;
    public ListView listMain;
    private UIRideItem selectedItem;

    private void Start()
    {
        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSelected;
    }

    private void OnDestroy()
    {
        
    }

    public void OnItemSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIRideItem;
        this.decript.text = this.selectedItem.item.Define.Description;
    }

    void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    void InitItems()
    {
        foreach(var kv in ItemManager.Instance.items)
        {
            if(kv.Value.Define.Type==ItemType.Ride&&kv.Value.Define.LimitClass==CharacterClass.None||kv.Value.Define.LimitClass==User.Instance.CurrentCharacterInfo.Class)
            {
                //已经装备就不显示
                if (EquipManager.Instance.Contains(kv.Key))
                    continue;
                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetEquipItem(kv.Value, this, false);
                this.listMain.AddItem(ui);
            }
        }
    }

    void ClearItems()
    {
        this.listMain.RemoveAll();
    }

    public void DoRide()
    {
        if(this.selectedItem==null)
        {
            MessageBox.Show("请选择要召唤的坐骑", "提示");
            return;
        }
        User.Instance.Ride(this.selectedItem.item.Id);
    }
}
