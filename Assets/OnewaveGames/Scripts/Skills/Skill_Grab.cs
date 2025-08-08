using UnityEngine;

public class Skill_Grab : Skill
{
    public override void InitializeSkill(Actor inOwnerActor, SkillData inSkillData, Hash128 inputID = default)
    {
        base.InitializeSkill(inOwnerActor, inSkillData, inputID);
    }

    public override void StartSkill()
    {
        base.StartSkill();
        Debug.Log("Skill Grab Start");
        ApplySkill(OwnerActor,null);
    }

    public override bool ApplySkill(Actor source, Actor target)
    {
        Debug.Log("Skill Grab apply");
        // ge 들 처리
        
        return true;
    }

    public override bool CanApplySkill()
    {
        if (!base.CanApplySkill())
        {
            return false;
        }
        
        return true;
    }

    public override void CompleteSkill()
    {
        Debug.Log("Skill Grab end");
        base.CompleteSkill();
    }
}
