using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Projectile : MonoBehaviour
{
    private Actor ownerActor;
    [SerializeField]
    private Rigidbody rb;
    private Vector3 shootDir;
    private float shootPower;
    private bool bIsInitialized = false;
    
    [SerializeField]
    private EffectData effectData;

    [SerializeField]
    private float projectileLifetime;
    private float curDuration = 0;

    #region Unity Method region

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        if (!bIsInitialized)
        {
            return;
        }
        
        curDuration += Time.deltaTime;
        if (curDuration >= projectileLifetime)
        {
            Debug.Log($"{nameof(Projectile)} {name} destroyed due to lifetime expiration");
            Destroy(gameObject);
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
        
        Destroy(this.gameObject);
    }

    #endregion

    public void Initialize(Actor inOwnerActor, Vector3 dir, float power, float duration = 10)
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
        projectileLifetime = duration;
        
        // 물리 기반 발사
        rb.AddForce(shootDir * shootPower, ForceMode.Impulse);
        
        Debug.Log($"{nameof(Projectile)} {name} initialized with direction: {shootDir}, power: {power}");
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
        ownerActor.ActorSkillSystem.ApplyEffectFromEffectData(effectData, ownerActor, hitActor);
    }
    
    // 디버깅용 - 발사 방향 시각화
    private void OnDrawGizmosSelected()
    {
        if (bIsInitialized)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, shootDir * 2f);
        }
    }
}
