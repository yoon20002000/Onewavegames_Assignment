using System;
using UnityEngine;
using UnityEngine.Assertions;

public class ActorCollider : MonoBehaviour
{
    [SerializeField]
    private Actor ownerActor;
    public Actor OwnerActor => ownerActor;

    private void Awake()
    {
        if (ownerActor == null)
        {
            ownerActor = GetComponentInParent<Actor>();
            Assert.IsNotNull(ownerActor, "Owner Actor is null in parents");
        }
    }
}
