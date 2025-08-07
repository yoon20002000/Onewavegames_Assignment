using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ActionEffectCostData", menuName = "Scriptable Objects/ActionEffectCostData")]
public class ActionEffectCostData : ScriptableObject
{
    [SerializeField] 
    private EGameplayTag_ActionEffectCost eTag;
    public EGameplayTag_ActionEffectCost Tag => eTag;
    [SerializeField]
    private EActionEffectCostType eCostType;
    public EActionEffectCostType ECostType => eCostType;
    
    [SerializeField]
    private EActionEffectCreateType eEffectInstanceType;
    public EActionEffectCreateType EffectInstanceType => eEffectInstanceType;
    
    [SerializeField]
    private float value;
    public float Value => value;
}
