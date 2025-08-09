# SkillSystem 프레임워크 사용 설명서

## 개요
SkillSystem은 확장 가능하고 데이터 기반의 스킬 시스템으로, 기획자가 수치와 옵션만 조정해서 다양한 스킬을 만들 수 있는 Unity 프레임워크입니다.

## 시스템 아키텍처

### 핵심 컴포넌트 구조
```
SkillSystem (MonoBehaviour)
├── Actor (MonoBehaviour) : 캐릭터 Actor
│   ├── HP/MP 관리
│   ├── 스킬 사용자 정보
│   └── SkillSystem 참조
├── SkillData (ScriptableObject) : 스킬 정보 
│   ├── 스킬 기본 정보
│   ├── 효과 데이터 리스트
│   └── 비용 데이터 리스트
├── Skill (추상 클래스)     : 상세 스킬 구현
│   ├── Skill_SelfTarget
│   ├── Skill_Grab
│   └── 커스텀 스킬들
├── Effect (추상 클래스)    : 상세 이팩트 구현
│   ├── Effect_Damage
│   ├── Effect_Heal
│   ├── Effect_ShootProjectile
│   └── Effect_PullObject
├── EffectData (Serializable) : 이팩트 데이터
│   ├── 효과 타입
│   ├── 효과 수치
│   ├── 지속 시간
│   ├── 커스텀 데이터
│   └── 프리팹 참조
└── CostEffectData (Serializable) : 비용 데이터
    ├── 비용 타입
    └── 소모량
```

### 데이터 흐름
```
Input → SkillSystem → Skill → Effect → Actor
```

## 기본 설정

### 1. Actor 설정
![액터](ReadmeResources/Actor.png)
```csharp
[RequireComponent(typeof(SkillSystem))]
public class Actor : MonoBehaviour
{
    [SerializeField] 
    private float curHP;
    [SerializeField] 
    private float maxHP;
    [SerializeField] 
    private float curMP;
    [SerializeField] 
    private float maxMP;
    
    [SerializeField] 
    private SkillSystem skillSystem;
    [SerializeField] 
    private Transform attackSocket; // 공격 위치
    
    protected virtual void Awake()
    {
        skillSystem.InitializeActionSystem(this);
    }
}
```

**설정 단계:**
1. GameObject에 Actor 컴포넌트 추가
2. HP/MP 값 설정
3. AttackSocket Transform 할당 (공격 생성 지점)
4. SkillSystem 자동 생성됨

### 2. SkillSystem 설정
**이미지 첨부 필요**: SkillSystem Inspector의 defaultSkills 설정 화면  
![스킬 시스템](ReadmeResources/SkillSystem.png)
```csharp
public class SkillSystem : MonoBehaviour
{
    [Header("스킬 정보")]
    [SerializeField]
    protected List<SkillDataWithInput> defaultSkills = new List<SkillDataWithInput>();
    
    [Serializable]
    public struct SkillDataWithInput
    {
        public SkillData Data;                      // 스킬 데이터
        public InputActionReference InputActionRef; // 입력 액션
        public bool bIsPerform;                     // performed 이벤트 사용
        public bool bIsStart;                       // started 이벤트 사용
        public bool bIsRelease;                     // canceled 이벤트 사용
    }
}
```

## 스킬 데이터 생성

### 1. SkillData ScriptableObject 생성

**생성 단계:**
1. Project 창에서 우클릭
2. Create → Skill ScriptableObject → SkillData
3. 파일명 지정 (예: "FireballSkill")

### 2. SkillData 설정
![스킬 데이터](ReadmeResources/SkillData.png)

```csharp
[CreateAssetMenu(fileName = "New SkillData", menuName = "Skill ScriptableObject/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("기본 정보")] 
    [SerializeField] 
    private ESkillTag skillTag;

    [SerializeField]
    private string skillName= "New Skill"; 
    
    [SerializeField]
    private string skillDescription ="스킬 설명"; 
    
    [Header("스킬 속성")]
    [SerializeField]
    private float range;
    [SerializeField]
    private float value;
    [SerializeField]
    private float cooldown;
    
    [Header("효과 설정")]
    [SerializeField]
    private List<EffectData> effects = new List<EffectData>();
    
    [Header("코스트 효과 설정")]
    [SerializeField]
    private List<CostEffectData> costEffectData = new List<CostEffectData>();
}
```

**설정 항목:**
- **Skill Tag**: 스킬 타입 (SelfTarget, Grab 등)
- **Skill Name**: 스킬 이름
- **Skill Description**: 스킬 설명
- **Range**: 스킬 범위
- **Value**: 기본 수치 (데미지, 힐량 등)
- **Cooldown**: 쿨다운 시간
- **Effects**: 적용될 효과들
- **Cost Effects**: 소모될 비용들

