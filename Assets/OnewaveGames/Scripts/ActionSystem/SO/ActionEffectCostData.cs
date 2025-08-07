using UnityEngine;
[CreateAssetMenu(fileName = "ActionEffectCostData", menuName = "Scriptable Objects/ActionEffectCostData")]
public class ActionEffectCostData : ActionEffectData
{
    [SerializeField]
    private ActionEffectCostType costType;
    public ActionEffectCostType CostType => costType;
}
