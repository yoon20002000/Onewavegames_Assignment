using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class ActionSystem : MonoBehaviour
{
   protected Actor ownerActor;

   [SerializeField]
   protected List<ActionData> defaultActionData = new List<ActionData>();

   private List<ActionInstance> addActions = new List<ActionInstance>();
   private void Awake()
   {
      ownerActor = GetComponent<Actor>();
      
      Assert.IsNotNull(ownerActor, "OwnerActor is null. Check Attach Actor Component.");
      
      if (ownerActor == null)
      {
         ownerActor = this.AddComponent<Actor>();
      }
   }

   public void AddAction(Actor instigator, ActionData actionData)
   {
      
   }
}
