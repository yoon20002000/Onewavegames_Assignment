using UnityEngine;

public class Skill_SelfTarget : Skill
{
    public override void StartSkill()
    {
        
    }

    public override bool ApplySkill(Actor source, Actor target)
    {
        return true;
    }
}
