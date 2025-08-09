using UnityEngine;

public class Effect_Heal : Effect
{
    public Effect_Heal(SkillSystem skillSystem, EffectData inEffectData) : base(skillSystem, inEffectData)
    {
    }

    public override void Apply(Actor source, Actor target)
    {
        // 타겟이 없으면 자신에게
        Actor healTarget = target ==null ? source : target; 
        healTarget.Heal(source, effectData.Value);
        EndEffect();
    }

    public override bool CanApply(Actor source, Actor target)
    {
        return source != null || target != null;
    }
}
