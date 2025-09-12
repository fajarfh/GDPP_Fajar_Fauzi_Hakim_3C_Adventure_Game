using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public Action<Vector2> onMoveInput;
    public Action<Boolean> onSprintInput;
    public Action onJumpInput;
    public Action onClimbInput;
    public Action onCancelInput;
    public Action onChangePOV;
    public Action onCrouchInput;
    public Action onGlideInput;
    public Action onCancelGlide;
    public Action onPunchInput;

    private void Update()
    {
        
        CheckMovementInput();
        CheckSprintInput();
        CheckJumpInput();
        CheckCrouchInput();
        CheckChangePOVInput();
        CheckClimbInput();
        CheckGlideInput();
        CheckCancelInput();
        CheckPunchInput();
        CheckMainMenuInput();

    }

    private void CheckMovementInput()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        
        //Debug.Log("Vertical Axis: " + verticalAxis);
        //Debug.Log("Horizontal Axis: " + horizontalAxis);

        Vector2 inputAxis = new Vector2(horizontalAxis, verticalAxis);

        onMoveInput?.Invoke(inputAxis);
        
    }

    private void CheckSprintInput()
    {
        bool isHoldSprintInput = Input.GetKey(KeyCode.LeftShift) ||
                                      Input.GetKey(KeyCode.RightShift);
        if (isHoldSprintInput)
        {
            Debug.Log("Sprinting");
            onSprintInput?.Invoke(true);
        }
        else
        {
            Debug.Log("Not Sprinting");
            onSprintInput?.Invoke(false);
        }
    }

    private void CheckJumpInput()
    {
        bool isPressJumpInput = Input.GetKeyDown(KeyCode.Space);

        if (isPressJumpInput)
        {
            Debug.Log("Jump");
            onJumpInput?.Invoke();
        }
    }

    private void CheckCrouchInput()
    {
        bool isPressCrouchInput = Input.GetKeyDown(KeyCode.LeftControl) ||
                                    Input.GetKeyDown(KeyCode.RightControl);
        if (isPressCrouchInput)
        {
            Debug.Log("Crouch");
            onCrouchInput?.Invoke();
        }
    }

    private void CheckChangePOVInput()
    {
        bool isPressChangePOVInput = Input.GetKeyDown(KeyCode.Q);

        if (isPressChangePOVInput)
        {
            Debug.Log("Change POV");
            onChangePOV?.Invoke();
        }
    }

    private void CheckClimbInput()
    {
        bool isPressClimbInput = Input.GetKeyDown(KeyCode.E);

        if (isPressClimbInput)
        {
            Debug.Log("Climb");
            onClimbInput?.Invoke();
        }
    }

    private void CheckGlideInput()
    {
        bool isPressGlideInput = Input.GetKeyDown(KeyCode.G);

        if (isPressGlideInput)
        {
            Debug.Log("Glide");
            onGlideInput?.Invoke();
        }
    }

    private void CheckCancelInput()
    {
        bool isPressCancelInput = Input.GetKeyDown(KeyCode.C);

        if (isPressCancelInput)
        {
            Debug.Log("Cancel Climb or Glide");
            onCancelInput?.Invoke();
            onCancelGlide?.Invoke();
        }
    }

    private void CheckPunchInput()
    {
        bool isPressPunchInput = Input.GetKeyDown(KeyCode.Mouse0);

        if (isPressPunchInput)
        {
            Debug.Log("Punch");
            onPunchInput?.Invoke();
        }
    }

    private void CheckMainMenuInput()
    {
        bool isPressMainMenuInput = Input.GetKeyDown(KeyCode.Escape);

        if (isPressMainMenuInput)
        {
            Debug.Log("Back To Main Menu");
        }
    }

}
