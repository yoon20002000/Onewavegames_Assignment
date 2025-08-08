using UnityEngine;
using UnityEngine.Assertions;

public class CostEffect : Effect
{
    public override void PreApply()
    {
        base.PreApply();
        Assert.IsTrue(effectData.effectType == EEffectType.Cost);
    }

    public override void Apply(Actor source, Actor target)
    {
        
    }

    public override bool CanApply(Actor source, Actor target)
    {
        if (!base.CanApply(source, target))
        {
            return false;
        }

        return true;
    }
}
