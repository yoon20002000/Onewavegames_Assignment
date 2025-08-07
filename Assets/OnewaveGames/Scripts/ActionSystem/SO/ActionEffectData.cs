using UnityEngine;

[CreateAssetMenu(fileName = "ActionEffectData", menuName = "Scriptable Objects/ActionEffectData")]
public class ActionEffectData : ScriptableObject
{
    [SerializeField]
    private ActionEffectCreateType eEffectInstanceType;
    public ActionEffectCreateType EffectInstanceType => eEffectInstanceType;
    
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