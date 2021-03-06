using Common.Battle;
using System.Collections.Generic;

public class SkillDefine{
    public int ID{get;set;}
    public string Name{get;set;}
    public string Description{get;set;}
    public string Icon{get;set;}
    public SkillType Type{get;set;}
    public TargetType CastTarget{get;set;}
    public int UnlockLevel{get;set;}
    public float CastRange{get;set;}
    public float CastTime{get;set;}
    public float CD{get;set;}
    public float MPCost{get;set;}
    public bool Bullet { get; set; }
    public float BulletSpeed { get; set; }
    public string BulletResource { get; set; }
    public float ADERange { get; set; }
    public float AOEEffect { get; set; }
    public string SkillAnim { get; set; }
    public float Duration { get; set; }
    public float Interval { get; set; }
    public List<float> HitTimes { get; set; }
    public string HitEffect { get; set; }
    public List<int> Buff { get; set; }
    public float AD { get; set; }
    public float AP { get; set; }
    public float ADFactor { get; set; }
    public float ADFactor { get; set; }
}