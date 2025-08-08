using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[DisallowMultipleComponent]
public class SkillSystem : MonoBehaviour
{
    
    private Actor ownerActor;
    [Header("스킬 정보")]
    [SerializeField]
    protected List<SkillData> defaultSkills = new List<SkillData>();
    
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
            if (skillData != null)
            {
                if (!haveSkills.TryGetValue(skillData.SkillName, out var skill))
                {
                    skill = SkillFactory.CreateSkillBySkillTag(skillData.SkillTag);
                }
                skill.InitializeSkill(ownerActor, skillData);
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
}
