using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private List<ActionEffectData>  applyEffects;

    [SerializeField] 
    private Rigidbody rb;

    private Vector3 direction;
    private float power;
    private bool bIsInitialized = false;

    private Actor ownerActor;
    private float lifeTime;
    private float spanTime = 0;
    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        bIsInitialized = false;
    }

    private void Update()
    {
        spanTime += Time.deltaTime;
        if (lifeTime <= spanTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!bIsInitialized)
        {
            return;
        }
        rb.AddForce(direction * power);
    }

    public void InitProjectile(Actor inOwnerActor ,Vector3 inDir, float inPower, float inLifeTime = 10)
    {
        ownerActor = inOwnerActor;
        bIsInitialized = true;
        direction = inDir;
        power = inPower;
        lifeTime = inLifeTime;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        ActorCollider hitActorCollider = other.gameObject.GetComponent<ActorCollider>();
        // 현재 콜라이더의 부모가 타겟임
        if (hitActorCollider == null)
        {
            return;
        }
        
        Actor hitActor = hitActorCollider.OwnerActor;
        if (hitActor == null)
        {
            return;
        }
        
        ActionSystem actionSystem = GameUtils.GetActionSystem(hitActor);
        
        if (actionSystem)
        {
            List<Actor> actors = new List<Actor>() { hitActor };
            foreach (var actionEffectData in applyEffects)
            {
                actionSystem.ExecuteGameplayEffectData(actionEffectData, in actors);
            }
        }
        
        Destroy(this.gameObject);
    }
}
