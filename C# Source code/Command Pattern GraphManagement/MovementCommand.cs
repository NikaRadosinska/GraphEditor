using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCommand : CommandCommonInfo, ICommand
{

    private List<Vector3> prevPositions;
    private List<Vector3> nextPositions;
    private List<int> IDs;

    public MovementCommand(List<Vector3> prevPositions)
    {
        this.prevPositions = prevPositions;
        nextPositions = new List<Vector3>();
        IDs = new List<int>();
        foreach (IGraphPart gp in selected.GetGraphPartsInList())
        {
            nextPositions.Add(gp.GetWorldPos());
            IDs.Add(gp.GetID());
        }
    }

    public bool Execute()
    {
        selected.StopMovement();
        return true;
    }

    public bool Redo()
    {
        for (int i = 0; i < IDs.Count; i++)
        {
            IDManager.GetGP(IDs[i]).MoveAt(nextPositions[i]);
        }
        selected.StopMovement();
        return true;
    }

    public bool Undo()
    {
        for (int i = 0; i < IDs.Count; i++)
        {
            IDManager.GetGP(IDs[i]).MoveAt(prevPositions[i]);
        }
        selected.StopMovement();
        return true;
    }
}
