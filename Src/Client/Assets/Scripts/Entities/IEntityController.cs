﻿using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    public interface IEntityController
    {
        void PlayAnim(string name);
        void SetStandBy(bool standby);
        void UpdateDirection();
        void PlayEffect(EffectType type, string name, Creature target, float duration);
        void PlayEffect(EffectType type, string name, NVector3 position, float duration);
        Transform GetTransform();
    }
}
