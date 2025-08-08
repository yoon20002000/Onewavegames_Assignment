using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New SkillData", menuName = "Skill ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("기본 정보")] 
    [SerializeField] 
    private ESkillTag skillTag;
    public ESkillTag SkillTag => skillTag;

    [SerializeField]
    private string skillName= "New Skill"; 
    public string  SkillName => skillName;
    
    [SerializeField]
    private string skillDescription ="스킬 설명"; 
    public string SkillDescription => skillDescription;
    
    [Header("스킬 속성")]
    [SerializeField]
    private float range;
    public float Range => range;
    [SerializeField]
    private float value;
    public float Value => value;
    [SerializeField]
    private float cooldown;
    public float Cooldown => cooldown;
    
    [Header("효과 설정")]
    [SerializeField]
    private List<EffectData> effects = new List<EffectData>();
    public List<EffectData> Effects => effects;
    [Header("코스트 효과 설정")]
    [SerializeField]
    private List<EffectData> costEffects = new List<EffectData>();
    public List<EffectData> CostEffects => costEffects;
}
