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
    protected override void OnStart()
    {
        base.OnStart();
    }

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}{1}", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
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
}
