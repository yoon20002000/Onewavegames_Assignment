using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Actor
{
    [Header("Input Action Ref")]
    [SerializeField]
    private InputActionReference grabSkillInput;

    protected override void Awake()
    {
        base.Awake();
        BindInputNAction();
    }

    private void BindInputNAction()
    {
        if (grabSkillInput != null && grabSkillInput.action != null)
        {
            grabSkillInput.action.performed+= OnInputPerformed;
            grabSkillInput.action.canceled+= OnInputCanceled;    
        }
    }

    private void OnInputPerformed(InputAction.CallbackContext ctx)
    {
        Hash128 inputID = GameUtils.GetInputActionHash(ctx.action);

        Debug.Log("Input ID Input : " + inputID);
        actionSystem.HardwareInputPerformed(inputID);
    }

    private void OnInputCanceled(InputAction.CallbackContext ctx)
    {
        Hash128 inputID = GameUtils.GetInputActionHash(ctx.action);

        Debug.Log("Input ID Released: " + inputID);
        actionSystem.HardwareInputCanceled(inputID);
    }
}
