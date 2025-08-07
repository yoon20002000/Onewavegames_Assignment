using UnityEngine;

public class ActionEffectCostData : ActionEffectData
{
    [SerializeField]
    private ActionEffectCostType costType;
    public ActionEffectCostType CostType => costType;
}
