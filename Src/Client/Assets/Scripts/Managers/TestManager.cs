using Common.Data;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : Singleton<TestManager> {
    public void Init()
    {
        NpcManager.Instance.RegisterNpcEvent(Common.Data.NpcFunction.InvokeShop, OnNpcInvokeShop);
        NpcManager.Instance.RegisterNpcEvent(Common.Data.NpcFunction.InvokeInsrance, OnNpcInvokeInsrance);
    }

    bool OnNpcInvokeShop(NpcDefine npc)
    {
        Debug.LogFormat("Testmanager{0}", npc.ID);
        UITest text = UIManager.Instance.Show<UITest>();
        return true;
    }

    bool OnNpcInvokeInsrance(NpcDefine npc)
    {
        MessageBox.Show("点击了NPC"+npc.Name, "npc对话");
        return true;
    }
}
