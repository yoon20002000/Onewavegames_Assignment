using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ActionData", menuName = "Scriptable Objects/ActionData")]
public class ActionData : ScriptableObject
{
    public float ActionRange;
    public float ActionValue;

    public List<ActionData> ApplyEffects;

    public float Cooldown;
    public ActionEffectData CostEffect;

    public virtual ActionInstance CreateInstance(ActionSystem system)
    {
        return new ActionInstance(this, system);
    }
}
