using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Skill 
{
    public SkillData ApplySkillData { get; protected set; }

    public Actor OwnerActor { get; private set; }
    public SkillSystem OwnerSkillSystem { get; private set; }
    public bool bIsRunning { get; private set; } = false;

    public Hash128 InputID { get; private set; }
    
    public virtual void InitializeSkill(Actor inOwnerActor, SkillData inSkillData, Hash128 inputID = default)
    {
        ApplySkillData = inSkillData;
        OwnerActor = inOwnerActor;
        OwnerSkillSystem = OwnerActor.ActorSkillSystem;
        bIsRunning = false;
        InputID = inputID;
        curCooldown = 0f; // 초기화 시 쿨타임을 0으로 설정
    }

    #region Skill default region

    public virtual bool CanApplySkill()
    {
        if (bIsRunning || !CanPayCost() || IsCooldown())
        {
            return false;
        }

        return true;
    }

    public virtual void StartSkill()
    {
        if (!CanApplySkill())
        {
            Debug.LogWarning($"Skill {ApplySkillData?.SkillName} cannot be started");
            return;
        }
        
        OwnerSkillSystem.ApplyCostEffectData(ApplySkillData.CostEffectData);
        curCooldown = ApplySkillData.Cooldown;
        bIsRunning = true;
        
        // 스킬 실행 시작
        onSkillStarted();
    }
    
    public abstract bool ApplySkill(Actor source, Actor target);
    
    public virtual void CompleteSkill()
    {
        
        onSkillCompleted();
        bIsRunning = false;
    }
    
    // 스킬 시작 시 호출되는 콜백
    protected virtual void onSkillStarted()
    {
        // 하위 클래스에서 오버라이드하여 스킬 시작 시 추가 로직 구현 가능
    }
    
    // 스킬 완료 시 호출되는 콜백
    protected virtual void onSkillCompleted()
    {
        // 하위 클래스에서 오버라이드하여 스킬 완료 시 추가 로직 구현 가능
    }

    #endregion

    #region Cooltime region

    private float curCooldown;
    
    public float CurrentCooldown => curCooldown;
    public float MaxCooldown => ApplySkillData?.Cooldown ?? 0f;
    public float CooldownProgress => MaxCooldown > 0 ? (MaxCooldown - curCooldown) / MaxCooldown : 1f;
    public bool bIsCooldownComplete => curCooldown <= 0f;
    
    public UnityEvent OnCooldownComplete = new UnityEvent();
    // 쿨타임 강제 리셋
    public virtual void ResetCooldown()
    {
        SetCooldown(0);
    }
    
    // 쿨타임 강제 설정 (디버깅이나 특수 상황용)
    public virtual void SetCooldown(float cooldown)
    {
        curCooldown = Mathf.Max(0f, cooldown);
        if (curCooldown <= 0)
        {
            OnCooldownComplete.Invoke();
        }
    }
    
    // 쿨타임 감소 (아이템이나 버프 효과용)
    public virtual void ReduceCooldown(float reductionAmount)
    {
        curCooldown = Mathf.Max(0f, curCooldown - reductionAmount);
        if (curCooldown <= 0f)
        {
            onCooldownComplete();
        }
    }
    
    // 쿨타임 증가 (디버프 효과용)
    public virtual void IncreaseCooldown(float increaseAmount)
    {
        curCooldown += increaseAmount;
    }
    // 쿨타임 업데이트 메서드 - 매 프레임 호출되어야 함
    public virtual void UpdateCooldown(float deltaTime)
    {
        if (curCooldown > 0f)
        {
            curCooldown -= deltaTime;
            if (curCooldown <= 0f)
            {
                curCooldown = 0f; // 음수가 되지 않도록 보장
                onCooldownComplete();
            }
        }
    }
    
    // 쿨타임 완료 시 호출되는 콜백
    protected virtual void onCooldownComplete()
    {
        // 하위 클래스에서 오버라이드하여 쿨타임 완료 시 추가 로직 구현 가능
        OnCooldownComplete.Invoke();
    }
    
    public bool IsCooldown()
    {
        return curCooldown > 0;
    }

    #endregion
    public bool CanPayCost()
    {
        foreach (var costEffectData in ApplySkillData.CostEffectData)
        {
            if (!OwnerSkillSystem.CanPayCost(costEffectData))
            {
                return false;
            }
        }

        return true;
    }
}