## 효과 시스템

### 1. EffectData 설정
![스킬 데이터에서 이펙트 데이터 설정](ReadmeResources/EffectData.png)

```csharp
[System.Serializable]
public class EffectData
{
    [SerializeField]
    private EEffectType eEffectType;

    [SerializeField] 
    private float value;

    [SerializeField] 
    private float duration;

    [SerializeField] 
    private string customData;

    [SerializeField] 
    private GameObject prefab;
}
```

### 2. 지원하는 효과 타입
```csharp
public enum EEffectType
{
    Projectile,     // 투사체 발사
    Pull,           // 끌어오기
    Damage,         // 데미지
    Heal            // 회복
}
```

### 3. 효과 조합 예시

#### 대미지를 입히는 그랩스킬
```csharp
// 투사체 발사 효과
이미지 projectile

// 데미지 효과
프로젝타일에 넣은 effect
```

#### 힐링 스킬
```csharp
// 회복 효과
이미지
```

## 비용 시스템

### 1. CostEffectData 설정
```csharp
[System.Serializable]
public class CostEffectData
{
    [SerializeField]
    private ECostEffectType eCostEffectType;

    [SerializeField]
    private float value;
}

public enum ECostEffectType
{
    None = 0,
    HP,
    MaxHP,
    MP,
    MaxMP,
}

```

### 2. 비용 설정 예시
```csharp
// MP 20 소모
이미지


// HP 5 소모 (자해 스킬)
이미지
```

## 🎮 입력 시스템 연동

### 1. Unity Input System 설정
**이미지 첨부 필요**: Input Actions 설정 화면

**설정 단계:**
1. Input Actions 에셋 생성
2. Action Maps 생성 (예: "Player")
3. Actions 생성 (예: "Fireball", "Heal")
4. Binding 설정 (키보드, 마우스 등)

### 2. SkillSystem에 입력 연결
![스킬 시스템에서 디폴트 스킬 및 인풋 바인드](ReadmeResources/SkillSystem%20Input.png)

```csharp
// SkillSystem의 defaultSkills에 추가
private void initializeSkillsBySkillData()
{
    if (defaultSkills == null || defaultSkills.Count == 0)
    {
        Debug.LogWarning($"{this.gameObject.name} {nameof(SkillSystem)} : No skills configured");
        return;
    }
    
    foreach (var skillData in defaultSkills)
    {
        if (skillData.Data != null)
        {
            if (!haveSkills.TryGetValue(skillData.Data.SkillName, out var skill))
            {
                skill = SkillFactory.CreateSkillBySkillTag(skillData.Data.SkillTag);
                if (skill == null)
                {
                    Debug.LogError($"{nameof(SkillSystem)} : Failed to create skill for tag {skillData.Data.SkillTag}");
                    continue;
                }
                haveSkills.Add(skillData.Data.SkillName, skill);
            }

            skill.InitializeSkill(ownerActor, skillData.Data,
                GameUtils.GetInputActionHash(skillData.InputActionRef.action));

            if (skillData.bIsPerform)
            {
                skillData.InputActionRef.action.performed += onInputPerformed;
            }

            if (skillData.bIsStart)
            {
                skillData.InputActionRef.action.started += onInputStarted;
            }

            if (skillData.bIsRelease)
            {
                skillData.InputActionRef.action.canceled += onInputCanceled;
            }
        }
        else
        {
            Debug.LogWarning($"{nameof(SkillSystem)} : Skill data is null in defaultSkills");
        }
    }
    
    Debug.Log($"{nameof(SkillSystem)} : Initialized {haveSkills.Count} skills");
}
```

**입력 이벤트 옵션:**
- **bIsPerform**: 키를 누르고 있을 때 
- **bIsStart**: 키를 누르기 시작할 때
- **bIsRelease**: 키를 놓을 때

## 커스텀 스킬 구현

### 1. 새로운 스킬 클래스 생성
```csharp
public class Skill_Fireball : Skill
{
    public override bool ApplySkill(Actor source, Actor target)
    {
        // 마우스 방향으로 발사
        Ray mouseRay = GameUtils.CreateRayFromMousePosition(Mouse.current.position.ReadValue());
        if (GameUtils.TryGetRaycastHitPosition(mouseRay, out Vector3 hitPosition))
        {
            Vector3 dir = GameUtils.CalculateDirection(source.transform.position, hitPosition, true);
            source.transform.forward = dir;
        }
        
        // 효과 적용
        OwnerSkillSystem.ApplyEffectsFromEffectData(ApplySkillData.Effects, source, target);
        
        CompleteSkill();
        return true;
    }
}
```

