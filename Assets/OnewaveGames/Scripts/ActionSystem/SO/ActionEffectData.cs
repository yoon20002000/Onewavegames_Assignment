using UnityEngine;

[CreateAssetMenu(fileName = "ActionEffectData", menuName = "Scriptable Objects/ActionEffectData")]
public class ActionEffectData : ScriptableObject
{
    public ActionEffectCreateType eEffectTInstanceType;
    public float Value;
    public float Duration = 1f;
    public float Period = 1f;
}