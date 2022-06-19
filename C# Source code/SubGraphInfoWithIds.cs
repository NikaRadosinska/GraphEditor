using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SubGraphInfoWithIds
{
    public List<Vector3> vertices;
    public List<int> verticesIDs;
    public List<EdgeInfoWithIds> edges;

    public SubGraphInfoWithIds(SubGraph subGraph)
    {
        Vector3 middleVector = subGraph.GetWorldPos();
        vertices = new List<Vector3>();
        edges = new List<EdgeInfoWithIds>();
        verticesIDs = new List<int>();

        //Debug.Log("middle point: " + middleVector);

        foreach (Vertex v in subGraph.GetVertices())
        {
            Vector3 v3 = v.GetWorldPos() - middleVector;
            //Debug.Log("vertex: " + v3);
            vertices.Add(v3);
            verticesIDs.Add(v.GetID());
        }

        foreach(Edge e in subGraph.GetEdges())
        {
            Vector3 leftEdgeEndPos = ((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)).GetWorldPos() - middleVector;
            Vector3 middlePos = e.GetWorldPos() - middleVector;
            Vector3 rightEdgeEndPos = ((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd)).GetWorldPos() - middleVector;
            edges.Add(new EdgeInfoWithIds(e.IsLocked(),leftEdgeEndPos, middlePos, rightEdgeEndPos, e.leftEdgeEnd, e.GetID(), e.rightEdgeEnd));
        }
    }
}
