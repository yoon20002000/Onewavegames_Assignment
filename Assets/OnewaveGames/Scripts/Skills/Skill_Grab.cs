using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

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

        // non target의 경우 마우스 방향으로
        if (target == null)
        {
            Ray mouseRay = GameUtils.CreateRayFromMousePosition(Mouse.current.position.ReadValue());
            if (GameUtils.TryGetRaycastHitPosition(mouseRay, out Vector3 hitPosition))
            {
                Vector3 dir = GameUtils.CalculateDirection(source.transform.position,hitPosition, true);
                Debug.DrawLine(source.AttackSocket.position, source.AttackSocket.position + dir * 5, Color.green, 5);
                source.transform.forward = dir;
            }
        }
        // target이 있을 경우 해당 타겟 방향으로
        else
        {
            Vector3 dir = (target.transform.position - source.transform.position).normalized;
            source.transform.forward = dir;
        }
        
        // Effect들을 순차적으로 적용
        OwnerSkillSystem.ApplyEffectsFromEffectData(ApplySkillData.Effects, source, target);

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