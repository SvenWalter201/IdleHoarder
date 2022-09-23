using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : HexCellContent
{
    public override bool IsDestructible => false;

    public override bool SpawnConditions => HexGrid.Instance.mainBaseInstance == null;

    public override void Initialize(HexCell hexCell)
    {
        base.Initialize(hexCell);
        HexGrid.Instance.mainBaseInstance = this;
    }

    public override void Destruct()
    {
        base.Destruct();
        HexGrid.Instance.mainBaseInstance = null;
    }
}
