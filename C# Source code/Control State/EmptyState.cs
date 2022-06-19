using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EmptyState : IAbstractState
{
    public void CtrlC()
    {
        return;
    }

    public void CtrlPressed()
    {
        return;
    }

    public void CtrlReleased()
    {
        return;
    }

    public void CtrlS()
    {
        return;
    }

    public void CtrlY()
    {
        return;
    }

    public void CtrlZ()
    {
        return;
    }

    public void DeleteButtonPressedDown()
    {
        return;
    }

    public IGraphPart LeftMouseButtonClick(Vector2 mousePos)
    {
        return null;
    }

    public void LeftMouseButtonReleased(InputAction.CallbackContext context)
    {
        return;
    }

    public void MouseMovement(InputAction.CallbackContext context)
    {
        return;
    }

    public void Scroll(InputAction.CallbackContext context)
    {
        return;
    }

    public void ShiftPressed()
    {
        return;
    }

    public void ShiftReleased()
    {
        return;
    }

    public void ToAddPressed()
    {
        return;
    }

    public void ToAddReleased()
    {
        return;
    }

    public void WheelButtonPressed()
    {
        return;
    }

    public void WheelButtonReleased()
    {
        return;
    }

    ControlStateType IAbstractState.GetType()
    {
        return ControlStateType.EMPTY;
    }
}
