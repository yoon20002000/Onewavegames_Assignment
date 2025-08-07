using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class ActionSystem : MonoBehaviour
{
   protected Actor OwnerActor;

   [SerializeField]
   protected List<ActionData> defaultActionData = new List<ActionData>();

   private List<ActionInstance> addActions = new List<ActionInstance>();
   private void Awake()
   {
      OwnerActor = GetComponent<Actor>();
      if (OwnerActor == null)
      {
         Assert.IsNotNull(OwnerActor, "OwnerActor is null. Check Attach Actor Component.");
         OwnerActor = this.AddComponent<Actor>();
      }
   }

   public void AddAction(Actor instigator, ActionData actionData)
   {
      
   }
}
