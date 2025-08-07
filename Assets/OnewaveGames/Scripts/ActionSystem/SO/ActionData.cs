using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionData", menuName = "Scriptable Objects/ActionData")]
public class ActionData : ScriptableObject
{
    public float actionRange;
    public float actionValue;

    public List<ActionData> applyEffects;

    public float CoolTime;
    public ActionData CostEffect;

    public virtual ActionInstance CreateInstance(ActionSystem system)
    {
        return new ActionInstance(this, system);
    }
}
