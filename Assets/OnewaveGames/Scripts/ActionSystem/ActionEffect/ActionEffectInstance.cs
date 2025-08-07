using UnityEngine;

public class ActionEffectInstance
{
    protected Actor effectInstigator;
    protected ActionEffectData actionEffectData;
    protected ActionSystem actionSystem;

    public float Duration { get; private set; }
    public float Period { get; private set; }

    public ActionEffectInstance(ActionEffectData inActionEffectData, ActionSystem system)
    {
        actionEffectData = inActionEffectData;
        actionSystem = system;

        Duration = inActionEffectData.Duration;
        Period = inActionEffectData.Period;
    }
    public virtual bool CanApplyEffect()
    {
        return true;
    }

    public virtual void ExecuteEffect()
    {
        
    }

    public virtual void OnEffectApplyComplete()
    {
        
    }
}
