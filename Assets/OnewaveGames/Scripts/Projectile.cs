using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
public class Projectile : MonoBehaviour, IPoolable
{
    
    private Actor ownerActor;
    [Header("이펙트 정보")]
    [SerializeField]
    private List<EffectData> effectData;
    [SerializeField]
    private Rigidbody rb;
    private Vector3 shootDir;
    private float shootPower;
    private bool bIsInitialized = false;
    
    [Header("기본 생명 유지 시간")]
    [SerializeField]
    private float defaultProjectileLifetime;

    private float maxDuration;
    private float curDuration = 0;

    #region Unity Method region
    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            Assert.IsNotNull(rb);
            Debug.LogError($"{nameof(Projectile)} {name} : Rigidbody component is required!");
        }
    }

    private void Update()
    {
        if (!bIsInitialized)
        {
            return;
        }

        curDuration += Time.deltaTime;
        
        if (curDuration >= maxDuration)
        {
            Debug.Log($"{nameof(Projectile)} {name} lifetime expired");
            ReturnToPool();
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!bIsInitialized)
        {
            return;
        }
        
        Debug.Log($"{nameof(Projectile)} {name} Collision with: {other.gameObject.name}");
        
        ActorCollider hitActorCollider = other.gameObject.GetComponent<ActorCollider>();
        if (hitActorCollider == null)
        {
            Debug.Log($"{nameof(Projectile)} hit non-actor object: {other.gameObject.name}");
            return;
        }
        
        Actor hitActor = hitActorCollider.OwnerActor;
        if (hitActor == null)
        {
            Debug.LogWarning($"{nameof(Projectile)} hit actor with null OwnerActor: {other.gameObject.name}");
            return;
        }
        
        if (hitActor == ownerActor)
        {
            Debug.Log($"{nameof(Projectile)} hit owner actor, ignoring");
            return;
        }
        
        applyEffect(hitActor);
        
        ReturnToPool();
    }
    #endregion

    
    public void Initialize(Actor inOwnerActor, Vector3 dir, float power, float duration = -1)
    {
        if (inOwnerActor == null)
        {
            Debug.LogError($"{nameof(Projectile)} Initialize : Owner actor is null");
            return;
        }
        
        if (rb == null)
        {
            Debug.LogError($"{nameof(Projectile)} Initialize : Rigidbody is null");
            return;
        }
        
        curDuration = 0f;
        ownerActor = inOwnerActor;
        shootDir = dir.normalized;
        shootPower = power;
        bIsInitialized = true;
        
        if (duration > 0)
        {
            maxDuration = defaultProjectileLifetime;    
        }
        
        // 물리 기반 발사
        rb.linearVelocity = Vector3.zero; // 이전 속도 초기화
        rb.angularVelocity = Vector3.zero; // 이전 각속도 초기화
        rb.AddForce(shootDir * shootPower, ForceMode.Impulse);
        
        Debug.Log($"{nameof(Projectile)} {name} initialized with direction: {shootDir}, power: {power}");
    }

    /// <summary>
    /// 풀에 반환합니다.
    /// </summary>
    public void ReturnToPool()
    {
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.Release(gameObject, "Projectile");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void applyEffect(Actor hitActor)
    {
        if (effectData == null)
        {
            Debug.LogWarning($"{nameof(Projectile)} {name} : No effect data assigned");
            return;
        }

        if (ownerActor?.ActorSkillSystem == null)
        {
            Debug.LogError($"{nameof(Projectile)} {name} : Owner actor or skill system is null");
            return;
        }

        Debug.Log($"{nameof(Projectile)} {name} applying effect to {hitActor.name}");
        ownerActor.ActorSkillSystem.ApplyEffectsFromEffectData(effectData, ownerActor, hitActor);
    }

    #region IPoolable Implementation region
    // 풀에서 생성될 때 초기화
    public void OnCreate()
    {
        bIsInitialized = false;
        curDuration = 0f;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        Debug.Log($"[{nameof(Projectile)}] OnCreate: {name}");
    }
    // 풀에서 가져올 때 초기화    
    public void OnGet()
    {
        bIsInitialized = false;
        curDuration = 0f;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        Debug.Log($"[{nameof(Projectile)}] OnGet: {name}");
    }

    public void OnRelease()
    {
        // 풀에 반환할 때 정리
        bIsInitialized = false;
        ownerActor = null;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        Debug.Log($"[{nameof(Projectile)}] OnRelease: {name}");
    }

    public void OnDestroy()
    {
        // 풀에서 파괴될 때 정리
        bIsInitialized = false;
        ownerActor = null;
        
        Debug.Log($"[{nameof(Projectile)}] OnDestroy: {name}");
    }
    #endregion
}
