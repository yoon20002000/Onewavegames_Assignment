using System;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class ActionInstance_Throw : ActionInstance
{
    private GameObject projectilePrefab => DataThrow.ProjectilePrefab;
    private ActionDataThrow DataThrow => Data as ActionDataThrow;
    public ActionInstance_Throw(ActionData data, ActionSystem inActionSystem, Hash128 inputID = default) : base(data, inActionSystem, inputID)
    {
        Assert.IsTrue(data is ActionDataThrow);
    }

    public override void StartAction()
    {
        base.StartAction();
        Debug.Log("Start Grab");
        // 생성 및 발사 방향 처리
        // 맞혔을 때 호출할 내용 delegate 처리
        // 현재 action system을 이용해서 ge를 처리하도록
        // ActionSystem actionSystem = GameUtils.GetActionSystem(hitActor);
        //
        // if (actionSystem)
        // {
        //     List<Actor> actors = new List<Actor>() { hitActor };
        //     foreach (var actionEffectData in applyEffects)
        //     {
        //         actionSystem.ExecuteGameplayEffectData(actionEffectData, in actors);
        //     }
        // }
    }

    public override void StopAction()
    {
        base.StopAction();
        Debug.Log("Stop Grab");
    }
}
