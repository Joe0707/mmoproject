﻿using Battle;
using Managers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public Image overlay;
    public Text cdText;
    Skill skill;


    void Start()
    {
        overlay.enabled = false;
        cdText.enabled = false;  

    }

    void Update()
    {
        if (this.skill == null) return;
        if (this.skill.CD > 0)
        {
            if (!overlay.enabled) overlay.enabled = true;
            if (!cdText.enabled) cdText.enabled = true;
            overlay.fillAmount = this.skill.CD / this.skill.Define.CD;
            this.cdText.text = ((int)Math.Ceiling(this.skill.CD)).ToString();
        }
        else
        {
            if (overlay.enabled) overlay.enabled = false;
            if (this.cdText.enabled) this.cdText.enabled = false;
        }
    }

    public void OnPositionSelected(Vector3 pos)
    {
        BattleManager.Instance.CurrentPosition = GameObjectTool.WorldToLogicN(pos);
        this.CastSkill();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(this.skill.Define.CastTarget==Common.Battle.TargetType.Position)
        {
            TargetSelector.ShowSelector(User.Instance.CurrentCharracter.position, this.skill.Define.CastRange, this.skill.Define.AOERange, OnPositionSelected);
            return;
        }
        CastSkill();
    }

    private void CastSkill() { 
        SkillResult result = this.skill.CanCast(BattleManager.Instance.CurrentTarget);
        switch (result)
        {
            case SkillResult.InvalidTarget:
                MessageBox.Show("技能" + this.skill.Define.Name + "目标无效");
                return;
            case SkillResult.OutOfMP:
                MessageBox.Show("技能" + this.skill.Define.Name + "MP不足");
                return;
            case SkillResult.Cooldown:
                MessageBox.Show("技能" + this.skill.Define.Name + "正在冷却");
                return;
            case SkillResult.OutOfRange:
                MessageBox.Show("技能" + this.skill.Define.Name + "目标超出施放范围");
                return;
        }
        BattleManager.Instance.CastSkill(this.skill);
        //MessageBox.Show("释放技能" + this.skill.Define.Name);
        //this.SetCD(this.skill.Define.CD);
        //this.skill.Cast();

        //if (this.overlay.fillAmount>0)
        //{
        //    MessageBox.Show("技能" + this.skill.Define.Name+"正在冷却");
        //}
        //else
        //{
        //    MessageBox.Show("使用技能");
        //    this.SetCD(this.skill.Define.CD);
        //}
    }

    public void SetCD(float cd)
    {
        if (!overlay.enabled) overlay.enabled = true;
        if (!cdText.enabled) cdText.enabled = true;
        this.cdText.text = ((int)Math.Floor(this.cdRemain)).ToString();
        overlay.fillAmount = 1f;
        overlaySpeed = 1f / cd;
        cdRemain = cd;
    }

    public void SetSkill(Skill value)
    {
        this.skill = value;
        if (this.icon != null)
        {
            this.icon.overrideSprite = Resloader.Load<Sprite>(this.skill.Define.Icon);
            this.icon.SetAllDirty();
        }
        this.SetCD(this.skill.Define.CD);
    }
}
