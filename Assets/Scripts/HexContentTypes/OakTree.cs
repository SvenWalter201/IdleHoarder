using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakTree : HexCellContent
{
    public override bool IsDestructible => false;

    public override void Initialize(HexCell hexCell)
    {
        base.Initialize(hexCell);
        HexGrid.Instance.AddProductionSite(this);
    }

    public override void Destruct()
    {
        base.Destruct();
        HexGrid.Instance.RemoveProductionSite(this);
    }
}
