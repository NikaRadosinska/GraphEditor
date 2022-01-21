using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddControlStrategy : AbstractControlStrategy
{
    private EdgeEnd lockedEdgeEnd = null; 
    private bool toCreate = false;
    private Vertex v;

    public AddControlStrategy(GraphManager graphManager) : base(graphManager){}

    public override IGraphPart OnClick(Vector2 mousePos)
    {
        graphManager.RemoveAllFromSelected();
        IGraphPart part = base.OnClick(mousePos);
        if (part == null)
        {
            return graphManager.CreateVertex(mousePos);
        }
        else if (part.GetGraphPartType() == GraphPartType.VERTEX)
        {
            v = (Vertex)part;
            toCreate = true;
        }
        return part; 
    }

    public override void OnHold(Vector2 delta, Vector2 mousPos)
    {
        if (toCreate)
        {
            Edge e = graphManager.CreateEdgeBeginning(v);
            lockedEdgeEnd = e.rightEdgeEnd;
            lastSelected = e.rightEdgeEnd;
            graphManager.AddToSelected(e.rightEdgeEnd);
            toCreate = false;
        }

        if (lockedEdgeEnd == null)
            return;
        lockedEdgeEnd.MoveAt(mousPos);
        Debug.Log("MovingLockedEdge" + lockedEdgeEnd.name);
    }

    public override void OnHoldEnd(Vector2 mousePos)
    {
        IGraphPart v = base.OnClick(mousePos);
        if (v == null)
        {
            lockedEdgeEnd = null;
            return;
        }
        if(v.GetGraphPartType() == GraphPartType.VERTEX && lockedEdgeEnd != null)
        {
            Vertex vertex = (Vertex)v;
            lockedEdgeEnd.AttachVertex(vertex);
            vertex.AddEdge(lockedEdgeEnd);
        }
        lockedEdgeEnd = null;
    }
}
