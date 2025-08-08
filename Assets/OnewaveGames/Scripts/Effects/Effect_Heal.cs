using UnityEngine;

public class Effect_Heal : Effect
{
    public Effect_Heal(SkillSystem skillSystem, EffectData inEffectData) : base(skillSystem, inEffectData)
    {
    }

    public override void Apply(Actor source, Actor target)
    {
        throw new System.NotImplementedException();
    }
}
