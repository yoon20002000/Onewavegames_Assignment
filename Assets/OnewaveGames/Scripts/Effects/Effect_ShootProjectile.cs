using UnityEngine;

public class Effect_ShootProjectile : Effect
{
    public Effect_ShootProjectile(SkillSystem skillSystem, EffectData inEffectData) : base(skillSystem, inEffectData)
    {
    }

    private GameObject projectilePrefab => effectData.Prefab;
    

    public override void Apply(Actor source, Actor target)
    {
        if (source == null)
        {
            Debug.LogError($"{nameof(Effect_ShootProjectile)} : Source actor is null");
            return;
        }
        
        if (projectilePrefab == null)
        {
            Debug.LogError($"{nameof(Effect_ShootProjectile)} : Projectile prefab is null");
            return;
        }
        
        Vector3 spawnPos = source.AttackSocket.position;
        Vector3 shootDir = source.transform.forward; // 전방 발사
        float shootPower = effectData.Value;
        float projectileDuration = effectData.Duration;
        
        // ObjectPool을 사용하여 투사체 생성
        GameObject projectileInstance = GameUtils.GetGameObjectFromPool(effectData.CustomData,spawnPos, Quaternion.identity, projectilePrefab);
        
        if (projectileInstance == null)
        {
            Debug.LogError($"{nameof(Effect_ShootProjectile)} : Failed to get projectile from pool");
            return;
        }
        
        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        
        if (projectile == null)
        {
            Debug.LogError($"{nameof(Effect_ShootProjectile)} : Projectile component not found on prefab");
            ObjectPoolManager.Instance?.Release(projectileInstance, "Projectile");
            return;
        }
        
        projectile.Initialize(OwnerSkillSystem.OwnerActor, shootDir, shootPower, projectileDuration);
        
        EndEffect();
    }
    
    public override bool CanApply(Actor source, Actor target)
    {
        if (!base.CanApply(source, target))
        {
            return false;
        }
        
        if (projectilePrefab == null)
        {
            Debug.LogWarning($"{nameof(Effect_ShootProjectile)} : Projectile prefab is null");
            return false;
        }
        
        if (source?.AttackSocket == null)
        {
            Debug.LogWarning($"{nameof(Effect_ShootProjectile)} : Source or AttackSocket is null");
            return false;
        }
        
        return true;
    }
}
