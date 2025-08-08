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
    public Actor OwnerActor => ownerActor;
    [Header("스킬 정보")]
    [SerializeField]
    protected List<SkillDataWithInput> defaultSkills = new List<SkillDataWithInput>();
    
    private readonly Dictionary<string, Skill> haveSkills = new Dictionary<string, Skill>();

    public void InitializeActionSystem(Actor actor)
    {
        if (actor == null)
        {
            Debug.LogError($"{nameof(SkillSystem)} : Cannot initialize with null actor");
            return;
        }
        
        ownerActor = actor;
        initializeSkillsBySkillData();
    }
    
    private void initializeSkillsBySkillData()
    {
        if (defaultSkills == null || defaultSkills.Count == 0)
        {
            Debug.LogWarning($"{this.gameObject.name} {nameof(SkillSystem)} : No skills configured");
            return;
        }
        
        foreach (var skillData in defaultSkills)
        {
            if (skillData.Data != null)
            {
                if (!haveSkills.TryGetValue(skillData.Data.SkillName, out var skill))
                {
                    skill = SkillFactory.CreateSkillBySkillTag(skillData.Data.SkillTag);
                    if (skill == null)
                    {
                        Debug.LogError($"{nameof(SkillSystem)} : Failed to create skill for tag {skillData.Data.SkillTag}");
                        continue;
                    }
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
            else
            {
                Debug.LogWarning($"{nameof(SkillSystem)} : Skill data is null in defaultSkills");
            }
        }
        
        Debug.Log($"{nameof(SkillSystem)} : Initialized {haveSkills.Count} skills");
    }

    #region Unity Events region

    private void Awake()
    {
        if (ownerActor == null)
        {
            ownerActor = GetComponent<Actor>();
            Assert.IsNotNull(ownerActor, "Owner Actor Component is not set.");
        }
    }

    private void Update()
    {
        if (haveSkills == null)
        {
            return;
        }
        
        foreach (var skill in haveSkills.Values)
        {
            if (skill != null)
            {
                skill.UpdateCooldown(Time.deltaTime);
            }
        }
        
        if (cachedEffects != null)
        {
            foreach (var cachedEffect in cachedEffects.Values)
            {
                if (cachedEffect != null)
                {
                    cachedEffect.Update(Time.fixedDeltaTime);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (cachedEffects != null)
        {
            foreach (var cachedEffect in cachedEffects.Values)
            {
                if (cachedEffect != null)
                {
                    cachedEffect.FixedUpdate(Time.fixedDeltaTime);
                }
            }
        }
    }

    #endregion

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
            if (skill != null && skill.InputID == inputID)
            {
                Debug.Log($"action performed : {skill.ApplySkillData.SkillName}");
                StartSkill(skill);
                break;
            }
        }
    }

    public void HardwareInputStart(Hash128 inputID)
    {
        foreach (var skill in haveSkills.Values) 
        {
            if (skill != null && skill.InputID == inputID)
            {
                Debug.Log($"action start : {skill.ApplySkillData.SkillName}");
                StartSkill(skill);
                break;
            }
        }
    }

    public void HardwareInputCanceled(Hash128 inputID)
    {
        foreach (var skill in haveSkills.Values) 
        {
            if (skill != null && skill.InputID == inputID)
            {
                Debug.Log($"action stop : {skill.ApplySkillData.SkillName}");
                completeSkill(skill);
                break;
            }
        }
    }

    #endregion

    #region Cost region

    private Dictionary<CostEffectData,CostEffect> cachedCostEffects = new Dictionary<CostEffectData,CostEffect>(4);
    
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
        if (costEffectData == null)
        {
            Debug.LogError($"{nameof(SkillSystem)} : CostEffectData is null");
            return null;
        }
        
        if (!cachedCostEffects.TryGetValue(costEffectData, out var costEffect))
        {
            costEffect = new CostEffect();
            cachedCostEffects.Add(costEffectData, costEffect);
        }
        costEffect.InitializeEffect(costEffectData);
        return costEffect;
    }

    public void ApplyCostEffectData(List<CostEffectData> costEffectDatas)
    {
        if (costEffectDatas == null)
        {
            return;
        }
        
        foreach (var costEffectData in costEffectDatas)
        {
            ApplyCostEffectData(costEffectData);
        }
    }
    
    public void ApplyCostEffectData(CostEffectData costEffectData)
    {
        if (costEffectData == null)
        {
            return;
        }
        
        CostEffect costEffect = GetOrCreateCostEffect(costEffectData);
        if (costEffect != null)
        {
            ApplyCostEffect(costEffect);
        }
    }
    
    public void ApplyCostEffects(List<CostEffect> costEffects)
    {
        if (costEffects == null)
        {
            return;
        }
        
        foreach (var costEffect in costEffects)
        {
            if (costEffect == null)
            {
                continue;
            }
            
            CostEffectData costEffectData = costEffect.CostEffectData;

            if (costEffectData == null)
            {
                Debug.LogWarning($"{nameof(SkillSystem)} : CostEffect has null CostEffectData");
                return;
            }

            if (!costEffect.CanApply(ownerActor, ownerActor))
            {
                Debug.LogWarning($"{nameof(SkillSystem)} : Cannot apply cost effect {costEffectData.ECostEffectType}");
                return;
            }
            applyCostEffect(costEffect);
        }
    }
    
    public void ApplyCostEffect(CostEffect costEffect)
    {
        if (costEffect == null)
        {
            Debug.LogError($"{nameof(SkillSystem)} : CostEffect is null");
            return;
        }
        
        CostEffectData costEffectData = costEffect.CostEffectData;

        if (costEffectData == null)
        {
            Debug.LogError($"{nameof(SkillSystem)} : CostEffectData is null");
            return;
        }

        if (!costEffect.CanApply(ownerActor, ownerActor))
        {
            Debug.LogWarning($"{nameof(SkillSystem)} : Cannot apply cost effect {costEffectData.ECostEffectType}");
            return;
        }

        applyCostEffect(costEffect);
    }

    private void applyCostEffect(CostEffect costEffect)
    {
        costEffect.PreApply();
        costEffect.Apply(ownerActor, ownerActor);
    }

    #endregion

    public void StartSkill(Skill skill)
    {
        if (skill == null)
        {
            Debug.LogError($"{nameof(SkillSystem)} : Cannot start null skill");
            return;
        }
        
        if (!haveSkills.ContainsKey(skill.ApplySkillData.SkillName))
        {
            Debug.LogError($"{nameof(SkillSystem)} : Skill {skill.ApplySkillData.SkillName} not found in haveSkills");
            return;
        }
        
        if (skill.CanApplySkill())
        {
            skill.StartSkill();    
        }
        else
        {
            Debug.LogWarning($"{nameof(SkillSystem)} : Cannot start skill {skill.ApplySkillData.SkillName}");
        }
    }

    private void completeSkill(Skill skill)
    {
        if (skill == null)
        {
            Debug.LogError($"{nameof(SkillSystem)} : Cannot complete null skill");
            return;
        }
        
        if (!haveSkills.ContainsKey(skill.ApplySkillData.SkillName))
        {
            Debug.LogError($"{nameof(SkillSystem)} : Skill {skill.ApplySkillData.SkillName} not found in haveSkills");
            return;
        }
        
        if (skill.bIsRunning)
        {
            skill.CompleteSkill();    
        }
    }

    #region Effect region
    private readonly Dictionary<EffectData, Effect> cachedEffects = new Dictionary<EffectData, Effect>();
    
    public Effect GetOrCreateEffect(EffectData effectData)
    {
        if (effectData == null)
        {
            Debug.LogError($"{nameof(SkillSystem)} : EffectData is null");
            return null;
        }
        
        if (!cachedEffects.TryGetValue(effectData, out var effect))
        {
            effect = EffectFactory.CreateEffect(this, effectData);
            if (effect == null)
            {
                Debug.LogError($"{nameof(SkillSystem)} : Failed to create effect for type {effectData.EffectType}");
                return null;
            }
            cachedEffects.Add(effectData, effect);
        }
        
        return effect;
    }
    
    public void ApplyEffectData(List<EffectData> effects, Actor source, Actor target)
    {
        if (effects == null)
        {
            return;
        }
        
        foreach (var effectData in effects)
        {
            ApplyEffectData(effectData, source, target);
        }
    }

    public void ApplyEffectData(EffectData effectData, Actor source, Actor target)
    {
        if (effectData == null)
        {
            Debug.LogError($"{nameof(SkillSystem)} : EffectData is null");
            return;
        }
        
        Effect effect = GetOrCreateEffect(effectData);
        if (effect == null)
        {
            Debug.LogError($"{nameof(SkillSystem)} : Failed to get or create effect for {effectData.EffectType}");
            return;
        }
        
        if (effect.CanApply(source, target))
        {
            effect.PreApply();
            effect.Apply(source, target);
        }
        else
        {
            Debug.LogWarning($"{nameof(SkillSystem)} : Cannot apply effect {effectData.EffectType}");
        }
    }
    #endregion
}
