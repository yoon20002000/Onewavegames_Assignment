using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionApplySelfAEData", menuName = "Scriptable Objects/ActionApplySelfAEData")]
public class ActionApplySelfAEData : ActionData
{
    [SerializeField]
    private List<ActionEffectData> applyEffects;
    public List<ActionEffectData> ApplyEffects => applyEffects;

    public override ActionInstance CreateInstance(ActionSystem system, Hash128 inputID = default)
    {
        return new ActionInstance_ApplySelfAE(this, system, inputID);
    }
}
