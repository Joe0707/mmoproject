using Common;
using Common.Battle;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    class EffectManager
    {
        private Creature Owner;
        Dictionary<BuffEffect, int> Effects = new Dictionary<BuffEffect, int>();

        public EffectManager(Creature owner)
        {
            this.Owner = owner;
        }

        internal bool HasEffect(BuffEffect effect)
        {
            if(this.Effects.TryGetValue(effect,out int val))
            {
                return val > 0;
            }
            return false;
        }

        internal void AddEffect(BuffEffect effect)
        {
            Log.InfoFormat("AddEffect");
            if (!this.Effects.ContainsKey(effect))
                this.Effects[effect] = 1;
            else
                this.Effects[effect]++;
        }
        internal void RemoveBuffEffect(BuffEffect effect)
        {
            Log.InfoFormat("RemoveEffect");
            if (this.Effects[effect] > 0)
            {
                this.Effects[effect]--;
            }
        }


    }
}
