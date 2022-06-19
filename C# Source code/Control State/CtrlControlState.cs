using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ExtentionMethods;

public class CtrlControlState : AbstractState, IAbstractState
{

    bool setToClassicControlState = false;

    bool toStartMovement = false;

    bool toRemoveAll = false;
    bool toRemoveSelected = false;

    public IGraphPart LeftMouseButtonClick(Vector2 mousePos)
    {
        IGraphPart selectedGP = MyExtentions.GetGraphObjectUnderCursor(mousePos);
        toRemoveSelected = false;
        toStartMovement = true;
        if (selectedGP == null)
        {
            toRemoveAll = true;
            collectorPrefab.gameObject.SetActive(true);
            collectorPrefab.Init(mousePos.x, mousePos.y);
            return collectorPrefab.subParts;
        }

        if (selectedGP.GetIsSelected())
        {
            isGraphPartMoving = true;
            toRemoveSelected = true;
            lastSelected = selectedGP;
            return selectedGP;
        }

        lastSelected = selectedGP;
        GraphManager.AddToSelected(selectedGP);
        return selectedGP;
    }

    public void MouseMovement(InputAction.CallbackContext context)
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 delta = mousePos - (Vector2)prevMouseWorldPos;
        toRemoveAll = false;
        toRemoveSelected = false;

        if (collectorPrefab.IsEnabled())
        {
            collectorPrefab.UpdatePosition(mousePos.x, mousePos.y);
        } 

        if(isGraphPartMoving)
        {
            if (toStartMovement)
            {
                GraphManager.StartMovement();
                toStartMovement = false;
            }
            GraphManager.MoveAllInSelectedByVector(delta);
        }
        MouseMovement(context, this);
    }

    public void LeftMouseButtonReleased(InputAction.CallbackContext context)
    {
        toStartMovement = false;
        GraphManager.StopMovement(!toRemoveSelected && !toRemoveAll && isGraphPartMoving, null);
        isGraphPartMoving = false;
        Vector2 mousPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (toRemoveSelected)
        {
            GraphManager.RemoveFromSelected(lastSelected);
        }
        else if (toRemoveAll)
        {
            GraphManager.RemoveAllFromSelected();
        }
        
        if (collectorPrefab.IsEnabled())
        {
            collectorPrefab.Disable();
            collectorPrefab.gameObject.SetActive(false);
        }

        toRemoveSelected = false;
        toRemoveAll = false;

        if (setToClassicControlState)
        {
            collectorPrefab.Disable();
            collectorPrefab.gameObject.SetActive(false);
            Settings.ChangeState(ControlStateType.CLASSIC);
        }

        GraphManager.Checkpoint();
    }

    public void WheelButtonPressed()
    {
        isGraphPartMoving = true;
    }

    public void WheelButtonReleased()
    {
        isGraphPartMoving = false;
    }

    public void DeleteButtonPressedDown()
    {
        GraphManager.DestroyAllSelected();
    }

    public void CtrlPressed()
    {
        return;
    }

    public void CtrlReleased()
    {
        if (collectorPrefab.IsEnabled())
        {
            setToClassicControlState = true;
            return;
        }

        Settings.ChangeState(ControlStateType.CLASSIC);
    }

    public void ShiftPressed()
    {
        return;
    }

    public void ShiftReleased()
    {
        return;
    }

    public void CtrlC()
    {
        if (isGraphPartMoving || isCameraMoving) return;
        Settings.ChangeStateOnHold();
    }

    public ControlStateType GetType()
    {
        return ControlStateType.CTRL;
    }

    public void ToAddPressed()
    {
        return;
    }

    public void ToAddReleased()
    {
        return;
    }
    public void CtrlZ()
    {
        Debug.Log("CtrlZ");
        GraphManager.Undo();
    }

    public void CtrlY()
    {
        Debug.Log("CtrlY");
        GraphManager.Redo();
    }

    public void CtrlS()
    {
        Settings.SaveSubGraph();
    }
}
