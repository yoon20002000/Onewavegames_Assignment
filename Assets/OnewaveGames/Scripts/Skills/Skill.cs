using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill 
{
    public SkillData ApplySkillData { get; protected set; }

    public Actor OwnerActor { get; private set; }
    public SkillSystem OwnerSkillSystem { get; private set; }
    public bool bIsRunning { get; private set; } = false;

    public Hash128 InputID { get; private set; }

    private float curCooldown;

    public virtual void InitializeSkill(Actor inOwnerActor, SkillData inSkillData, Hash128 inputID = default)
    {
        ApplySkillData = inSkillData;
        OwnerActor = inOwnerActor;
        OwnerSkillSystem = OwnerActor.ActorSkillSystem;
        bIsRunning = false;
        InputID = inputID;
    }

    public virtual bool CanApplySkill()
    {
        if (bIsRunning || !CanPayCost() || IsCooldown())
        {
            return false;
        }

        return true;
    }

    public virtual void StartSkill()
    {
        OwnerSkillSystem.ApplyCostEffectData(ApplySkillData.CostEffectData);
        curCooldown = ApplySkillData.Cooldown;
    }
    public abstract bool ApplySkill(Actor source, Actor target);
    
    public virtual void CompleteSkill()
    {
        bIsRunning = false;
    }
    
    public bool CanPayCost()
    {
        foreach (var costEffectData in ApplySkillData.CostEffectData)
        {
            if (!OwnerSkillSystem.CanPayCost(costEffectData))
            {
                return false;
            }
        }

        return true;
    }

    #region Cooldown region
    public void UpdateCooldown(float deltaTime)
    {
        curCooldown -= deltaTime;
    }
    public bool IsCooldown()
    {
        return curCooldown > 0;
    }

    #endregion
}
