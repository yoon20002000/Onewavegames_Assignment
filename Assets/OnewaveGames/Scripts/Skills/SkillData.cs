using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New SkillData", menuName = "Skill ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("기본 정보")] 
    [SerializeField] 
    private ESkillTag skillTag;
    public ESkillTag SkillTag 
    { 
        get => skillTag; 
        set => skillTag = value; 
    }

    [SerializeField]
    private string skillName= "New Skill"; 
    public string SkillName 
    { 
        get => skillName; 
        set => skillName = value; 
    }
    
    [SerializeField]
    private string skillDescription ="스킬 설명"; 
    public string SkillDescription 
    { 
        get => skillDescription; 
        set => skillDescription = value; 
    }
    
    [Header("Icon 설정")]
    [SerializeField]
    private Sprite skillIcon;
    public Sprite SkillIcon 
    { 
        get => skillIcon; 
        set => skillIcon = value; 
    }
    
    [Header("스킬 속성")]
    [SerializeField]
    private float range;
    public float Range 
    { 
        get => range; 
        set => range = value; 
    }
    [SerializeField]
    private float value;
    public float Value 
    { 
        get => value; 
        set => this.value = value; 
    }
    [SerializeField]
    private float cooldown;
    public float Cooldown 
    { 
        get => cooldown; 
        set => cooldown = value; 
    }
    
    [Header("효과 설정")]
    [SerializeField]
    private List<EffectData> effects = new List<EffectData>();
    public List<EffectData> Effects => effects;
    [FormerlySerializedAs("costEffects")]
    [Header("코스트 효과 설정")]
    [SerializeField]
    private List<CostEffectData> costEffectData = new List<CostEffectData>();
    public List<CostEffectData> CostEffectData => costEffectData;
}
