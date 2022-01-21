using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractControlStrategy 
{
    public IGraphPart lastSelected;
    public GraphManager graphManager;
    

    public AbstractControlStrategy(GraphManager graphManager)
    {
        this.graphManager = graphManager;
    }

    /// <summary>
    /// Returns IGraphPart on which one was clicked.
    /// </summary>
    /// <param name="mousePos"></param>
    /// <returns></returns>
    public virtual IGraphPart OnClick(Vector2 mousePos)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, 1<<9);
        if (hit.collider != null){
            IGraphPart selected = hit.collider.GetComponent<IGraphPart>();
            if (selected == null)
            {
                selected = hit.transform.GetComponentInParent<IGraphPart>();
            }
            return selected;
        }
        return null;
    }

    public virtual void OnHold(Vector2 delta, Vector2 mousePos)
    {

    }

    public virtual void OnHoldEnd(Vector2 mousePos)
    {
        
    }

    public virtual void OnDoubleClick()
    {

    }

    public virtual void OnDelete()
    {
        graphManager.DestroyAllSelected();
    }

    public virtual void OnMouseMove(Vector2 delta, Vector2 mousePos)
    {

    }
}
