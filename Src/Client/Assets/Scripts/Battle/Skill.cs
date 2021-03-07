using Entities;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Battle
{
    public class Skill
    {
        public NSkillInfo Info;
        public Creature Owner;
        public Creature Target;
        public NVector3 TargetPosition;
        public SkillDefine Define;
        private float cd = 0;
        private float castTime = 0;
        private float skillTime;
        public int Hit = 0;
        private SkillStatus Status;

        public bool IsCasting = false;

        Dictionary<int, List<NDamageInfo>> HitMap = new Dictionary<int, List<NDamageInfo>>();
        List<Bullet> Bullets = new List<Bullet>();
        public float CD {
            get { return cd; }
        }
        public Skill(NSkillInfo info,Creature owner)
        {
            this.Info = info;
            this.Owner = owner;
            this.Define = DataManager.Instance.Skills[this.Owner.Define.TID][this.Info.Id];
        }

        public SkillResult CanCast(Creature target)
        {
            if (this.Define.CastTarget == Common.Battle.TargetType.Target && BattleManager.Instance.Target == null)
            {
                if(target==null||target==this.Owner)
                return SkillResult.InvalidTarget;
                int distance = this.Owner.Distance(target);
                if (distance > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }
            if(this.Define.CastTarget == Common.Battle.TargetType.Position&&BattleManager.Instance.CurrentPosition==null)
            {
                return SkillResult.InvalidTarget;
            }

            if(this.Owner.Attributes.MP<this.Define.MPCost)
            {
                return SkillResult.OutOfMP;
            }
            if (this.cd > 0)
            {
                return SkillResult.Cooldown;
            }
            return SkillResult.OK;
        }

        public void BeginCast(Creature target,NVector3 pos)
        {
            this.IsCasting = true;
            this.castTime = 0;
            this.skillTime = 0;
            this.Hit = 0;
            this.cd = this.Define.CD;
            this.Target = target;
            this.TargetPosition = pos;
            this.Owner.PlayAnim(this.Define.SkillAnim);
            this.Bullets.Clear();
            this.HitMap.Clear();
            if(this.Define.CastTarget == Common.Battle.TargetType.Position)
            {
                this.Owner.FaceTo(this.TargetPosition.ToVector3Int());
            }else if (this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                this.Owner.FaceTo(this.Target.position);
            }
            if (this.Define.CastTime > 0)
            {
                this.Status = SkillStatus.Casting;
            }
            else
            {
                this.StartSkill();
            }
        }
        /// <summary>
        /// 技能执行开始
        /// </summary>
        private void StartSkill()
        {
            this.Status = SkillStatus.Running;
            if (!string.IsNullOrEmpty(this.Define.AOEEffect))
            {
                if (this.Define.CastTarget == Common.Battle.TargetType.Position)
                    this.Owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.TargetPosition);
                else if (this.Define.CastTarget == Common.Battle.TargetType.Target)
                    this.Owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.Target);
                else if (this.Define.CastTarget == Common.Battle.TargetType.Self)
                    this.Owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.Owner);
            }
        }

        public void OnUpdate(float delta)
        {
            UpdateCD(delta);

            if (this.Status == SkillStatus.Casting)
            {
                this.UpdateCasting();
            }
            if (this.Status == SkillStatus.Running)
            {
                this.UpdateSkill();
            }

        }

        internal void DoHit(NSkillHitInfo hit)
        {
            if(hit.isBullet||!this.Define.Bullet)
            {
                this.DoHit(hit.hitId,hit.Damages);
            }
        }

        internal void BeginCast(Creature target)
        {
            
        }

        //private void DoHit()
        //{
        //    if (this.Damage != null)
        //    {
        //        var cha = CharacterManager.Instance.GetCharacter(this.Damage.entityId);
        //        cha.DoDamage(this.Damage);
        //    }
        //    this.Hit++;
        //}

        private void UpdateCD(float delta)
        {
            if(this.cd>0)
            {
                this.cd -= delta;
            }
            if (cd < 0)
            {
                this.cd = 0;
            }
        }

        internal void DoHit(int hitId, List<NDamageInfo> damages)
        {
            if (hitId > this.Hit)
                this.HitMap[hitId] = damages;
            else
                DoHitDamages(damages);
        }

        private void UpdateSkill()
        {
            throw new NotImplementedException();
        }

        private void UpdateCasting()
        {
            if (this.castTime < this.Define.CastTime)
            {
                this.castTime += Time.deltaTime;
            }
            else
            {
                this.castTime = 0;
                this.StartSkill();
                Debug.LogFormat("SkillUpdateCasting");
            }
        }

        private void UpdateSkill()
        {
            this.skillTime += Time.deltaTime;
            if (this.Define.Duration > 0)
            {
                //持续技能
                if (this.skillTime > this.Define.Interval * (this.Hit + 1))
                {
                    this.DoHit();
                }
                if(this.skillTime>=this.Define.Duration)
                {
                    this.Status = SkillStatus.None;
                    this.IsCasting = false;
                    Debug.LogFormat("SkillUpdateSkill");
                }
            }
            else if (this.Define.HitTimes != null && this.Define.HitTimes.Count > 0)
            {
                if (this.Hit < this.Define.HitTimes.Count)
                {
                    if (this.skillTime > this.Define.HitTimes[this.Hit])
                    {
                        this.DoHit();
                    }
                }
                else
                {
                    if (!this.Define.Bullet) { 
                        this.Status = SkillStatus.None;
                        this.IsCasting = false; 
                        Debug.LogFormat("SkillUpdateSkillFinish");
                    }
                }
            }

            if(this.Define.Bullet)
            {
                bool finish = true;
                foreach(Bullet bullet in this.Bullets)
                {
                    bullet.Update();
                    if (!bullet.Stoped) finish = false;
                }

                if(finish&&this.Hit>=this.Define.HitTimes.Count)
                {
                    this.Status = SkillStatus.None;
                    this.IsCasting = false;
                    Debug.LogFormat("Skill UpdateSkill Finish");
                }
            }
        }

        private void DoHit()
        {
            //List<NDamageInfo> damages;
            //if(this.HitMap.TryGetValue(this.Hit,out damages))
            //{
            //    DoHitDamages(damages);
            //}
            if (this.Define.Bullet)
            {
                this.CastBullet();
            }
            else
                this.DoHitDamages(this.Hit);
            this.Hit++;
        }

        private void CastBullet()
        {
            Bullet bullet = new Bullet(this);
            Debug.LogFormat("Skill CastBullet");
            this.Bullets.Add(bullet);
            this.Owner.PlayEffect(EffectType.Bullet, this.Define.BulletResource, this.Target, bullet.duration);
        }



        private void DoHitDamages(int hit)
        {
            List<NDamageInfo> damages;
            if(this.HitMap.TryGetValue(hit,out damages))
            {
                DoHitDamages(damages);
            }
        }

        private void DoHitDamages(List<NDamageInfo> damages)
        {
            foreach(var dmg in damages)
            {
                Creature target = EntityManager.Instance.GetEntity(dmg.entityId) as Creature;
                if (target == null) continue;
                target.DoDamage(dmg,true);
                if (this.Define.HitEffect != null)
                {
                    target.PlayEffect(EffectType.Hit, this.Define.HitEffect, target);
                }
            }
        }
    }
}
