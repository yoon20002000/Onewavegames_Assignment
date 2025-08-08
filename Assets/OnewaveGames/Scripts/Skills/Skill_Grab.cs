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
        
        OwnerSkillSystem.ApplyEffectData(ApplySkillData.Effects, source, target);
        
        
        return true;
    }

    public override bool CanApplySkill()
    {
        if (!base.CanApplySkill())
        {
            Debug.Log("Skill " + nameof(Skill_Grab) + " not applied");
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
