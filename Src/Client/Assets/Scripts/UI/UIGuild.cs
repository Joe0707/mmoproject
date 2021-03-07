using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIGuild:UIWindow
    {
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;
    public GameObject panelLeader;
    private void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

    private void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();

        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
    }

    void InitItems()
    {
        foreach(var item in GuildManager.Instance.guildInfo,Members)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    void ClearList()
    {
        this.listMain.RemoveAll();
    }

    public void OnClickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }

    public void OnClickLeave()
    {

    }

    public void OnClickChat()
    {

    }

    public void OnClickKickout()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员");
            return;
        }
        MessageBox.Show(string.Format("要踢{0}出公会吗"), "踢出公会", MessageBoxType.Confirm).OnYes=()=> { GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout,this.selectedItem.Info.Info.Id)};

    }

    public void OnClickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("选择要晋升的成员");
            return;
        }
        if (selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方已经身份尊贵");
            return;
        }
        MessageBox.Show(string.Format("要晋升{0}为公会副会长吗"), "晋升", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要罢免的成员");
            return;
        }
        if(selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方貌似无知可免");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("会长不是你能动的");
            return;
        }
        MessageBox.Show(string.Format("要罢免的公会职务吗"), "罢免职务", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.InfoInfo.Id);
        };

    }

    public void OnClickTransfer()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要把会长转让给的成员");
            return;
        }
        MessageBox.Show(string.Format("要吧会长转给{0}吗？"), "转移会长", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickSetNotice()
    {
    }
}
