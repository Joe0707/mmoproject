using Candlelight.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIChat : MonoBehaviour
{
    public HyperText textArea;//聊天内容显示区域
    public TabView cannelTab;
    public InputField chatText;//聊天输入控件
    public Text chatTarget;
    public Dropdown channelSelect;

    private void Start()
    {
        this.cannelTab.OnTabSelect += OnDisplayChannelSelected;
        ChatManager.Instance.OnChat += RefreshUI;
    }

    private void OnDestroy()
    {
        ChatManager.Instance.OnChat -= RefreshUI;
    }

    private void Update()
    {
        InputManager.Instance.IsInputMode = chatText.isFocused;
    }

    void OnDisplayChannelSelected(int idx)
    {
        ChatManager.Instance.displayChannel = (ChatManager.LocalChannel)idx;
        RefreshUI();
    }

    public void RefreshUI()
    {
        this.textArea.text = ChatManager.Instance.GetCurrentMessages();
        this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        if (ChatManager.Instance.SendChannel == SkillBridge.Message.ChatChannel.Private)
        {
            this.chatTarget.gameObject.SetActive(true);
            if(ChatManager.Instance.PrivateID!=0)
            {
                this.chatTarget.text = ChatManager.Instance.PrivateName + ":";
            }
            else
            {
                this.chatTarget.text = "<无>：";
            }
        }
        else
        {
            this.chatTarget.gameObject.SetActive(false);
        }
    }

    public void OnClickChatLink(HyperText text,HyperText.LinkInfo link)
    {
        if (string.IsNullOrEmpty(link.Name))
            return;
        if(link.Name.StartsWith("c:"))
        {
            string[] strs = link.Name.Split(":".ToCharArray());
            UIPopCharMenu menu = UIManager.Instance.Show<UIPopCharMenu>();
            menu.targetId = int.Parse(strs[1]);
            menu.targetName = strs[2];
        }
    }

    public void OnClickSend()
    {
        OnEndInput(this.chatText.text);
    }

    public void OnEndInput(string text)
    {
        if (!string.IsNullOrEmpty(text.Trim()))
            this.SendChat(text);
        this.chatText.text = "";
    }

    void SendChat(string content)
    {
        ChatManager.Instance.SendChat(content, ChatManager.Instance.PrivateID, ChatManager.Instance.PrivateName);
    }

    public void OnSendChannelChanged(int idx)
    {
        //判断是否设置的就是当前频道
        if (ChatManager.Instance.sendChannel == (ChatManager.LocalChannel)(idx + 1))
            return;
        if (!ChatManager.Instance.SetSendChannel((ChatManager.LocalChannel)idx + 1))
        {
            //设置失败则设置选择频道为当前频道
            this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        }
        else
        {
            //设置成功刷新UI
            this.RefreshUI();
        }
    }
}
