using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    [Header("이펙트 정보")]
    protected EffectData effectData;

    private SkillSystem ownerSkillSystem;
    public SkillSystem OwnerSkillSystem => ownerSkillSystem;
    public Effect(SkillSystem skillSystem ,EffectData inEffectData)
    {
        ownerSkillSystem = skillSystem;
        effectData = inEffectData;
    }
    public virtual void PreApply()
    {
        
    }
    public abstract void Apply(Actor source, Actor target);

    public virtual bool CanApply(Actor source, Actor target)
    {
        return true;
    }
}
