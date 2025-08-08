using UnityEngine;

public class Skill_Grab : Skill
{
    public override void InitializeSkill(Actor inOwnerActor, SkillData inSkillData, Hash128 inputID = default)
    {
        base.InitializeSkill(inOwnerActor, inSkillData, inputID);
    }

    protected override void onSkillStarted()
    {
        Debug.Log($"{nameof(Skill_Grab)} Started : {ApplySkillData.SkillName}");
        // 스킬 시작 시 즉시 실행
        ApplySkill(OwnerActor, null);
    }

    public override bool ApplySkill(Actor source, Actor target)
    {
        Debug.Log($"{nameof(Skill_Grab)} Applying : {ApplySkillData.SkillName}");
        
        // Effect들을 순차적으로 적용
        OwnerSkillSystem.ApplyEffectData(ApplySkillData.Effects, source, target);
        
        // 스킬 실행 완료
        CompleteSkill();
        
        return true;
    }

    public override bool CanApplySkill()
    {
        if (!base.CanApplySkill())
        {
            Debug.LogWarning($"{nameof(Skill_Grab)} cannot be applied - Check cooldown, cost, or running state");
            return false;
        }
        
        return true;
    }

    protected override void onSkillCompleted()
    {
        Debug.Log($"{nameof(Skill_Grab)} Completed : {ApplySkillData.SkillName}");
    }
    
    protected override void onCooldownComplete()
    {
        Debug.Log($"{nameof(Skill_Grab)} Cooldown Complete : {ApplySkillData.SkillName}");
    }
}
