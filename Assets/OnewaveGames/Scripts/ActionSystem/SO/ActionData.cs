using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ActionData", menuName = "Scriptable Objects/ActionData")]
public class ActionData : ScriptableObject
{
    public EGameplayTag_Action eActionTag;
    public float ActionRange;
    public float ActionValue;

    public List<ActionEffectData> ApplyEffects;

    public float Cooldown;
    public ActionEffectCostData EffectCost;

    public virtual ActionInstance CreateInstance(ActionSystem system, Hash128 inputID = default)
    {
        return new ActionInstance(this, system, inputID);
    }
}
