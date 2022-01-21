using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGraph : IGraphPart
{
    public List<IGraphPart> subParts;

    public SubGraph()
    {
        subParts = new List<IGraphPart>();
    }

    public void Add(IGraphPart part)
    {
        subParts.Add(part);
    }

    public void Remove(IGraphPart part)
    {
        if (!subParts.Remove(part)) //neni medzi subParts
        {
            foreach (IGraphPart gp in subParts)
            {
                gp.Remove(part);
            }
        }
    }

    public void Clear()
    {
        subParts = new List<IGraphPart>();
    }

    public void ChangeWidth(float widthValue)
    {
        foreach (IGraphPart gp in subParts)
        {
            gp.ChangeWidth(widthValue);
        }
    }

    public IGraphPart GetGraphBeh()
    {
        return this;
    }

    public List<IGraphPart> GetGraphPartsInList()
    {
        return subParts;
    }

    public void Init(Vector3 spawnPosition, GraphPartType graphPartType)
    {
        throw new System.NotImplementedException();
    }

    public void MoveBy(Vector3 delta)
    {
        foreach (IGraphPart gp in subParts)
        {
            gp.MoveBy(delta);
        }
    }

    public void MoveAt(Vector3 position)
    {
        foreach (IGraphPart gp in subParts)
        {
            gp.MoveAt(position);
        }
    }

    public void Select()
    {
        foreach (IGraphPart gp in subParts)
        {
            gp.Select();
        }
    }

    public void ExpandedSelect()
    {
        foreach (IGraphPart gp in subParts)
        {
            gp.ExpandedSelect();
        }
    }

    public void UnSelect()
    {
        foreach (IGraphPart gp in subParts)
        {
            gp.UnSelect();
        }
    }

    public bool isContainingGraphPart(IGraphPart part)
    {
        bool returnValue = false;
        foreach (IGraphPart gp in subParts)
        {
            returnValue = returnValue || gp.isContainingGraphPart(part);
        }
        return returnValue;
    }

    public bool GetIsSelected()
    {
        return false;
    }
    public void StopMovement()
    {
        foreach (IGraphPart gp in subParts)
        {
            gp.StopMovement();
        }
    }

    public GraphPartType GetGraphPartType()
    {
        return GraphPartType.SUBGRAPH;
    }

    public Vector3 GetWorldPos()
    {
        return new Vector3((GetSmallestX() + GetBiggestX()) / 2f, (GetSmallestY() + GetBiggestY()) / 2f, 0);
    }

    public void Destroy()
    {
        foreach (IGraphPart gp in subParts)
        {
            if (gp.GetGraphPartType() == GraphPartType.EDGE_END)
                gp.UnSelect();
            gp.Destroy();
        }
        subParts = new List<IGraphPart>();
    }

    public float GetBiggestX()
    {
        float biggestX = float.MinValue;
        foreach (IGraphPart gp in subParts)
        {
            if (gp.GetBiggestX() > biggestX)
            {
                biggestX = gp.GetBiggestX();
            }
        }
        return biggestX;
    }

    public float GetSmallestX()
    {
        float smallestX = float.MaxValue;
        foreach (IGraphPart gp in subParts)
        {
            if (gp.GetSmallestX() < smallestX)
            {
                smallestX = gp.GetBiggestX();
            }
        }
        return smallestX;
    }

    public float GetBiggestY()
    {
        float biggestY = float.MinValue;
        foreach (IGraphPart gp in subParts)
        {
            if (gp.GetBiggestY() > biggestY)
            {
                biggestY = gp.GetBiggestX();
            }
        }
        return biggestY;
    }

    public float GetSmallestY()
    {
        float smallestY = float.MaxValue;
        foreach (IGraphPart gp in subParts)
        {
            if (gp.GetSmallestY() < smallestY)
            {
                smallestY = gp.GetBiggestX();
            }
        }
        return smallestY;
    }
}
