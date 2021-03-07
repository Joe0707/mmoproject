using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Battle
{
    public class Buff
    {
        internal bool Stoped;
        private Creature owner;
        private int buffId;
        private BuffDefine define;
        private int casterId;
        public float time;

        public Buff(Creature owner, int buffId, BuffDefine define, int casterId)
        {
            this.owner = owner;
            this.buffId = buffId;
            this.define = define;
            this.casterId = casterId;
            this.OnAdd();
        }

        private void OnAdd()
        {
            Debug.LogFormat("BuffOnAdd BuffId BuffName");

            if (this.define.Effect!=BuffEffect.None)
            {
                this.owner.AddBuffEffect(this.define.Effect);
            }
            AddAttr();
        }

        private void AddAttr()
        {
            if(this.define.DERRatio!=0)
            {
                this.owner.Attributes.Buff.DET += this.owner.Attributes.Basic.DET * this.define.DEFRatio;
            }
            this.owner.Attributes.InitFanalAttributes();
        }

        internal void OnRemove()
        {
            Debug.LogFormat("BuffOnRemove BuffId BuffName");
            RemoveAttr();
            Stoped = true;
            if(this.define.Effect!=BuffEffect.None)
            {
                this.owner.RemoveBuffEffect(this.define.Effect);
            }
        }

        private void RemoveAttr()
        {
            if (this.define.DERRatio != 0)
            {
                this.owner.Attributes.Buff.DET -= this.owner.Attributes.Basic.DET * this.define.DEFRatio;
            }
            this.owner.Attributes.InitFanalAttributes();
        }

        internal void OnUpdate(float delta)
        {
            if (Stoped) return;
            this.time += delta;
            if(time>this.define.Duration)
            {
                this.OnRemove();
            }
        }
    }
}
