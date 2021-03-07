using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIGuildList:UIWindow
    {
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRott;
    public UIGuildInfo uiInfo;
    public UIGuildItem selectedItem;

    private void Start()
    {
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.uiInfo.Info = null;
        GuildService.Instance.OnGuildListResult += UpdateGuildList;
        GuildService.Instance.SendGuildListRequest();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildListResult -= UpdateGuildList;

    }

    void UpdateGuildList(List<NGuildInfo> guilds)
    {
        ClearList();
        InitItems(guilds);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildItem;
        this.uiInfo.Info = this.selectedItem.Info;
    }

    /// <summary>
    /// 初始化所有装备列表
    /// </summary>
    /// <param name="guilds"></param>
    void InitItems(List<NGuildInfo> guilds)
    {
        foreach(var item in guilds)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildItem ui = go.GetComponent<UIGuildItem>();
            ui.SetGuildInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    void ClearList()
    {
        this.listMain.RemoveAll();
    }

    public void OnClickJoin()
    {
        if(selectedItem==null)
        {
            MessageBox.Show("请选择要加入的公会");
            return;
        }
        MessageBox.Show(string.Format("确定要加入公会{0}吗", selectedItem.Info.GuildName), "申请加入公会").OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinRequest(this.selectedItem.Info.Id);
        };
    }
}
