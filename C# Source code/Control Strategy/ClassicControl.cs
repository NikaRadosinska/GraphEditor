using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicControl : AbstractControlStrategy
{
    public ClassicControl(GraphManager graphManager) : base(graphManager){}

    private bool toRemove = false;

    public override IGraphPart OnClick(Vector2 mousePos)
    {
        IGraphPart selectedGP = base.OnClick(mousePos); 

        if (selectedGP == null)
        {
            lastSelected = null;
            graphManager.RemoveAllFromSelected();
            return null;
        }

        if (selectedGP.GetIsSelected())
        {
            toRemove = true;
        } 
        else if (lastSelected != selectedGP)
        {
            graphManager.RemoveAllFromSelected();
        }

        graphManager.RemoveAllFromSelected();
        lastSelected = selectedGP;
        graphManager.AddToSelected(selectedGP);
        return selectedGP;
    }

    public override void OnHold(Vector2 delta, Vector2 mousPos)
    {
        toRemove = false;
        graphManager.MoveAllInSelectedOnVector(mousPos);
    }

    public override void OnHoldEnd(Vector2 mousePos)
    {
        if (toRemove)
        {
            graphManager.RemoveAllFromSelected();
            toRemove = false;
        }
    }
}
