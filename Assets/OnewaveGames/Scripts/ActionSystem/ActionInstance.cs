using UnityEngine;

public class ActionInstance
{
    public ActionData Data { get; private set; }
    public Actor ActionOwner { get; private set; }

    protected ActionSystem actionSystem;

    public ActionInstance(ActionData data, ActionSystem inActionSystem)
    {
        actionSystem = inActionSystem;
    }

    public virtual bool CanStartAction()
    {
        // cool time
        // cost 확인
        return true;
    }

    public virtual void StartAction()
    {
        // 보유중인 GE 처리
    }

    public virtual void StopAction()
    {
        // Tag 제거 
    }
}
