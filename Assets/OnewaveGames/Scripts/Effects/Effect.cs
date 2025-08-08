using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    [Header("이펙트 정보")]
    protected EffectData effectData;

    public virtual void InitializeEffect(EffectData inEffectData)
    {
        effectData = inEffectData;
    }
    
    public virtual void PreApply()
    {
        
    }
    public abstract void Apply(Actor source, Actor target);

    public virtual bool CanApply(Actor source, Actor target)
    {
        return source != target && target != null;
    }
}
