using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public UnityEvent OnHit = new UnityEvent();
    [SerializeField]
    private List<ActionEffectData>  applyEffects;
    
    private void OnCollisionEnter(Collision other)
    {
        OnHit.Invoke();
        // or
        Actor hitActor = other.gameObject.GetComponent<Actor>();
        ActionSystem actionSystem = GameUtils.GetActionSystem(hitActor);

        if (actionSystem)
        {
            List<Actor> actors = new List<Actor>() { hitActor };
            foreach (var actionEffectData in applyEffects)
            {
                actionSystem.ExecuteGameplayEffectData(actionEffectData, in actors);
            }
        }
        
    }
}
