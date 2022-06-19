using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllInSelectedCommand : CommandCommonInfo, ICommand
{
    private SubGraphInfoWithIds subGraphInfo;
    private Vector3 offset;

    private GraphManager GraphManager;

    public DestroyAllInSelectedCommand(GraphManager graphManager)
    {
        subGraphInfo = new SubGraphInfoWithIds(selected);
        offset = selected.GetWorldPos();
        GraphManager = graphManager;
    }

    public bool Execute()
    {
        //znic
        foreach (IGraphPart gp in selected.GetGraphPartsInList())
        {
            allEdges.Remove(gp);
            allVertices.Remove(gp);
        }
        selected.Destroy();
        selected.Clear();
        //delete record
        return true;
    }

    public bool Redo()
    {
        Execute();
        return true;
    }

    public bool Undo()
    {
        //vytvor
        SubGraph subGraph = new SubGraph();
        for (int i = 0; i < subGraphInfo.vertices.Count; i++)
        {
            Vector3 v3 = offset + subGraphInfo.vertices[i];
            Vertex v = GraphManager.CreateVertex(v3, subGraphInfo.verticesIDs[i]);
            subGraph.Add(v);
        }
        foreach (EdgeInfoWithIds edgeInfo in subGraphInfo.edges)
        {
            Edge e = GraphManager.CreateEdge(edgeInfo, offset);
            subGraph.Add(e);
            subGraph.Add(((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)));
            subGraph.Add(((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd)));
        }
        GraphManager.RemoveAllFromSelected();
        foreach(IGraphPart part in subGraph.GetGraphPartsInList())
        {
            if (part.GetGraphPartType() == GraphPartType.EDGE_END) continue;
            GraphManager.AddToSelected(part);
        }
        selected.StopMovement();
        allEdges.StopMovement();
        return true;
    }
}
