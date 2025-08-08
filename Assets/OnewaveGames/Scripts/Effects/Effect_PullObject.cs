using UnityEngine;

public class Effect_PullObject : Effect
{
    private Actor sourceActor;
    private Actor targetActor;
    private const float endRange = 2;
    
    private float effectDuration => effectData.Duration;
    private float curDuration;
    private float pullSpeed => effectData.Value; 
    public Effect_PullObject(SkillSystem skillSystem, EffectData inEffectData) : base(skillSystem, inEffectData)
    {
    }

    public override void Apply(Actor source, Actor target)
    {
        curDuration = 0;
        bIsRunning = true;
        sourceActor = source;
        targetActor = target;
        // target.transform.SetPositionAndRotation(source.transform.position, Quaternion.identity);
    }

    public override void FixedUpdate(float deltaTime)
    {
        if (!bIsRunning)
        {
            return;
        }
        base.FixedUpdate(deltaTime);

        curDuration += deltaTime;
        
        // 최소 거리 혹은 최대 유지 시간
        if (Vector3.Distance(sourceActor.transform.position, targetActor.transform.position) <= endRange
            || curDuration >= effectDuration)
        {
            EndEffect();
        }
        else
        {
            Vector3 targetPosition = Vector3.MoveTowards(targetActor.transform.position,sourceActor.transform.position,
                pullSpeed * deltaTime);
            targetActor.transform.SetPositionAndRotation(targetPosition, Quaternion.identity);
            // Vector3.Lerp(sourceActor.transform.position, targetActor.transform.position, );
        }
    }
}
