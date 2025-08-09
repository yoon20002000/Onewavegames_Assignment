using UnityEngine;

public class Effect_PullObject : Effect
{
    private Actor sourceActor;
    private Actor targetActor;
    private const float END_RANGE = 2f;
    
    private float effectDuration => effectData.Duration;
    private float curDuration;
    private float pullSpeed => effectData.Value; 
    
    public Effect_PullObject(SkillSystem skillSystem, EffectData inEffectData) : base(skillSystem, inEffectData)
    {
    }

    public override void Apply(Actor source, Actor target)
    {
        if (source == null)
        {
            Debug.LogError($"{nameof(Effect_PullObject)} : Source actor is null");
            return;
        }
        
        if (target == null)
        {
            Debug.LogError($"{nameof(Effect_PullObject)} : Target actor is null");
            return;
        }
        
        curDuration = 0f;
        bIsRunning = true;
        sourceActor = source;
        targetActor = target;
        
        Debug.Log($"{nameof(Effect_PullObject)} : Started pulling {target.name} to {source.name}");
    }

    public override void FixedUpdate(float deltaTime)
    {
        if (!bIsRunning)
        {
            return;
        }
        
        base.FixedUpdate(deltaTime);

        curDuration += deltaTime;
        
        // 타겟이나 소스가 파괴되었는지 확인
        if (targetActor == null || sourceActor == null)
        {
            Debug.LogWarning($"{nameof(Effect_PullObject)} : Target or source actor was destroyed");
            EndEffect();
            return;
        }
        
        float distance = Vector3.Distance(sourceActor.transform.position, targetActor.transform.position);
        
        // 최소 거리 혹은 최대 유지 시간에 도달하면 종료
        if (distance <= END_RANGE || curDuration >= effectDuration)
        {
            Debug.Log($"{nameof(Effect_PullObject)} : Pull completed. Distance: {distance}, Duration: {curDuration}");
            EndEffect();
        }
        else
        {
            // 끌어오기 로직
            Vector3 targetPosition = Vector3.MoveTowards(
                targetActor.transform.position, 
                sourceActor.transform.position, 
                pullSpeed * deltaTime
            );

            targetActor.transform.position = targetPosition;
        }
    }
    
    public override bool CanApply(Actor source, Actor target)
    {
        if (!base.CanApply(source, target))
        {
            return false;
        }
        
        if (source == null || target == null)
        {
            Debug.LogWarning($"{nameof(Effect_PullObject)} : Source or target is null");
            return false;
        }
        
        if (source == target)
        {
            Debug.LogWarning($"{nameof(Effect_PullObject)} : Cannot pull self");
            return false;
        }
        
        return true;
    }
    
    public override void EndEffect()
    {
        bIsRunning = false;
        sourceActor = null;
        targetActor = null;
        Debug.Log($"{nameof(Effect_PullObject)} : Effect ended");
    }
}
