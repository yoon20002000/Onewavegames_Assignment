using UnityEngine;

public class Effect_PullObject : Effect
{
    public Effect_PullObject(SkillSystem skillSystem, EffectData inEffectData) : base(skillSystem, inEffectData)
    {
    }

    public override void Apply(Actor source, Actor target)
    {
        target.transform.SetPositionAndRotation(source.transform.position, Quaternion.identity);
        
        EndEffect();
    }
}
