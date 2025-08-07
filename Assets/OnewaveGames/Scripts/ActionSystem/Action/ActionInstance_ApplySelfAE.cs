using System.Collections.Generic;
using UnityEngine;

public class ActionInstance_ApplySelfAE : ActionInstance
{
    private ActionApplySelfAEData SelfApplyData => Data as ActionApplySelfAEData;
    public ActionInstance_ApplySelfAE(ActionData data, ActionSystem inActionSystem, Hash128 inputID = default) : base(data, inActionSystem, inputID)
    {
    }

    public override void StartAction()
    {
        base.StartAction();
        List<Actor> actors = new List<Actor>(){ActionOwner};
        foreach (var effect in SelfApplyData.ApplyEffects)
        {
            actionSystem.ExecuteGameplayEffectData(effect, actors);
        }
    }
}
