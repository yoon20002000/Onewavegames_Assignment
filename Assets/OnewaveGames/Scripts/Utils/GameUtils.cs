using UnityEngine;
using UnityEngine.Assertions;
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

    public static Ray CreateRayFromMousePosition(Vector2 mousePosition)
    {
        Camera mainCamera = Camera.main;
        Assert.IsNotNull(mainCamera, "Main camera is null");
        return mainCamera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, 0));
    }
    
    public static bool TryGetRaycastHitPosition(Ray ray, out Vector3 hitPosition, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
        {
            hitPosition = hit.point;
            return true;
        }

        hitPosition = Vector3.zero;
        return false;
    }
    
    public static Vector3 CalculateDirection(Vector3 sourcePosition, Vector3 targetPosition, bool bKeepY = false)
    {
        if (bKeepY)
        {
            targetPosition.y = sourcePosition.y;
        }

        Vector3 direction = targetPosition - sourcePosition;
        return direction.normalized;
    }
}
