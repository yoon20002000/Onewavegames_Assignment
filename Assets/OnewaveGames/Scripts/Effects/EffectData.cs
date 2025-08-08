using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class EffectData
{
    [SerializeField]
    private EEffectType eEffectType;
    public EEffectType EffectType => eEffectType;

    [SerializeField]
    private float value;
    public float Value => value;

    [SerializeField]
    private float duration;
    public float Duration => duration; 

    [SerializeField]
    private string customData;
    public string CustomData=>customData;

    [SerializeField] 
    private GameObject prefab;
    public GameObject Prefab => prefab;
}