using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(SkillSystem))]
public class Actor : MonoBehaviour
{
    #region Attribute region
    [Header("기본 스텟 설정")]
    private float curHP;
    public float CurHP => curHP;
    [SerializeField]
    private float maxHP;
    public float MaxHP => maxHP;
    private UnityEvent<float> onMaxHPChanged = new UnityEvent<float>();
    private UnityEvent onDeath = new UnityEvent();
    
    private float curMP;
    public float CurMP => curMP;
    [SerializeField]
    private float maxMP;
    public float MaxMP => maxMP;
    private UnityEvent<float> onHPChange = new UnityEvent<float>();

    protected const float MIN_HP = 1;
    private void setHP(float hp)
    {
        
        curHP = Mathf.Clamp(hp, 0, maxHP);
        onHPChange.Invoke(curHP);
        if (curHP <= 0)
        {
            onDeath.Invoke();
        }
    }

    private void setMaxHP(float inMaxHP)
    {
        maxHP = Mathf.Clamp(maxHP, MIN_HP, inMaxHP);
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

    public void AddMaxHP(float amount)
    {
        setMaxHP(curHP + amount);
    }

    public void AddMP(float amount)
    {
        setHP(curMP + amount);
    }

    public void ConsumeMP(float amount)
    {
        setHP(curMP - amount);
    }

    #endregion

    [Space(10)]
    
    #region ActionSystem region

    private SkillSystem skillSystem;
    public SkillSystem ActorSkillSystem => skillSystem;

    protected virtual void Awake()
    {
        if (skillSystem == null)
        {
            skillSystem = GetComponent<SkillSystem>();
        }
        skillSystem.InitializeActionSystem(this);
    }

    #endregion
    public void ApplySkill(Actor target)
    {
        // skill 찾아서
        // source에 this
        // target에 target
        // actionsystem에 이 두 변수를 전달
        // action systme에서 can play check 후 apply
        //actionSystem.ApplySkill(skill);
    }
}
