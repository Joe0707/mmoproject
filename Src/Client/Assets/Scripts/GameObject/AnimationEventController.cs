using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AnimationEventController:MonoBehaviour
{
    public EntityEffecetManager EffectMgr;

    void PlayEffect(string name)
    {
        Debug.LogFormat("AnimationEventController:PlayEffect");
        EffectMgr.PlayEffect(name);
    }

    void PlaySound(string name)
    {
        Debug.LogFormat("AnimationEventController:PlaySound");
    }
}
