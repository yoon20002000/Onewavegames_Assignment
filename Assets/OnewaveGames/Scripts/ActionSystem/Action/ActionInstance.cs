using System.Collections;
using UnityEngine;

public class ActionInstance
{
    public ActionData Data { get; private set; }
    public Actor ActionOwner { get; private set; }

    protected ActionSystem actionSystem;

    public int InputID { get; private set; }
    
    public float ActionRange { get; private set; }
    public float ActionCooldown {get; private set;}

    public bool bIsActionRunning { get; private set; } = false;
    protected float curCooldown;
    public ActionInstance(ActionData data, ActionSystem inActionSystem, int inputID = -1)
    {
        actionSystem = inActionSystem;
        ActionRange = data.ActionRange;
        ActionCooldown = data.Cooldown;
        InputID = -1;
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

        bIsActionRunning = true;
        curCooldown = ActionCooldown;
        
        // 보유중인 GE 처리
        // applyactioneffect()
    }

    public virtual void StopAction()
    {
        
    }

    public bool IsCooldown()
    {
        return curCooldown > 0;
    }

    public bool CanPayCost()
    {
        return true;
    }
}
