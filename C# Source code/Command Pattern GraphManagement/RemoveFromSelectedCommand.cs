using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromSelectedCommand : CommandCommonInfo, ICommand
{
    private int ID;

    public RemoveFromSelectedCommand(IGraphPart gp)
    {
        ID = gp.GetID();
    }

    public bool Execute()
    {
        IGraphPart part = IDManager.GetGP(ID);
        selected.Remove(part);
        part.UnSelect();
        return true;
    }

    public bool Redo()
    {
        Execute();
        return true;
    }

    public bool Undo()
    {
        IGraphPart part = IDManager.GetGP(ID);

        if (part.GetIsSelected())
            return true;
        selected.Add(part);
        part.Select();
        return true;
    }
}
