using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ActionDataThrow", menuName = "Scriptable Objects/ActionDataThrow")]
public class ActionDataThrow : ActionData
{
    [SerializeField]
    private GameObject projectilePrefab;
    public GameObject ProjectilePrefab =>projectilePrefab;
    public override ActionInstance CreateInstance(ActionSystem system, Hash128 inputID = default)
    {
        return new ActionInstance_Throw(this, system, inputID);
    }
}
