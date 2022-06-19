using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtentionMethods;
using UnityEngine.InputSystem;

public class HoldControlState : AbstractState, IAbstractState
{
    SubGraph subGraph;

    bool toDrop = false;

    public HoldControlState(SubGraphInfo subGraphInfo){

        subGraph = new SubGraph();
        //Debug.Log("create middle point: " + prevMouseWorldPos);
        foreach (Vector3 vectorPos in subGraphInfo.vertices)
        {
            Vector3 v3 = prevMouseWorldPos + vectorPos;
            subGraph.Add(GraphManager.CreateVertex(v3));
        }
        foreach (EdgeInfo edgeInfo in subGraphInfo.edges)
        {
            Edge e = GraphManager.CreateEdge(edgeInfo, prevMouseWorldPos);
            subGraph.Add(e);
            subGraph.Add(((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)));
            subGraph.Add(((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd)));

        }

        GraphManager.RemoveAllFromSelected();
        
        foreach (IGraphPart part in subGraph.GetGraphPartsInList())
        {
            if (part.GetGraphPartType() == GraphPartType.EDGE_END) continue;
            GraphManager.AddToSelected(part);
        }
        GraphManager.StartMovement();

    }

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
        //isGraphPartMoving = true;
        IGraphPart clickedOn = MyExtentions.GetGraphObjectUnderCursor(mousePos);
        toDrop = true;
        isCameraMoving = true;
        return clickedOn;
    }

    public void LeftMouseButtonReleased(InputAction.CallbackContext context)
    {
        isCameraMoving=false;
        //isGraphPartMoving = false;
        if (toDrop)
        {
            toDrop = false;
            GraphManager.StopMovement(true, null);
            Settings.ChangeState(ControlStateType.CLASSIC);
        }
        GraphManager.Checkpoint();
    }

    public void MouseMovement(InputAction.CallbackContext context)
    {
        toDrop = false;
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 delta = mousePos - (Vector2)prevMouseWorldPos;
        subGraph.MoveBy(delta);
        MouseMovement(context, this);
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
        isGraphPartMoving = true;
    }

    public void WheelButtonReleased()
    {
        isGraphPartMoving = false;
    }

    public ControlStateType GetType()
    {
        return ControlStateType.HOLD;
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
