using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Actor : MonoBehaviour
{
    [SerializeField]
    protected ActionSystem actionSystem;
    public ActionSystem GetActionSystem() => actionSystem;
    
    [SerializeField] private float maxHP;
    public float MaxHP { get { return maxHP; } set { maxHP = value; } }
    private float curHP;
    public float CurHP { get { return curHP; } set { curHP = value; } }
    
    [SerializeField]
    private float maxMP;
    public float MaxMP { get { return maxMP; } set { maxMP = value; } }
    
    private float curMP;
    public float CurMP { get { return curMP; } set { curMP = value; } }
    
    
    protected virtual void Awake()
    {
        actionSystem = GetComponent<ActionSystem>();
        Assert.IsNotNull(actionSystem, "Actor action system is null");
        if (actionSystem == null)
        {
            actionSystem = this.AddComponent<ActionSystem>();
        }

        curHP = Mathf.Clamp(curHP, 0, maxHP);
        curMP = Mathf.Clamp(curMP, 0, maxMP);
    }
}
