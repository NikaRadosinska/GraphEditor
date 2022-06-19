using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWholeEdgeCommand : CommandCommonInfo, ICommand
{
        
    private Vector3 left;
    private Vector3 mid;
    private Vector3 right;

    private int IDleft;
    private int IDmid;
    private int IDright;

    //private GraphManager graph;

    private Edge e;

    public CreateWholeEdgeCommand(Edge e)
    {
        this.e = e;

        left = ((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)).GetWorldPos();
        mid = e.GetWorldPos();
        right = ((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd)).GetWorldPos();

        IDleft = ((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)).GetID();
        IDmid = e.GetID();
        IDright = ((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd)).GetID();

        e.StopMovement();
        //this.graph = graph;
    }

    public CreateWholeEdgeCommand(EdgeInfoWithIds edgeInfo, Vector3 pivot)
    {
        IDmid = edgeInfo.middleID;
        IDright = edgeInfo.rightEdgeEndID;
        IDleft = edgeInfo.leftEdgeEndID;
        pivot = new Vector3(pivot.x, pivot.y, 0);
        e = Instantiate(edgePrefab, edgesParent).transform.GetChild(0).GetComponent<Edge>();
        IDManager.SetById(e, IDmid);
        IDManager.SetById(e.gameObject.transform.parent.GetChild(1).GetComponent<EdgeEnd>(), IDleft);
        ((EdgeEnd)IDManager.GetGP(IDleft)).SetMidId(IDmid);
        IDManager.SetById(e.gameObject.transform.parent.GetChild(2).GetComponent<EdgeEnd>(), IDright);
        ((EdgeEnd)IDManager.GetGP(IDright)).SetMidId(IDmid);
        ((Edge)IDManager.GetGP(IDmid)).rightEdgeEnd = IDright;
        ((Edge)IDManager.GetGP(IDmid)).leftEdgeEnd = IDleft;

        allEdges.Add(e);
        ((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)).MoveAt(edgeInfo.leftEdgeEndPos + pivot);
        ((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd)).MoveAt(edgeInfo.rightEdgeEndPos + pivot);
        e.MoveAt(edgeInfo.middlePos + pivot);
        e.MyStart();
        //graph.AddToSelected(e);
    }

    public CreateWholeEdgeCommand(EdgeInfo edgeInfo, Vector3 pivot)
    {
        pivot = new Vector3(pivot.x, pivot.y, 0);
        e = Instantiate(edgePrefab, edgesParent).transform.GetChild(0).GetComponent<Edge>();
        IDmid = IDManager.SetId(e);
        IDleft = IDManager.SetId(e.gameObject.transform.parent.GetChild(1).GetComponent<EdgeEnd>());
        ((EdgeEnd)IDManager.GetGP(IDleft)).SetMidId(IDmid);
        IDright = IDManager.SetId(e.gameObject.transform.parent.GetChild(2).GetComponent<EdgeEnd>());
        ((EdgeEnd)IDManager.GetGP(IDright)).SetMidId(IDmid);
        ((Edge)IDManager.GetGP(IDmid)).rightEdgeEnd = IDright;
        ((Edge)IDManager.GetGP(IDmid)).leftEdgeEnd = IDleft;

        allEdges.Add(e);
        ((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)).MoveAt(edgeInfo.leftEdgeEndPos + pivot);
        ((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd)).MoveAt(edgeInfo.rightEdgeEndPos + pivot);
        e.MoveAt(edgeInfo.middlePos + pivot);
        e.MyStart();
        //graph.AddToSelected(e);
    }

    public Edge GetCreated()
    {
        return e;
    }

    public bool Execute()
    {
        return true;
    }

    public bool Redo()
    {
        e = Instantiate(edgePrefab, edgesParent).transform.GetChild(0).GetComponent<Edge>();
        IDManager.SetById(e, IDmid);
        IDManager.SetById(((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)), IDleft);
        IDManager.SetById(((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd)), IDright);
        allEdges.Add(e);
        ((EdgeEnd)IDManager.GetGP(e.leftEdgeEnd)).MoveAt(left);
        ((EdgeEnd)IDManager.GetGP(e.rightEdgeEnd)).MoveAt(right);
        e.MoveAt(mid);
        //graph.AddToSelected(e);
        e.StopMovement();
        return true;
    }

    public bool Undo()
    {
        IGraphPart edge = IDManager.GetGP(IDmid);
        allEdges.Remove(edge);
        //edge.UnSelect();
        edge.Destroy();
        return true;
    }
}
