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
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(nameof(Projectile)+"Collision enter :" + other.gameObject.name);
        
        ActorCollider hitActorCollider = other.gameObject.GetComponent<ActorCollider>();
        if (hitActorCollider == null)
        {
            return;
        }
        
        Actor hitActor = hitActorCollider.OwnerActor;
        if (hitActor == null || hitActor == ownerActor)
        {
            return;
        }
        
        applyEffect(hitActor);
        
        Destroy(this.gameObject);
    }

    #endregion

    public void Initialize(Actor inOwnerActor,Vector3 dir, float power, float duration = 10)
    {
        curDuration = 0;
        ownerActor = inOwnerActor;
        shootDir = dir;
        shootPower = power;
        bIsInitialized = true;
        rb.AddForce(shootDir * shootPower);
        projectileLifetime = duration;
    }

    private void applyEffect(Actor hitActor)
    {
        if (effectData == null)
        {
            return;
        }
        
        hitActor.ActorSkillSystem.ApplyEffectData(effectData, ownerActor, hitActor);
    }
}
