using System;
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
        
        Debug.Log("부여한 GE 발동");
        
        Destroy(this.gameObject);
    }
}
