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



    public Stack<ICommand> commands = new Stack<ICommand>();
    public Stack<ICommand> redoCommands = new Stack<ICommand>();

    private void Start()
    {
        selected = new SubGraph();
        allEdges = new SubGraph();
        allVertices = new SubGraph();

        CommandCommonInfo.MyStart(selected, allEdges, allVertices, vertexPrefab, edgePrefab, vertexParent, edgesParent);
    }

    private bool isUnduing;

    public void Undo()
    {
        if(commands.Count <= 1) return;
        isUnduing = true;
        commands.Pop();
        ICommand c = commands.Pop();
        while(commands.Count > 0 && c.Undo())
        {
            redoCommands.Push(c);
            c = commands.Pop();
        }
        redoCommands.Push(c);
        commands.Push(c);
        isUnduing=false;
    }

    public void Redo()
    {
        if (redoCommands.Count <= 0) return;
        redoCommands.Pop();
        ICommand c = redoCommands.Pop();
        while (redoCommands.Count > 0 && c.Redo())
        {
            commands.Push(c);
            c = redoCommands.Pop();
        }
        redoCommands.Push(c);
        commands.Push(c);
    }


    public void Checkpoint()
    {
        NewCommand(new CheckpointCommand());
    }

    private void NewCommand(ICommand command)
    {
        if(isUnduing)
        {
            command.Execute();
            return;
        }
        if (redoCommands.Count != 0) redoCommands = new Stack<ICommand>();
        commands.Push(command);
        command.Execute();
    }

    public void AddToSelected(IGraphPart part)
    {
        NewCommand(new AddToSelectedCommand(part));
    }

    public void RemoveFromSelected(IGraphPart part)
    {
        NewCommand(new RemoveFromSelectedCommand(part));
    }

    //NO COMMAND
    public bool IsInSelected(IGraphPart part)
    {
        return selected.isContainingGraphPart(part);
    }

    public void RemoveAllFromSelected()
    {
        NewCommand(new RemoveAllFromSelectedCommand());
    }

    //NO COMMAND
    public void MoveAllInSelectedByVector(Vector2 delta)
    {
        selected.MoveBy(delta);
    }

    //NO COMMAND
    public void MoveAllInSelectedOnVector(Vector2 vector)
    {
        selected.MoveAt(vector);
    }

    //NO current COMMAND
    public void ChangeWidthForAllEdges(float width)
    {
        foreach (IGraphPart gp in allEdges.GetGraphPartsInList())
        {
            gp.ChangeWidth(width);
        }
    }

    //NO current COMMAND
    public void ChangeWidthForAllVertices(float width)
    {
        foreach (IGraphPart gp in allVertices.GetGraphPartsInList())
        {
            gp.ChangeWidth(width);
        }
        foreach (IGraphPart gp in allEdges.GetGraphPartsInList())
        {
            ((Edge)gp).ChangeEdgeDistance(width);
        }
    }

    //NO COMMAND
    public void ControlEdgesVertices(Vector2 vector)
    {
        for (int i = 0; i < edgesParent.transform.childCount; i++)
        {
            edgesParent.transform.GetChild(i).GetComponent<Edge>().ControlVertices();
        }
    }

    //NO COMMAND
    private List<Vector3> prevPositions;
    public void StartMovement()
    {
        prevPositions = new List<Vector3>();
        foreach (IGraphPart gp in selected.GetGraphPartsInList())
        {
            prevPositions.Add(gp.GetWorldPos());
        }
    }

    public void StopMovement(bool movement, Edge edgeCreation)
    {
        if (movement)
        {
            NewCommand(new MovementCommand(prevPositions));
        }
        else if (edgeCreation != null)
        {
            NewCommand(new CreateWholeEdgeCommand(edgeCreation));
        }
    }

    public void DestroyAllSelected()
    {
        NewCommand(new DestroyAllInSelectedCommand(this));
    }

    //NO COMMAND
    public Vector2 GetSelectedNumsOfVerticesAndEdges()
    {
        return selected.GetNumOfVerticesAndEdges();
    }

    //NO COMMAND
    public SubGraphInfoWithIds GetSelectedSubGraphInfoWithIds()
    {
        return new SubGraphInfoWithIds(selected);
    }

    //NO COMMAND
    public SubGraphInfo GetSelectedSubGraphInfo()
    {
        return new SubGraphInfo(selected);
    }

    //NO COMMAND
    public string GetAllGraphInfo()
    {
        string s = "";

        foreach (int v in allVertices.subParts)
        {
            s += allVertices.subParts.IndexOf(v) + ":";
            foreach (Vertex v2 in ((Vertex)IDManager.GetGP(v)).outgoingEdgesDestination())
            {
                s += " " + allVertices.GetGraphPartsInList().IndexOf(v2);
            }
            s += "\n";
        }
        return s;
    }

    public Vertex CreateVertex(Vector3 mousePos)
    {
        CreateVertexCommand cvc = new CreateVertexCommand(mousePos, this);
        NewCommand(cvc);
        return cvc.GetCreated();
    }

    public Vertex CreateVertex(Vector3 mousePos, int ID)
    {
        CreateVertexCommand cvc = new CreateVertexCommand(mousePos, this, ID);
        NewCommand(cvc);
        return cvc.GetCreated();
    }


    public Edge CreateEdge(EdgeInfoWithIds edgeInfo, Vector3 pivot)
    {
        CreateWholeEdgeCommand cwec = new CreateWholeEdgeCommand(edgeInfo, pivot);
        NewCommand(cwec);
        return cwec.GetCreated();
    }

    public Edge CreateEdge(EdgeInfo edgeInfo, Vector3 pivot)
    {
        CreateWholeEdgeCommand cwec = new CreateWholeEdgeCommand(edgeInfo, pivot);
        NewCommand(cwec);
        return cwec.GetCreated();
    }

    //NO COMMAND
    public Edge CreateEdgeBeginning(Vertex vertex)
    {
        Edge e = Instantiate(edgePrefab, edgesParent.transform).transform.GetChild(0).GetComponent<Edge>();
        int IDmid = IDManager.SetId(e);
        int IDleft = IDManager.SetId(e.gameObject.transform.parent.GetChild(1).GetComponent<EdgeEnd>());
        ((EdgeEnd)IDManager.GetGP(IDleft)).SetMidId(IDmid);
        int IDright = IDManager.SetId(e.gameObject.transform.parent.GetChild(2).GetComponent<EdgeEnd>());
        ((EdgeEnd)IDManager.GetGP(IDright)).SetMidId(IDmid);
        ((Edge)IDManager.GetGP(IDmid)).rightEdgeEnd = IDright;
        ((Edge)IDManager.GetGP(IDmid)).leftEdgeEnd = IDleft;
        
        e.MyStart();
        allEdges.Add(e);

        e.leftVertex = vertex;
        ((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)).MoveAt(vertex.GetWorldPos());
        vertex.AddEdge(e.leftEdgeEnd);
        AddToSelected(e);
        return e;
    }

    public void SetVertexName(Vertex vertex, string name)
    {
        NewCommand(new SetVertexNameCommand(vertex, name));
    }
}
