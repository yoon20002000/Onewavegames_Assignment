using UnityEngine;

public class Skill_SelfTarget : Skill
{
    public override void StartSkill()
    {
        // 부모 클래스의 StartSkill 호출 (비용 적용, 쿨타임 설정, bIsRunning 설정)
        base.StartSkill();

        // 자기 자신을 대상으로 하는 스킬 시작 로그
        Debug.Log($"[{nameof(StartSkill)}] {ApplySkillData.SkillName} 스킬 시작됨");
        ApplySkill(OwnerActor, OwnerActor);
    }

    public override bool ApplySkill(Actor source, Actor target)
    {
        // 자기 자신을 대상으로 하는 스킬이므로 source와 target이 같아야 함
        if (source == null)
        {
            Debug.LogError($"[{nameof(ApplySkill)}] 소스 액터가 null입니다.");
            return false;
        }

        // 자기 자신을 대상으로 설정
        target = source;

        Debug.Log($"[{nameof(ApplySkill)}] {ApplySkillData.SkillName} 스킬을 {source.name}에게 적용");


        bool bAllEffectsApplied = true;
        foreach (var effectData in ApplySkillData.Effects)
        {
            OwnerSkillSystem.ApplyEffectFromEffectData(effectData, source, source);
        }

        Debug.Log($"[{nameof(ApplySkill)}] {ApplySkillData.SkillName} 스킬이 성공적으로 적용됨");
        // 스킬 완료 처리
        CompleteSkill();
        
        return bAllEffectsApplied;
    }

    protected override void onSkillStarted()
    {
        // 스킬 시작 시 추가 로직 (필요시 구현)
        Debug.Log($"[{nameof(onSkillStarted)}] {ApplySkillData.SkillName} 스킬 시작 콜백");
    }

    protected override void onSkillCompleted()
    {
        // 스킬 완료 시 추가 로직 (필요시 구현)
        Debug.Log($"[{nameof(onSkillCompleted)}] {ApplySkillData.SkillName} 스킬 완료 콜백");
    }
}