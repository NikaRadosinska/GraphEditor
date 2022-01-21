using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EdgeEnd : MonoBehaviour, IGraphPart
{
    private Edge edgeMid;
    public Vertex vertex = null;
    private bool isSelected = false;
    private SpriteRenderer selectedSR;
    public Vector3 position { get { return transform.position; } }

    public class EdgeEndMoveEvent : UnityEvent { }
    [FormerlySerializedAs("onMove")]
    [SerializeField]
    private EdgeEndMoveEvent m_OnMove = new EdgeEndMoveEvent();
    public EdgeEndMoveEvent onMove
    {
        get { return m_OnMove; }
        set { m_OnMove = value; }
    }

    private void Awake()
    {
        edgeMid = transform.parent.GetChild(0).GetComponentInParent<Edge>();
        selectedSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        UnSelect();
        if (vertex != null)
        {
            transform.position = vertex.GetWorldPos();
        }
    }

    public Vertex GetOtherVertex(Vertex vertex) { return edgeMid.GetOtherVertex(vertex); }

    public void ChangeWidth(float widthValue)
    {
        
    }

    public IGraphPart GetGraphBeh() { return this; }

    public bool GetIsSelected() { return isSelected; }

    public bool isContainingGraphPart(IGraphPart part) { return part == (IGraphPart)this; }

    public void MoveAt(Vector3 position)
    {
        //if (!areBothSelected())
        //{
        //    return;
        //}
        transform.position = position;
        m_OnMove.Invoke();
    }

    public void MoveBy(Vector3 delta)
    {
        //if (!areBothSelected())
        //{
        //    return;
        //}
        transform.position += delta;
        m_OnMove.Invoke();
    }

    public void CheckVertex()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, 1 << 9);
        if (hit.collider != null)
        {
            IGraphPart selected = hit.collider.GetComponent<IGraphPart>();
            if (selected.GetGraphPartType() == GraphPartType.VERTEX)
            {
                AttachVertex((Vertex)selected);
            }
        }
        else
        {
            RemoveVertex();
        }
    }

    public void Destroy()
    {
        RemoveVertex();
    }

    public void RemoveVertex()
    {
        if (vertex != null)
        {
            vertex.RemoveEdge(this);
            vertex = null;
            edgeMid.CheckConnections();
        }
    }

    public void AttachVertex(Vertex vertex)
    {
        if (this.vertex != null)
        {
            if (this.vertex != vertex)
            {
                this.vertex.RemoveEdge(this);
            } 
            else
            {
                transform.position = vertex.position;
                return;
            }
        }
        this.vertex = vertex;
        vertex.AddEdge(this);
        transform.position = vertex.position;
    }

    public void Remove(IGraphPart part) { return; }

    public void Select()
    {
        selectedSR.enabled = true;
        isSelected = true;
    }

    public void ExpandedSelect()
    {
        Select();
        edgeMid.Select();
    }

    public void UnSelect()
    {
        if (isOneSelected())
        {
            return;
        }
        selectedSR.enabled = false;
        isSelected = false;
    }

    private bool areBothSelected()
    {
        if (!edgeMid.GetIsSelected())
        {
            //return false;
        }
        if (vertex != null)
        {
            if (!vertex.GetIsSelected())
            {
                return false;
            }
        }
        return true;
    }

    private bool isOneSelected()
    {
        if (vertex != null)
        {
            if (vertex.GetIsSelected())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (edgeMid.GetIsSelected())
        {
            return true;
        }
        return false;
    }

    public GraphPartType GetGraphPartType() { return GraphPartType.EDGE_END; }

    public Vector3 GetWorldPos()
    {
        return transform.position;
    }
    public void StopMovement()
    {
        edgeMid.CheckLock();
        CheckVertex();
    }

    public void TurnToVector(Vector3 vector)
    {
        if (vector.y - transform.position.y < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 + Mathf.Rad2Deg * Mathf.Asin((vector.x - transform.position.x) / Vector3.Distance(vector, transform.position))));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0,0, - Mathf.Rad2Deg * Mathf.Asin((vector.x - transform.position.x) / Vector3.Distance(vector, transform.position))));
        }
    }

    public float GetBiggestX()
    {
        return transform.position.x;
    }

    public float GetSmallestX()
    {
        return transform.position.x;
    }

    public float GetBiggestY()
    {
        return transform.position.y;
    }

    public float GetSmallestY()
    {
        return transform.position.y;
    }
}
