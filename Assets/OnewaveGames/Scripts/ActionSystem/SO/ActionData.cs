using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ActionData", menuName = "Scriptable Objects/ActionData")]
public class ActionData : ScriptableObject
{
    [SerializeField]
    private EGameplayTag_Action eActionTag;
    public EGameplayTag_Action ActionTag => eActionTag;
    [SerializeField]
    private float actionRange;
    public float ActionRange => actionRange;
    [SerializeField]
    private float actionValue;
    public float ActionValue => actionValue;

    [SerializeField]
    private List<ActionEffectData> applyEffects;
    public List<ActionEffectData> ApplyEffects => applyEffects;
    
    [SerializeField]
    private float cooldown;
    public float Cooldown => cooldown;
    
    [SerializeField]
    private ActionEffectCostData effectCost;
    public ActionEffectCostData EffectCost => effectCost;

    public virtual ActionInstance CreateInstance(ActionSystem system, Hash128 inputID = default)
    {
        return new ActionInstance(this, system, inputID);
    }
}
