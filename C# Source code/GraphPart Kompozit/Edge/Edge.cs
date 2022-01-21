using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Edge : AccessBehaviour, IGraphPart
{
    private bool isSelected = false;
    private SpriteRenderer selectedSR;

    private LineRenderer lineRenderer;

    [SerializeField]
    private bool isLocked = true;
    public bool IsLocked() { return isLocked; }

    private Vector3[] positions;

    [HideInInspector]
    public EdgeEnd leftEdgeEnd;
    [HideInInspector]
    public EdgeEnd rightEdgeEnd;
    public Vertex leftVertex { 
        get { return leftEdgeEnd.vertex; }
        set { leftEdgeEnd.vertex = value; }
    }
    public Vertex rightVertex
    {
        get { return rightEdgeEnd.vertex; }
        set { rightEdgeEnd.vertex = value; }
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
        
        leftEdgeEnd = transform.parent.GetChild(1).GetComponent<EdgeEnd>();
        rightEdgeEnd = transform.parent.GetChild(2).GetComponent<EdgeEnd>();
    }

    private void Start()
    {
        lineRenderer.positionCount = Settings.PlayerSettings.curvesQuality;
        positions = new Vector3[Settings.PlayerSettings.curvesQuality];
        leftEdgeEnd.onMove.AddListener(LockedMove);
        rightEdgeEnd.onMove.AddListener(LockedMove);
        DrawCurve();
        UnSelect();
    }

    private void DrawCurve()
    {
        for (int i = 0; i <= Settings.PlayerSettings.curvesQuality-1; i++)
        {
            float t = i / ((float)Settings.PlayerSettings.curvesQuality-1);
            Vector3 middlepoint = CalculateMiddlePoint();
            /*Debug.Log("Af = " + middlepoint);
            Debug.Log("Am = " + transform.position);
            Debug.Log("A1 = " + pointLeft.transform.position);
            Debug.Log("A2 = " + pointRight.transform.position);*/
            positions[i] = GetPoint(leftEdgeEnd.position, middlepoint, rightEdgeEnd.position, t);
        }
        lineRenderer.SetPositions(positions);
        leftEdgeEnd.TurnToVector(positions[1]);
        rightEdgeEnd.TurnToVector(positions[Settings.PlayerSettings.curvesQuality-2]);
    }

    private Vector3 CalculateMiddlePoint()
    {
        return 2f * transform.position - (leftEdgeEnd.position + rightEdgeEnd.position) / 2f;
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
    }

    private bool HaveConnection()
    {
        return leftEdgeEnd.vertex != null || rightEdgeEnd.vertex != null;
    }

    public void CheckLock()
    {
        if (ShouldBeLocked())
        {
            isLocked = true;
            leftEdgeEnd.onMove.AddListener(LockedMove);
            rightEdgeEnd.onMove.AddListener(LockedMove);
            transform.position = (leftEdgeEnd.position + rightEdgeEnd.position) / 2f;
            DrawCurve();
        }
        else
        {
            isLocked = false;
            leftEdgeEnd.onMove.RemoveListener(LockedMove);
            rightEdgeEnd.onMove.RemoveListener(LockedMove);
            leftEdgeEnd.onMove.AddListener(DrawCurve);
            rightEdgeEnd.onMove.AddListener(DrawCurve);
        }
    }

    private bool ShouldBeLocked()
    {
        return (Vector3.Distance(leftEdgeEnd.transform.position, transform.position) + Vector3.Distance(transform.position, rightEdgeEnd.transform.position) - Vector3.Distance(leftEdgeEnd.transform.position, rightEdgeEnd.transform.position) < Settings.PlayerSettings.lockedSensitivity) ;
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
        transform.position = (leftEdgeEnd.position + rightEdgeEnd.position) / 2f;
        DrawCurve();
    }

    public void Select()
    {
        isSelected = true;
        selectedSR.enabled = true;
        if (leftVertex == null)
        {
            leftEdgeEnd.Select();
        }
        if (rightVertex == null)
        {
            rightEdgeEnd.Select();
        }
    }

    public void ExpandedSelect()
    {
        Select();
        leftEdgeEnd.ExpandedSelect();
        rightEdgeEnd.ExpandedSelect();
    }

    public void UnSelect()
    {
        isSelected = false;
        selectedSR.enabled = false;
        leftEdgeEnd.UnSelect();
        rightEdgeEnd.UnSelect();
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
        CheckLock();
        ControlVertices();
    }

    public void ControlVertices()
    {
        leftEdgeEnd.CheckVertex();
        rightEdgeEnd.CheckVertex();
        CheckConnections();
    }

    public void Destroy()
    {
        leftEdgeEnd.Destroy();
        rightEdgeEnd.Destroy();
        Destroy(transform.parent.gameObject);
    }

    public float GetBiggestX()
    {
        return Mathf.Max(leftEdgeEnd.GetWorldPos().x, rightEdgeEnd.GetWorldPos().x, GetWorldPos().x);
    }

    public float GetSmallestX()
    {
        return Mathf.Min(leftEdgeEnd.GetWorldPos().x, rightEdgeEnd.GetWorldPos().x, GetWorldPos().x);
    }

    public float GetBiggestY()
    {
        return Mathf.Max(leftEdgeEnd.GetWorldPos().y, rightEdgeEnd.GetWorldPos().y, GetWorldPos().y);
    }

    public float GetSmallestY()
    {
        return Mathf.Min(leftEdgeEnd.GetWorldPos().x, rightEdgeEnd.GetWorldPos().x, GetWorldPos().x);
    }
}
