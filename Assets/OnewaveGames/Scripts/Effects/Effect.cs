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
    
    public bool bIsRunning { get; protected set; }
    public Effect(SkillSystem skillSystem ,EffectData inEffectData)
    {
        ownerSkillSystem = skillSystem;
        effectData = inEffectData;
        bIsRunning = false;
    }

    public virtual void PreApply()
    {
        
    }
    public abstract void Apply(Actor source, Actor target);

    public virtual void Update(float deltaTime)
    {
        
    }
    public virtual void FixedUpdate(float deltaTime)
    {
        
    }
    public virtual bool CanApply(Actor source, Actor target)
    {
        return !bIsRunning;
    }

    public virtual void EndEffect()
    {
        bIsRunning = false;
        ownerSkillSystem.RemoveEffect(this);
    }
}
