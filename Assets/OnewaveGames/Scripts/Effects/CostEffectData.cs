using System;
using UnityEngine;
[Serializable]
public class CostEffectData
{
    [SerializeField]
    private ECostEffectType eCostEffectType;
    public ECostEffectType ECostEffectType => eCostEffectType;

    [SerializeField]
    private EffectData effectData;
    public EffectData EffectData => effectData;
}
