using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill 
{
    public SkillData ApplySkillData { get; protected set; }

    public Actor OwnerActor { get; private set; }
    public SkillSystem OwnerSkillSystem { get; private set; }
    public bool bIsRunning { get; private set; } = false;
    public virtual void InitializeSkill(Actor inOwnerActor, SkillData inSkillData)
    {
        ApplySkillData = inSkillData;
        OwnerActor = inOwnerActor;
        OwnerSkillSystem = OwnerActor.ActorSkillSystem;
        bIsRunning = false;
    }

    public virtual bool CanApplySkill()
    {
        // 이건 action에서 처리할 것
        if (bIsRunning && CanPayCost())
        {
            return false;
        }

        return true;
    }
    public abstract bool ApplySkill(Actor source, Actor target);

    public virtual void CompleteSkill()
    {
        bIsRunning = false;
    }
    public bool CanPayCost()
    {
        foreach (var costEffect in ApplySkillData.CostEffects)
        {
            //costEffect.
        }

        return true;
    }
}
