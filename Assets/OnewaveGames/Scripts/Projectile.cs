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
    
    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    public void Initialize(Actor inOwnerActor,Vector3 dir, float power)
    {
        ownerActor = inOwnerActor;
        shootDir = dir;
        shootPower = power;
        bIsInitialized = true;
    }

    private void FixedUpdate()
    {
        if (!bIsInitialized)
        {
            return;
        }
        rb.AddForce(shootDir * shootPower);
    }
    private void OnCollisionEnter(Collision other)
    {
        applyEffect(other);
        
        Destroy(this.gameObject);
    }

    private void applyEffect(Collision collision)
    {
        if (effectData == null)
        {
            return;
        }
        
        Debug.Log(nameof(Projectile)+"Collision enter :" + collision.gameObject.name);
        ActorCollider hitActorCollider = collision.gameObject.GetComponent<ActorCollider>();
        if (hitActorCollider == null)
        {
            return;
        }
        
        Actor hitActor = hitActorCollider.OwnerActor;
        if (hitActor == null || hitActor == ownerActor)
        {
            return;
        }
        
        hitActor.ActorSkillSystem.ApplyEffectData(effectData, ownerActor, hitActor);
    }
}
