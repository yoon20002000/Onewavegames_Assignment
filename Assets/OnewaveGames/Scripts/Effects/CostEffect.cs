using System;
using UnityEngine;
using UnityEngine.Assertions;

public class CostEffect
{
    protected CostEffectData costEffectData;
    public CostEffectData CostEffectData => costEffectData;
    
    public virtual void InitializeEffect(CostEffectData costEffect)
    {
        costEffectData = costEffect;
    }
    
    public virtual void PreApply()
    {
        Assert.IsTrue(costEffectData.EffectData.EffectType == EEffectType.Cost, 
            "Cost Effect의 Type 설정 오류. Cost로 설정해주십시오. CostEffectData Name : " + costEffectData.EffectData);
    }

    public virtual void Apply(Actor source, Actor target)
    {
        Debug.Log("Pay Cost Effect");
        if (source == null || source.ActorSkillSystem == null)
        {
            return;
        }
        switch (costEffectData.ECostEffectType)
        {
            case ECostEffectType.None:
            default:
            {
                break;
            }
            case ECostEffectType.HP:
            {
                source.TakeDamage(costEffectData.EffectData.Value) ;
                break;
            }
            case ECostEffectType.MaxHP:
            {
                source.AddMaxHP(-costEffectData.EffectData.Value) ;
                break;
            }
            case ECostEffectType.MP:
            {
                source.ConsumeMP(costEffectData.EffectData.Value);
                break;
            }
            case ECostEffectType.MaxMP:
            {
                source.AddMaxMP(-costEffectData.EffectData.Value) ;
                break;
            }
        }
        Debug.Log("Pay Cost Effect done : " + costEffectData.ECostEffectType);
    }
    // modifier가 없어서 발생 됨
    // attribute가 리플렉션 돼서 각자 설정이 가능했다면 그냥 effect로 만들었을 듯 함.
    public virtual bool CanApply(Actor source, Actor target)
    {
        switch (costEffectData.ECostEffectType)
        {
            case ECostEffectType.None:
            default:
            {
                return true;
            }
            case ECostEffectType.HP:
            {
                return source.CurHP >= costEffectData.EffectData.Value;
            }
            case ECostEffectType.MaxHP:
            {
                return source.MaxHP >= costEffectData.EffectData.Value;
            }
            case ECostEffectType.MP:
            {
                return source.CurMP >= costEffectData.EffectData.Value;
            }
            case ECostEffectType.MaxMP:
            {
                return source.MaxMP >= costEffectData.EffectData.Value;
            }
        }
    }
}
