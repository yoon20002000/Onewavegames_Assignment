using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class Actor : MonoBehaviour
{
    [SerializeField]
    ActionSystem actionSystem;

    [SerializeField]
    private float maxMana;
    public float GetMaxMana => maxMana;
    
    private float curMana;
    public float GetMana => curMana;
    private void Awake()
    {
        actionSystem = GetComponent<ActionSystem>();
        Assert.IsNotNull(actionSystem, "Actor action system is null");
        if (actionSystem == null)
        {
            actionSystem = this.AddComponent<ActionSystem>();
        }

        curMana = maxMana;
        // action 시스템을 통한 input binding 추가 필요
    }
}
