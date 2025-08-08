using UnityEngine;
using UnityEngine.InputSystem;

public static class GameUtils 
{
    public static Hash128 GetInputActionHash(InputAction action)
    {
        if (action == null)
        {
            return default(Hash128);
        }
        return Hash128.Compute(action.name);
    }
}
