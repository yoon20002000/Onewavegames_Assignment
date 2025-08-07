using UnityEngine;
using UnityEngine.InputSystem;

public static class GameUtils 
{
    public static void ApplyDamage(Actor target, float damage)
    {
        float curHP =target.CurHP; 
        curHP = Mathf.Clamp(curHP - damage, 0, target.MaxMP);
        target.CurHP = curHP;
    }

    public static void ApplyDecreaseMaxHP(Actor target, float value)
    {
        float maxHP = target.MaxHP;
        maxHP = Mathf.Clamp(maxHP - value, 1, target.MaxHP);
        target.MaxHP = maxHP;
    }

    public static void ApplyCost(Actor target, float cost)
    {
        float curMP = target.CurMP;
        curMP = Mathf.Clamp(curMP - cost, 0, target.MaxMP);
        target.CurMP = curMP;
    }

    public static void ApplyDecreaseMaxMp(Actor target, float value)
    {
        float maxMP = target.MaxMP;
        maxMP = Mathf.Clamp(maxMP - value, 1, target.MaxMP);
        target.MaxMP = maxMP;
    }

    public static ActionSystem GetActionSystem(Actor actor)
    {
        return actor.GetActionSystem() ? actor.GetActionSystem() : actor.GetComponent<ActionSystem>();
    }
    
    public static Hash128 GetInputActionHash(InputAction action)
    {
        return Hash128.Compute(action.name);
    }
}
