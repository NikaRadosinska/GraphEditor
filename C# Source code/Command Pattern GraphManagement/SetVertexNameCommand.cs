using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVertexNameCommand : CommandCommonInfo, ICommand
{

    private string prevName;
    private string nextName;
    private int id;

    public SetVertexNameCommand(Vertex vertex, string name)
    {
        prevName = vertex.name;
        nextName = name;
        id = vertex.GetID();
    }

    public bool Execute()
    {
        ((Vertex)IDManager.GetGP(id)).Name = nextName;
        return true;
    }

    public bool Redo()
    {
        ((Vertex)IDManager.GetGP(id)).Name = nextName;
        return true;
    }

    public bool Undo()
    {
        ((Vertex)IDManager.GetGP(id)).Name = prevName;
        return true;
    }
}
