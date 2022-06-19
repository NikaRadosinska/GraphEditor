using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EdgeEnd : MonoBehaviour, IGraphPart
{
    private int ID;
    private int toStopMovement;

    private int edgeMid;   
    public void SetMidId(int midId) { edgeMid = midId; }
    public Edge GetMid() {  return (Edge)IDManager.GetGP(edgeMid); }
    [SerializeField]
    private int vertex;
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
        selectedSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        selectedSR.enabled = false;
        isSelected = false;
        gameObject.tag = "Collectorable";
    }

    void Start()
    {
        if (vertex != 0)
        {
            transform.position = IDManager.GetGP(vertex).GetWorldPos();
        }
    }

    private void LateUpdate()
    {
        if (toStopMovement == 2)
        {
            GetMid().StopMovement();
            CheckVertex();

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

    public Vertex GetOtherVertex(Vertex vertex) { return ((Edge)IDManager.GetGP(edgeMid)).GetOtherVertex(vertex); }

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
                MoveAt(selected.GetWorldPos());
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

    public Vertex GetVertex()
    {
        return (Vertex)IDManager.GetGP(vertex);
    }

    public void RemoveVertex()
    {
        Vertex vertex = (Vertex)IDManager.GetGP(this.vertex);
        if (vertex != null)
        {
            vertex.RemoveEdge(ID);
            this.vertex = 0;
            GetMid().CheckConnections();
            gameObject.tag = "Collectorable";
        }
    }

    public void AttachVertex(Vertex vertex)
    {
        Vertex _vertex = (Vertex)IDManager.GetGP(this.vertex);

        if (_vertex != null)
        {
            if (_vertex != vertex)
            {
                _vertex.RemoveEdge(ID);
            } 
            else
            {
                transform.position = vertex.position;
                return;
            }
        }
        this.vertex = vertex.ID;
        gameObject.tag = "Untagged";
        vertex.AddEdge(ID);
        transform.position = vertex.position;
        GetMid().DrawCurve();
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
        GetMid().Select();
    }

    public void UnSelect()
    {
        if (isOneSelected())
        {
            return;
        }
        if(selectedSR == null)
        {
            return;
        }
        selectedSR.enabled = false;
        isSelected = false;
    }

    private bool areBothSelected()
    {
        Vertex vertex = (Vertex)IDManager.GetGP(this.vertex);

        if (!GetMid().GetIsSelected())
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
        Vertex vertex = (Vertex)IDManager.GetGP(this.vertex);

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
        if (GetMid().GetIsSelected())
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
        toStopMovement = 1;
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

    public Vector2 GetNumOfVerticesAndEdges()
    {
        return Vector2.zero;
    }

    public List<Vertex> GetVertices()
    {
        return new List<Vertex>();
    }

    public List<Edge> GetEdges()
    {
        return new List<Edge>();
    }
}
