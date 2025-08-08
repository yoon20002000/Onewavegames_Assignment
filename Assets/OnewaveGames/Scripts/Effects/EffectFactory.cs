using System;
using UnityEngine;

public static class EffectFactory 
{
    public static Effect CreateEffect(SkillSystem skillSystem, EffectData effectData)
    {
        switch (effectData.EffectType)
        {
            case EEffectType.Projectile:
            {
                return new Effect_ShootProjectile(skillSystem,effectData);
            }
            case EEffectType.Pull:
            {
                return new Effect_FullObject(skillSystem,effectData);
                
            }
            case EEffectType.Damage:
            {
                return new Effect_Damage(skillSystem,effectData);
            }
            case EEffectType.Heal:
            {
                return new Effect_Heal(skillSystem,effectData);
            }
            default:
            {
                return new Effect_Default(skillSystem,effectData);
            }
        }
    }
}