### 2. SkillFactory에 등록
```csharp
public static class SkillFactory
{
    public static Skill CreateSkillBySkillTag(ESkillTag eSkillTag)
    {
        switch (eSkillTag)
        {
            case ESkillTag.Skill_Fireball:
                return new Skill_Fireball();
            case ESkillTag.Skill_SelfTarget:
                return new Skill_SelfTarget();
            case ESkillTag.Skill_Grab:
                return new Skill_Grab();
            default:
                return new Skill_SelfTarget();
        }
    }
}
```

### 3. 새로운 스킬 태그 추가
```csharp
public enum ESkillTag
{
    Skill_SelfTarget,
    Skill_Grab,
    Skill_Fireball,  // 새로 추가
    Skill_Teleport,  // 새로 추가
    Skill_Shield     // 새로 추가
}
```

## 커스텀 이펙트 구현

### 1. 새로운 이펙트 클래스 생성
```csharp
public class Effect_Burn : Effect
{
    private float burnDuration;
    private float burnTick;
    
    public Effect_Burn(SkillSystem skillSystem, EffectData inEffectData) : base(skillSystem, inEffectData)
    {
        burnDuration = effectData.Duration;
        burnTick = 0f;
    }
    
    public override void Apply(Actor source, Actor target)
    {
        if (target != null)
        {
            bIsRunning = true;
            burnTick = 0f;
            Debug.Log($"[{nameof(Effect_Burn)}] {target.name} is burning!");
        }
    }
    
    public override void Update(float deltaTime)
    {
        if (!bIsRunning) return;
        
        burnTick += deltaTime;
        
        // 1초마다 데미지
        if (burnTick >= 1f)
        {
            burnTick = 0f;
            target?.TakeDamage(source, effectData.Value);
        }
        
        // 지속 시간 종료
        if (burnTick >= burnDuration)
        {
            EndEffect();
        }
    }
}
```

### 2. EffectFactory에 등록
```csharp
public static class EffectFactory 
{
    public static Effect CreateEffect(SkillSystem skillSystem, EffectData effectData)
    {
        switch (effectData.EffectType)
        {
            case EEffectType.Burn:
                return new Effect_Burn(skillSystem, effectData);
            case EEffectType.Damage:
                return new Effect_Damage(skillSystem, effectData);
            case EEffectType.Heal:
                return new Effect_Heal(skillSystem, effectData);
            case EEffectType.Projectile:
                return new Effect_ShootProjectile(skillSystem, effectData);
            case EEffectType.Pull:
                return new Effect_PullObject(skillSystem, effectData);
            default:
                return new Effect_Default(skillSystem, effectData);
        }
    }
}
```

### 3. EEffectType enum에 추가
```csharp
public enum EEffectType
{
    Projectile,     // 투사체 발사
    Pull,           // 끌어오기
    Damage,         // 데미지
    Heal,           // 회복
    Burn            // 새로 추가 - 화상
}
```

## 🔧 고급 기능

### 1. 조건부 스킬 실행
```csharp
public override bool CanApplySkill()
{
    if (!base.CanApplySkill())
        return false;
    
    // 추가 조건 검사
    if (OwnerActor.CurHP < OwnerActor.MaxHP * 0.3f)
    {
        Debug.Log("HP가 30% 이하일 때만 사용 가능한 스킬");
        return false;
    }
    
    return true;
}
```

### 2. 동적 스킬 추가/제거
```csharp
// 런타임에 스킬 추가
public void AddSkill(SkillData skillData, InputActionReference inputAction)
{
    var skill = SkillFactory.CreateSkillBySkillTag(skillData.SkillTag);
    skill.InitializeSkill(OwnerActor, skillData);
    haveSkills.Add(skillData.SkillName, skill);
}

// 스킬 제거
public void RemoveSkill(string skillName)
{
    if (haveSkills.ContainsKey(skillName))
    {
        haveSkills.Remove(skillName);
    }
}
```

### 3. ObjectPool 연동
![오브젝트 풀 매니저](ReadmeResources/ObjectPoolManager.png)

```csharp
// GameUtils Pool을 이용해 GameObject를 받아오는 기능
public static GameObject GetGameObjectFromPool(string poolName, Vector3 position, Quaternion rotation, GameObject defaultPrefab)
{
    // ObjectPoolManager가 없으면 기존 방식 사용
    if (ObjectPoolManager.Instance == null)
    {
        Debug.LogWarning($"{nameof(Effect_ShootProjectile)} : ObjectPoolManager not found, using Instantiate");
        return GameObject.Instantiate(defaultPrefab, position, rotation);
    }
    
    // 풀이 존재하지 않으면 런타임에 생성
    if (!ObjectPoolManager.Instance.PoolExists(poolName))
    {
        ObjectPoolManager.Instance.CreateRuntimePool(poolName, defaultPrefab, 10, 50);
    }
    
    return ObjectPoolManager.Instance.Get(poolName, position, rotation);
}
```