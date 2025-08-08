using UnityEngine;

public class Skill_SelfTarget : Skill
{
    public override bool ApplySkill(Actor source, Actor target)
    {
        return true;
    }
}
