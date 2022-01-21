using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour, IGraphPart
{
    private SpriteRenderer selectedSR;

    public List<EdgeEnd> edges = new List<EdgeEnd>();

    public bool isSelected;

    public Vector3 position { get { return transform.position;} }

    private void Awake()
    {
        selectedSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        UnSelect();
    }

    public void ChangeWidth(float widthValue)
    {
        transform.localScale = new Vector3(widthValue,widthValue,1);
    }

    public void Init(Vector3 spawnPosition, GraphPartType graphPartType)
    {
        /////////////////////////////////////
    }

    public void MoveBy(Vector3 delta)
    {
        transform.position += delta; 
        foreach (EdgeEnd edge in edges)
        {
            edge.MoveBy(delta);
        }
    }

    public void MoveAt(Vector3 position)
    {
        transform.position = position;
        foreach (EdgeEnd edge in edges)
        {
            edge.MoveAt(position);
        }
    }

    public void Select()
    {
        selectedSR.enabled = true;
        isSelected = true;
        foreach (EdgeEnd edge in edges)
        {
            edge.Select();
        }
    }

    public void UnSelect()
    {
        isSelected = false;
        selectedSR.enabled = false;
        foreach (EdgeEnd edge in edges)
        {
            edge.UnSelect();
        }
    }

    public IGraphPart GetGraphBeh() { return this; }
    public void Remove(IGraphPart part) { return; }
    public bool isContainingGraphPart(IGraphPart part) { return (IGraphPart)this == part; }
    public bool GetIsSelected() { return isSelected; }
    public GraphPartType GetGraphPartType() { return GraphPartType.VERTEX; }

    public void AddEdge(EdgeEnd edgeEnd)
    {
        if (edges.Contains(edgeEnd))
            return;
        edges.Add(edgeEnd);
    }

    public void RemoveEdge(EdgeEnd edgeEnd)
    {
        edges.Remove(edgeEnd);
    }

    public List<Vertex> outgoingEdgesDestination()
    {
        List<Vertex> outgoingEdges = new List<Vertex>();
        foreach (EdgeEnd e in edges)
        {
            Vertex v = e.GetOtherVertex(this);
            if (v == null)
                continue;
            outgoingEdges.Add(v);
        }
        return outgoingEdges;
    }

    public int GetDegree() { return outgoingEdgesDestination().Count; }

    public void ExpandedSelect()
    {
        Select();
        foreach (EdgeEnd e in edges)
        {
            e.ExpandedSelect();
        }
    }

    public Vector3 GetWorldPos()
    {
        return transform.position;
    }
    public void StopMovement()
    {
        return;
    }

    public void Destroy()
    {
        isSelected = false;
        GetComponent<Collider2D>().enabled = false;
        List<EdgeEnd> es = new List<EdgeEnd>(edges);
        foreach (EdgeEnd e in es)
        {
            e.UnSelect();
            e.RemoveVertex();
        }
        Destroy(gameObject);
    }

    public float GetBiggestX()
    {
        return transform.position.x;
    }

    public float GetSmallestX()
    {
        return transform.position.x;
    }

    public float GetBiggestY()
    {
        return transform.position.y;
    }

    public float GetSmallestY()
    {
        return transform.position.y;
    }
}

