using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(SkillSystem))]
public class Actor : MonoBehaviour
{
    #region Attribute region
    [Header("기본 스텟 설정")]
    private float curHP;
    public float CurHP => curHP;
    private UnityEvent<float> onHPChanged = new UnityEvent<float>();
    
    [SerializeField]
    private float maxHP;
    public float MaxHP => maxHP;
    private UnityEvent<float> onMaxHPChanged = new UnityEvent<float>();
    private UnityEvent onDeath = new UnityEvent();
    
    private float curMP;
    public float CurMP => curMP;
    private UnityEvent<float> onMPChanged = new UnityEvent<float>();
    [SerializeField]
    private float maxMP;
    public float MaxMP => maxMP;
    private UnityEvent<float> onMaxMPChanged = new UnityEvent<float>();

    protected const float MIN_HP = 1;
    protected const float MIN_MP = 0;
    private void setHP(float hp)
    {
        
        curHP = Mathf.Clamp(hp, 0, maxHP);
        onHPChanged.Invoke(curHP);
        if (curHP <= 0)
        {
            onDeath.Invoke();
        }
    }

    private void setMaxHP(float inMaxHP)
    {
        maxHP = Mathf.Max(inMaxHP, MIN_HP);
        onMaxHPChanged.Invoke(maxHP);
    }

    public void TakeDamage(float damage)
    {
        setHP(curHP - damage);
    }

    public void Heal(float heal)
    {
        setHP(curHP + heal);
    }

    public void AddMaxHP(float value)
    {
        setMaxHP(curHP + value);
    }

    private void setMP(float mp)
    {
        curMP = Mathf.Clamp(mp, 0, maxHP);
        onMPChanged.Invoke(curMP);
    }
    
    private void setMaxMP(float inMaxMP)
    {
        maxMP = Mathf.Max(MIN_MP, inMaxMP);
        onMaxMPChanged.Invoke(maxMP);
    }

    public void AddMaxMP(float value)
    {
        setMaxMP(maxMP + value);
    }
    
    public void ConsumeMP(float amount)
    {
        setMP(curMP - amount);
    }

    #endregion

    [Space(10)]
    
    #region ActionSystem region
    [Header("스킬 시스템")]
    [SerializeField]
    private SkillSystem skillSystem;
    public SkillSystem ActorSkillSystem => skillSystem;
    #endregion
    #region Unity Method region

    #region Socket region

    [SerializeField]
    private Transform attackSocket;
    public Transform AttackSocket => attackSocket;
    #endregion

    protected virtual void Awake()
    {
        if (skillSystem == null)
        {
            skillSystem = GetComponent<SkillSystem>();
        }
        skillSystem.InitializeActionSystem(this);
        
        setHP(maxHP);
        setMP(maxMP);
        
        Assert.IsNotNull(attackSocket, "Attack Socket is null");
    }

    #endregion
}
