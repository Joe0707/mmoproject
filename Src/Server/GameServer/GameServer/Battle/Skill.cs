﻿using Battle;
using Common;
using Common.Battle;
using Common.Utils;
using GameServer.Core;
using GameServer.Entities;
using GameServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    class Skill
    {
        public NSkillInfo Info;
        public Creature Owner;
        public SkillDefine Define;

        public SkillStatus Status;
        private float cd = 0;

        public float CD
        {
            get { return cd; }
        }

        private float castingTime = 0;
        private float skillTime = 0;
        private int Hit = 0;
        BattleContext Context;
        List<Bullet> Bullets = new List<Bullet>();


        public bool Instant { get {
                if (this.Define.CastTime > 0) return false;
                if (this.Define.Bullet) return false;
                if (this.Define.Duration > 0) return false;
                if (this.Define.HitTimes != null && this.Define.HitTimes.Count > 0) return false;
                return true;

            }
        }

        public Skill(NSkillInfo info,Creature owner,SkillDefine define)
        {
            this.Info = info;
            this.Owner = owner;
            this.Define = define;
        }

        public Skill(NSkillInfo info,Creature owner)
        {
            this.Info = info;
            this.Owner = owner;
            this.Define = DataManager.Instance.Skills[this.Owner.Define.TID][this.Info.Id];
        }

        public SkillResult CanCast(BattleContext context)
        {
            if (this.Status != SkillStatus.None)
            {
                return SkillResult.Casting;
            }
            if (this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                if (context.Target == null || context.Target == this.Owner)
                    return SkillResult.InvalidTarget;
                int distance = this.Owner.Distance(context.Target);
                if (distance > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }
            if (this.Define.CastTarget == Common.Battle.TargetType.Position)
            {
                if (context.CastSkill.Position == null)
                    return SkillResult.InvalidTarget;
                if (this.Owner.Distance(context.Position) > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }

            if (this.Owner.Attributes.MP > this.Define.MPCost)
            {
                return SkillResult.OutOfMP;
            }

            if (this.cd > 0)
            {
                return SkillResult.Cooldown;
            }
            return SkillResult.OK;
        }

        internal SkillResult Cast(BattleContext context)
        {
            SkillResult result = this.CanCast(context);
            if (result == SkillResult.OK)
            {
                this.castingTime = 0;
                this.skillTime = 0;
                this.cd = this.Define.CD;
                this.Context = context;
                this.Hit = 0;
                this.Bullets.Clear();

                this.AddBuff(TriggerType.SkillCast,this.Context.Target);

                if (this.Instant)
                {
                    this.DoHit();
                }
                else
                { 
                    if (this.Define.CastTime > 0)
                    {
                        this.Status = SkillStatus.Casting;
                    }
                    else
                        this.Status = SkillStatus.Running;
                }
            }
            Log.InfoFormat("SkillCast");
            return result;
        }

        private void DoHit()
        {
            var hitInfo =  this.InitHitInfo(false);
            Log.InfoFormat("SkillDoHit");
            this.Hit++;
            if (this.Define.Bullet)
            {
                CastBullet(hitInfo);
                return;
            }
            DoHit(hitInfo);
        }

        public void DoHit(NSkillHitInfo hitInfo)
        {
            Context.Battle.AddHitInfo(hitInfo);
            Log.InfoFormat("Skill DoHit IsBullet");

            if(this.Define.ADERange>0)
            {
                this.HitRange(hitInfo);
                return;
            }

            if(this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                this.HitTarget(Context.Target, hitInfo);
            }
        }

        private NSkillHitInfo InitHitInfo(bool isBullet)
        {
            var hitInfo = new NSkillHitInfo();
            hitInfo.casterId = this.Context.Caster.entityId;
            hitInfo.skillId = this.Info.Id;
            hitInfo.hitId = this.Hit;
            hitInfo.isBullet = isBullet;
            return hitInfo;
            //Context.Battle.AddHitInfo(hitInfo);

        }

        private void HitTarget(Creature target,NSkillHitInfo hit)
        {
            if (this.Define.CastTarget == Common.Battle.TargetType.Self && (target != Context.Caster)) return;
            else if (target == Context.Caster) return;

            NDamageInfo damage = this.CalcSkillDamage(Context.Caster, target);
            Log.InfoFormat("Skill.HitTarget");
            target.DoDamage(damage, Context.Caster);
            hit.Damages.Add(damage);

            this.AddBuff(TriggerType.SkillHit,target);
        }

        private NDamageInfo CalcSkillDamage(Creature caster, Creature target)
        {
            float ad = this.Define.AD + caster.Attributes.AD + this.Define.ADFactor;
            float ap = this.Define.AP + caster.Attributes.AP + this.Define.APFactor;

            float addmg = ad * (1 - target.Attributes.DEF / (target.Attributes.DEF + 100));
            float apdmg = ap * (1 - target.Attributes.MDEF / (target.Attributes.MDEF + 100));

            float final = addmg + apdmg;
            bool isCrit = IsCrit(caster.Attributes.CRI);
            if (isCrit)
                final = final * 2f;
            Log.InfoFormat("Skill.HitTarget");

            //随机浮动
            final = final * (float)MathUtil.Random.NextDouble() * 0.1f - 0.05f;

            NDamageInfo damage = new NDamageInfo();
            damage.entityId = target.entityId;
            damage.Damage = Math.Max(1, (int)final);
            damage.Crit = isCrit;
            return damage;
        }

        private bool IsCrit(float crit)
        {
            return MathUtil.Random.NextDouble() < crit;
        }

        private void HitRange(NSkillHitInfo hitInfo)
        {
            Vector3Int pos;
            if (this.Define.CastTarget == Common.Battle.TargetType.Target)
            {
                pos = Context.Target.Position;
            }else if (this.Define.CastTarget == Common.Battle.TargetType.Position)
            {
                pos = Context.Position;
            }
            else
            {
                pos = this.Owner.Position;
            }

            List<Creature> units = this.Context.Battle.FindUnitsInMapRange(pos, this.Define.ADERange);
            foreach(var target in units)
            {
                this.HitTarget(target,hit);
            }
        }

        private void CastBullet(NSkillHitInfo hitInfo)
        {
            Context.Battle.AddHitInfo(hitInfo);
            Log.InfoFormat("Skill CastBullet");
            Bullet bullet = new Bullet(this,this.Context.Target,hitInfo);
            this.Bullets.Add(bullet);
        }

        //private void DoSkillDamage(BattleContext context)
        //{
        //    context.Damage = new NDamageInfo();
        //    context.Damage.entityId = context.Target.entityId;
        //    context.Damage.Damage = 100;
        //    context.Target.DoDamage(context.Damage.Damage);

        //}


        private void AddBuff(TriggerType trigger, Creature target)
        {
            if (this.Define.Buff == null || this.Define.Buff.Count == 0) return;

            foreach(var buffId in this.Define.Buff)
            {
                var buffDefine = DataManager.Instance.Buffs[buffId];
                if (buffDefine.Trigger != trigger) continue;//触发类型不一致

                if (buffDefine.Target == TargetType.Self)
                {
                    this.Owner.AddBuff(this.Context, buffDefine);
                }else if(buffDefine.Target==TargetType.Target)
                {
                    target.AddBuff(this.Context, buffDefine);
                }
            }
        }

        internal void Update()
        {
            UpdateCD();
            if (this.Status == SkillStatus.Casting)
            {
                this.UpdateCasting();
            }else if(this.Status == SkillStatus.Running)
            {
                this.UpdateSkill();
            }
        }

        private void UpdateSkill()
        {
            this.skillTime += Time.deltaTime;
            if (this.Define.Duration > 0)
            {
                if (this.skillTime > this.Define.Interval * (this.Hit + 1))
                {
                    this.DoHit();
                }

                if (this.skillTime >= this.Define.Duration)
                {
                    this.Status = SkillStatus.None;
                    Log.InfoFormat("SkillUpdateSkill");
                }
            }else if(this.Define.HitTimes!=null&&this.Define.HitTimes.Count>0)
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
                        Log.InfoFormat("SkillUpdateSkill");
                    }
                }
            }

            if (this.Define.Bullet)
            {
                bool finish = true;
                foreach(Bullet bullet in this.Bullets)
                {
                    bullet.Update();
                    if (!bullet.isStoped) finish = false;
                }
                if (finish && this.Hit >= this.Define.HitTimes.Count)
                {
                    this.Status = SkillStatus.None;
                    Log.InfoFormat("SkillUpdateSkill Finish");
                }
            }
        }

        private void UpdateCasting()
        {
            if (this.castingTime < this.Define.CastTime)
            {
                this.castingTime += Time.deltaTime;
            }
            else
            {
                this.castingTime = 0;
                this.Status = SkillStatus.Running;
                Log.InfoFormat("Skill UpdateCasting Finish");
            }
        }

        private void UpdateCD()
        {
            if (this.cd > 0)
            {
                this.cd -= Time.deltaTime;
            }
            if (cd < 0)
            {
                this.cd = 0;
            }
        }
    }
}
