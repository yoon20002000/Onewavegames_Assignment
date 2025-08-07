using System;
using System.Collections;
using UnityEngine;

public class ActionInstance
{
    public ActionData Data { get; private set; }
    public Actor ActionOwner { get; private set; }

    protected ActionSystem actionSystem;

    public EGameplayTag_Action ActionTag { get; private set; }
    public Hash128 InputID { get; private set; }
    
    public float ActionRange { get; private set; }
    public float ActionCooldown {get; private set;}

    public bool bIsActionRunning { get; private set; } = false;
    protected float curCooldown;
    public ActionInstance(ActionData data, ActionSystem inActionSystem, Hash128 inputID = default)
    {
        Data = data;
        actionSystem = inActionSystem;
        ActionRange = data.ActionRange;
        ActionCooldown = data.Cooldown;
        InputID = inputID;
        ActionTag = data.eActionTag;
    }

    public virtual bool CanStartAction()
    {
        if (IsCooldown() || !CanPayCost())
        {
            return false;
        }
        
        return true;
    }

    public virtual void StartAction()
    {
        if (!CanStartAction() || bIsActionRunning)
        {
            return;
        }
        
        ApplyCost();

        bIsActionRunning = true;
        curCooldown = ActionCooldown;
        
        foreach (var effect in Data.ApplyEffects)
        {
            //actionSystem.ExecuteGameplayEffect(effect,targets);
        }
    }

    public virtual void StopAction()
    {
        
    }

    public bool IsCooldown()
    {
        return curCooldown > 0;
    }

    // 공통 데이터용 Attribute set을 사용하지 않아 일단은 enum 하드코딩으로 대체
    public bool CanPayCost()
    {
        if (Data.EffectCost == null)
        {
            return true;
        }
        switch (Data.EffectCost.CostType)
        {
            case ActionEffectCostType.HP:
            {
                return ActionOwner.CurHP > Data.EffectCost.Value;
            }
            case ActionEffectCostType.MaxHP:
            {
                return ActionOwner.MaxHP > Data.EffectCost.Value;
            }
            case ActionEffectCostType.MP:
            {
                return ActionOwner.CurHP > Data.EffectCost.Value;
            }
            case ActionEffectCostType.MaxMP:
            {
                return ActionOwner.MaxMP > Data.EffectCost.Value;
            }
            case ActionEffectCostType.None:
            default:
            {
                return true;
            }
        }
    }
    public void ApplyCost()
    {
        actionSystem.ApplyCostEffect(ActionOwner, Data.EffectCost);
    }
}
