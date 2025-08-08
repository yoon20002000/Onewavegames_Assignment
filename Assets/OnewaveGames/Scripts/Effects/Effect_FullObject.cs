using UnityEngine;

public class Effect_FullObject : Effect
{
    public Effect_FullObject(SkillSystem skillSystem, EffectData inEffectData) : base(skillSystem, inEffectData)
    {
    }

    public override void Apply(Actor source, Actor target)
    {
        throw new System.NotImplementedException();
    }
}
