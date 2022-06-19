using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour, IGraphPart
{
    public string Name;

    public int ID;

    private bool toStopMovement;

    private SpriteRenderer selectedSR;

    public List<int> edgeIDs = new List<int>();

    public bool isSelected;

    public Vector3 position { get { return transform.position;} }

    private void Awake()
    {
        selectedSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        isSelected = false;
        selectedSR.enabled = false;
    }

    private void LateUpdate()
    {
        if (toStopMovement)
        {
            List<int> edgeIDs = new List<int>(this.edgeIDs);
            foreach (int ee in edgeIDs)
            {
                ((EdgeEnd)IDManager.GetGP(ee)).StopMovement();
            }
            toStopMovement = false;
        }   
    }

    public int GetID()
    {
        return ID;
    }
    public void SetID(int id)
    {
        ID = id;
    }

    public void ChangeWidth(float widthValue)
    {
        transform.localScale = new Vector3(widthValue,widthValue,1);
    }

    public void MoveBy(Vector3 delta)
    {
        transform.position += delta; 
        foreach (int edge in edgeIDs)
        {
            IDManager.GetGP(edge).MoveBy(delta);
        }
    }

    public void MoveAt(Vector3 position)
    {
        transform.position = position;
        foreach (int edge in edgeIDs)
        {
            (IDManager.GetGP(edge)).MoveAt(position);
        }
    }

    public void Select()
    {
        selectedSR.enabled = true;
        isSelected = true;
        foreach (int edge in edgeIDs)
        {
            (IDManager.GetGP(edge)).Select();
        }
    }

    public void UnSelect()
    {
        isSelected = false;
        selectedSR.enabled = false;
        foreach (int edge in edgeIDs)
        {
            IDManager.GetGP(edge).UnSelect();
        }
    }

    public IGraphPart GetGraphBeh() { return this; }
    public void Remove(IGraphPart part) { return; }
    public bool isContainingGraphPart(IGraphPart part) { return (IGraphPart)this == part; }
    public bool GetIsSelected() { return isSelected; }
    public GraphPartType GetGraphPartType() { return GraphPartType.VERTEX; }

    public void AddEdge(int edgeEnd)
    {
        if (edgeIDs.Contains(edgeEnd))
            return;
        edgeIDs.Add(edgeEnd);
    }

    public void RemoveEdge(int edgeEnd)
    {
        edgeIDs.Remove(edgeEnd);
    }

    public List<Vertex> outgoingEdgesDestination()
    {
        List<Vertex> outgoingEdges = new List<Vertex>();
        foreach (int e in edgeIDs)
        {
            Vertex v = ((EdgeEnd)IDManager.GetGP(e)).GetOtherVertex(this);
            outgoingEdges.Add(v);
        }
        return outgoingEdges;
    }

    public int GetDegree() { return outgoingEdgesDestination().Count; }

    public void ExpandedSelect()
    {
        Select();
        foreach (int e in edgeIDs)
        {
            ((EdgeEnd)IDManager.GetGP(e)).ExpandedSelect();
        }
    }

    public Vector3 GetWorldPos()
    {
        return transform.position;
    }
    public void StopMovement()
    {
        toStopMovement = true;
    }

    public void Destroy()
    {
        isSelected = false;
        GetComponent<Collider2D>().enabled = false;
        List<int> es = new List<int>(edgeIDs);
        foreach (int e in es)
        {
            ((EdgeEnd)IDManager.GetGP(e)).UnSelect();
            ((EdgeEnd)IDManager.GetGP(e)).RemoveVertex();
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

    public Vector2 GetNumOfVerticesAndEdges()
    {
        return new Vector2(1, 0);
    }

    public List<Vertex> GetVertices()
    {
        List<Vertex> vertices = new List<Vertex>();
        vertices.Add(this);
        return vertices;
    }

    public List<Edge> GetEdges()
    {
        return new List<Edge>();
    }
}

