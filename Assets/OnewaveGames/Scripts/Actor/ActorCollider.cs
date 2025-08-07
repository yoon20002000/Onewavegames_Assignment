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
            if (ownerActor == null)
            {
                Assert.IsNotNull(ownerActor, "Owner Actor is null. Set Owner Actor.");
            }
        }
    }
}
