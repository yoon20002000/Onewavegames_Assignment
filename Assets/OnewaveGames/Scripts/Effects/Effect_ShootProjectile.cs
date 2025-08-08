using UnityEngine;

public class Effect_ShootProjectile : Effect
{
    private readonly Vector3 spawnPosOffset = new Vector3(0f, 1f, 2f);

    public Effect_ShootProjectile(SkillSystem skillSystem, EffectData inEffectData) : base(skillSystem, inEffectData)
    {
    }

    private GameObject projectilePrefab => effectData.Prefab;
    

    public override void Apply(Actor source, Actor target)
    {
        Vector3 spawnPos = source.transform.position + spawnPosOffset;
        Vector3 shootDir = source.transform.forward;
        float shootPower = effectData.Value;
        GameObject projectileInstance = GameObject.Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        projectile.Initialize(OwnerSkillSystem.OwnerActor, shootDir, shootPower);
    }

    public override bool CanApply(Actor source, Actor target)
    {
        if (!base.CanApply(source, target))
        {
            return false;
        }
        return projectilePrefab != null;
    }
}
