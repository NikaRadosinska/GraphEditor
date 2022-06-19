using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateVertexCommand : CommandCommonInfo, ICommand
{

    private Vector3 pos;
    private GraphManager graph;

    private int ID;
    private Vertex v;


    public CreateVertexCommand(Vector3 pos, GraphManager graph)
    {
        this.pos = pos;
        this.graph = graph;
    }

    public CreateVertexCommand(Vector3 pos, GraphManager graph, int ID)
    {
        this.pos = pos;
        this.graph = graph;
        this.ID = ID;
    }

    public bool Execute()
    {
        v = Instantiate(vertexPrefab, vertexParent.transform);
        if(ID == 0) ID = IDManager.SetId(v);
        IDManager.SetById(v, ID);
        allVertices.Add(v);
        v.MoveAt(new Vector3(pos.x, pos.y, 0));
        //graph.AddToSelected(v);
        return true;
    }

    public Vertex GetCreated()
    {
        return v;
    }

    public bool Redo()
    {
        v = Instantiate(vertexPrefab, vertexParent.transform);
        IDManager.SetById(v, ID);
        allVertices.Add(v);
        v.MoveAt(new Vector3(pos.x, pos.y, 0));
        //graph.AddToSelected(v);
        return true;
    }

    public bool Undo()
    {
        IGraphPart v = IDManager.GetGP(ID);
        allVertices.Remove(v);
        //v.UnSelect();
        v.Destroy();
        IDManager.DeleteRecord(ID);
        return true;
    }
}
