using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlControl : AbstractControlStrategy
{
    public CtrlControl(Collector collectorPrefab, GraphManager graphManager) : base(graphManager) {
        this.collectorPrefab = collectorPrefab;
    }

    bool toCreate = false;
    bool toRemove = false;
    Collector collectorPrefab;
    Collector collectorInUse;

    public override IGraphPart OnClick(Vector2 mousePos)
    {
        IGraphPart selectedGP = base.OnClick(mousePos);
        toRemove = false;

        if (selectedGP == null)
        {
            toCreate = true;
            collectorInUse = Object.Instantiate(collectorPrefab);
            collectorInUse.Init(mousePos.x, mousePos.y);
            return collectorInUse.subParts;
        }

        if (selectedGP.GetIsSelected())
        {
            toRemove = true;
            lastSelected = selectedGP;
            return selectedGP;
        }

        lastSelected = selectedGP;
        graphManager.AddToSelected(selectedGP);
        return selectedGP;
    }

    public override void OnHold(Vector2 delta, Vector2 mousePos)
    {
        toCreate = false;
        toRemove = false;
        if (collectorInUse != null)
        {
            collectorInUse.UpdatePosition(mousePos.x, mousePos.y);
        } 
        else
        {
            graphManager.MoveAllInSelectedByVector(delta);
        }
    }

    public override void OnHoldEnd(Vector2 mousePos)
    {
        if (toRemove)
        {
            graphManager.RemoveFromSelected(lastSelected);
        }
        else if (toCreate)
        {
            graphManager.RemoveAllFromSelected();
        }
        else if (collectorInUse != null)
        {
            collectorInUse.Disable();
            Object.Destroy(collectorInUse.gameObject);
        }
        toRemove = false;
        toCreate = false;
    }

    public override void OnDelete()
    {
        graphManager.DestroyAllSelected();
    }
}
