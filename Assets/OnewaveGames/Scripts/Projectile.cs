using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public UnityEvent OnHit = new UnityEvent();
    private void OnCollisionEnter(Collision other)
    {
        OnHit.Invoke();
    }
}
