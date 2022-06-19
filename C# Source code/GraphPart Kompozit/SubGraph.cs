using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGraph : IGraphPart
{
    public List<int> subParts;

    public SubGraph()
    {
        subParts = new List<int>();
    }

    public void Add(IGraphPart part)
    {
        if(part.GetGraphPartType() == GraphPartType.SUBGRAPH)
        {
            Debug.LogError("Trying add subgraph to graph");
        }

        if (subParts.Contains(part.GetID()))
        {
            return;
        }
        subParts.Add(part.GetID());
    }

    public void Remove(IGraphPart part)
    {
        if (!subParts.Remove(part.GetID())) //neni medzi subParts
        {
            foreach (int gp in subParts)
            {
                IDManager.GetGP(gp).Remove(part);
            }
        }
    }

    public void Clear()
    {
        subParts = new List<int>();
    }

    public void ChangeWidth(float widthValue)
    {
        foreach (int gp in subParts)
        {
            IDManager.GetGP(gp).ChangeWidth(widthValue);
        }
    }

    public IGraphPart GetGraphBeh()
    {
        return this;
    }

    public List<IGraphPart> GetGraphPartsInList()
    {
        List<IGraphPart> list = new List<IGraphPart>();
        foreach (int gp in subParts)
        {
            list.Add(IDManager.GetGP(gp));
        }
        return list;
    }

    public void Init(Vector3 spawnPosition, GraphPartType graphPartType)
    {
        throw new System.NotImplementedException();
    }

    public void MoveBy(Vector3 delta)
    {
        foreach (int gp in subParts)
        {
            IDManager.GetGP(gp).MoveBy(delta);
        }
    }

    public void MoveAt(Vector3 position)
    {
        foreach (int gp in subParts)
        {
            IDManager.GetGP(gp).MoveAt(position);
        }
    }

    public void Select()
    {
        foreach (int gp in subParts)
        {
            IDManager.GetGP(gp).Select();
        }
    }

    public void ExpandedSelect()
    {
        foreach (int gp in subParts)
        {
            IDManager.GetGP(gp).ExpandedSelect();
        }
    }

    public void UnSelect()
    {
        foreach (int gp in subParts)
        {
            IDManager.GetGP(gp).UnSelect();
        }
    }

    public bool isContainingGraphPart(IGraphPart part)
    {
        bool returnValue = false;
        foreach (int gp in subParts)
        {
            returnValue = returnValue || IDManager.GetGP(gp).isContainingGraphPart(part);
        }
        return returnValue;
    }

    public bool GetIsSelected()
    {
        return false;
    }
    public void StopMovement()
    {
        foreach (int gp in subParts)
        {
            IDManager.GetGP(gp).StopMovement();
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
        foreach (int gp in subParts)
        {
            if (IDManager.GetGP(gp).GetGraphPartType() == GraphPartType.EDGE_END)
                IDManager.GetGP(gp).UnSelect();
            else IDManager.GetGP(gp).Destroy();
        }
        subParts = new List<int>();
    }

    public float GetBiggestX()
    {
        float biggestX = float.MinValue;
        foreach (int gp in subParts)
        {
            float value = IDManager.GetGP(gp).GetBiggestX();
            if (value > biggestX)
            {
                biggestX = value;
            }
        }
        return biggestX;
    }

    public float GetSmallestX()
    {
        float smallestX = float.MaxValue;
        foreach (int gp in subParts)
        {
            float value = IDManager.GetGP(gp).GetSmallestX();
            if (value < smallestX)
            {
                smallestX = value;
            }
        }
        return smallestX;
    }

    public float GetBiggestY()
    {
        float biggestY = float.MinValue;
        foreach (int gp in subParts)
        {
            float value = IDManager.GetGP(gp).GetBiggestY();
            if (value > biggestY)
            {
                biggestY = value;
            }
        }
        return biggestY;
    }

    public float GetSmallestY()
    {
        float smallestY = float.MaxValue;
        foreach (int gp in subParts)
        {
            float value = IDManager.GetGP(gp).GetSmallestY();
            if (value < smallestY)
            {
                smallestY = value;
            }
        }
        return smallestY;
    }

    public Vector2 GetNumOfVerticesAndEdges()
    {
        Vector2 v2 = new Vector2(0,0);
        foreach (int gp in subParts)
        {
            v2 += IDManager.GetGP(gp).GetNumOfVerticesAndEdges();
        }
        return v2;
    }

    public List<Vertex> GetVertices()
    {
        List<Vertex> vertices = new List<Vertex>();
        foreach (int gp in subParts)
        {
            vertices.AddRange(IDManager.GetGP(gp).GetVertices());
        }
        return vertices;
    }

    public List<Edge> GetEdges()
    {
        List<Edge> edges = new List<Edge>();
        foreach (int gp in subParts)
        {
            edges.AddRange(IDManager.GetGP(gp).GetEdges());
        }
        return edges;
    }

    public int GetID()
    {
        throw new System.NotImplementedException();
    }

    public void SetID(int id)
    {
        throw new System.NotImplementedException();
    }
}
