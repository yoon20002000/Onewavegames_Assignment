using UnityEngine;

public class ActionInstance_Grab : ActionInstance
{
    [SerializeField]
    private GameObject projectilePrefab;
    
    public ActionInstance_Grab(ActionData data, ActionSystem inActionSystem, Hash128 inputID = default) : base(data, inActionSystem, inputID)
    {
    }

    public override void StartAction()
    {
        base.StartAction();
        // 생성 및 발사 방향 처리
        // 맞혔을 때 호출할 내용 delegate 처리
        // 현재 action system을 이용해서 ge를 처리하도록
    }
}
