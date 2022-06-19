using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : AccessBehaviour
{
    public SubGraph subParts;
    private bool isDisabled = true; 

    public void Init(float x, float y) 
    {
        isDisabled = false;
        transform.position = new Vector3(x,y,0);
        transform.localScale = Vector3.zero;
        subParts = new SubGraph();
    }

    public void UpdatePosition(float mousePosX, float mousePosY)
    {
        transform.localScale = new Vector3(mousePosX - transform.position.x, transform.position.y - mousePosY, 1);
    }

    public bool IsEnabled()
    {
        return !isDisabled;
    }

    public void Enable()
    {
        isDisabled = false;
    }

    public void Disable()
    {
        isDisabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectorable"))
        {
            IGraphPart gp = collision.gameObject.GetComponent<IGraphPart>();
            subParts.Add(gp);
            GraphManager.AddToSelected(gp);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectorable") && !isDisabled)
        {
            IGraphPart gp = collision.gameObject.GetComponent<IGraphPart>();
            subParts.Remove(gp);
            GraphManager.RemoveFromSelected(gp);
        }
    }
}
