using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public UnityEvent<Actor> OnHit = new ();
    [SerializeField]
    private List<ActionEffectData>  applyEffects;
    
    private void OnCollisionEnter(Collision other)
    {
        Actor hitActor = other.gameObject.GetComponent<Actor>();
        OnHit.Invoke(hitActor);
    }
}
