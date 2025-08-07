using UnityEngine;

[CreateAssetMenu(fileName = "ActionEffectData", menuName = "Scriptable Objects/ActionEffectData")]
public class ActionEffectData : ScriptableObject
{
    [SerializeField]
    private EGameplayTag_ActionEffect eEffectTag;
    public EGameplayTag_ActionEffect EEffectTag => eEffectTag;
    
    [SerializeField]
    private EActionEffectCreateType eEffectInstanceType;
    public EActionEffectCreateType EffectInstanceType => eEffectInstanceType;
    
    [SerializeField]
    private float value;
    public float Value => value;
    [SerializeField]
    private float duration = 1f;
    public float Duration => duration;
    
    [SerializeField]
    private float period = 1f;
    public float Period => period;
    
    public virtual ActionEffectInstance CreateInstance(ActionSystem system)
    {
        return new ActionEffectInstance(this, system);
    }
}