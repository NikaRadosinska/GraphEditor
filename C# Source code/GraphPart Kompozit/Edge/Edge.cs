using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Edge : AccessBehaviour, IGraphPart
{
    public int ID;
    private int toStopMovement;

    public Gradient colorTwoConnected;
    public Gradient colorOneConnected;

    public Tuple<string, string> Name
    {
        get { return new Tuple<string, string>(leftVertex.Name, rightVertex.Name); }
        set { throw new System.Exception("Prepisovanie edge ID"); }
    }

    private bool isSelected = false;
    private SpriteRenderer selectedSR;

    private LineRenderer lineRenderer;

    [SerializeField]
    private bool isLocked = true;
    public bool IsLocked() { return isLocked; }

    private Vector3[] positions;

    [HideInInspector]
    public int leftEdgeEnd;
    [HideInInspector]
    public int rightEdgeEnd;
    public Vertex leftVertex { 
        get { return ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).GetVertex(); }
        set { ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).AttachVertex(value); }
    }
    public Vertex rightVertex
    {
        get { return ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).GetVertex(); }
        set { ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).AttachVertex(value); }
    }

    private void LateUpdate()
    {
        if (toStopMovement == 2)
        {
            CheckLock();
            ControlVertices();
            DrawCurve();
            CheckColors();

            toStopMovement = 0;
        } else if (toStopMovement == 1)
        {
            toStopMovement++;
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

    public Vertex GetOtherVertex(Vertex v)
    {
        if (v == leftVertex)
        {
            return rightVertex;
        }
        if (v == rightVertex)
        {
            return leftVertex;
        }
        return null;
    }
    private void Awake()
    {
        selectedSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();

        positions = new Vector3[Settings.PlayerSettings.curvesQuality];

        isSelected = false;
        selectedSR.enabled = false;
    }

    public void MyStart()
    {
        lineRenderer.positionCount = Settings.PlayerSettings.curvesQuality;
        ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).onMove.AddListener(LockedMove);
        ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).onMove.AddListener(LockedMove);
    }

    public void DrawCurve()
    {
        for (int i = 0; i <= Settings.PlayerSettings.curvesQuality-1; i++)
        {
            float t = i / ((float)Settings.PlayerSettings.curvesQuality-1);
            Vector3 middlepoint = CalculateMiddlePoint();
            /*Debug.Log("Af = " + middlepoint);
            Debug.Log("Am = " + transform.position);
            Debug.Log("A1 = " + leftEdgeEnd.transform.position);
            Debug.Log("A2 = " + rightEdgeEnd.transform.position);*/
            positions[i] = GetPoint(((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).position, middlepoint, ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).position, t);
        }
        lineRenderer.SetPositions(positions);
        ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).TurnToVector(positions[1]);
        ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).TurnToVector(positions[Settings.PlayerSettings.curvesQuality-2]);
    }

    private Vector3 CalculateMiddlePoint()
    {
        return 2f * transform.position - (((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).position + ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).position) / 2f;
    }

    private Vector3 GetPoint(Vector3 p0, Vector3 pmiddle, Vector3 p1, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(p0, pmiddle, t), Vector3.Lerp(pmiddle, p1, t), t);
    }

    public void CheckConnections()
    {
        if (!HaveConnection())
        {
            this.Destroy();
        }
        CheckColors();
    }

    private bool HaveConnection()
    {
        return ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).GetVertex() != null || ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).GetVertex() != null;
    }

    public void CheckLock()
    {
        if (ShouldBeLocked())
        {
            isLocked = true;
            ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).onMove.AddListener(LockedMove);
            ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).onMove.AddListener(LockedMove);
            transform.position = (((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).position + ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).position) / 2f;
            DrawCurve();
        }
        else
        {
            isLocked = false;
            ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).onMove.RemoveListener(LockedMove);
            ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).onMove.RemoveListener(LockedMove);
            ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).onMove.AddListener(DrawCurve);
            ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).onMove.AddListener(DrawCurve);
        }
    }

    private bool ShouldBeLocked()
    {
        return (Vector3.Distance(((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).transform.position, transform.position) + Vector3.Distance(transform.position, ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).transform.position) - Vector3.Distance(((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).transform.position, ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).transform.position) < Settings.PlayerSettings.lockedSensitivity) ;
    }

    public void Remove(IGraphPart part) { return; }

    public void MoveBy(Vector3 delta)
    {
        transform.position += delta;
        DrawCurve();
    }

    public void MoveAt(Vector3 position)
    {
        transform.position = position;
        DrawCurve();
    }

    public void LockedMove() { 
        transform.position = (((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).position + ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).position) / 2f;
        DrawCurve();
    }

    public void Select()
    {
        isSelected = true;
        selectedSR.enabled = true;
        /*
        if (leftVertex == null)
        {
            leftEdgeEnd.Select();
        }
        if (rightVertex == null)
        {
            rightEdgeEnd.Select();
        }
        */
    }

    public void ExpandedSelect()
    {
        Select();
        ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).ExpandedSelect();
        ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).ExpandedSelect();
    }

    public void UnSelect()
    {
        isSelected = false;
        selectedSR.enabled = false;
        ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).UnSelect();
        ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).UnSelect();
    }

    public void ChangeWidth(float widthValue)
    {
        throw new System.NotImplementedException();
    }

    public void ChangeEdgeDistance(float widthValue)
    {

    }

    public IGraphPart GetGraphBeh() { return this; }

    public bool isContainingGraphPart(IGraphPart part) { return part == (IGraphPart)this; }

    public bool GetIsSelected() { return isSelected; }

    public GraphPartType GetGraphPartType() { return GraphPartType.EDGE; }

    public Vector3 GetWorldPos()
    {
        return transform.position;
    }
    public void StopMovement()
    {
        toStopMovement = 1;
    }

    public void ControlVertices()
    {
        ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).CheckVertex();
        ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).CheckVertex();
        CheckConnections();
        CheckColors();
    }

    public void CheckColors()
    {
        int num = 0;
        num += (((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).GetVertex() != null) ? 1 : 0;
        num += (((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).GetVertex() != null) ? 1 : 0;
        if(num == 1)
        {
            lineRenderer.colorGradient = colorOneConnected;
        } else
        {
            lineRenderer.colorGradient = colorTwoConnected;
        }
    }

    public void Destroy()
    {
        ((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).Destroy();
        ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).Destroy();
        Destroy(transform.parent.gameObject);
    }

    public float GetBiggestX()
    {
        return Mathf.Max(((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).GetWorldPos().x, ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).GetWorldPos().x, GetWorldPos().x);
    }

    public float GetSmallestX()
    {
        return Mathf.Min(((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).GetWorldPos().x, ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).GetWorldPos().x, GetWorldPos().x);
    }

    public float GetBiggestY()
    {
        return Mathf.Max(((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).GetWorldPos().y, ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).GetWorldPos().y, GetWorldPos().y);
    }

    public float GetSmallestY()
    {
        return Mathf.Min(((EdgeEnd)IDManager.GetGP(leftEdgeEnd)).GetWorldPos().x, ((EdgeEnd)IDManager.GetGP(rightEdgeEnd)).GetWorldPos().x, GetWorldPos().x);
    }

    public Vector2 GetNumOfVerticesAndEdges()
    {
        return new Vector2(0, 1);
    }

    public List<Vertex> GetVertices()
    {
        return new List<Vertex>();
    }

    public List<Edge> GetEdges()
    {
        List<Edge> edges = new List<Edge>();
        edges.Add(this);
        return edges;
    }
}
