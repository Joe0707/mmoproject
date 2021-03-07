﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Battle
{
    public class EffectManager
    {
        private Creature Owner;
        Dictionary<BuffEffect, int> Effects = new Dictionary<BuffEffect, int>();

        public EffectManager(Creature owner)
        {
            this.Owner = owner;
        }

        internal bool HasEffect(BuffEffect effect)
        {
            int val;
            if (this.Effects.TryGetValue(effect, out val))
            {
                return val > 0;
            }
            return false;
        }

        internal void AddEffect(BuffEffect effect)
        {
            Debug.LogFormat("AddEffect");
            if (!this.Effects.ContainsKey(effect))
                this.Effects[effect] = 1;
            else
                this.Effects[effect]++;
        }
        internal void RemoveBuffEffect(BuffEffect effect)
        {
            Debug.LogFormat("RemoveEffect");
            if (this.Effects[effect] > 0)
            {
                this.Effects[effect]--;
            }
        }
    }
}
