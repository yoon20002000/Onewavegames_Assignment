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
                    GameUtils.GetInputActionHash(skillData.InputActionRef));

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
        foreach (var action in haveSkills.Values) 
        {
            if (action.InputID == inputID)
            {
                Debug.Log("action performed : " + action.ApplySkillData.SkillName);
                //StartAction(action.ActionTag);
                break;
            }
        }
    }

    public void HardwareInputStart(Hash128 inputID)
    {
        foreach (var action in haveSkills.Values) 
        {
            if (action.InputID == inputID)
            {
                Debug.Log("action start : " + action.ApplySkillData.SkillName);
                //StartAction(action.ActionTag);
                break;
            }
        }
    }

    public void HardwareInputCanceled(Hash128 inputID)
    {
        foreach (var action in haveSkills.Values) 
        {
            if (action.InputID == inputID)
            {
                Debug.Log("action stop : " + action.ApplySkillData.SkillName);
                //StopAction(action.ActionTag);
                break;
            }
        }
    }
}
