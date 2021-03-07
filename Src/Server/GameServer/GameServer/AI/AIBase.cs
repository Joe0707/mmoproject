using Common.Battle;
using GameServer.Battle;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.AI
{
    class AIBase
    {
        protected Monster owner;
        private Creature target;
        Skill normalSkill;
        public AIBase(Monster monster)
        {
            this.owner = monster;
            normalSkill = this.owner.SkillMgr.NormalSkill;
        }
        internal void Update()
        {
            if(owner.State == Common.Battle.CharState.InBattle)
            {
                this.UpdateBattle();
            }   
        }


        private void UpdateBattle()
        {
            if (this.target == null)
            {
                this.owner.State = Common.Battle.BattleState.Idle;
                return;
            }
            if (!TryCastSkill()) {
                if (!TryCastNormal())
                {
                    FollowTarget();
                }
            }
            //{
            //    var context = new BattleContext(owner.Map.Battle)
            //    {
            //        Target = this.target,
            //        Caster = owner,
            //    };
            //    Skill skill = owner.FindSkill(context);
            //    if (skill != null)
            //    {
            //        owner.CastSkill(context, skill.Define.ID);
            //    }
            //}
        }

        private void FollowTarget()
        {
            int distance = this.owner.Distance(this.target);
            if (distance > normalSkill.Define.CastRange - 50)
            {
                this.owner.MoveTo(this.target.Position);
            }
            else
                this.owner.StopMove();
        }

        private bool TryCastNormal()
        {
            if (this.target != null)
            {
                var context = new BattleContext(owner.Map.Battle)
                {
                    Target = this.target,
                    Caster = owner,
                };
                var result = normalSkill.CanCast(context);
                if (result == SkillResult.OK)
                {
                    owner.CastSkill(context, normalSkill.Define.ID);
                    return true;
                }
                if(result == SkillResult.OutOfRange)
                {
                    return false;
                }
            }
            return true;

        }

        private bool TryCastSkill()
        {
            if (this.target != null)
            {
                var context = new BattleContext(owner.Map.Battle)
                {
                    Target = this.target,
                    Caster = owner,
                };
                Skill skill = owner.FindSkill(context,SkillType.Skill);
                if (skill != null)
                {
                    owner.CastSkill(context, skill.Define.ID);
                    return true;
                }
            }
            return false;
        }

        internal void OnDamage(NDamageInfo damage, Creature source)
        {
            this.target = source;
        }
    }
}
