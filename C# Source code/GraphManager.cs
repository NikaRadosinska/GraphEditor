using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GraphManager : MainBehaviour
{
    public Vertex vertexPrefab;
    public GameObject edgePrefab;

    private SubGraph selected;
    private SubGraph allEdges;
    private SubGraph allVertices;

    public Transform vertexParent;
    public Transform edgesParent;

    private void Start()
    {
        selected = new SubGraph();
        allEdges = new SubGraph();
        allVertices = new SubGraph();
    }

    public void AddToSelected(IGraphPart part)
    {
        if (part.GetIsSelected())
            return;
        selected.Add(part);
        part.Select();
    }

    public void RemoveFromSelected(IGraphPart part)
    {
        selected.Remove(part);
        part.UnSelect();
    }

    public bool IsInSelected(IGraphPart part)
    {
        return selected.isContainingGraphPart(part);
    }

    public void RemoveAllFromSelected()
    {
        selected.UnSelect();
        selected.Clear();
    }

    public void MoveAllInSelectedByVector(Vector2 delta)
    {
        selected.MoveBy(delta);
    }

    public void MoveAllInSelectedOnVector(Vector2 vector)
    {
        selected.MoveAt(vector);
    }

    public void ChangeWidthForAllEdges(float width)
    {
        foreach (IGraphPart gp in allEdges.subParts)
        {
            gp.ChangeWidth(width);
        }
    }

    public void ChangeWidthForAllVertices(float width)
    {
        foreach (IGraphPart gp in allVertices.subParts)
        {
            gp.ChangeWidth(width);
        }
        foreach (IGraphPart gp in allEdges.subParts)
        {
            ((Edge)gp).ChangeEdgeDistance(width);
        }
    }

    public void ControlEdgesVertices(Vector2 vector)
    {
        for (int i = 0; i < edgesParent.transform.childCount; i++)
        {
            edgesParent.transform.GetChild(i).GetComponent<Edge>().ControlVertices();
        }
    }

    public void StopMovement(InputAction.CallbackContext context)
    {
        selected.StopMovement();
    }

    public void DestroyAllSelected()
    {
        foreach (IGraphPart gp in selected.subParts)
        {
            allEdges.Remove(gp);
            allVertices.Remove(gp);
        }
        selected.Destroy();
        selected.UnSelect();
    }

    public string GetAllGraphInfo()
    {
        string s = "";

        foreach (IGraphPart v in allVertices.subParts)
        {
            s += allVertices.subParts.IndexOf(v) + ":";
            foreach (Vertex v2 in ((Vertex)v).outgoingEdgesDestination())
            {
                s += " " + allVertices.subParts.IndexOf(v2);
            }
            s += "\n";
        }
        return s;
    }

    public Vertex CreateVertex(Vector3 mousePos)
    {
        Vertex v = Instantiate(vertexPrefab, vertexParent.transform);
        allVertices.Add(v);
        v.MoveAt(mousePos);
        AddToSelected(v);
        return v;
    }

    public Edge CreateEdge(EdgeInfo edgeInfo, Vector3 pivot)
    {
        Edge e = Instantiate(edgePrefab, edgesParent).transform.GetChild(0).GetComponent<Edge>();
        allEdges.Add(e);
        e.leftEdgeEnd.MoveAt(edgeInfo.leftEdgeEndPos + pivot);
        e.rightEdgeEnd.MoveAt(edgeInfo.rightEdgeEndPos + pivot);
        if (!edgeInfo.isLocked)
            e.MoveAt(edgeInfo.middlePos + pivot);
        AddToSelected(e);
        return e;
    }

    public Edge CreateEdgeBeginning(Vertex vertex)
    {
        Edge e = Instantiate(edgePrefab, edgesParent.transform).transform.GetChild(0).GetComponent<Edge>();
        allEdges.Add(e);
        e.leftVertex = vertex;
        e.leftEdgeEnd.MoveAt(vertex.GetWorldPos());
        vertex.AddEdge(e.leftEdgeEnd);
        AddToSelected(e);
        return e;
    }
}
