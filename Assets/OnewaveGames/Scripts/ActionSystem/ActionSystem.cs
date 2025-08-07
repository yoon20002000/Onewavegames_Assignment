using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
[Serializable]
public struct ActionDataWithInputID
{
   public ActionData actionData;
   public InputActionReference inputAction;
}

public class ActionSystem : MonoBehaviour
{
   protected Actor ownerActor;
   public Actor OwnerActor => ownerActor;
   [SerializeField]
   protected List<ActionDataWithInputID> defaultActionData = new List<ActionDataWithInputID>();
   
   private Dictionary<EGameplayTag_Action, ActionInstance> addedActions = new ();
   private List<ActionEffectData> addedActionEffects = new ();
   
   private void Awake()
   {
      ownerActor = GetComponent<Actor>();
      
      Assert.IsNotNull(ownerActor, "OwnerActor is null. Check Attach Actor Component.");
      
      if (ownerActor == null)
      {
         ownerActor = this.AddComponent<Actor>();
      }

      foreach (var actionData in defaultActionData)
      {
         AddAction(ownerActor, actionData.actionData, GameUtils.GetInputActionHash(actionData.inputAction.action));
      }
   }

   public void AddAction(Actor instigator, ActionData actionData, Hash128 inputID = default)
   {
      ActionInstance newAction = actionData.CreateInstance(this, inputID);
      addedActions[actionData.ActionTag] = newAction;
   }

   public void StartAction(EGameplayTag_Action eActionTag)
   {
      if (addedActions.TryGetValue(eActionTag, out ActionInstance action))
      {
         if (!action.CanStartAction() || action.bIsActionRunning)
         {
            return;
         }
         action.StartAction();
      }
   }

   public void StopAction(EGameplayTag_Action eActionTag)
   {
      if (addedActions.TryGetValue(eActionTag, out ActionInstance action))
      {
         if (action.bIsActionRunning)
         {
            action.StopAction();   
         }
      }
   }

   public void ApplyCostEffect(Actor applyActor, ActionEffectCostData costData)
   {
      if (costData == null)
      {
         return;
      }
      
      switch (costData.ECostType)
      {
         case EActionEffectCostType.HP:
         {
            GameUtils.ApplyDamage(applyActor,costData.Value);
            break;
         }
         case EActionEffectCostType.MaxHP:
         {
            GameUtils.ApplyDecreaseMaxMp(applyActor,costData.Value);
             break;
         }
         case EActionEffectCostType.MP:
         {
            GameUtils.ApplyCost(applyActor,costData.Value);
            break;
         }
         case EActionEffectCostType.MaxMP:
         {
            GameUtils.ApplyDecreaseMaxMp(applyActor,costData.Value);
            break;
         }
         case EActionEffectCostType.None:
         default:
         {
            break;
         }
      }
   }

   public void ExecuteGameplayEffect(ActionEffectInstance effect, in List<Actor> targetActors)
   {
      effect.ExecuteEffect(targetActors);
   }
   public void ExecuteGameplayEffectData(ActionEffectData effectData, in List<Actor> targetActors)
   {
      ActionEffectInstance effect = effectData.CreateInstance(this);
      ExecuteGameplayEffect(effect, targetActors);
   }

   public void HardwareInputPerformed(Hash128 inputID)
   {
      foreach (var action in addedActions.Values) 
      {
         if (action.InputID == inputID)
         {
            StartAction(action.ActionTag);
            break;
         }
      }
   }

   public void HardwareInputCanceled(Hash128 inputID)
   {
      foreach (var action in addedActions.Values) 
      {
         if (action.InputID == inputID)
         {
            StopAction(action.ActionTag);
            break;
         }
      }
   }
}
