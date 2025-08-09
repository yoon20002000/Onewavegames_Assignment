using UnityEngine;

public class Effect_Damage : Effect
{
    public Effect_Damage(SkillSystem skillSystem, EffectData inEffectData) : base(skillSystem, inEffectData)
    {
    }

    public override void Apply(Actor source, Actor target)
    {
        if (target != null)
        {
            target.TakeDamage(effectData.Value);
        }
        EndEffect();
    }
}
