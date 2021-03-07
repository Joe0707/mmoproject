using Entities;
using Managers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI;
using UnityEngine.UI;

public class UIMain:MonoSingleton<UIMain>
    {
    public Text avatarName;
    public Text avatarLevel;

    public UITeam TeamWindow;
    public UICreatureInfo targetUI;
    public UISkillSlots skillSlots;
    protected override void OnStart()
    {
        //base.OnStart();
        this.UpdateAvatar();
        this.targetUI.gameObject.SetActive(false);
        BattleManager.Instance.OnTargetChanged += OnTargetChanged;
        User.Instance.OnCharacterInit += this.skillSlots.UpdateSkills;
        this.skillSlots.UpdateSkills();
    }
     
    private void OnTargetChanged(Creature target)
    {
        if (target != null)
        {
            if (!targetUI.isActiveAndEnabled) targetUI.gameObject.SetActive(true);
            targetUI.Target = target;
        }
        else
        {
            targetUI.gameObject.SetActive(false);
        }
    }

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}{1}", User.Instance.CurrentCharacterInfo.Name, User.Instance.CurrentCharacterInfo.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacterInfo.Level.ToString();
    }

    private void Update()
    {
        
    }

    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }

    public void OnClickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnClickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }

    public void OnClickFriends()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }

    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }

    public void OnClickRide()
    {
        UIManager.Instance.Show<UIRide>();
    }
}
