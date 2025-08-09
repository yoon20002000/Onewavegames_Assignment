using System;
using UnityEngine;
[Serializable]
public class CostEffectData
{
    [SerializeField]
    private ECostEffectType eCostEffectType;
    public ECostEffectType ECostEffectType 
    { 
        get => eCostEffectType; 
        set => eCostEffectType = value; 
    }

    [SerializeField]
    private float value;
    public float Value 
    { 
        get => value; 
        set => this.value = value; 
    }
}
