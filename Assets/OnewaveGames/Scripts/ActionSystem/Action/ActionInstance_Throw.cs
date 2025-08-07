using System;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class ActionInstance_Throw : ActionInstance
{
    private GameObject projectilePrefab => DataThrow.ProjectilePrefab;
    private ActionData_Throw DataThrow => Data as ActionData_Throw;
    private Vector3 spawnOffset = new Vector3(0,1,2);
    public ActionInstance_Throw(ActionData data, ActionSystem inActionSystem, Hash128 inputID = default) : base(data, inActionSystem, inputID)
    {
        Assert.IsTrue(data is ActionData_Throw);
    }

    public override void StartAction()
    {
        base.StartAction();
        Debug.Log("Start Grab");

        Vector3 actorLocation = ActionOwner.transform.position;
        actorLocation += spawnOffset;
        Vector3 actorForward = ActionOwner.transform.forward;
        
        GameObject instanceProjectile = GameObject.Instantiate(projectilePrefab, actorLocation, Quaternion.identity);
        Projectile projectile = instanceProjectile.GetComponent<Projectile>();

        if (projectile != null)
        {
            // 발사체 초기화
            projectile.InitProjectile(ActionOwner,actorForward, Data.ActionValue);
        }

        // 생성 및 발사 방향 처리
        // 맞혔을 때 호출할 내용 delegate 처리
        // 현재 action system을 이용해서 ge를 처리하도록
        
    }

    public override void StopAction()
    {
        base.StopAction();
        Debug.Log("Stop Grab");
    }
}
