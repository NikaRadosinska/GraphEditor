using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtentionMethods;
using UnityEngine.InputSystem;

public class ClassicControlState : AbstractState, IAbstractState
{
    private bool toStartMovement = false;

    public void CtrlC()
    {
        Settings.ChangeStateOnHold();
    }

    public void CtrlPressed()
    {
        if (isCameraMoving || isGraphPartMoving)
            return;

        Settings.ChangeState(ControlStateType.CTRL);
    }

    public void CtrlReleased()
    {
        return;
    }

    public void DeleteButtonPressedDown()
    {
        GraphManager.DestroyAllSelected();
        GraphManager.Checkpoint();
    }

    public IGraphPart LeftMouseButtonClick(Vector2 mousePos)
    {
        IGraphPart selectedGP = MyExtentions.GetGraphObjectUnderCursor(mousePos);
        toStartMovement = true;
        if (selectedGP == null)
        {
            lastSelected = null;
            toRemoveSelection = true;
            isCameraMoving = true;
            return null;
        }

        if (selectedGP.GetIsSelected())
        {
            toRemoveSelection = true;
        }
        
        isGraphPartMoving = true;

        GraphManager.RemoveAllFromSelected();
        lastSelected = selectedGP;
        GraphManager.AddToSelected(selectedGP);
        return selectedGP;
    }

    public void LeftMouseButtonReleased(InputAction.CallbackContext context)
    {
        toStartMovement = false;
        isCameraMoving = false;
        GraphManager.StopMovement(isGraphPartMoving && !toRemoveSelection, null);
        if (lastSelected != null && lastSelected.GetGraphPartType() == GraphPartType.EDGE_END) GraphManager.RemoveFromSelected(lastSelected);
        isGraphPartMoving = false;

        if (toRemoveSelection)
        {
            GraphManager.RemoveAllFromSelected();
            toRemoveSelection = false;
        }
        GraphManager.Checkpoint();
    }

    public void MouseMovement(InputAction.CallbackContext context)
    {
        Vector2 mousPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        toRemoveSelection = false;
        if (isGraphPartMoving)
        {
            if (toStartMovement)
            {
                GraphManager.StartMovement();
                toStartMovement = false;
            }
            GraphManager.MoveAllInSelectedOnVector(mousPos);
        }

        base.MouseMovement(context, this);
    }

    public void ShiftReleased()
    {
        return;
    }

    public void ShiftPressed()
    {
        if (isCameraMoving || isGraphPartMoving)
            return;

        Settings.ChangeState(ControlStateType.SHIFT);
    }

    public void ToAddPressed()
    {
        if (isCameraMoving || isGraphPartMoving)
            return;

        Settings.ChangeState(ControlStateType.ADD);
    }

    public void ToAddReleased()
    {
        return;
    }

    public void WheelButtonPressed()
    {
        if (isGraphPartMoving)
            return;

        isCameraMoving = true;
    }

    public void WheelButtonReleased()
    {
        isCameraMoving = false;
    }

    public ControlStateType GetType()
    {
        return ControlStateType.CLASSIC;
    }
    public void CtrlZ()
    {
        return;
    }

    public void CtrlY()
    {
        return;
    }

    public void CtrlS()
    {
        return;
    }
}
