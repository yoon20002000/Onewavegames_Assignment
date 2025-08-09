using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class EffectData
{
    [SerializeField]
    private EEffectType eEffectType;
    public EEffectType EffectType 
    { 
        get => eEffectType; 
        set => eEffectType = value; 
    }

    [SerializeField] 
    private float value;
    public float Value 
    { 
        get => value; 
        set => this.value = value; 
    }

    [SerializeField] 
    private float duration;
    public float Duration 
    { 
        get => duration; 
        set => duration = value; 
    }

    [SerializeField] 
    private string customData;
    public string CustomData 
    { 
        get => customData; 
        set => customData = value; 
    }

    [SerializeField] 
    private GameObject prefab;
    public GameObject Prefab 
    { 
        get => prefab; 
        set => prefab = value; 
    }
}