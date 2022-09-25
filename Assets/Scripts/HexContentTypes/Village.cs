using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : HexCellContent
{
    public override bool IsDestructible => true;
    public override bool IsInteractable => false;

    public override string Name => "Village";
    public override ResourceContainer GetCost()
    {
        var c = new ResourceContainer();
        c.storedResources[0] = 40;
        c.storedResources[1] = 20;
        return c;
    }
}
