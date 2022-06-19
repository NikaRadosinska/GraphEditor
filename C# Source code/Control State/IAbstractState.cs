using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IAbstractState
{
    public void WheelButtonPressed();
    public void WheelButtonReleased();
    public IGraphPart LeftMouseButtonClick(Vector2 mousePos);
    public void LeftMouseButtonReleased(InputAction.CallbackContext context);
    public void MouseMovement(InputAction.CallbackContext context);
    public void Scroll(InputAction.CallbackContext context);
    public void DeleteButtonPressedDown();
    public void CtrlPressed();
    public void CtrlReleased();
    public void ShiftPressed();
    public void ShiftReleased();
    public void ToAddPressed();
    public void ToAddReleased();
    public void CtrlC();
    public void CtrlZ();
    public void CtrlY();
    public void CtrlS();
    public ControlStateType GetType();
}
