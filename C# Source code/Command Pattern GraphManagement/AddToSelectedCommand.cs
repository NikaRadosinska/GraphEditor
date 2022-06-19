using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToSelectedCommand : CommandCommonInfo, ICommand
{
    private int ID;

    public AddToSelectedCommand(IGraphPart gp)
    {
        ID = gp.GetID();
    }

    public bool Execute()
    {
        IGraphPart part = IDManager.GetGP(ID);

        if (part.GetIsSelected())
            return true;
        selected.Add(part);
        part.Select();
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
        selected.Remove(part);
        part.UnSelect();
        return true;
    }
}
