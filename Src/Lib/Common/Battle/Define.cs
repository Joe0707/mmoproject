using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Battle
{
    public enum AttributeType
    {
        None = -1,
        MaxHP = 0,//最大生命
        MaxMP = 1,//最大法力
        STR=2,//力量
        INT = 3,//智力
        DEX = 4,//敏捷
        AD = 5,//物理攻击
        AP = 6,//法术攻击
        DEF = 7,//物理防御
        MDEF = 8,//法术防御
        SPD = 9,//攻击速度
        CRI  =10,//暴击概率
        MAX
    }
    public enum SkillType
    {
        All = -1,
        Normal = 1,
        Skill = 2,
        Passive = 4
    }

    public enum TargetType { 
        None =0,
        Target = 1,
        Self = 2,
        Position
    }

    public enum BuffEffect
    {
        None = 0,
        Stun = 1,   //眩晕
        Invincible = 2  //无敌
    }

    public enum SkillResult
    {
        OK, 
        InvalidTarget,
        OutOfMP,
        Cooldown,
        Casting,
        OutOfRange
    }

    public enum TriggerType
    {
        None = 0,   
        SkillCast = 1,  //技能释放时
        SkillHit = 2    //技能命中时
    }

    public enum BattleState{
        None,
        Idle,   //空闲
        InBattle//战斗中
    }
}
