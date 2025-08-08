using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[Serializable]
public struct SkillDataWithInput
{
    public SkillData Data;
    public InputActionReference InputActionRef;
    public bool bIsPerform;
    public bool bIsStart;
    public bool bIsRelease;
}

[DisallowMultipleComponent]
public class SkillSystem : MonoBehaviour
{
    
    private Actor ownerActor;
    [Header("스킬 정보")]
    [SerializeField]
    protected List<SkillDataWithInput> defaultSkills = new List<SkillDataWithInput>();
    
    private Dictionary<string, Skill> haveSkills = new Dictionary<string, Skill>();

    public void InitializeActionSystem(Actor actor)
    {
        ownerActor = actor;
        initializeSkillsBySkillData();
    }
    
    private void initializeSkillsBySkillData()
    {
        foreach (var skillData in defaultSkills)
        {
            if (skillData.Data != null)
            {
                if (!haveSkills.TryGetValue(skillData.Data.SkillName, out var skill))
                {
                    skill = SkillFactory.CreateSkillBySkillTag(skillData.Data.SkillTag);
                    haveSkills.Add(skillData.Data.SkillName, skill);
                }

                skill.InitializeSkill(ownerActor, skillData.Data,
                    GameUtils.GetInputActionHash(skillData.InputActionRef.action));

                if (skillData.bIsPerform)
                {
                    skillData.InputActionRef.action.performed += onInputPerformed;
                }

                if (skillData.bIsStart)
                {
                    skillData.InputActionRef.action.started += onInputStarted;
                }

                if (skillData.bIsRelease)
                {
                    skillData.InputActionRef.action.canceled += onInputCanceled;
                }
            }
        }
    }

    private void Awake()
    {
        if (ownerActor == null)
        {
            ownerActor = GetComponent<Actor>();
            Assert.IsNotNull(ownerActor, "Owner Actor Component is not set.");
        }
    }

    #region Input Bind region

    private void onInputPerformed(InputAction.CallbackContext cct)
    {
        Hash128 inputID = GameUtils.GetInputActionHash(cct.action);
        HardwareInputPerformed(inputID);
    }

    private void onInputStarted(InputAction.CallbackContext cct)
    {
        Hash128 inputID = GameUtils.GetInputActionHash(cct.action);
        HardwareInputStart(inputID);
    }

    private void onInputCanceled(InputAction.CallbackContext cct)
    {
        Hash128 inputID = GameUtils.GetInputActionHash(cct.action);
        HardwareInputCanceled(inputID);
    }
    public void HardwareInputPerformed(Hash128 inputID)
    {
        foreach (var skill in haveSkills.Values) 
        {
            if (skill.InputID == inputID)
            {
                Debug.Log("action performed : " + skill.ApplySkillData.SkillName);
                StartSkill(skill);
                break;
            }
        }
    }

    public void HardwareInputStart(Hash128 inputID)
    {
        foreach (var skill in haveSkills.Values) 
        {
            if (skill.InputID == inputID)
            {
                Debug.Log("action start : " + skill.ApplySkillData.SkillName);
                StartSkill(skill);
                break;
            }
        }
    }

    public void HardwareInputCanceled(Hash128 inputID)
    {
        foreach (var skill in haveSkills.Values) 
        {
            if (skill.InputID == inputID)
            {
                Debug.Log("action stop : " + skill.ApplySkillData.SkillName);
                CompleteSkill(skill);
                break;
            }
        }
    }

    #endregion

    #region Cost region

    private Dictionary<CostEffectData,CostEffect> cachedEffects = new Dictionary<CostEffectData,CostEffect>(4);
    
    public bool CanPayCost(CostEffectData costEffectData)
    {
        if (costEffectData == null)
        {
            return true;
        }
        
        CostEffect costEffect = GetOrCreateCostEffect(costEffectData);

        return costEffect.CanApply(ownerActor,ownerActor);
    }

    public CostEffect GetOrCreateCostEffect(CostEffectData costEffectData)
    {
        if (!cachedEffects.TryGetValue(costEffectData, out var costEffect))
        {
            costEffect = new CostEffect();
            cachedEffects.Add(costEffectData, costEffect);
        }
        costEffect.InitializeEffect(costEffectData);
        return costEffect;
    }

    public void ApplyCostEffectData(List<CostEffectData> costEffectDatas)
    {
        foreach (var costEffectData in costEffectDatas)
        {
            ApplyCostEffectData(costEffectData);
        }
    }
    public void ApplyCostEffectData(CostEffectData costEffectData)
    {
        CostEffect costEffect = GetOrCreateCostEffect(costEffectData);
        ApplyCostEffect(costEffect);
    }
    public void ApplyCostEffects(List<CostEffect> costEffects)
    {
        foreach (var costEffect in costEffects)
        {
            CostEffectData costEffectData = costEffect.CostEffectData;

            if (costEffectData == null)
            {
                return;
            }

            if (!costEffect.CanApply(ownerActor, ownerActor))
            {
                return;
            }
            costEffect.PreApply();
            costEffect.Apply(ownerActor, ownerActor);
        }
    }
    public void ApplyCostEffect(CostEffect costEffect)
    {
        CostEffectData costEffectData = costEffect.CostEffectData;

        if (costEffectData == null)
        {
            return;
        }

        if (!costEffect.CanApply(ownerActor, ownerActor))
        {
            return;
        }
        
        costEffect.Apply(ownerActor, ownerActor);
    }

    #endregion

    public void StartSkill(Skill skill)
    {
        if (haveSkills.ContainsKey(skill.ApplySkillData.SkillName))
        {
            if (skill.CanApplySkill())
            {
                skill.StartSkill();    
            }
        }
    }

    public void CompleteSkill(Skill skill)
    {
        if (haveSkills.ContainsKey(skill.ApplySkillData.SkillName))
        {
            if (skill.bIsRunning)
            {
                skill.CompleteSkill();    
            }
        }
    }
}
