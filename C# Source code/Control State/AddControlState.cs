using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtentionMethods;
using UnityEngine.InputSystem;

public class AddControlState : AbstractState, IAbstractState
{
    private EdgeEnd lockedEdgeEnd = null; 
    private bool toCreateEdge = false;
    private bool toCreateVertex = false;
    private Vertex v;
    private bool changeToClassic;

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

    public void DeleteButtonPressedDown()
    {
        return;
    }

    public IGraphPart LeftMouseButtonClick(Vector2 mousePos)
    {
        GraphManager.RemoveAllFromSelected();
        IGraphPart part = MyExtentions.GetGraphObjectUnderCursor(mousePos);

        if (part == null)
        {
            toCreateVertex = true;
            return null;
        }
        else if (part.GetGraphPartType() == GraphPartType.VERTEX)
        {
            v = (Vertex)part;
            toCreateEdge = true;
        }
        return part; 
    }

    public void MouseMovement(InputAction.CallbackContext context)
    {
        Vector2 mousPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        toCreateVertex = false;
        /*
        if (isGraphPartMoving)
        {
            toRemoveSelection = false;
            GraphManager.MoveAllInSelectedOnVector(mousPos);
        }
        */

        if (toCreateEdge)
        {
            Edge e = GraphManager.CreateEdgeBeginning(v);
            lockedEdgeEnd = ((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd));
            lastSelected = e;
            //GraphManager.AddToSelected(((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)));
            toCreateEdge = false;
        }

        if (lockedEdgeEnd == null)
            return;

        lockedEdgeEnd.MoveAt(mousPos);

    }

    public void LeftMouseButtonReleased(InputAction.CallbackContext context)
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        GraphManager.StopMovement(false, (lockedEdgeEnd != null) ? lockedEdgeEnd.GetMid() : null);
        toCreateEdge=false;
        //isGraphPartMoving = false;
        isCameraMoving = false;
        if (toCreateVertex)
        {
            toCreateVertex = false;
            Vertex vLocal = GraphManager.CreateVertex(mousePos);
            GraphManager.AddToSelected(vLocal);
            GraphManager.Checkpoint();
            return;
        }

        IGraphPart v = MyExtentions.GetGraphObjectUnderCursor(mousePos);
        if (v == null)
        {
            lockedEdgeEnd = null;
            GraphManager.Checkpoint();
            return;
        }
        if(v.GetGraphPartType() == GraphPartType.VERTEX && lockedEdgeEnd != null)
        {
            Vertex vertex = (Vertex)v;
            lockedEdgeEnd.AttachVertex(vertex);
            vertex.AddEdge(lockedEdgeEnd.GetID());
            GraphManager.RemoveFromSelected(lockedEdgeEnd);
        }
        lockedEdgeEnd = null;
        GraphManager.Checkpoint();
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
        if (changeToClassic)
        {
            changeToClassic = false;
            Settings.ChangeState(ControlStateType.CLASSIC);
        }
        else
        {
            changeToClassic = true;
        }
    }

    public void WheelButtonPressed()
    {
        if (lockedEdgeEnd != null)
            return;
        isCameraMoving = true;
    }

    public void WheelButtonReleased()
    {
        isCameraMoving = false;
    }

    public ControlStateType GetType()
    {
        return ControlStateType.ADD;
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
        return;
    }
}
