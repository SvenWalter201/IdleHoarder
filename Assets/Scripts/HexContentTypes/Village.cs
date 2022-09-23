using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : HexCellContent
{
    public override bool IsDestructible => true;
    public override ResourceContainer GetCost()
    {
        return new ResourceContainer { acorn = 10};
    }
}
