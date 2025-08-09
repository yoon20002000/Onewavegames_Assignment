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
        float previousHP = curHP;
        curHP = Mathf.Clamp(hp, 0, maxHP);
        onHPChanged.Invoke(curHP);
        
        if (curHP <= 0 && previousHP > 0)
        {
            Debug.Log($"[{nameof(setHP)}] {name} has died! (Previous HP: {previousHP:F1}, Current HP: {curHP:F1})");
            onDeath.Invoke();
            Destroy(gameObject);
        }
    }

    private void setMaxHP(float inMaxHP)
    {
        maxHP = Mathf.Max(inMaxHP, MIN_HP);
        onMaxHPChanged.Invoke(maxHP);
    }

    public void TakeDamage(Actor instigator, float damage)
    {
        string instigatorName = instigator != null ? instigator.name : "Unknown";
        
        Debug.Log($"[{nameof(TakeDamage)}] {name} (HP: {curHP:F1}/{maxHP:F1}) took {damage:F1} damage from {instigatorName}.");
        
        setHP(curHP - damage);
        
        Debug.Log($"[{nameof(TakeDamage)}] {name} HP changed to {curHP:F1}/{maxHP:F1}");
    }

    public void Heal(Actor instigator, float heal)
    {
        string instigatorName = instigator != null ? instigator.name : "Unknown";
        
        Debug.Log($"[{nameof(Heal)}] {name} (HP: {curHP:F1}/{maxHP:F1}) received {heal:F1} healing from {instigatorName}.");
        
        setHP(curHP + heal);
        
        Debug.Log($"[{nameof(Heal)}] {name} HP changed to {curHP:F1}/{maxHP:F1}");
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
    
    #region Socket region

    [SerializeField]
    private Transform attackSocket;
    public Transform AttackSocket => attackSocket;
    #endregion
    
    #region Unity Method region
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
