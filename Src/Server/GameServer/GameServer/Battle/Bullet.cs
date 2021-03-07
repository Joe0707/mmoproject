﻿using Common;
using GameServer.Battle;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    class Bullet
    {
        private Skill skill;
        private Creature target;
        bool TimeMode = true;
        float duration = 0;
        float flyTime = 0;
        public bool isStoped = false;
        NSkillHitInfo hitInfo;
        public Bullet(Skill skill, Creature target, NSkillHitInfo hitInfo)
        {
            this.skill = skill;
            this.target = target;
            int distance = skill.Owner.Distance(target);
            if (TimeMode)
            {
                duration = distance / this.skill.Define.BulletSpeed;
            }
            Log.InfoFormat("Bullet CastBullet Target Distance Time");
        }

        public void Update()
        {
            if (isStoped) return;
            if (TimeMode)
            {
                this.UpdateTime();
            }
            else
            {
                this.UpdatePos();
            }
        }
        private void UpdateTime()
        {
            flyTime += Time.deltaTime;
            if(this.flyTime>duration)
            {
                this.hitInfo.isBullet = true;
                this.skill.DoHit(this.hitInfo);
                this.isStoped = true;
            }
        }

        private void UpdatePos()
        {
            /*
             * int distance = skill.Owner.Distance(target);
             * if(distance>50)
             * {
             *     pos +=speed*Time.deltaTime;
             * }else
             * {
             *      this.hitInfo.isBullet = true;
             *      this.skill.DoHit(this.hitInfo);
             *      this.stoped = true;
             * }
             */
        }

    }
}
