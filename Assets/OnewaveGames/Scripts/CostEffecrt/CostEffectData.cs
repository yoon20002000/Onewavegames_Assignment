using System;
using UnityEngine;
[Serializable]
public class CostEffectData
{
    [SerializeField]
    private ECostEffectType eCostEffectType;
    public ECostEffectType ECostEffectType => eCostEffectType;

    [SerializeField]
    private float value;
    public float Value => value;
}
