using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SubGraphInfo
{
    public List<Vector3> vertices;
    public List<EdgeInfo> edges;
    public List<Vector2> subGraphForOutput;

    public SubGraphInfo(SubGraph subGraph)
    {
        Vector3 middleVector = subGraph.GetWorldPos();
        vertices = new List<Vector3>();
        edges = new List<EdgeInfo>();

        //Debug.Log("middle point: " + middleVector);

        foreach (Vertex v in subGraph.GetVertices())
        {
            Vector3 v3 = v.GetWorldPos() - middleVector;
            //Debug.Log("vertex: " + v3);
            vertices.Add(v3);
        }

        foreach (Edge e in subGraph.GetEdges())
        {
            Vector3 leftEdgeEndPos = ((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)).GetWorldPos() - middleVector;
            Vector3 middlePos = e.GetWorldPos() - middleVector;
            Vector3 rightEdgeEndPos = ((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd)).GetWorldPos() - middleVector;
            edges.Add(new EdgeInfo(e.IsLocked(), leftEdgeEndPos, middlePos, rightEdgeEndPos));
        }

        List<Vertex> allVertices = subGraph.GetVertices();

        subGraphForOutput = new List<Vector2>();

        for (int i = 0; i < allVertices.Count; i++)
        {
            foreach (Vertex v2 in allVertices[i].outgoingEdgesDestination())
            {
                subGraphForOutput.Add(new Vector2(i,allVertices.IndexOf(v2)));
            }
        }
    }
}
